namespace GaziHastane.Models
{
    // ErrorViewModel: Hata sayfasýnda (Error.cshtml) görüntülenecek verileri taþýyan sýnýftýr.
    public class ErrorViewModel
    {
        // RequestId: Her bir web isteðine sistem tarafýndan atanan benzersiz kimlik numarasýdýr.
        // Hata oluþtuðunda bu ID üzerinden log kayýtlarýnda inceleme yapýlabilir.
        // 'string?' ifadesi bu alanýn boþ (null) olabileceðini belirtir.
        public string? RequestId { get; set; }

        // ShowRequestId: Hata sayfasýnda RequestId'nin gösterilip gösterilmeyeceðine karar veren mantýksal alandýr.
        // Eðer RequestId doluysa (boþ veya null deðilse) true deðerini döndürür.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}