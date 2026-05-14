# 🏥 Gazi Üniversitesi Hastanesi - Profesyonel Sağlık Yönetim Sistemi

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core%208.0-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/aspnet)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)
[![Gemini AI](https://img.shields.io/badge/Gemini%20AI-4285F4?style=for-the-badge&logo=google&logoColor=white)](https://ai.google.dev/)

Gazi Üniversitesi Hastanesi için geliştirilmiş, modern web teknolojileri ve yapay zeka entegrasyonu ile donatılmış kapsamlı bir hastane yönetim ve portal sistemidir. Bu proje, hem hastalar için kullanıcı dostu bir arayüz hem de hastane personeli için güçlü bir yönetim paneli sunar.

---

## 📺 Proje Tanıtım Videosu

> [!TIP]
> Projenin tüm özelliklerini ve çalışma mantığını aşağıdaki videodan izleyebilirsiniz:

<div align="center">
  <a href="https://youtu.be/JSDaG0zK1kY" target="_blank">
    <img src="https://img.youtube.com/vi/JSDaG0zK1kY/maxresdefault.jpg" alt="Proje Tanıtım Videosu" width="100%">
  </a>
  <p><i>▶️ Proje Tanıtım Videosunu İzlemek İçin Görsele Tıklayın</i></p>
</div>

---

## ✨ Öne Çıkan Özellikler

### 🤖 Gazi Asistan (AI Chatbot)
Google'ın **Gemini 1.5 Flash** modeli ile entegre çalışan akıllı asistan.
- Tıbbi terim açıklamaları ve genel sağlık bilgilendirmesi.
- Hastane birimleri, doktor çalışma saatleri ve ulaşım rehberliği.
- Gerçek zamanlı etkileşimli sohbet deneyimi.

### 📅 Akıllı Randevu Sistemi
- **Dinamik Takvim:** Doktorların çalışma planına göre gerçek zamanlı uygunluk kontrolü.
- **Kişiselleştirilmiş Deneyim:** TCKN ve doğum tarihi ile güvenli giriş.
- **QR Kodlu Randevu Fişi:** Oluşturulan randevular için anında QR kodlu PDF/Görsel fiş üretimi ve konum rehberliği.
- **Güvenlik:** Geçmiş tarihlere veya dolu saatlere randevu alınmasını engelleyen gelişmiş doğrulama.

### 🏢 Kurumsal ve Birim Yönetimi
- **Genişletilebilir Yapı:** Enfeksiyon, Eczacılık, Kalite gibi 20'den fazla kurumsal birim sayfası.
- **Dinamik İçerik:** Haberler, Duyurular ve Etkinlikler modülü.
- **Kroki ve Ulaşım:** Hastane içi birimlere kolay ulaşım için interaktif kroki sistemi.

### 🔐 Gelişmiş Yönetim Paneli (Admin)
- **Role-Based Access Control:** Yetkilendirilmiş personel girişi.
- **Full CRUD:** Doktorlar, bölümler, haberler ve ayarlar üzerinde tam kontrol.
- **Planlama:** Doktorlar için aylık/günlük randevu slotları oluşturma ve kapasite yönetimi.
- **Sistem Logları:** Tüm kritik işlemlerin takibi ve denetimi.

---

## 🛠️ Teknik Altyapı

| Teknoloji | Kullanım Amacı |
| :--- | :--- |
| **ASP.NET Core 8.0 MVC** | Ana Uygulama Çerçevesi |
| **PostgreSQL** | Veri Tabanı Yönetimi |
| **Entity Framework Core** | ORM ve Veri Erişimi |
| **Tailwind CSS** | Modern ve Responsive Arayüz Tasarımı |
| **JavaScript (Vanilla/ES6)** | Dinamik UI ve API Etkileşimleri |
| **QRCoder** | Randevu Fişleri İçin QR Kod Üretimi |
| **SweetAlert2** | Şık ve Kullanıcı Dostu Uyarı Mesajları |
| **Animate.css** | UI Animasyonları ve Geçiş Efektleri |

---

## 🚀 Kurulum ve Çalıştırma

1. **Repository'yi Klonlayın:**
   ```bash
   git clone https://github.com/Hasretozdemir/GaziHastanesi.git
   ```

2. **Veritabanı Ayarlarını Yapın:**
   `appsettings.json` dosyasındaki `DefaultConnection` bilgisini kendi PostgreSQL ayarlarınıza göre güncelleyin.

3. **Migrations Uygulayın:**
   ```bash
   dotnet ef database update
   ```

4. **Projeyi Başlatın:**
   ```bash
   dotnet run
   ```

---

## 📸 Ekran Görüntüleri

| Ana Sayfa | Randevu Sistemi | Admin Paneli |
| :---: | :---: | :---: |
| ![Ana Sayfa](https://raw.githubusercontent.com/Hasretozdemir/GaziHastanesi/main/screenshots/home.png) | ![Randevu](https://raw.githubusercontent.com/Hasretozdemir/GaziHastanesi/main/screenshots/appointment.png) | ![Admin](https://raw.githubusercontent.com/Hasretozdemir/GaziHastanesi/main/screenshots/admin.png) |

---

## 🤝 İletişim ve Geliştirici

**Hasret Özdemir**
- [GitHub](https://github.com/Hasretozdemir)
- [LinkedIn](https://linkedin.com/in/hasretozdemir)

<div align="center">
<br/>
<i>Gazi Üniversitesi Hastanesi © 2026 - Geleceğin Sağlık Teknolojileri</i>
</div>
