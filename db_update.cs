using System;
using Npgsql;

class Program
{
    static void Main()
    {
        string connStr = "Host=localhost;Database=GaziHastane;Username=postgres;Password=1123";
        using (var conn = new NpgsqlConnection(connStr))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("INSERT INTO \"AdminMenuItems\" (\"Section\", \"Url\", \"Label\", \"IconClass\", \"SortOrder\", \"IsSuperAdminOnly\", \"IsActive\") VALUES ('Main', '/Admin/BorcYonetim/Index', 'Borç Yönetimi', 'fa-solid fa-money-bill-wave', 99, false, true);", conn))
            {
                try { cmd.ExecuteNonQuery(); } catch {}
            }
            Console.WriteLine("Menu eklendi.");
        }
    }
}
