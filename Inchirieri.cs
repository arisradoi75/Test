namespace ProiectOOP;

public class Inchirieri
{
    public DateTime dataStart;
    public DateTime dataStop; 
    public int durata;
    public List<Client> listaClienti;
    public Masini masinaClient; // masina asociata clientului
    public bool areDaune;
    
    public Inchirieri() {}

    public Inchirieri(DateTime dataStart, DateTime dataStop,int durata)
    {
        this.dataStart = dataStart;
        this.dataStop = dataStop;
        this.durata = durata;
        this.masinaClient = masinaClient;
        this.areDaune = false;
    }

    public void ArataDaune(bool daune)
    {
        this.areDaune = daune;
    }
   
}