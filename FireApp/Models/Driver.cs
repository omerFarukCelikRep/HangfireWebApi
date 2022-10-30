namespace FireApp.Models;

public class Driver
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
    public int Status { get; set; }
}