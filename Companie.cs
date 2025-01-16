namespace ProiectOOP;

public class Companie
{
    private readonly ILogger _logger;
    public string nume;
    public string adresa;
    public int cod;
    public List<Masini> flotaMasini;
    public string admin;
    public int index;
    public List<Client> listaClienti;
    public List<Inchirieri> istoricInchirieri;


    public Companie()
    {
        _logger = new ErrorLogger();
        listaClienti = new List<Client>();
        flotaMasini = new List<Masini>();
        istoricInchirieri = new List<Inchirieri>();
    }

    public Companie(string nume, string adresa, int cod, string admin)
    {
        _logger = new ErrorLogger();
        this.nume = nume;
        this.adresa = adresa;
        this.cod = cod;
        flotaMasini = new List<Masini>();
        this.admin = admin;
        listaClienti = new List<Client>();
        istoricInchirieri = new List<Inchirieri>();
    }

    public void AdaugaMasina(Masini masini) // alexa
    {
        try
        {
            flotaMasini.Add(masini);
            FisiereOperatii.SaveState(this); // fisier + logger aris
        }
        catch (Exception e)
        {
            _logger.Log($"Eroare la adaugare de masini: {e.Message}");
            throw;
        }
        
    }
    public void StergereMasini(int index)
    {
        try
        {
            if (index >= 0 && index < flotaMasini.Count)
            {
                flotaMasini.RemoveAt(index);
                FisiereOperatii.SaveState(this);
            }
            else
            {
                _logger.Log($"Invalid index pentru stergere masina: {index}");
            }
        }
        catch (Exception e)
        {
            _logger.Log($"Erroare la stergere masina: {e.Message}");
            throw;
        }
    }

    public void AdaugaClienti(Client client) // alexa
    {
        listaClienti.Add(client);
        FisiereOperatii.SaveState(this);
    }
    
    public void InchiriazaMasinaClient(Client client, int indexMasina , int durata , DateTime dataStart)
    {
        try
        {
            if (client.AreDaune())
            {
                Console.WriteLine($"Clientul {client.nume} nu poate sa inchirieze masina deoarece in istoricul sau are daune");
                return;
            }

            if (indexMasina >= 0 && indexMasina < flotaMasini.Count)
            {
                var masina = flotaMasini[indexMasina];
                if (masina.disponibila)
                {
                    client.InchiriazaMasina(client, masina, durata, dataStart);
                    var inchiriere = new Inchirieri(dataStart , dataStart.AddDays(durata) , durata);
                    istoricInchirieri.Add(inchiriere);
                    FisiereOperatii.SaveState(this);
                }
                else
                {
                    Console.WriteLine("Masina selectată este deja închiriată.");
                    _logger.Log($"Masina {masina.marca} {masina.model}");
                }
            }
            else
            {
                Console.WriteLine("Index invalid al mașinii.");
                _logger.Log($"Index invalid: {indexMasina}");
            }
        }
        catch (Exception ex)
        {
            _logger.Log($"Erroare la inchiriere: {ex.Message}");
        }
    }

    public void ReturnareMasinaClient(Client client , int indexMasina , bool daune)
    {
        if (indexMasina >= 0 && indexMasina < client.listaInchirieri.Count)
        {
            var masina = flotaMasini[indexMasina];
            var inchiriere = istoricInchirieri.FindLast(i => i.masinaClient == masina && !masina.disponibila);
            //var inchiriere = client.listaInchirieri[indexMasina];

            if (inchiriere != null)
            {
                client.ReturnareMasina(inchiriere.masinaClient, daune);
                inchiriere.areDaune = daune;
                FisiereOperatii.SaveState(this);
            }
            else
            {
                Console.WriteLine("Nu s-a gasit inchiriere");
            }
        }
        else
        {
            Console.WriteLine("Index invalid");
        }
    }

    public void AfiseazaMasinaInchiriateClient(string username) // alexa
    {
        var client = listaClienti.Find(c => c.username == username);
        if (client != null)
        {
            Console.WriteLine($"Istoric inchirieri pt clientul cu username-ul {client.username}");
            client.AfiseazaMasiniInchiriate();
        }
        else
        {
            Console.WriteLine("Nu s-a gasit clientul cu username-ul acesta!");
        }
    }

    public void AfiseazaIstoricInchirieri()
    {
        if (listaClienti == null || listaClienti.Count == 0)
        {
            Console.WriteLine("Nu exista clienti inregistrati in sistem!");
        }

        Console.WriteLine("=== Istoric Inchirieri ====");
        bool existaInchirieri = false;

        foreach (var client in listaClienti)
        {
            if (client.listaInchirieri != null && client.listaInchirieri.Count > 0)
            {
                existaInchirieri = true;
                Console.WriteLine($"Client: {client.nume} cu username-ul:{client.username}");

                foreach (var inchiriere in client.listaInchirieri)
                {
                    Console.WriteLine($"- Mașina: {inchiriere.masinaClient.marca} {inchiriere.masinaClient.model}");
                    Console.WriteLine($"  Nr. Înmatriculare: {inchiriere.masinaClient.nrInmatriculare}");
                    Console.WriteLine($"  Data început: {inchiriere.dataStart:dd/MM/yyyy}");
                    Console.WriteLine($"  Data sfârșit: {inchiriere.dataStop:dd/MM/yyyy}");
                    Console.WriteLine($"  Durată: {inchiriere.durata} zile");
                    Console.WriteLine($"  Cost total: {inchiriere.masinaClient.CalcCost()} RON");
                    Console.WriteLine($"  Status daune: {(inchiriere.areDaune ? "DA" : "NU")}");
                }
            }
        }

        if (!existaInchirieri)
        {
            Console.WriteLine("Nu exista inchirieri inregistrate in sistem!");
        }
        
    }

    public double CalcCastiguri(DateTime data)
    {
        double castiguri = 0;
        bool existaInchirieri = false;
        foreach (var client in listaClienti)
        {
            foreach (var inchirieri in client.listaInchirieri)
            {
                if (data >= inchirieri.dataStart && data <= inchirieri.dataStop)
                {
                    existaInchirieri = true;
                    castiguri += inchirieri.masinaClient.CalcCost();
                    Console.WriteLine($"Inchiriere activa pentru {client.nume}");
                    Console.WriteLine($"Cost pe zi: {inchirieri.masinaClient.CalcCost()} RON");
                }
            }
        }

        if (!existaInchirieri)
        {
            Console.WriteLine("Nu exista Inchirieri inregistrate in sistem!");
            return 0;
        }
        return castiguri;
    }

    public void AfiseazaCastiguri(DateTime data) 
    {
        double castiguri = CalcCastiguri(data);
        if (castiguri > 0)
        {
            Console.WriteLine($"Castigurile pentru data {data:dd/MM/yyyy} sunt: {castiguri} RON");
        }
        else
        {
            Console.WriteLine($"Nu exista castiguri pentru data {data:dd/MM/yyyy}");
        }
    }

    public void AfiseazaMasiniInchiriate() //  metoda afisare masini inchiriate alexa 
    {
        foreach (var m in flotaMasini)
        {
            if (m.disponibila == false) 
            {
                Console.Write($"{flotaMasini.IndexOf(m)}."); // sa-i arate indexu in fata
                m.AfiseazaDetalii();
            }
        }
    }
    
     public void AfiseazaMasini() // alexa
    {
        foreach (var m in flotaMasini)
        {
            if (m.disponibila == true)
            {
                Console.Write($"{flotaMasini.IndexOf(m)}.");
                m.AfiseazaDetalii();
            }
        }
    } 
    
}