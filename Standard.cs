using System;
using System.Collections.Generic;

namespace ProiectOOP;

public class Standard : Masini
{
    public Standard(double nrzile , string consum, double cost , string marca, string model, int an, double km, string nrInmatriculare, bool disponibila) : base(nrzile ,consum , cost ,marca,model,an,km,nrInmatriculare,disponibila)
    {}
    
    public Standard() {}

    public override double CalcCost()
    {
        return nrzile * cost;
    }
    
    public override void AfiseazaDetalii()
    {
            Console.WriteLine($" Masina {marca} , {model} , {nrInmatriculare} , Cost pe zi {CalcCost()} RON");
    }
}