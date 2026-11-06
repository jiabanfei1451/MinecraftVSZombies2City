using System.Threading.Tasks;
using d;
using Game.Get;
using Godot;
using PVZEngine.Static;

namespace UIObject;

public partial class StopMenu : Control
{
    [Export] public Touch.TouchPad touchPad = null;
    [ExportGroup("AnimationNode")]
    [Export] public Panel Meun = null;
    [Export] public ColorRect Backdrop = null;
    [Export] public Label Meun_Title = null;
    [Export] public Panel Meun_Hollow = null;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }
    public override void _Ready() {
        base._Ready();
        Game.Get_GlobalNode.TimeScale.TimeScaleType = "Stop";
        Start();
        if (touchPad != null)
        {
            touchPad.Button_Pressedvoid += Exit;
        }
    }
    public async void Start()
    {
        Backdrop.Modulate = new Color(1,1,1,0);
        Meun.OffsetTransformPosition = new Vector2(0,60);
        Meun.Modulate = new Color(1,1,1,0);
        var Settings = Meun_Title.LabelSettings;
        Settings.FontSize = 40;
        Meun_Title.Position = new(106.5f,27.5f);
        Meun_Hollow.Size = new(393,369);
        Meun_Hollow.Position = new(10,90.5f);
        Tween Tw = CreateTween().SetParallel(true);
        Tw.SetIgnoreTimeScale(true);
        Tw.TweenProperty(Backdrop,new NodePath(ColorRect.PropertyName.Modulate),new Color(1,1,1,1),0.5f).SetTrans(Game.Get.TweenType.GetTweenType(Game.Get.TweenType.Twee.正弦));
        Tw.Parallel().TweenProperty(Meun,new NodePath(Panel.PropertyName.OffsetTransformPosition),new Vector2(0,0),0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.三次));
        Tw.Parallel().TweenProperty(Meun,new NodePath(Panel.PropertyName.Modulate),new Color(1,1,1,1),0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.三次));
        Tw.Parallel().TweenProperty(Settings,new NodePath(LabelSettings.PropertyName.FontSize),16,0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.五次方));
        Tw.Parallel().TweenProperty(Meun_Title,new NodePath(Label.PropertyName.Position),new Vector2(-44,2.5f),0.6f).SetTrans(TweenType.GetTweenType(TweenType.Twee.三次));
        Tw.Parallel().TweenProperty(Meun_Hollow,new NodePath(Label.PropertyName.Position),new Vector2(10,37.5f),0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.二次抛物线));
        Tw.Parallel().TweenProperty(Meun_Hollow,new NodePath(Label.PropertyName.Size),new Vector2(393,422),0.75f).SetTrans(TweenType.GetTweenType(TweenType.Twee.四次方));
    }
    public async void Exit()
    {
        touchPad.Enable = false;
        NodeData.Get_Node<LevelUi>("Current_Level_UI").Stop_Menu = null;
        Game.Get_GlobalNode.TimeScale.TimeScaleType = "Normal";
        var Settings = Meun_Title.LabelSettings;
        Tween Tw = CreateTween().SetParallel(true);
        Tw.SetIgnoreTimeScale(true);
        Tw.TweenProperty(Backdrop,new NodePath(ColorRect.PropertyName.Modulate),new Color(1,1,1,0),0.5f).SetTrans(Game.Get.TweenType.GetTweenType(Game.Get.TweenType.Twee.正弦));
        Tw.Parallel().TweenProperty(Meun,new NodePath(Panel.PropertyName.OffsetTransformPosition),new Vector2(0,60),0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.三次));
        Tw.Parallel().TweenProperty(Meun,new NodePath(Panel.PropertyName.Modulate),new Color(1,1,1,0),0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.三次));
        Tw.Parallel().TweenProperty(Settings,new NodePath(LabelSettings.PropertyName.FontSize),40,0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.五次方));
        Tw.Parallel().TweenProperty(Meun_Title,new NodePath(Label.PropertyName.Position),new Vector2(106.5f,27.5f),0.6f).SetTrans(TweenType.GetTweenType(TweenType.Twee.三次));
        Tw.Parallel().TweenProperty(Meun_Hollow,new NodePath(Label.PropertyName.Position),new Vector2(10,90.5f),0.5f).SetTrans(TweenType.GetTweenType(TweenType.Twee.二次抛物线));
        Tw.Parallel().TweenProperty(Meun_Hollow,new NodePath(Label.PropertyName.Size),new Vector2(393,369),0.75f).SetTrans(TweenType.GetTweenType(TweenType.Twee.四次方));
        await Task.Delay(1500);
        ThisFree();
    }
    public void ThisFree()
    {
        GetNode<Node>("..").QueueFree();
    }
}