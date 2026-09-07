using Godot;
using My_Csharp_Node;
using System;

public partial class Add_Speed_Button : Touch.TouchPad
{
    [Export] public StringName Theme_Name = "panel";
    [Export] public Panel Change_Theme_Object = null;
    [Export] public StyleBoxTexture Down_Texture = null;
    [Export] public StyleBoxTexture UP_Texture = null;
    [Export] public Audio_Plus Souds = null;
    [Export] public Audio_Plus Souds2 = null;
    [Export] public TextureRect Speed_Texture = null;
    public override void _Ready() {
        base._Ready();   
        Button_Downvoid += ButtonDowm;
        Button_UPvoid += ButtonUP;
        Button_Pressedvoid += ButtonPressed;
    }
    public void ButtonPressed()
    {
        if (Engine.TimeScale == 1)
        {
            if (Souds != null)
            {
                Souds.Play();
            }
            if (Speed_Texture != null)
        {
            CreateTween().TweenProperty(Speed_Texture,new NodePath(TextureRect.PropertyName.SelfModulate),new Color(0,1,0,1),0.5);
        }
            Engine.TimeScale = 2;
        }
        else
        {
            if (Souds2 != null)
            {
                Souds2.Play();
            }
            if (Speed_Texture != null)
        {
            CreateTween().TweenProperty(Speed_Texture,new NodePath(TextureRect.PropertyName.SelfModulate),new Color(1,1,1,1),0.5);
        }
            Engine.TimeScale = 1;
        }
    }
    public void ButtonDowm()
    {
        
        Change_Theme_Object.AddThemeStyleboxOverride(Theme_Name,Down_Texture);
    }
    public void ButtonUP()
    {
        
        Change_Theme_Object.AddThemeStyleboxOverride(Theme_Name,UP_Texture);
    }
}
