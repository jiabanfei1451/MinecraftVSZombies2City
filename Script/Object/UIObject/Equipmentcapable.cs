using Godot;
using PVZEngine.Data;
using PVZEngine.Static;

public partial class Equipmentcapable : TextureRect
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Level_Script.Use_Prop == Level_Script.Prop.Equipment_capable)
        {
            Position = GetNode<Control>("..").GetLocalMousePosition() - new Vector2(20,20);
            Rotation += 1 * (float)delta;
            if (Rotation > 3.14)
            {
                Rotation -= 3.14f;
            }
            Visible = true;
        }
        else
        {
            Visible = false;
            return;
        }
    }

}
