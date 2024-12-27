using Godot;

namespace Zawieszka.Scenes.Lobby;

public partial class SeatsList : Node
{
    private List<Seat> _seats = [];
    [Signal]
    public delegate void RequestTakeSeatEventHandler(int seat);
    public override void _Ready()
    {
        var nani = FindChildren("Seat?", nameof(Seat)); // Seat has to have GlobalClass attribute
        _seats = nani.Select(x => (Seat)x).OrderBy(x => x.SeatNumber).ToList();
        foreach (var seat in _seats)
        {
            seat.TakeSeatClick += seatNumber => EmitSignal(SignalName.RequestTakeSeat, seatNumber);
        }
    }

    public void SetSeat(int seatNumber, string username)
    {
        if (username is null)
        {
            _seats[seatNumber].EmptySeat();
        }
        else
        {
            _seats[seatNumber].TakeSeat(username);
        }
    }
}