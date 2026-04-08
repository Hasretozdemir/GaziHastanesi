using System.ComponentModel.DataAnnotations;

namespace GaziHastane.Models
{
    public class DoktorRandevuPlanViewModel
    {
        public int DoktorId { get; set; }
        public int? BolumId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }

        [Range(5, 120)]
        public int SlotSureDakika { get; set; } = 30;

        public string BaslangicSaati { get; set; } = "09:00";
        public string BitisSaati { get; set; } = "17:00";
        public string OgleMolaBaslangicSaati { get; set; } = "12:00";
        public string OgleMolaBitisSaati { get; set; } = "13:00";

        [Range(1, 500)]
        public int VarsayilanGunlukMaxRandevu { get; set; } = 20;

        public List<DoktorRandevuGunSatirViewModel> Gunler { get; set; } = new();
    }

    public class DoktorRandevuGunSatirViewModel
    {
        public int? PlanGunId { get; set; }
        public DateTime Tarih { get; set; }
        public bool IsRandevuAcik { get; set; }
        public int GunlukMaxRandevu { get; set; } = 20;
        public string? BaslangicSaati { get; set; }
        public string? BitisSaati { get; set; }
    }
}
