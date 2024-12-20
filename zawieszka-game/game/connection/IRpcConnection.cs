namespace Zawieszka.Connection;

public interface IRpcConnection
{
    public void Server_SetUsername(string username);
    public void Server_EndTurn();
    public void Client_DisplayNotification(string message);
    public void Client_DisplayMessage(string message);
    public void Client_NextTurn(string username);
}