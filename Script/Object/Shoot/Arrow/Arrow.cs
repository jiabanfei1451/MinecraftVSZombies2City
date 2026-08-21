using Godot;
namespace Csharp_Object;
public partial class Arrow : Level.Object.Data
{
    public override void _Ready() {
        base._Ready();
        Enable_Health = false;
        Health.Free();
        Health = null;
    }
    public override void _Process(double delta) {
        base._Process(delta);
        Vector2 Temp_Position = Position;
        if (GlobalPosition.X >= 800)
        {
            QueueFree();
        }
    }
}
