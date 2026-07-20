using Godot;
using System;

public partial class Audio_List : Node
{
	[Export] public Godot.Collections.Array<Godot.Collections.Array> Muisc_List = new Godot.Collections.Array<Godot.Collections.Array>()
	{new Godot.Collections.Array(){"主菜单","选卡","灾变行者"},
	new Godot.Collections.Array(){"MVZ2:Menu","MVZ2:Select_Blueprint","MVZ2_City:Cataclysm_Walker"},
	new Godot.Collections.Array(){GD.Load<AudioStream>("uid://bdjsd8tb6k4mm"), GD.Load<AudioStream>("uid://xkigyx7w0icw"),GD.Load<AudioStream>("uid://dvs4erog4jnkg")}};
	[Export] public float Muisc_Volume = 100;
	[Export] public float Souds_Volume = 100;
}
