using Godot;
using System;
using System.Diagnostics;

public partial class Selected_slot : GridContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		int dex = 0;
		foreach (Control node in GetChildren())
		{
			if (dex <= Game.AutoLoad.PlayerData.Card_Quantity)
			{
				node.Visible = true;
			}
			else
			{
				node.Visible =false;
			}
			dex += 1;
		}
	}
}
