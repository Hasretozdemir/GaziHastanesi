namespace GaziHastane.E2ETests;

public static class TestSettings
{
    public static string BaseUrl => Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "https://localhost:7028";
    public static string AdminEmail => Environment.GetEnvironmentVariable("E2E_ADMIN_EMAIL") ?? "hasret@gazihastanesi.com";
    public static string AdminPassword => Environment.GetEnvironmentVariable("E2E_ADMIN_PASSWORD") ?? "12345";
}
