namespace FireApp.Services;

public class ServiceManagement : IServiceManagement
{
    public void GenerateMerchandise()
    {
        Console.WriteLine($"Generate Merchandise : Long running task {DateTime.Now : dd-MM-yyyy HH:mm:ss}");
    }

    public void SendMail()
    {
        Console.WriteLine($"Send Email : short running task {DateTime.Now : dd-MM-yyyy HH:mm:ss}");
    }

    public void SyncData()
    {
        Console.WriteLine($"Sync Data : Short running task {DateTime.Now : dd-MM-yyyy HH:mm:ss}");
    }

    public void Update()
    {
        Console.WriteLine($"Update Database : Long running task {DateTime.Now : dd-MM-yyyy HH:mm:ss}");
    }
}