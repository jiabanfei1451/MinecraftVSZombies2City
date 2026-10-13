using Game;
using Game.Static;
using Godot;
using System;

namespace UIObject;

public partial class StarCard : Control
{
    public override void _ExitTree()
    {
        base._ExitTree();
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode -= Key_DownKeyCode;
    }

    public override void _Ready()
    {
        base._Ready();
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode += Key_DownKeyCode;
    }
    public void Key_DownKeyCode(Key key)
    {
        if (key == Key.W)
        {
            if (Level_Script.Use_Prop != Level_Script.Prop.Star_Fragment){
                Level_Script.Use_Prop = Level_Script.Prop.Star_Fragment;
            }
            else
            {
                Level_Script.Use_Prop = Level_Script.Prop.Not;
            }
        }
    }
}
