using System;
using Godot;
using PVZEngine.Enum;
using PVZEngine.Static;

public partial class Equipmentcapable : TextureRect
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Level_Script.Data.Use_Prop == Prop.Equipment_capable)
        {
            Position -= Position - Game.Get_GlobalNode.InputPosition.Position + new Godot.Vector2(20,20);
            Rotation += 1 * (float)delta;
            Visible = true;
        }
        else
        {
            Visible = false;
            return;
        }
    }

}
