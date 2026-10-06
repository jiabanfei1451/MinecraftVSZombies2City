using Godot;
using Level.Enum;
using MVZ2_City.Type;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public partial class Text : Node2D
{
    
    public override void _Ready() {
        base._Ready();
        PackedScene scene = GD.Load<PackedScene>("uid://lyb0noko5sk1");
        MVZ2.Object.Equipment.Equipment equipment = scene.Instantiate<MVZ2.Object.Equipment.Equipment>();
        GD.Print("CH:地雷出土"[3..]);
        Data.Level.Summand_Data d = new()
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
        var dd = JsonSerializer.Deserialize<Data.Level.Summand_Data>(sd);
        GD.Print(dd.Summand_Position);
    }
}

