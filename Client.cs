using System.Threading.Channels;

namespace ProiectOOP;

public class Client
{
    public string nume;
    public string CNP;
    public string username;
    public string password;
    public List<Inchirieri> listaInchirieri;
    public List<string> istoricDaune;

    public Client()
    {
        listaInchirieri = new List<Inchirieri>();
        istoricDaune = new List<string>();
    }

    public Client(string nume, string CNP, string username, string password)
    {
        this.nume = nume;
        this.CNP = CNP;
        this.username = username;
        this.password = password;
        this.listaInchirieri = new List<Inchirieri>();
        this.istoricDaune = new List<string>();
    }

    public bool AreDaune()
    {
        return istoricDaune.Count > 0;
    }

    public static bool ValidareCnp(string CNP)
    {
        if (CNP.Length != 13)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public void InchiriazaMasina(Client client ,Masini masina, int durata , DateTime dataStart)
    {
        if (istoricDaune != null && istoricDaune.Count > 0)
        {
            Console.WriteLine($"Clientul {client.nume} nu poate inchiria masina deoarece are daune in istoricul sau");
            return;
        }
        
        if (masina == null)
        {
            Console.WriteLine("Nu poate fii null!");
        }

        if (durata <= 0)
        {
            Console.WriteLine("Durata trebuie sa fie mai mare decat 0");
            return;
        }

        if (masina.disponibila)
        {
            masina.disponibila = false; // Marcăm mașina ca indisponibilă
            masina.nrzile = durata;
            
            
            var inchiriere = new Inchirieri(dataStart,dataStart.AddDays(durata) , durata);
            inchiriere.masinaClient = masina; // Asociem mașina închiriată
            listaInchirieri.Add(inchiriere);
            
            Console.WriteLine($"Masina {masina.marca} {masina.model} a fost închiriată pentru {durata} zile.");
        }
        else
        {
            Console.WriteLine("Masina selectată nu este disponibilă pentru închiriere.");
        }
    }


    public  void ReturnareMasina(Masini masina, bool daune)
         {
             var inchiriere = listaInchirieri.Find(i => i.masinaClient == masina);
     
             if (inchiriere != null)
             {
                 listaInchirieri.Remove(inchiriere);
                 masina.disponibila = true;
                 Console.WriteLine($"Masina {masina.marca} {masina.model} a fost returnata");
                 if (daune)
                 {
                     inchiriere.areDaune = true;
                     
                     istoricDaune.Add($"Daune la {masina.marca} {masina.model} ({masina.nrInmatriculare})");
                 }
                 else
                 {
                     inchiriere.areDaune = false;
                 }
             }
             else
             {
                 Console.WriteLine("Nu a fost gasita!");
             }
         }
    
    public void AfiseazaMasiniInchiriate()
    {
        if (listaInchirieri == null || listaInchirieri.Count == 0)
        {
            Console.WriteLine("Nu exista inchirieri pentru acest client");
            return;
        }
        else
        {
            foreach (var inchirie in listaInchirieri)
            {
                
                    Console.WriteLine($"Masina {inchirie.masinaClient.marca} {inchirie.masinaClient.model} , cu nr inmatriculare {inchirie.masinaClient.nrInmatriculare}");
                    Console.WriteLine($"Durata: {inchirie.durata}");
                    Console.WriteLine($"Pret: {inchirie.masinaClient.CalcCost()}");
                
            }
        }
    }
    
}