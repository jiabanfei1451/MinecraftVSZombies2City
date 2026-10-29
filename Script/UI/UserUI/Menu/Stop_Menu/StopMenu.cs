using System.Threading.Tasks;
using d;
using Godot;

namespace UIObject;

public partial class StopMenu : Control
{
    [Export] public AnimationPlayer animationNode = null;
    [Export] public Touch.TouchPad touchPad = null;
    [Export] public double TimeScale = 1;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Engine.TimeScale = TimeScale;
    }
    public override void _Ready() {
        base._Ready();
        animationNode.Play("Start");
        if (touchPad != null)
        {
            touchPad.Button_Pressedvoid += Exit;
        }
    }
    public async void Exit()
    {
        animationNode.Play("Exit");
        animationNode.AnimationFinished += Free;
    }
    public void Free(StringName d)
    {
        GetNode<Node>("..").QueueFree();
    }
}