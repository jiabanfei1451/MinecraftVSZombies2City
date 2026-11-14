using Godot;
using PVZEngine.Data;
using PVZEngine.Object.Module;
using System;

public partial class Follow_Mouse : Control
{
	public GlobalData TempData = new();
	public override void _Process(double delta) {
		base._Process(delta);
		if (PVZEngine.Static.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object != null){
			if (TempData.Scene != PVZEngine.Static.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data.Scene)
			{
				TempData = PVZEngine.Static.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
				foreach(Node No in GetChildren())
				{
					No.QueueFree();
				}
				LevelObject Instan = TempData.Scene.Instantiate<LevelObject>();
				Instan.Enable = false;
				Instan.Enable_Health = false;
				Instan.Enable_Placed_Reliant = false;
				Instan.Physics_Enable = false;
				AddChild(Instan);
				Instan.Position = TempData.Mouse_Offset;
				Instan.Scale = TempData.Mouse_Scale;
			}
			Position = Game.Get_GlobalNode.InputPosition.Global_Position;
			Visible = true;
		}
		else
		{
			Visible = false;
		}
	}
}
