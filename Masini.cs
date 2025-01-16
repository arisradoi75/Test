using Microsoft.VisualBasic;

namespace ProiectOOP;

public abstract class Masini // clasa masini + metode de calc cost alexa
{
    public double nrzile;
    public string consum;
    public double cost;
    public string marca;
    public string model;
    public int an;
    public double km;
    public string nrInmatriculare;
    public bool disponibila;
    
    protected Masini() {}
    
    public Masini(double nrzile, string consum, double cost, string marca, string model, int an, double km,
        string nrInmatriculare, bool disponibila)
    {
        this.nrzile = nrzile;
        this.consum = consum;
        this.cost = cost;
        this.marca = marca;
        this.model = model;
        this.an = an;
        this.km = km;
        this.nrInmatriculare = nrInmatriculare;
        this.disponibila = disponibila;
    }

    public static bool ValidareNr(string nrInmatriculare) // aris
    {
        string[] judete =
        {
            "AB", "AR", "AG", "BC", "BH", "BN", "BT", "BV", "BR", "BZ", "CS", "CL", "CJ", "CT", "CV", "DB", "DJ", "GL",
            "GR", "GJ", "HR", "HD", "IL", "IS", "IF", "MM", "MH", "MS", "NT", "OT", "PH", "SM", "SJ", "SB", "SV", "TR",
            "TM", "TL", "VS", "VL", "VN", "B"
        };

        if (nrInmatriculare.Length != 9 || nrInmatriculare[2] != '-' || nrInmatriculare[5] != '-')
        {
            return false;
        }

        string judet = nrInmatriculare.Substring(0, 2);
        if (!judete.Contains(judet))
        {
            return false;
        }

        string numere = nrInmatriculare.Substring(3, 2);
        if (!(numere[0] >= '0' && numere[0] <= '9'
                               && numere[1] >= '0' && numere[1] <= '9'))
        {
            return false;
        }

        string litere = nrInmatriculare.Substring(6, 3);
        if (!(litere[0] >= 'A' && litere[0] <= 'Z' &&
              litere[1] >= 'A' && litere[1] <= 'Z' &&
              litere[2] >= 'A' && litere[2] <= 'Z'))
        {
            return false;
        }

        return true;
    }

    public abstract double CalcCost();
    
    public abstract void AfiseazaDetalii();

}