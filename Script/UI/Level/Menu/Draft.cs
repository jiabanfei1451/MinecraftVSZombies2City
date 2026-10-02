using Game;
using Godot;
using My_Csharp_Node;
using System;
namespace GameUI.Object;
public partial class Draft : Control
{
    [Export] public TextureRect Iron = null;
    [Export] public Touch.TouchPad touchPad = null;
    [Export] public Audio_Plus Pick_UP = null;
    [Export] public Audio_Plus Cancel = null;
    Godot.Vector2 Temp_Pos;
    public override void _ExitTree() {
        base._ExitTree();
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode -= Key_DownKeyCode;
        Game.Get_GlobalNode.GetKey.Mouse_DownKeyCode -= Mouse_DownKeyCode;
    }
    public override void _Ready() {
        base._Ready();
        Temp_Pos = Iron.Position;
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode += Key_DownKeyCode;
        Game.Get_GlobalNode.GetKey.Mouse_DownKeyCode += Mouse_DownKeyCode;
        if (touchPad != null)
        {
            touchPad.Button_Pressedvoid += pressed;
        }
    }
    public void Mouse_DownKeyCode(MouseButtonMask key)
    {
        if (Level_Script.Use_Prop != Level_Script.Prop.iron_pickaxe){return;}
        if (key == MouseButtonMask.Right)
        {
            Set_Player_Use();
        }
    }
    public void Key_DownKeyCode(Key key)
    {
        if (key == Key.Q)
        {
            Set_Player_Use();
        }
    }
    public void pressed()
    {
        GD.Print(Touch.Touch_Index.Get_Index(3));
        Set_Player_Use();
    }
    public void Set_Player_Use()
    {
        if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object != null)
        {
           Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = null; 
        }
        if (Level_Script.Use_Prop != Level_Script.Prop.iron_pickaxe)
        {
            Pick_UP.Play();
            Level_Script.Use_Prop = Level_Script.Prop.iron_pickaxe;
        }
        else
        {
            Cancel.Play();
            Level_Script.Use_Prop = Level_Script.Prop.Not;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Level_Script.Use_Prop != Level_Script.Prop.iron_pickaxe){
            Iron.Position = Temp_Pos ;
            return;}
        if (Iron == null){return;}
        Iron.Position = GetLocalMousePosition() + new Vector2(-15,-15);
    }

}
