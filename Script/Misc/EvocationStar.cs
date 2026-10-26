using Godot;
using System;

public partial class EvocationStar : Node2D
{
    [Export] public AnimationPlayer GetAnimation = null;
    public override void _Ready() {
        base._Ready();
        if (!(GetAnimation is null))
        {
            GetAnimation.AnimationFinished += Finale;
        }
    }
    public void Finale(StringName d)
    {
        QueueFree();
    }
}
