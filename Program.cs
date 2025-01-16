using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Xml;
namespace ProiectOOP;

class Program
{
    static void Main(string[] args)
    {
        ILogger logger = new ErrorLogger();
        IConsole Console = new ConsoleWrapper();
        IFileSystem fileSystem = new FileSystemWrapper();
        
        try
        {
            var Companie = FisiereOperatii.LoadState();

            if (Companie == null)
            {
                Companie = new Companie("OSF Company", "Str Poraisles nr.2", 422, "Marius Bicatenar");
                Companie.AdaugaMasina(new Standard(7, "Normal", 150, "OPEL", "Astra", 2010, 30000, "TM-21-OSF", true));
                Companie.AdaugaMasina(new Standard(7, "Normal", 1000, "BMW", "Seria 4", 2017, 20000, "TM-23-OSF", true));
                Companie.AdaugaMasina(new Electric(5, "Redus", 1200, "Hyundai", "I30", 2020, 10000, "TM-24-OSF", true));
                Companie.AdaugaMasina(new Electric(5, "Redus", 1500, "Tesla", "Model S", 2021, 5000, "TM-25-OSF", true));

                FisiereOperatii.SaveState(Companie);
            }

            if (Companie.flotaMasini == null)
            {
                Companie.flotaMasini = new List<Masini>();
            }

            if (Companie.listaClienti == null)
            {
                Companie.listaClienti = ContClientOperatii.IncarcaConturi();
            }

            bool esteAdminConectat = false;
            bool esteClient = false;
            Client clientCurent = null;



            while (true)
            {
                Console.WriteLine("1.Vizualizare mașini disponibile pentru închiriat");
                Console.WriteLine("2.Autentificare/Inregistrare");
                if (esteAdminConectat)
                {
                    Console.WriteLine("3.Adauga Masina(ADMIN)");
                    Console.WriteLine("4.Stergere masina(ADMIN)");
                    Console.WriteLine("11.Deconectare!");
                }

                if (esteClient)
                {
                    Console.WriteLine("5.Vizualizeaza masini disponibile pentru inchiriat");
                    Console.WriteLine("6.Inchiriere masina");
                    Console.WriteLine("7.Inapoiere masina");
                    Console.WriteLine("12.Deconectare!");
                }

                Console.WriteLine("8.Vizualizeaza istoric inchirieri ale companiei");
                Console.WriteLine("9.Vizualizare istoric inchirieri ale unui client");
                Console.WriteLine("10.Vizualizare castiguri pe o data specifica");
                int optiune = int.Parse(Console.ReadLine());


                if (optiune == 0) break;

                switch (optiune)
                {
                    case 1:
                        Console.WriteLine("=== Masini disponibile in companie ===");
                        Companie.AfiseazaMasini();
                        break;
                    case 2:
                        Console.WriteLine("a.Autentificare admin");
                        Console.WriteLine("b.Inregistrare client");
                        Console.WriteLine("c.Autentificare client");
                        string alegere = Console.ReadLine();

                        if (alegere == "a")
                        {
                            Console.WriteLine("Username: ");
                            string username = Console.ReadLine();

                            Console.WriteLine("Password: ");
                            string password = Console.ReadLine();

                            if (Admin.username == username && Admin.password == password)
                            {
                                esteAdminConectat = true;
                                Console.WriteLine("Conectare reusita ca admin!");
                            }
                        }
                        else if (alegere == "b")
                        {

                            // Creăm un client nou direct
                            Console.WriteLine("Introduceti datele pentru crearea contului:");
                            Console.WriteLine("Nume: ");
                            string nume = Console.ReadLine();

                            Console.WriteLine("CNP: ");
                            string cnp = Console.ReadLine();

                            /*   if (!Client.ValidareCnp(cnp))
                               {
                                   Console.WriteLine("CNP invalid");
                                   break;
                               }.  */

                            Console.WriteLine("Username-ul contului:");
                            string username = Console.ReadLine();

                            Console.WriteLine("Parola: ");
                            string parola = Console.ReadLine();

                            Client clientNou = new Client(nume, cnp, username, parola);
                            Companie.AdaugaClienti(clientNou);
                            ContClientOperatii.SalveazaConturi(Companie.listaClienti);
                            clientCurent = clientNou;
                            esteClient = true;
                            Console.WriteLine($"S-a creat si conectat clientul cu username-ul {username}");
                        }
                        else if (alegere == "c")
                        {
                            Console.WriteLine("Username: ");
                            string usernameLogin = Console.ReadLine();

                            Console.WriteLine("Password: ");
                            string passwordLogin = Console.ReadLine();

                            // Încărcăm lista actualizată de clienți din fișier
                            var conturiSalvate = ContClientOperatii.IncarcaConturi();
            
                            // Căutăm clientul în conturile salvate
                            var clientGasit = conturiSalvate.Find(c => 
                                c.username == usernameLogin && c.password == passwordLogin);

                            if (clientGasit != null)
                            {
                                esteClient = true;
                                clientCurent = clientGasit;
                
                                // Actualizăm lista de clienți din companie dacă e necesar
                                if (!Companie.listaClienti.Any(c => c.username == clientGasit.username))
                                {
                                    Companie.listaClienti.Add(clientGasit);
                                }
                
                                Console.WriteLine($"Bine ati revenit, {clientGasit.nume}!");
                            }
                            else
                            {
                                Console.WriteLine("Username sau parola incorecta!");
                                Console.WriteLine("Verificați că ați introdus datele corect sau înregistrați un cont nou.");
                            }
                            
                        }

                        break;

                    case 3:
                        if (esteAdminConectat)
                        {
                            Console.WriteLine("Adauga Masina: ");
                            Console.WriteLine("Alege tipul: ");
                            Console.WriteLine("1.Standard");
                            Console.WriteLine("2.Electric");
                            int tipMasina = int.Parse(Console.ReadLine());

                            Console.WriteLine("Numar de zile: ");
                            int zile = int.Parse(Console.ReadLine());

                            Console.WriteLine("Consumul este Normal/Redus");
                            string consum = Console.ReadLine();

                            Console.WriteLine("Costul pe zi:");
                            double cost = double.Parse(Console.ReadLine());

                            Console.WriteLine("Marca masinii: ");
                            string marca = Console.ReadLine();

                            Console.WriteLine("Modelul masinii:");
                            string model = Console.ReadLine();

                            Console.WriteLine("Anul masinii");
                            int anul = int.Parse(Console.ReadLine());



                            Console.WriteLine("Kilometrii:");
                            double kilometrii = double.Parse(Console.ReadLine());

                            Console.WriteLine("Nr Inmatriculare(de forma XX-XX-XXX , unde primul XX reprezinta judetul): ");
                            string nr = Console.ReadLine().ToUpper(); // pt a transforma toate literele mari

                            if (!Masini.ValidareNr(nr))
                            {
                                Console.WriteLine("Numar de inmatriculare invalid!");
                                break;
                            }

                            Masini masinaNoua;

                            if (tipMasina == 1) // Standard
                            {
                                masinaNoua = new Standard(zile, consum, cost, marca, model, anul, kilometrii, nr, true);
                            }
                            else if (tipMasina == 2) // Electric
                            {
                                masinaNoua = new Electric(zile, consum, cost, marca, model, anul, kilometrii, nr, true);
                            }
                            else
                            {
                                Console.WriteLine("INVALID!");
                                break;
                            }

                            Companie.AdaugaMasina(masinaNoua);
                        }

                        break;

                    case 4:
                        if (esteAdminConectat)
                        {
                            Console.WriteLine("\nSterge Masina:");
                            Companie.AfiseazaMasini();

                            Console.WriteLine("Introdu nr indexului: ");
                            int indx;
                            if (int.TryParse(Console.ReadLine(), out indx))

                                try
                                {
                                    Companie.StergereMasini(indx);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error!");
                                    throw;
                                }
                        }

                        break;

                    case 5:
                        if (!esteAdminConectat)
                        {
                            Companie.AfiseazaMasini();
                        }

                        break;

                    case 6:
                        if (esteClient && clientCurent != null)
                        {
                            Companie.AfiseazaMasini();
                            Console.WriteLine("Introduceti indexul masinii: ");
                            int indxMasina = int.Parse(Console.ReadLine());
                            
                            Console.WriteLine("Introduceti data in care vreti sa incepeti inchirierea masinii(de forma dd/mm/yyyy):");
                            string s = Console.ReadLine();
                            
                            if (DateTime.TryParse(s, out DateTime dataInchiriere))
                            {
                                Console.WriteLine($"Data introdusa este valida: {dataInchiriere}");
                            }
                            else
                            {
                                Console.WriteLine("Data introdusa nu este valida!");
                            }

                            Console.WriteLine("Introduceti nr de zile: ");
                            int durata = int.Parse(Console.ReadLine());

                            Console.WriteLine("Introduceti username-ul: ");
                            string usernameClient = Console.ReadLine();

                            var clientInchiriere = Companie.listaClienti.Find(c => c.username == usernameClient);
                            if (clientInchiriere != null)
                            {
                                if (clientInchiriere.AreDaune())
                                {
                                    Console.WriteLine(
                                        $"Clientul {clientInchiriere.nume} nu poate inchiria masina deoarece are daune in istoricul sau");
                                    break;
                                }

                                if (indxMasina >= 0 && indxMasina < Companie.flotaMasini.Count)
                                {
                                    var masina = Companie.flotaMasini[indxMasina];
                                    if (masina.disponibila)
                                    {
                                        masina.nrzile = durata;
                                        Companie.InchiriazaMasinaClient(clientInchiriere, indxMasina, durata,
                                            dataInchiriere);
                                        Console.WriteLine(
                                            $"Ati inchiriat masina {masina.marca} , {masina.model} , pt {durata} zile.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nu este disponibila");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Index invalid");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu s-a gasit client cu username");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Trebuie sa fii logat!");
                        }

                        break;

                    case 7:
                         /*Console.WriteLine("Introduceti username-ul: ");
                        string usernameClientInapoiere = Console.ReadLine();
                        
                        var clientInapoiere = Companie.listaClienti.Find(c => c.username == usernameClientInapoiere);
                        if (clientInapoiere == null)
                        {
                            Console.WriteLine("Username-ul nu este valid!");
                            break;
                        } */
                         if(esteClient && clientCurent != null)
                        Companie.AfiseazaMasiniInchiriate();
                        Console.WriteLine("Introduceti indexul masinii pe care ati inchiriat-o: ");
                        
                        int indxMasina2 = int.Parse(Console.ReadLine());
                        
                        if (indxMasina2 < 0 || indxMasina2 >= Companie.flotaMasini.Count)
                        {
                            Console.WriteLine("Indexul mașinii este invalid. Încercați din nou.");
                            break;
                        }

                        var masina2 = Companie.flotaMasini[indxMasina2];
                        Console.WriteLine("Au fost daune cauzate masinii pe perioada inchirierii?(da/nu)");
                        string raspuns = Console.ReadLine();
                        
                        bool daune = raspuns.ToLower() == "da";
                         // clientInapoiere.ReturnareMasina(masina2, daune);
                         clientCurent.ReturnareMasina(masina2 , daune);
                        break;
                    
                    case 8:
                        Companie.AfiseazaIstoricInchirieri();
                        break;

                    case 9:
                        Console.WriteLine("Introduceti username: ");
                        string usernameClient1 = Console.ReadLine();

                        Companie.AfiseazaMasinaInchiriateClient(usernameClient1);

                        break;

                    case 10:
                        Console.WriteLine("Introduceti data in care sa va afisam castigurile(de forma dd/mm/yyyy):");
                        string input = Console.ReadLine();
                        if (DateTime.TryParse(input, out DateTime data))
                        {
                            Companie.AfiseazaCastiguri(data);
                        }
                        else
                        {
                            Console.WriteLine("Data introdusa nu este valida!");
                        }

                        break;

                    case 11:
                        if (esteAdminConectat)
                        {
                            esteAdminConectat = false;
                            Console.WriteLine("V-ati deconectat cu succes!");
                        }

                        break;
                    
                    case 12:
                        if (esteClient)
                        {
                            esteClient = false;
                            Console.WriteLine("V-ati deconectat cu succes!");
                        }

                        break;

                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }

            }
        }
        catch (Exception ex)
        {
            logger.Log($"Critical error {ex.Message}");
        }
    }
}
