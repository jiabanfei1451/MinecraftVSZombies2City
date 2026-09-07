using Godot;
using System;

public partial class LevelUiMenu : Control
{
    [Export] public TextureRect Speed_Texture = null;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (Speed_Texture != null){
            Godot.Vector2 Temp_Posiiton = Speed_Texture.Position;
            Temp_Posiiton.X += 32 * (float)delta;
            Speed_Texture.Position = Temp_Posiiton;
            if (Speed_Texture.Position.X > 0)
            {
                Speed_Texture.Position = new Vector2(-40,0);
            }  
        }
    }
}
