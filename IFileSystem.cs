namespace ProiectOOP;

public interface IFileSystem
{ // toate functiile is pentru scriere/citire/verificare daca exista fisier/adauga la finalul fisierelui
    void WriteAllText(string path, string text);
    string ReadAllText(string path);
    bool FileExists(string path);
    void AppendAllText(string path, string text);
}

public class FileSystemWrapper : IFileSystem
{ //implementari pentru functiile de mai sus.
    public void WriteAllText(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public void AppendAllText(string path, string content)
    {
        File.AppendAllText(path, content);
    }
}