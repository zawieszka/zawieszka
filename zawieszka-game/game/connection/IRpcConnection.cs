namespace Zawieszka.Connection;

public interface IClientServerConnection
{ 
    public void SetUsername(string username);
    public void DisplayNotification(string message);
    public void DisplayMessage(string message);
}