using Godot;
using Godot.NativeInterop;
using System;

public partial class 红石 : Node2D
{
    Vector2 NewPosition = new Vector2(0,0);
    public override void _Ready() {
        base._Ready();
        NewPosition = Position;
        Control Touchbutton = GetNode<Control>("Touchbutton");
        Touchbutton.Connect("点击时void", new Callable(this, "on点击时"));
    }
    public override void _Process(double delta) {
        base._Process(delta);

        Position = NewPosition;
    }
    private void on点击时(){
        Node2D tree = (Node2D)GetTree().CurrentScene;
        int 当前器械能 = (int)tree.Get("器械能");
        GD.Print(当前器械能);
        tree.Set("器械能",当前器械能 + 25);
        QueueFree();
    }
}
