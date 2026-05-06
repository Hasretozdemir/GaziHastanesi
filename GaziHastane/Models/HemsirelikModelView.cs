using System.Collections.Generic;

namespace GaziHastane.Models
{
    public class HemsirelikViewModel
    {
        public HemsirelikAyar Ayarlar { get; set; }
        public List<HemsirelikSekme> Sekmeler { get; set; }
        public List<HemsirelikIcerik> YonetimKadrosu { get; set; }
        public List<HemsirelikIcerik> Gorevler { get; set; }
        public List<HemsirelikIcerik> Mevzuatlar { get; set; }
        public List<HemsirelikIcerik> GaleriFotograflari { get; set; }
        public List<HemsirelikIcerik> Etkinlikler { get; set; }
        public List<HemsirelikIcerik> AkisSemalari { get; set; }
        public List<HemsirelikIcerik> TumIcerikler { get; set; }
    }
}
