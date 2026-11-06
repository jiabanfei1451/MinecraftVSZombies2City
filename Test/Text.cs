using Godot;
using PVZEngine.Enum;
using PVZEngine.Object.Module;
using PVZEngine.Data;
using PVZEngine.Type;
using System.Text.Json;
using System;
using System.Threading.Tasks;

public partial class Text : Node2D
{
    
    public override async void _Ready() {
        base._Ready();
        await Task.Delay(100);
        var d = PVZEngine.Static.FileSystem.Handle_JsonText(JsonSerializer.Serialize<PVZEngine.Data.PlayerData.Data>(PlayerData.Savedata));
        GD.Print(d);
        var w = JsonSerializer.Deserialize<PVZEngine.Data.PlayerData.Data>(d);
        GD.Print(w.Version);
    }
}

