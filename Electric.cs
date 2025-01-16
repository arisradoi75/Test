namespace ProiectOOP;

public class Electric : Masini
{
    public Electric(double nrzile , string consum,double cost , string marca, string model, int an, double km, string nrInmatriculare, bool disponibila) : base(nrzile ,consum , cost ,marca,model,an,km,nrInmatriculare,disponibila)
    {}
    
    public Electric() {}

    public override double CalcCost()
    {
        return nrzile * cost + 50;
    }

    public override void AfiseazaDetalii()
    {
        Console.WriteLine($" Masina {marca} , {model} , {nrInmatriculare} , Cost pe zi {CalcCost()} RON");
    }
}