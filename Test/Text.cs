using Godot;
using PVZEngine.Enum;
using PVZEngine.Object.Module;
using PVZEngine.Data;
using PVZEngine.Type;
using System.Text.Json;

public partial class Text : Node2D
{
    
    public override void _Ready() {
        base._Ready();
        PackedScene scene = GD.Load<PackedScene>("uid://lyb0noko5sk1");
        Equipment equipment = scene.Instantiate<Equipment>();
        GD.Print("CH:地雷出土"[3..]);
        Summand_Data d = new()
        {
        };
        d.Add_Wave(new()
        {
            Add_ID = new()
            {
                new ID()
                {
                    Object_ID = 1,
                    Object_Name_ID = "",
                    CH_Name = ""
                }
            },
            Number = 1,
            Await_Monster_Kill = true,
            IsWave = false,
            waveTag =  WaveTag.Normal,
            Next_Wave_Timer = 45
        });
        d.Save_Random();
        var sd = JsonSerializer.Serialize(d);
        GD.Print(sd);
        var dd = JsonSerializer.Deserialize<Summand_Data>(sd);
        GD.Print(dd.Summand_Position);
    }
}

