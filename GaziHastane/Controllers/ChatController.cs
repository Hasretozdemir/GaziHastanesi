using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GaziHastane.Data;
using GaziHastane.Models;

namespace GaziHastane.Controllers
{
    public class ChatController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly GaziHastaneContext _context;

        public ChatController(IConfiguration configuration, GaziHastaneContext context)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserMessage))
            {
                return Json(new { success = false, reply = "Lütfen bir mesaj yazın." });
            }

            var normalizedMessage = NormalizeMessage(request.UserMessage);

            var mealResponse = await BuildMealResponse(normalizedMessage);
            if (!string.IsNullOrWhiteSpace(mealResponse))
            {
                return Json(new { success = true, reply = mealResponse });
            }

            if (IsSensitiveRequest(normalizedMessage))
            {
                var actions = BuildSensitiveActions(normalizedMessage);
                var linkText = actions.Any()
                    ? string.Join(" | ", actions.Select(a => $"<a href=\"{a.Url}\" style=\"color:#2563eb;font-weight:700;text-decoration:underline;\">{a.Title}</a>"))
                    : string.Empty;
                return Json(new
                {
                    success = true,
                    reply = string.IsNullOrWhiteSpace(linkText)
                        ? "Kişisel veri güvenliği kuralları gereği tahlil, randevu ve ödeme işlemlerinizi bu ekran üzerinden yapamıyoruz. Lütfen ilgili hasta panelini kullanınız."
                        : $"Kişisel veri güvenliği kuralları gereği tahlil, randevu ve ödeme işlemlerinizi bu ekran üzerinden yapamıyoruz. Lütfen ilgili hasta panelini kullanınız. {linkText}",
                    actions
                });
            }

         

            // 1. Kullanıcının sorusuna göre GÜVENLİ tablolardan bilgi çekiyoruz
            string dbContextInfo = await GetContextFromDatabase(request.UserMessage);

            // 2. Yapay Zeka Kuralları (Özel verileri koruma kalkanımız)
            string systemPrompt = $@"Sen Gazi Hastanesi'nin resmi dijital asistanısın. 
Kuralların şunlardır:
1. Gelen sorulara SADECE aşağıda verdiğim 'Hastane Veritabanı Bilgileri' kısmını kullanarak cevap ver.
2. Sana verilen veritabanı bilgilerinde sorunun cevabı YOKSA, kesinlikle internetten bilgi uydurma ve 'Bu konuda detaylı bilgiye şu an ulaşamıyorum, lütfen hastanemizin iletişim kanallarını kullanın.' de.
3. KESİNLİKLE kullanıcılardan TC Kimlik No, tahlil sonucu, randevu numarası, ödeme bilgisi veya şifre isteme! Eğer tahlil, randevu veya borç sorarlarsa: 'Kişisel veri güvenliği kuralları gereği tahlil, randevu ve ödeme işlemlerinizi bu ekran üzerinden yapamıyoruz. Lütfen hasta panelimizi kullanınız.' de.
4. Asla tıbbi tavsiye (şu ilacı kullan, şu teşhis konulur vb.) verme. Sadece hastanenin işleyişi hakkında bilgi ver.
5. Cevapların kısa, net, kibar ve okunabilir olsun.

Hastane Veritabanı Bilgileri:
{dbContextInfo}";

            string fullPrompt = $"{systemPrompt}\n\nKullanıcı Sorusu: {request.UserMessage}";

            // 3. API'ye İstek Atma
            string? apiKey = _configuration["GeminiSettings:ApiKey"]?.Trim();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return Json(new { success = false, reply = "Yapay zeka servis anahtarı appsettings.json dosyasında bulunamadı." });
            }

            // Google'ın en güncel ve çalışan 2.5 Flash modeli
            string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            var payload = new
            {
                contents = new[] { new { parts = new[] { new { text = fullPrompt } } } }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseData);
                    var aiText = doc.RootElement
                                    .GetProperty("candidates")[0]
                                    .GetProperty("content")
                                    .GetProperty("parts")[0]
                                    .GetProperty("text").GetString();

                    var actions = await BuildContextualActions(normalizedMessage);
                    return Json(new { success = true, reply = aiText, actions });
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        return Json(new { success = false, reply = "Yapay zeka servisi şu anda yoğun. Lütfen biraz sonra tekrar deneyin." });
                    }

                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, reply = $"Google Hatası: {response.StatusCode} - Detay: {errorContent}" });
                }
            }
            catch
            {
                return Json(new { success = false, reply = "Sistemsel bir bağlantı hatası oluştu." });
            }
        }

        // --- GÜVENLİ TABLOLARDA AKILLI ARAMA ---
        private async Task<string> GetContextFromDatabase(string userMessage)
        {
            var contextBuilder = new StringBuilder();
            var normalizedMessage = NormalizeMessage(userMessage);
            var terms = ExtractTerms(normalizedMessage);

            // 1. DOKTORLAR TABLOSU
            if (ContainsAny(normalizedMessage, "doktor", "hekim", "uzman", "prof", "doç"))
            {
                var doktorlar = await _context.Doktorlar
                    .Include(d => d.Bolum)
                    .Where(d => d.IsActive)
                    .Take(50)
                    .ToListAsync();

                var filtreliDoktorlar = doktorlar
                    .Where(d => terms.Any(t =>
                        (!string.IsNullOrWhiteSpace(d.Ad) && d.Ad.ToLower(new CultureInfo("tr-TR")).Contains(t)) ||
                        (!string.IsNullOrWhiteSpace(d.Soyad) && d.Soyad.ToLower(new CultureInfo("tr-TR")).Contains(t)) ||
                        (!string.IsNullOrWhiteSpace(d.UzmanlikAlani) && d.UzmanlikAlani.ToLower(new CultureInfo("tr-TR")).Contains(t)) ||
                        (!string.IsNullOrWhiteSpace(d.Bolum?.Ad) && d.Bolum.Ad.ToLower(new CultureInfo("tr-TR")).Contains(t))))
                    .DefaultIfEmpty()
                    .ToList();

                var hedefDoktorlar = filtreliDoktorlar.Where(d => d != null).Any() ? filtreliDoktorlar.Where(d => d != null) : doktorlar.Take(15);

                if (hedefDoktorlar.Any())
                {  
                    contextBuilder.AppendLine("Doktor Bilgileri:");
                    foreach (var dr in hedefDoktorlar)
                    {
                        var unvan = string.IsNullOrWhiteSpace(dr.Unvan) ? "Dr." : dr.Unvan;
                        var bolumAdi = dr.Bolum?.Ad;
                        var uzmanlik = dr.UzmanlikAlani;
                        contextBuilder.AppendLine($"- {unvan} {dr.Ad} {dr.Soyad} | Birim: {bolumAdi ?? "Bilinmiyor"} | Uzmanlık: {uzmanlik ?? "Bilinmiyor"}");
                        if (!string.IsNullOrWhiteSpace(dr.Ozgecmis))
                        {
                            contextBuilder.AppendLine($"  Özgeçmiş: {dr.Ozgecmis}");
                        }
                    }
                }
            }

            // 2. BÖLÜMLER TABLOSU
            if (ContainsAny(normalizedMessage, "bölüm", "klinik", "poliklinik", "hangi", "birim"))
            {
                var bolumler = await _context.Bolumler
                    .Where(b => b.IsActive)
                    .Take(50)
                    .ToListAsync();
                var filtreliBolumler = bolumler
                    .Where(b => terms.Any(t => b.Ad.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                    .ToList();

                var hedefBolumler = filtreliBolumler.Any() ? filtreliBolumler : bolumler.Take(15).ToList();
                if (bolumler.Any())
                {
                    contextBuilder.AppendLine("Hastanemizdeki Tıbbi Birimler ve Bölümler:");
                    foreach (var bolum in hedefBolumler)
                    {
                        var konum = string.IsNullOrWhiteSpace(bolum.Blok) && string.IsNullOrWhiteSpace(bolum.Kat)
                            ? ""
                            : $" | Konum: {bolum.Blok ?? ""} {bolum.Kat ?? ""}".Trim();
                        contextBuilder.AppendLine($"- {bolum.Ad}{konum}");
                    }
                }

                if (filtreliBolumler.Any() && ContainsAny(normalizedMessage, "doktor", "hekim", "uzman", "kim", "kimler"))
                {
                    var bolumIds = filtreliBolumler.Select(b => b.Id).ToList();
                    var bolumDoktorlari = await _context.Doktorlar
                        .Include(d => d.Bolum)
                        .Where(d => d.IsActive && d.BolumId.HasValue && bolumIds.Contains(d.BolumId.Value))
                        .OrderBy(d => d.Ad)
                        .ThenBy(d => d.Soyad)
                        .ToListAsync();

                    if (bolumDoktorlari.Any())
                    {
                        contextBuilder.AppendLine("Bölüm Doktorları:");
                        foreach (var dr in bolumDoktorlari)
                        {
                            var unvan = string.IsNullOrWhiteSpace(dr.Unvan) ? "Dr." : dr.Unvan;
                            contextBuilder.AppendLine($"- {unvan} {dr.Ad} {dr.Soyad} | Uzmanlık: {dr.UzmanlikAlani ?? "Bilinmiyor"}");
                            if (!string.IsNullOrWhiteSpace(dr.Ozgecmis))
                            {
                                contextBuilder.AppendLine($"  Özgeçmiş: {dr.Ozgecmis}");
                            }
                        }
                    }
                }
            }

            // 3. İLETİŞİM VE ULAŞIM BİLGİLERİ
            if (ContainsAny(normalizedMessage, "iletişim", "adres", "telefon", "nerede", "ulaşım", "nasıl gidilir", "konum", "harita", "metro", "otobüs", "otobus", "dolmuş", "dolmus", "tramvay", "servis"))
            {
                var iletisimler = await _context.IletisimBilgileri
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Id)
                    .ToListAsync();
                if (iletisimler.Any())
                {
                    contextBuilder.AppendLine("Hastane İletişim Bilgileri:");
                    foreach (var iletisim in iletisimler)
                    {
                        contextBuilder.AppendLine($"- {iletisim.Baslik} {iletisim.AltBaslik}".Trim());
                        if (!string.IsNullOrWhiteSpace(iletisim.KisaAdres))
                        {
                            contextBuilder.AppendLine($"  Kısa Adres: {iletisim.KisaAdres}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.Koordinat))
                        {
                            contextBuilder.AppendLine($"  Koordinat: {iletisim.Koordinat}");
                        }
                        contextBuilder.AppendLine($"  Adres: {iletisim.Adres}");
                        if (!string.IsNullOrWhiteSpace(iletisim.CagriMerkezi))
                        {
                            contextBuilder.AppendLine($"  Çağrı Merkezi: {iletisim.CagriMerkezi}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.Santral))
                        {
                            contextBuilder.AppendLine($"  Santral: {iletisim.Santral}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.DigerTelefonlar))
                        {
                            contextBuilder.AppendLine($"  Diğer Telefonlar: {iletisim.DigerTelefonlar}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.Email))
                        {
                            contextBuilder.AppendLine($"  E-Posta: {iletisim.Email}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.CalismaSaatleri))
                        {
                            contextBuilder.AppendLine($"  Çalışma Saatleri: {iletisim.CalismaSaatleri}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.EkBilgi))
                        {
                            contextBuilder.AppendLine($"  Ek Bilgi: {iletisim.EkBilgi}");
                        }
                        if (!string.IsNullOrWhiteSpace(iletisim.HaritaUrl))
                        {
                            contextBuilder.AppendLine($"  Harita: {iletisim.HaritaUrl}");
                        }
                    }
                }

                var ulasim = await _context.UlasimRehberleri.Where(x => x.IsActive).Take(10).ToListAsync();
                if (ulasim.Any())
                {
                    contextBuilder.AppendLine("Hastaneye Ulaşım Bilgileri:");
                    foreach (var u in ulasim) { contextBuilder.AppendLine($"- {u.UlasimTipi}: {u.Icerik}"); }
                }
            }

            // 4. DUYURULAR VE HABERLER
            if (ContainsAny(normalizedMessage, "duyuru", "haber", "yenilik", "güncel"))
            {
                var duyurular = await _context.Duyurular.OrderByDescending(d => d.Id).Take(3).ToListAsync();
                if (duyurular.Any())
                {
                    contextBuilder.AppendLine("Güncel Hastane Duyuruları:");
                    foreach (var duyuru in duyurular)
                    {
                        contextBuilder.AppendLine($"- {duyuru.Baslik}");
                        if (ContainsAny(normalizedMessage, "detay", "içerik", "açıklama") || terms.Any(t => duyuru.Baslik.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                        {
                            contextBuilder.AppendLine($"  Detay: {duyuru.Icerik}");
                        }
                    }
                }

                var haberler = await _context.Haberler.OrderByDescending(h => h.Id).Take(3).ToListAsync();
                if (haberler.Any())
                {
                    contextBuilder.AppendLine("Güncel Hastane Haberleri:");
                    foreach (var haber in haberler)
                    {
                        contextBuilder.AppendLine($"- {haber.Baslik}");
                        if (ContainsAny(normalizedMessage, "detay", "içerik", "açıklama") || terms.Any(t => haber.Baslik.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                        {
                            var detay = string.IsNullOrWhiteSpace(haber.Icerik) ? haber.Ozet : haber.Icerik;
                            contextBuilder.AppendLine($"  Detay: {detay}");
                        }
                    }
                }
            }

            // 5. ETKİNLİKLER TABLOSU
            if (ContainsAny(normalizedMessage, "etkinlik", "program", "toplantı", "kongre", "seminer"))
            {
                var etkinlikler = await _context.Etkinlikler.OrderByDescending(e => e.Id).Take(3).ToListAsync();
                if (etkinlikler.Any())
                {
                    contextBuilder.AppendLine("Yaklaşan Etkinlikler:");
                    foreach (var etkinlik in etkinlikler)
                    {
                        contextBuilder.AppendLine($"- {etkinlik.Baslik} | Tarih: {etkinlik.Tarih:dd.MM.yyyy} | Konum: {etkinlik.Konum}");
                        if (ContainsAny(normalizedMessage, "detay", "içerik", "açıklama") || terms.Any(t => etkinlik.Baslik.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                        {
                            var detay = string.IsNullOrWhiteSpace(etkinlik.ModalIcerik) ? etkinlik.Aciklama : etkinlik.ModalIcerik;
                            if (!string.IsNullOrWhiteSpace(detay))
                            {
                                contextBuilder.AppendLine($"  Detay: {detay}");
                            }
                        }
                    }
                }
            }

            // 6. HASTA REHBERLERİ TABLOSU
            if (ContainsAny(normalizedMessage, "rehber", "ziyaretçi", "refakat", "kural", "saat"))
            {
                var rehberler = await _context.HastaRehberleri.Take(10).ToListAsync();
                if (rehberler.Any())
                {
                    contextBuilder.AppendLine("Hasta ve Ziyaretçi Rehberi Bilgileri:");
                    foreach (var rehber in rehberler)
                    {
                        contextBuilder.AppendLine($"- {rehber.Baslik}");
                        if (ContainsAny(normalizedMessage, "detay", "içerik", "açıklama", "iletişim", "ulaşım", "ulasim", "metro", "otobüs", "otobus", "dolmuş", "dolmus", "tramvay", "servis") || terms.Any(t => rehber.Baslik.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                        {
                            var detay = string.IsNullOrWhiteSpace(rehber.ModalIcerik) ? rehber.Icerik : rehber.ModalIcerik;
                            if (!string.IsNullOrWhiteSpace(detay))
                            {
                                contextBuilder.AppendLine($"  Detay: {detay}");
                            }
                        }
                    }
                }
            }

            // 7. YÖNETİM (BAŞHEKİMLİK VE BAŞMÜDÜRLÜK) TABLOLARI
            if (ContainsAny(normalizedMessage, "başhekim", "bashekim", "müdür", "basmüdür", "basmudur", "yönetim", "yardımcı", "yardimci"))
            {
                var bashekimlik = await _context.BashekimlikPersoneller
                    .Where(x => x.AktifMi)
                    .OrderBy(x => x.Sira)
                    .ToListAsync();
                if (bashekimlik.Any())
                {
                    contextBuilder.AppendLine("Hastane Başhekimlik Yönetimi:");
                    var bashekim = bashekimlik.FirstOrDefault(x => x.IsBashekim);
                    if (bashekim != null)
                    {
                        contextBuilder.AppendLine($"- Başhekim: {bashekim.AdSoyad} ({bashekim.Unvan})");
                    }

                    var yardimcilar = bashekimlik.Where(x => !x.IsBashekim).ToList();
                    if (yardimcilar.Any())
                    {
                        contextBuilder.AppendLine("- Başhekim Yardımcıları:");
                        foreach (var kisi in yardimcilar)
                        {
                            contextBuilder.AppendLine($"  • {kisi.AdSoyad} ({kisi.Unvan})");
                        }
                    }
                }

                var basmudurluk = await _context.BasmudurlikPersoneller
                    .Where(x => x.AktifMi)
                    .OrderBy(x => x.Sira)
                    .ToListAsync();
                if (basmudurluk.Any())
                {
                    contextBuilder.AppendLine("Hastane Başmüdürlük Yönetimi:");
                    var basmudur = basmudurluk.FirstOrDefault(x => x.IsBasmudur);
                    if (basmudur != null)
                    {
                        contextBuilder.AppendLine($"- Başmüdür: {basmudur.AdSoyad} ({basmudur.Unvan})");
                    }

                    var yardimcilar = basmudurluk.Where(x => !x.IsBasmudur).ToList();
                    if (yardimcilar.Any())
                    {
                        contextBuilder.AppendLine("- Başmüdür Yardımcıları:");
                        foreach (var kisi in yardimcilar)
                        {
                            contextBuilder.AppendLine($"  • {kisi.AdSoyad} ({kisi.Unvan})");
                        }
                    }
                }
            }

            // 8. YEMEK LİSTESİ TABLOSU
            if (ContainsAny(normalizedMessage, "yemek", "menü", "kahvaltı", "öğle", "akşam", "bugün", "yarın"))
            {
                var hedefTarih = normalizedMessage.Contains("yarın") ? DateTime.Today.AddDays(1) : DateTime.Today;
                var yemekler = await _context.YemekListesi
                    .Where(y => y.Tarih.Date == hedefTarih.Date)
                    .ToListAsync();

                if (!yemekler.Any())
                {
                    yemekler = await _context.YemekListesi.OrderByDescending(y => y.Id).Take(3).ToListAsync();
                }

                if (yemekler.Any())
                {
                    contextBuilder.AppendLine("Yemek Listesi:");
                    foreach (var yemek in yemekler.OrderBy(y => y.Ogun))
                    {
                        var ogun = yemek.Ogun switch
                        {
                            1 => "Kahvaltı",
                            2 => "Öğle Yemeği",
                            3 => "Akşam Yemeği",
                            _ => "Öğün"
                        };
                        var menu = $"Çorba: {yemek.Corba}, Ana Yemek: {yemek.AnaYemek}, Yardımcı: {yemek.YardimciYemek}, Tatlı/Meyve: {yemek.TatliMeyve}";
                        contextBuilder.AppendLine($"- {yemek.Tarih:dd.MM.yyyy} {ogun} | Menü: {menu}");
                    }
                }
            }

            // 9. KURUMSAL SAYFALAR
            if (ContainsAny(normalizedMessage, "kurumsal", "hakkımızda", "vizyon", "misyon", "tarihçe"))
            {
                var kurumsal = await _context.KurumsalSayfalar.Take(5).ToListAsync();
                if (kurumsal.Any())
                {
                    contextBuilder.AppendLine("Hastanemiz Hakkında Kurumsal Bilgiler:");
                    foreach (var k in kurumsal)
                    {
                        contextBuilder.AppendLine($"- {k.Baslik}");
                        if (ContainsAny(normalizedMessage, "detay", "içerik", "açıklama") || terms.Any(t => k.Baslik.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                        {
                            if (!string.IsNullOrWhiteSpace(k.Icerik))
                            {
                                contextBuilder.AppendLine($"  Detay: {k.Icerik}");
                            }
                        }
                    }
                }
            }

            // 10. KROKİ / BİRİM YÖNLENDİRME (DÜZELTİLDİ)
            if (ContainsAny(normalizedMessage, "nerede", "kat", "blok", "kroki", "nasıl giderim", "yerleşim", "konum"))
            {
                var krokiBolumler = await _context.KrokiBolumler
                    .Include(b => b.Kat)
                    .ThenInclude(k => k.Blok)
                    .Take(50)
                    .ToListAsync();

                var ilgiliBirimler = krokiBolumler
                    .Where(b => terms.Any(t =>
                        (!string.IsNullOrWhiteSpace(b.BirimAdi) && b.BirimAdi.ToLower(new CultureInfo("tr-TR")).Contains(t)) ||
                        (!string.IsNullOrWhiteSpace(b.Kat?.KatAdi) && b.Kat.KatAdi.ToLower(new CultureInfo("tr-TR")).Contains(t)) ||
                        (!string.IsNullOrWhiteSpace(b.Kat?.Blok?.BlokAdi) && b.Kat.Blok.BlokAdi.ToLower(new CultureInfo("tr-TR")).Contains(t))))
                    .ToList();

                if (ilgiliBirimler.Any())
                {
                    contextBuilder.AppendLine("Yerleşim Bilgileri:");
                    foreach (var birim in ilgiliBirimler)
                    {
                        var blok = birim.Kat?.Blok?.BlokAdi ?? "Bilinmiyor";
                        var kat = birim.Kat?.KatAdi ?? "Bilinmiyor";
                        contextBuilder.AppendLine($"- {birim.BirimAdi} | Blok: {blok} | Kat: {kat}");
                    }
                }
                else if (krokiBolumler.Any())
                {
                    contextBuilder.AppendLine("Hastane İçi Birimlerin Konumları (Kroki):");
                    foreach (var birim in krokiBolumler.Take(10))
                    {
                        var blok = birim.Kat?.Blok?.BlokAdi ?? "Bilinmiyor";
                        var kat = birim.Kat?.KatAdi ?? "Bilinmiyor";
                        contextBuilder.AppendLine($"- {birim.BirimAdi} | Blok: {blok} | Kat: {kat}");
                    }
                }
            }

            if (ContainsAny(normalizedMessage, "kalite", "belge", "eğitim", "komite"))
            {
                var kaliteBelgeleri = await _context.KaliteBelgeleri.Take(5).ToListAsync();
                if (kaliteBelgeleri.Any())
                {
                    contextBuilder.AppendLine("Kalite Yönetim Bilgileri:");
                    foreach (var belge in kaliteBelgeleri)
                    {
                        contextBuilder.AppendLine($"- {belge.BelgeAdi}");
                    }
                }

                var egitimIcerikleri = await _context.EgitimIcerikleri.Take(5).ToListAsync();
                if (egitimIcerikleri.Any())
                {
                    contextBuilder.AppendLine("Eğitim Komitesi İçerikleri:");
                    foreach (var egitim in egitimIcerikleri)
                    {
                        contextBuilder.AppendLine($"- {egitim.Baslik}");
                        if (ContainsAny(normalizedMessage, "detay", "içerik", "açıklama") || terms.Any(t => egitim.Baslik.ToLower(new CultureInfo("tr-TR")).Contains(t)))
                        {
                            if (!string.IsNullOrWhiteSpace(egitim.Icerik))
                            {
                                contextBuilder.AppendLine($"  Detay: {egitim.Icerik}");
                            }
                        }
                    }
                }
            }

            // HİÇBİR EŞLEŞME YOKSA
            if (contextBuilder.Length == 0)
            {
                contextBuilder.AppendLine("Gazi Hastanesi dijital asistanına hoş geldiniz. Doktorlarımız, bölümlerimiz, ulaşım, etkinlikler ve hastane işleyişimiz hakkında bana her şeyi sorabilirsiniz.");
            }

            return contextBuilder.ToString();
        }

        private static string NormalizeMessage(string message)
        {
            return message.ToLower(new CultureInfo("tr-TR"));
        }

        private static IEnumerable<string> ExtractTerms(string message)
        {
            return message
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.Trim().Trim(',', '.', '?', '!', ':', ';', '-', '"', '\''))
                .Where(term => term.Length > 2)
                .Distinct();
        }

        private static bool ContainsAny(string message, params string[] keywords)
        {
            return keywords.Any(message.Contains);
        }

        private static bool IsSensitiveRequest(string normalizedMessage)
        {
            return ContainsAny(normalizedMessage, "tahlil", "sonuç", "randevu", "ödeme", "borç", "protokol", "tc", "kimlik", "şifre", "parola");
        }

        private static List<ActionLink> BuildSensitiveActions(string normalizedMessage)
        {
            var actions = new List<ActionLink>();

            if (ContainsAny(normalizedMessage, "randevu"))
            {
                actions.Add(new ActionLink("Randevu Paneli", "/Randevu/Giris"));
            }

            if (ContainsAny(normalizedMessage, "tahlil", "sonuç"))
            {
                actions.Add(new ActionLink("Tahlil Sonuçları", "/Sonuc/Giris"));
            }

            if (ContainsAny(normalizedMessage, "ödeme", "borç", "protokol"))
            {
                actions.Add(new ActionLink("Ödeme Paneli", "/Odeme/Giris"));
            }

            if (!actions.Any())
            {
                actions.AddRange(new[]
                {
                    new ActionLink("Randevu Paneli", "/Randevu/Giris"),
                    new ActionLink("Tahlil Sonuçları", "/Sonuc/Giris"),
                    new ActionLink("Ödeme Paneli", "/Odeme/Giris")
                });
            }

            return actions;
        }

        private async Task<string?> BuildMealResponse(string normalizedMessage)
        {
            if (!ContainsAny(normalizedMessage, "yemek", "menü", "kahvaltı", "kahvaltida", "kahvaltıda", "öğle", "ogle", "akşam", "aksam", "bugün", "bugun", "yarın", "yarin"))
            {
                return null;
            }

            var hedefTarih = ContainsAny(normalizedMessage, "yarın", "yarin") ? DateTime.Today.AddDays(1) : DateTime.Today;
            var yemekler = await _context.YemekListesi
                .Where(y => y.Tarih.Date == hedefTarih.Date)
                .OrderBy(y => y.Ogun)
                .ToListAsync();

            if (!yemekler.Any())
            {
                return "Bu tarih için yemek listesi bulunamadı.";
            }

            var sadeceKahvalti = ContainsAny(normalizedMessage, "kahvaltı", "kahvaltida", "kahvaltıda");
            var sadeceOgle = ContainsAny(normalizedMessage, "öğle", "ogle");
            var sadeceAksam = ContainsAny(normalizedMessage, "akşam", "aksam");

            var cevap = new StringBuilder();
            cevap.AppendLine($"{hedefTarih:dd.MM.yyyy} tarihli yemek listesi:");

            foreach (var yemek in yemekler)
            {
                if (sadeceKahvalti && yemek.Ogun != 1) continue;
                if (sadeceOgle && yemek.Ogun != 2) continue;
                if (sadeceAksam && yemek.Ogun != 3) continue;

                var ogun = yemek.Ogun switch
                {
                    1 => "Kahvaltı",
                    2 => "Öğle Yemeği",
                    3 => "Akşam Yemeği",
                    _ => "Öğün"
                };
                var menu = string.Join(", ", new[] { yemek.Corba, yemek.AnaYemek, yemek.YardimciYemek, yemek.TatliMeyve }.Where(s => !string.IsNullOrWhiteSpace(s)));
                cevap.AppendLine($"- {ogun}: {menu}");
            }

            return cevap.ToString().Trim();
        }

        private sealed record ActionLink(string Title, string Url);

        private async Task<List<object>> BuildHizliIslemActions(string normalizedMessage)
        {
            var actions = new List<object>();
            var hizliIslemler = await _context.HizliIslemler
                .Where(x => x.IsActive)
                .OrderBy(x => x.SiraNo)
                .ToListAsync();

            foreach (var hizli in hizliIslemler)
            {
                var baslik = NormalizeMessage(hizli.Baslik);
                if (baslik.Split(' ', StringSplitOptions.RemoveEmptyEntries).Any(normalizedMessage.Contains))
                {
                    actions.Add(new { title = hizli.Baslik.Replace("<br>", " "), url = hizli.Url });
                }
            }

            if (!actions.Any() && ContainsAny(normalizedMessage, "mail", "e-posta", "eposta"))
            {
                actions.Add(new { title = "Gazi Mail", url = "/Mail/Giris" });
            }

            if (!actions.Any() && ContainsAny(normalizedMessage, "ebys"))
            {
                actions.Add(new { title = "EBYS", url = "/EBYS/Giris" });
            }

            return actions;
        }

        private async Task<List<object>> BuildContextualActions(string normalizedMessage)
        {
            var actions = new List<object>();

            if (ContainsAny(normalizedMessage, "konum", "harita", "ulaşım", "adres"))
            {
                var iletisim = await _context.IletisimBilgileri.FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(iletisim?.HaritaUrl))
                {
                    actions.Add(new { title = "Haritayı Aç", url = iletisim.HaritaUrl });
                }
            }

            return actions;
        }
    }

    // İstek Modeli
    public class ChatRequest
    {
        public string UserMessage { get; set; } = string.Empty;
    }
}


//✅ Bölümler - Blok ve kat bilgileri ile birlikte gösteriliyor
//✅ Doktorlar - Tam detaylı bilgilerle listeleniyor
//✅ Birim Doktorları - Bölüm sorulduğunda o bölümün doktorları gösteriliyor
//✅ Hasta Rehberi - Tüm detaylar (ModalIcerik ve Icerik) gösteriliyor
//✅ İletişim - Tüm telefon numaraları, e-posta, adres bilgileri veriliyor
//✅ Konum/Harita - Konum bilgisi + harita linki ile yönlendirme
//✅ Ulaşım Rehberi - Tüm ulaşım seçenekleri detaylı gösteriliyor
//✅ Randevu/Tahlil/Ödeme - Güvenli şekilde ilgili panellere yönlendirme
//✅ Bölüm-Blok-Kat - Kroki bilgileri ile yerleşim detayları
//✅ HBYS/EBYS/Mail/Uluslararası/Emzirme - Hızlı işlemlerle yönlendirme
//✅ Eğitim ve Kalite Komiteleri - Tüm içerikler analiz edilerek cevap veriliyor
//✅ Haber/Etkinlik/Duyurular - Son 5 içerik detaylıca gösteriliyor