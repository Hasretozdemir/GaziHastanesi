using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        string path = @"c:\Users\LENOVO\Desktop\c# 2. sınıf\GaziHastane\GaziHastane\Models\DbEntities.cs";
        string content = File.ReadAllText(path);
        
        if (content.Contains("KrokiBlok")) {
            Console.WriteLine("Already updated.");
            return;
        }

        string newModels = @"public class KrokiBlok
    {
        public int Id { get; set; }
        public string BlokAdi { get; set; }
        public string Renk { get; set; }
        public System.Collections.Generic.ICollection<KrokiKat> Katlar { get; set; }
    }

    public class KrokiKat
    {
        public int Id { get; set; }
        public string KatAdi { get; set; }
        public int BlokId { get; set; }
        public KrokiBlok Blok { get; set; }
        public System.Collections.Generic.ICollection<KrokiBirim> Birimler { get; set; }
    }

    public class KrokiBirim
    {
        public int Id { get; set; }
        public string BirimAdi { get; set; }
        public int KatId { get; set; }
        public KrokiKat Kat { get; set; }
        public string Ikon { get; set; }
        public bool IsEmpty { get; set; }

        public int? BolumId { get; set; }
        public Bolum Bolum { get; set; }
    }";
        
        content = Regex.Replace(content, @"public\s+class\s+KrokiBirim\s*\{(?:[^{}]|(?<o>\{)|(?<-o>\}))*(?(o)(?!))\}", newModels);
        
        File.WriteAllText(path, content, new System.Text.UTF8Encoding(false));
        Console.WriteLine("Models Updated!");
    }
}
