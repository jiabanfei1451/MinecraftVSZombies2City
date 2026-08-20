using Game;
using Godot;
using System;

public partial class TipUI : Control
{
    Control TipPanel = null;
    Label Label = null;
    String Temp_Text = "";
    double Await_Time = 0;
    public override void _Ready() {
        base._Ready();
        Game.Tip.Ready_Text = GetNode<Label>("Ready");
        Game.Tip.Tip_Text = GetNode<Label>("Tip/Text");
        TipPanel = GetNode<Control>("Tip");
        Label = GetNode<Label>("Tip/Text");
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (TipPanel == null){return;}
        if (Label == null){return;}
        if (Label.Text.Length > 0)
        {
            TipPanel.Visible = true;
            Await_Time += delta;
            if (Await_Time >= 8)
            {
                Label.Text = "";
            }
            if (Label.Text != Temp_Text)
            {
                Temp_Text = Label.Text;
                Await_Time = 0;
            }
        }
        else
        {
            TipPanel.Visible = false;
        }
    }
}
