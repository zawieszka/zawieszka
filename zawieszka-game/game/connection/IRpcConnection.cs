namespace Zawieszka.Connection;

public interface IRpcConnection
{
    public void Server_RegisterConnection(string username);
    public void Server_TakeSeat(int seat);
    public void Server_EndTurn();
    public void Client_RegisteredConnection(int peerId, string username);
    public void Client_UpdateLobby(string lobby);
    public void Client_DisplayNotification(string message);
    public void Client_DisplayMessage(string message);
    public void Client_NextTurn(string username);
}