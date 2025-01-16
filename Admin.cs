namespace ProiectOOP;

public class Admin
{
    public const string username = "admin";
    public const string password = "admin1234";
    
    public int index;
    public Admin(string username, string password,int index)
    {
        this.index = index;
    }
    
    public void AdaugaMasina(Masini masini,Companie companie)
    {
        companie.flotaMasini.Add(masini);
    }

    public void StergereMasini(Masini masini, Companie companie)
    {
        if (index >= 0 && index < companie.flotaMasini.Count - 1)
        {
            companie.flotaMasini.RemoveAt(index);
        }
    }
}