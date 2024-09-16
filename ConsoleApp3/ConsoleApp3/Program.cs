public class Servers
{
    private static Servers example = new Servers();
    private HashSet<string> servers;
    private object llock = new object();

    private Servers()
    {
        servers = new HashSet<string>();
    }

    public static Servers Example => example;

    public bool AddServer(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || (!url.StartsWith("http://") && !url.StartsWith("https://")))
        {
            return false;
        }

        lock (llock)
        {
            return servers.Add(url);
        }
    }

    public IEnumerable<string> GetHttpServers()
    {
        lock (llock)
        {
            return servers.Where(s => s.StartsWith("http://")).ToList();
        }
    }

    public IEnumerable<string> GetHttpsServers()
    {
        lock (llock)
        {
            return servers.Where(s => s.StartsWith("https://")).ToList();
        }
    }
}

class Program
{
    static void Main()
    {
        var servers = Servers.Example;

        bool added1 = servers.AddServer("http://zaimi2024.com");
        bool added2 = servers.AddServer("https://detskiydom.com");
        bool added3 = servers.AddServer("ftp://omon.com");
        bool added4 = servers.AddServer("ntp://tankionline.com");

        Console.WriteLine($"Added http://zaimi2024.com: {added1}"); 
        Console.WriteLine($"Added https://detskiydom.com: {added2}"); 
        Console.WriteLine($"Added ftp://omon.com: {added3}");
        Console.WriteLine($"Added ntp://tankionline.com: {added4}");

        var httpServers = servers.GetHttpServers();
        var httpsServers = servers.GetHttpsServers();

        Console.WriteLine("HTTP Servers:");
        foreach (var server in httpServers)
        {
            Console.WriteLine(server);
        }

        Console.WriteLine("HTTPS Servers:");
        foreach (var server in httpsServers)
        {
            Console.WriteLine(server);
        }
    }
}