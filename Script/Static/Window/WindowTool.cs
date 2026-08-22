using System;
using System.Text;
using Godot;

namespace Game;

public static class WindowTool
{
    public static Window Process_Window = null;
    public static Tween This_Tween = null;
    public static void Set_Title(float AnimationTime,Tween.TransitionType transitionType = Tween.TransitionType.Linear,params object[] Why)
    {
        if (Process_Window == null){return;}
        StringBuilder @string = new StringBuilder();
        foreach(var Variant in Why)
        {
            @string.Append(Variant.ToString());
        }
        if (This_Tween != null)
        {
            This_Tween.Kill();
        }
        String Temp_String = @string.ToString();
        This_Tween = Process_Window.CreateTween();
        This_Tween.TweenProperty(Process_Window,new NodePath(Window.PropertyName.Title),Temp_String,AnimationTime).SetTrans(transitionType);
    }
        public static void Set_Title(params object[] Why)
    {
        if (Process_Window == null){return;}
        StringBuilder @string = new StringBuilder();
        foreach(var Variant in Why)
        {
            @string.Append(Variant.ToString());
        }
        String Temp_String = @string.ToString();
        Process_Window.Title = Temp_String;
    }
}