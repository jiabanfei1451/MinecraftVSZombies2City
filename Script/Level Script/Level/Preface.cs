using Game;
using Godot;
using Level;
using System;
using System.Threading.Tasks;

namespace Level;
/// <summary>
/// 序章
/// </summary>
public partial class Preface : Level_Master_Script
{
    public override async void _Ready() {
        
        base._Ready();
        await ToSignal(GetTree().CreateTimer(3),SceneTreeTimer.SignalName.Timeout);
        choose_Card();
    }
}
