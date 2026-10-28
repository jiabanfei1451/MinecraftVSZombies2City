using PVZEngine.Static;
using Godot;
using System;
using PVZEngine.Data;
using PVZEngine.Module;

namespace UIObject;

public partial class StarCard : Control
{
    [Export]
    public Control Local_Position_Object = null;
    [Export]
    public Control Get_Position = null;
    [Export] public Control Star_Number_VIsi_Card = null;
    [Export] public Control Star_Number_Node = null;
    [Export] public Godot.StyleBox style = GD.Load<StyleBox>("res://Image/UI/Panel/Star Card/Star numbe.tres");
    [Export] public Godot.StyleBox style2 = GD.Load<StyleBox>("res://Image/UI/Panel/Star Card/Star numbe2.tres");
    [Export] public Touch.TouchPad pad = null;
    [Export] public Touch.TouchPad Pad_Eumu = null;
    [Export] public Label Label_Numbet = null;
    [Export] public Label visibleEnable = null;
    [Export] public Audio_Plus Buzzer = null;
    public bool is_Show_fragment_menu = false;
    short Current_Star_Fragment = -1;
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
        Pad_Eumu.Button_Pressedvoid += Show_fragment_menu;
        pad.Button_Pressedvoid += Show_fragment_menu;
        Touch.Touch_Index.Set_Index_Enable("Show fragment menu",false);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Cheak.is_null<Control>(Local_Position_Object) == true){return;}
        if (Cheak.is_null<Control>(Get_Position) == true){return;}
        if (Level_Script.Use_Prop == Level_Script.Prop.Star_Fragment)
        {
            visibleEnable.Visible = true;
            Get_Position.Position = Local_Position_Object.GetLocalMousePosition() - new Vector2(20,20);
        }
        else
        {
            visibleEnable.Visible = false;
            Get_Position.Position = new(0,0);
        }
        if (!Cheak.is_null<Control>(Star_Number_VIsi_Card) && !Cheak.is_null<Control>(Star_Number_Node))
        {
            if (Current_Star_Fragment != Level_Script.Star_Fragment)
            {
                Current_Star_Fragment = Level_Script.Star_Fragment;
                foreach (var Star_Number in Star_Number_VIsi_Card.GetChildren())
                {
                    Star_Number.QueueFree();
                }
                for (short MaxStar_Fragment = 0;MaxStar_Fragment < PlayerData.Max_Star_Fragment; MaxStar_Fragment++)
                {
                    Panel New_Panel = new();
                    if (Level_Script.Star_Fragment > MaxStar_Fragment)
                    {
                        New_Panel.AddThemeStyleboxOverride("panel",style);
                    }
                    else
                    {
                        New_Panel.AddThemeStyleboxOverride("panel",style2);
                    }
                    Star_Number_VIsi_Card.AddChild(New_Panel);
                    New_Panel.CustomMinimumSize = new(15,15);
                }
            }
            Label_Numbet.Text = $"{PlayerData.Max_Star_Fragment} / {Level_Script.Star_Fragment}";
            Star_Number_Node.CustomMinimumSize = new Vector2(Star_Number_VIsi_Card.Size.X + 14,Star_Number_Node.CustomMinimumSize.Y);
        }
    }    
    public void Show_fragment_menu()
    {
        if (is_Show_fragment_menu == false)
        {
            is_Show_fragment_menu = true;
            Tween d = CreateTween();
            d.TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,0),0.2f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.五次方));
            d.Parallel().TweenProperty(Pad_Eumu,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.2f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.三次));
            d.Parallel().TweenProperty(Pad_Eumu,new NodePath(Control.PropertyName.Size),new Vector2(Star_Number_Node.CustomMinimumSize.X + 40,88),0.5f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.三次));
            d.Parallel().TweenProperty(Pad_Eumu,new NodePath(Control.PropertyName.Position),new Vector2((-Star_Number_Node.CustomMinimumSize.X - 40) / 2,-88),0.5f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.三次));
            Touch.Touch_Index.Set_Index_Enable("Show fragment menu",true);
        }
        else
        {
            is_Show_fragment_menu = false;
            Tween d = CreateTween();
            d.TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.2f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.五次方));
            d.Parallel().TweenProperty(Pad_Eumu,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,0),0.2f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.三次));
            d.Parallel().TweenProperty(Pad_Eumu,new NodePath(Control.PropertyName.Size),new Vector2(Star_Number_Node.CustomMinimumSize.X + 40,40),0.5f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.三次));
            d.Parallel().TweenProperty(Pad_Eumu,new NodePath(Control.PropertyName.Position),new Vector2((-Star_Number_Node.CustomMinimumSize.X - 40) / 2,-40),0.5f).SetTrans(Game.Get.TweenType.GetTweenType( Game.Get.TweenType.Twee.三次));
            Touch.Touch_Index.Set_Index_Enable("Show fragment menu",false);
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
    public void SelectedStar_Fragment()
    {
        if (Level_Script.Star_Fragment > 0)
        {
            Level_Script.Set_Use_porp(Level_Script.Prop.Star_Fragment);
        }
        else
        {
            Level_Script.Use_Prop = Level_Script.Prop.Not;
            Buzzer.Play();
        }
    }
    public void Key_DownKeyCode(Key key)
    {
        if (key == Key.E)
        {
            Level_Script.Set_Use_porp(Level_Script.Prop.Equipment_capable);
        }
        if (key == Key.W)
        {
            SelectedStar_Fragment();
        }
    }
}
