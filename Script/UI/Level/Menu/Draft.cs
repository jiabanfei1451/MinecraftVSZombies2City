using PVZEngine.Static;
using Godot;
using PVZEngine.Module;
namespace GameUI.Object;
public partial class Draft : Control
{
    [Export] public TextureRect Iron = null;
    [Export] public Touch.TouchPad touchPad = null;
    [Export] public Audio_Plus Pick_UP = null;
    [Export] public Audio_Plus Cancel = null;
    [Export] public Panel Texture = null;
    [Export] public StyleBoxTexture Normal_Texture = null;
    [Export] public StyleBoxTexture Down_Texture = null;
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
            touchPad.Button_Downvoid += Down;
            touchPad.Button_UPvoid += UP;
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
    public void Down()
    {
        Texture.AddThemeStyleboxOverride("panel",Down_Texture);
    }
    public void UP()
    {
        Texture.AddThemeStyleboxOverride("panel",Normal_Texture);
    }
    public void pressed()
    {
        Set_Player_Use();
    }
    public void Set_Player_Use()
    {
        if (PVZEngine.Static.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object != null)
        {
           PVZEngine.Static.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = null; 
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
        if (Cheak.is_null(Iron)){return;}
        Iron.Position = GetLocalMousePosition() + new Vector2(-15,-15);
    }

}
