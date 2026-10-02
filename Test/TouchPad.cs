using Godot;
using System;
namespace d;
public partial class TouchPad : Touch.TouchPad
{
    public override void _Ready() {
        base._Ready();
        Button_Down += huh;
    }
    public void huh(Touch.TouchPad t,Godot.Vector2 v)
    {
        GetNode<ColorRect>("ColorRect").Position = v / GetWindow().GetCamera2D().Zoom;
    }
}
