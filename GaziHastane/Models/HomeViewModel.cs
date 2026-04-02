using System.Collections.Generic;

namespace GaziHastane.Models
{
    public class HomeViewModel
    {
        // View'a gönderilecek listeler
        public IEnumerable<Haber> Haberler { get; set; }
        public IEnumerable<Etkinlik> Etkinlikler { get; set; }
        public IEnumerable<Duyuru> Duyurular { get; set; }
        public List<HizliIslem> HizliIslemler { get; set; }
        public List<Medya> SliderGorselleri { get; set; }
    }
}