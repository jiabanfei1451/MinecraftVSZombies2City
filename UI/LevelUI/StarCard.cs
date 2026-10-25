using PVZEngine.Static;
using Godot;

namespace UIObject;

public partial class StarCard : Control
{
    [Export]
    public Control Local_Position_Object = null;
    [Export]
    public Control Get_Position = null;
    public override void _ExitTree()
    {
        base._ExitTree();
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode -= Key_DownKeyCode;
    }

    public override void _Ready()
    {
        base._Ready();
        Game.Get_GlobalNode.GetKey.Mouse_Down += Mouse_Down;
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode += Key_DownKeyCode;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Cheak.is_null<Control>(Local_Position_Object) == true){return;}
        if (Cheak.is_null<Control>(Get_Position) == true){return;}
        if (Level_Script.Use_Prop == Level_Script.Prop.Star_Fragment)
        {
            Get_Position.Position = Local_Position_Object.GetLocalMousePosition() - new Vector2(20,20);
        }
        else
        {
            Get_Position.Position = new(0,0);
        }
    }
    public void Mouse_Down(InputEventMouseButton Mouse)
    {
        if (Mouse.ButtonIndex == MouseButton.Right)
        {
            if (Level_Script.Use_Prop == Level_Script.Prop.Equipment_capable || Level_Script.Use_Prop == Level_Script.Prop.Star_Fragment){
                Level_Script.Use_Prop = Level_Script.Prop.Not;
            }
        }
    }
    public void Key_DownKeyCode(Key key)
    {
        if (key == Key.Key1)
        {
            if (Level_Script.Use_Prop != Level_Script.Prop.Equipment_capable){
                Level_Script.Use_Prop = Level_Script.Prop.Equipment_capable;
            }
            else
            {
                Level_Script.Use_Prop = Level_Script.Prop.Not;
            }
        }
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
