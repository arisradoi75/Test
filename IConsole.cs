namespace ProiectOOP;

public interface IConsole
{
    string ReadLine(); // citeste o linie de la consola
    void WriteLine(string message); // scrie o linie in consola
    void Write(string message); // scrie in consola fara.
}

public class ConsoleWrapper : IConsole
{ // implementarile pentru citire si scriere in consola
    public string ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void Write(string message)
    {
        Console.Write(message);
    }
}