using Godot;
using System;

namespace MVZ2.Object.Equipment;
public partial class Landmine : MVZ2.Object.Equipment.Equipment
{
    [Export] public Godot.Collections.Array<Sprite2D> sprite2Ds = new();
    [Export] public int Sprite2D_ID = new();
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        int d = 0;
        foreach (Sprite2D sprite2D in sprite2Ds)
        {
            if (d != Sprite2D_ID)
            {
                sprite2D.Visible = false;
            }
            else
            {
                sprite2D.Visible = true;
            }
            d += 1;
        }
    }
}
