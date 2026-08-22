using Godot;
using DEBUG;
namespace Level;
/// <summary>
/// 序章
/// </summary>
public partial class Preface : Level_Master_Script
{
    public override async void _Ready() {
        base._Ready();
        Game.WindowTool.Set_Title("114514");
        choose_Card();
        await ToSignal(GetTree().CreateTimer(6),SceneTreeTimer.SignalName.Timeout);
        DEBUG.Info.Save_Info("C:/Info.txt");
    }
}
