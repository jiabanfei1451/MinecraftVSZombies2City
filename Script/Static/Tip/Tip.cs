using System.Threading.Tasks;
using Godot;

namespace Game;
public static class Tip
{
    /// <summary>
    /// 准备字幕
    /// </summary>
    public static Godot.Label Ready_Text = null;
    /// <summary>
    /// 提示字幕
    /// </summary>
    public static Godot.Label Tip_Text = null;
    /// <summary>
    /// 设置准备文本
    /// </summary>
    /// <param name="What"></param>
    /// <returns></returns>
    public static void Set_Ready_Text(params object[] What)
    {
        string Str = "";
        foreach (var W in What)
        {
            if (W != null)
            {
                Str += W.ToString();
            }
        }
        Ready_Text.Text = Str;
    }
    /// <summary>
    /// Set_Ready_Text 等待的方法
    /// </summary>
    /// <param name="Await"></param>
    /// <param name="What"></param>
    public static async Task<int> Set_Ready_Text(bool Await,double Delay,bool Administration,float StartScale,float EndScale,params object[] What)
    {
        if (Administration == true)
        {
            Ready_Text.Modulate = new Color(0,0,0,0);
            Ready_Text.Scale = new Godot.Vector2(StartScale,StartScale);
            Tween tion = Ready_Text.CreateTween();
            tion.TweenProperty(Ready_Text,new NodePath(Godot.Label.PropertyName.Modulate),new Color(1,1,1,1),Delay).SetTrans(Tween.TransitionType.Sine);
            tion.Parallel().TweenProperty(Ready_Text,new NodePath(Godot.Label.PropertyName.Scale),new Godot.Vector2(EndScale,EndScale),Delay).SetTrans(Tween.TransitionType.Cubic);
        }
        string Str = "";
        foreach (var W in What)
        {
            if (W != null)
            {
                Str += W.ToString();
            }
        }
        Ready_Text.Text = Str;
        int time = (int)Delay;
        await Task.Delay(1000 * time);
        return 0;
    }
    /// <summary>
    /// 设置提示文本
    /// </summary>
    /// <param name="What"></param>
    /// <returns></returns>
    public static void Set_Tip_Text(params object[] What)
    {
        string Str = "";
        foreach (var W in What)
        {
            if (W is Godot.Color)
            {
                Godot.LabelSettings settings = Tip_Text.LabelSettings;
                settings.FontColor = (Godot.Color)W;
                Tip_Text.LabelSettings = settings;
            }
            if (W != null)
            {
                Str += W.ToString();
            }
        }
        Tip_Text.Text = Str;
    }
}
