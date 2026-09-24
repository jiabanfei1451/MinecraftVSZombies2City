using Godot;
using DEBUG;
using System.Threading.Tasks;
namespace Level;
/// <summary>
/// 序章
/// </summary>
public partial class Preface : Level_Master_Script
{
    public override async void _Ready() {
        base._Ready();
        Static.Summand.While_Mode = Static.Summand.WhileMode.While;
        Game.WindowTool.Set_Title(10, Tween.TransitionType.Circ,"MVZ2_City");
        choose_Card();
        MVZ2_City.Object_List list = Game.Get_GlobalNode.object_List; 
        await ToSignal(GetTree().CreateTimer(6),SceneTreeTimer.SignalName.Timeout);
        Static.Summand.Add_wave(new System.Collections.Generic.List<MVZ2_City.Type.ID>(){list.Get_ID("0")},1,10,true);
        Static.Summand.Add_wave(new System.Collections.Generic.List<MVZ2_City.Type.ID>(){list.Get_ID("0")},1,10,false);
        Static.Summand.Add_wave(new System.Collections.Generic.List<MVZ2_City.Type.ID>(){list.Get_ID("0")},1,10,true);
        Static.Summand.Add_wave(new System.Collections.Generic.List<MVZ2_City.Type.ID>(){list.Get_ID("0")},1,10,true);
        Static.Summand.Start_Timer = 15;

        Static.Summand._Ready();
    }
}
