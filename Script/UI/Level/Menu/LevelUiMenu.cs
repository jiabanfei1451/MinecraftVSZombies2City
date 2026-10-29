using Godot;
using PVZEngine;
using PVZEngine.Static;
using System;
namespace UIObject;
public partial class LevelUiMenu : Control
{
    [Export] public TextureRect Speed_Texture = null;
    [Export] public Label Level_Label = null;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!(Level_Label is null))
        {
            String Level_Name = Level_Script.Level_Name;
            String Chapter_Name = NodeData.Get_Node<Level_Master_Script>("Level").Chapter_Name;
            String Text = "";
            if (Chapter_Name != "")
            {
                Text += Chapter_Name;
            }
            if (Level_Name != "")
            {
                if (Chapter_Name != "")
                {
                    Text += " ";
                }
                Text += Level_Name;
            }
            Level_Label.Text = Text;
        }
        if (!(Speed_Texture is null)){
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
