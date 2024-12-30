using Godot;

namespace Zawieszka.Scenes.Lobby;

[GlobalClass]
public partial class Seat : Node
{
    private const string EmptySeatCaption = "Empty";
    [Export] public int SeatNumber { get; private set; } = 0;
    [Export] private Label UsernameDisplay { get; set; }
    [Export] private Label PlayerDisplay { get; set; }
    [Export] private Button SeatButton { get; set; }
    
    [Signal]
    public delegate void TakeSeatClickedEventHandler(int seat);

    public override void _Ready()
    {
        base._Ready();
        PlayerDisplay.Text += SeatNumber;
    }

    public void TakeSeat(string username)
    {
        UsernameDisplay.Text = username;
        SeatButton.Visible = false;
    }

    public void EmptySeat()
    {
        SeatButton.Visible = true;
        UsernameDisplay.Text = EmptySeatCaption;
    }

    private void _on_seat_button_button_up()
    {
        EmitSignal(SignalName.TakeSeatClicked, SeatNumber);
    }
}