using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProiectOOP;

// Convertor custom pentru a gestiona serializarea claselor derivate
public class MasiniConverter : JsonConverter<Masini>
{
    public override Masini Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var tipMasina = root.GetProperty("TipMasina").GetString();
            
            var nrzile = root.GetProperty("nrzile").GetDouble();
            var consum = root.GetProperty("consum").GetString();
            var cost = root.GetProperty("cost").GetDouble();
            var marca = root.GetProperty("marca").GetString();
            var model = root.GetProperty("model").GetString();
            var an = root.GetProperty("an").GetInt32();
            var km = root.GetProperty("km").GetDouble();
            var nrInmatriculare = root.GetProperty("nrInmatriculare").GetString();
            var disponibila = root.GetProperty("disponibila").GetBoolean();

            if (tipMasina == "Standard")
            {
                return new Standard(nrzile, consum, cost, marca, model, an, km, nrInmatriculare, disponibila);
            }
            else if (tipMasina == "Electric")
            {
                return new Electric(nrzile, consum, cost, marca, model, an, km, nrInmatriculare, disponibila);
            }
            
            throw new JsonException("Tip masina necunoscut");
        }
    }

    public override void Write(Utf8JsonWriter writer, Masini value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        // Scriem tipul mașinii pentru a ști ce să deserializăm
        writer.WriteString("TipMasina", value.GetType().Name);
        
        // Scriem toate proprietățile comune
        writer.WriteNumber("nrzile", value.nrzile);
        writer.WriteString("consum", value.consum);
        writer.WriteNumber("cost", value.cost);
        writer.WriteString("marca", value.marca);
        writer.WriteString("model", value.model);
        writer.WriteNumber("an", value.an);
        writer.WriteNumber("km", value.km);
        writer.WriteString("nrInmatriculare", value.nrInmatriculare);
        writer.WriteBoolean("disponibila", value.disponibila);

        writer.WriteEndObject();
    }
}

// Clasa pentru a gestiona salvarea și încărcarea stării
public static class FisiereOperatii
{
    private const string FilePath = "company_state.json";
    
    private static JsonSerializerOptions GetSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Converters = { new MasiniConverter() }
        };
    }

    // Clasă pentru a stoca starea companiei într-un format serializabil
    private class CompanieState
    {
        public string Nume { get; set; }
        public string Adresa { get; set; }
        public int Cod { get; set; }
        public string Admin { get; set; }
        public List<Masini> FlotaMasini { get; set; }
        public List<Client> ListaClienti { get; set; }
        public List<Inchirieri> IstoricInchirieri { get; set; }
    }

    public static void SaveState(Companie companie)
    {
        try
        {
            var state = new CompanieState
            {
                Nume = companie.nume,
                Adresa = companie.adresa,
                Cod = companie.cod,
                Admin = companie.admin,
                FlotaMasini = companie.flotaMasini,
                ListaClienti = companie.listaClienti,
                IstoricInchirieri = companie.istoricInchirieri
            };

            string jsonString = JsonSerializer.Serialize(state, GetSerializerOptions());
            File.WriteAllText(FilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la salvarea stării: {ex.Message}");
            throw;
        }
    }

    public static Companie LoadState()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            string jsonString = File.ReadAllText(FilePath);
            var state = JsonSerializer.Deserialize<CompanieState>(jsonString, GetSerializerOptions());

            var companie = new Companie(state.Nume, state.Adresa, state.Cod, state.Admin);
            companie.flotaMasini = state.FlotaMasini ?? new List<Masini>();
            companie.listaClienti = state.ListaClienti ?? new List<Client>();
            companie.istoricInchirieri = state.IstoricInchirieri ?? new List<Inchirieri>();

            return companie;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la încărcarea stării: {ex.Message}");
            return null;
        }
    }
}