using Godot;

namespace Game;
public static partial class TweenType
{
    public enum Twee
    {
        回弹 = 6,
        弹跳 = 7,
        圆形 = 12,
        三次 = 3,
        弹性 = 8,
        指数 = 11,
        线性 = 0,
        二次抛物线 = 2,
        四次方 = 4,
        五次方 = 5,
        正弦 = 10,
        弹簧 = 9
    }
    /// <summary>
    /// 获取中文名称的动画类
    /// </summary>
    /// <param name="TweenType"></param>
    /// <returns></returns>
    public static Godot.Tween.TransitionType GetTweenType(Twee TweenType = Twee.线性)
    {
        switch (TweenType)
        {
            case Twee.回弹:
                return Tween.TransitionType.Back;
            case Twee.弹跳:
                return Tween.TransitionType.Bounce;
            case Twee.圆形:
                return Tween.TransitionType.Circ;
            case Twee.三次:
                return Tween.TransitionType.Cubic;
            case Twee.弹性:
                return Tween.TransitionType.Elastic;
            case Twee.指数:
                return Tween.TransitionType.Expo;
            case Twee.线性:
                return Tween.TransitionType.Linear;
            case Twee.二次抛物线:
                return Tween.TransitionType.Quad;
            case Twee.四次方:
                return Tween.TransitionType.Quart;
            case Twee.五次方:
                return Tween.TransitionType.Quint;
            case Twee.正弦:
                return Tween.TransitionType.Sine;
            case Twee.弹簧:
                return Tween.TransitionType.Spring;
        }
        return Tween.TransitionType.Linear;
    }
}
