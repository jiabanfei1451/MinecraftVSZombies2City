using Godot;
using System;
using My_Csharp_Node;

namespace Monster_Audio;
public partial class ZombiesDamage : Audio_Plus
{
    public override void _Ready() {
        base._Ready();
    }
    public void change_Souds()
    {
        String[] MuiscName = new string[]{"MVZ2:Splat_1","MVZ2:Splat_2","MVZ2:Splat_3"};
        Audio_ID = MuiscName[new Random().Next(0,MuiscName.Length)];
        Stream = Game.Get_GlobalNode.Get_Audio_List(GetTree()).Get_Souds(Audio_ID);
        Play();
    }
}
