using System.Text.Json;

namespace ProiectOOP;

public static class ContClientOperatii
{
    private const string ClientiFilePath = "client_accounts.json";
    
    private class ClientAccount
    {
        
        public string Nume { get; set; }
        public string CNP { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public static void SalveazaConturi(List<Client> clienti)
    {
        try
        {
            var clientAccounts = clienti.Select(c => new ClientAccount
            {
                Nume = c.nume,
                CNP = c.CNP,
                Username = c.username,
                Password = c.password
            }).ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            string jsonString = JsonSerializer.Serialize(clientAccounts, options);
            File.WriteAllText(ClientiFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la salvarea conturilor: {ex.Message}");
            throw;
        }
    }
    
    public static List<Client> IncarcaConturi()
    {
        try
        {
            if (!File.Exists(ClientiFilePath))
            {
                return new List<Client>();
            }

            string jsonString = File.ReadAllText(ClientiFilePath);
            var clientAccounts = JsonSerializer.Deserialize<List<ClientAccount>>(jsonString);

            return clientAccounts.Select(ca => new Client(
                ca.Nume,
                ca.CNP,
                ca.Username,
                ca.Password
            )).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la încărcarea conturilor: {ex.Message}");
            return new List<Client>();
        }
    }
}