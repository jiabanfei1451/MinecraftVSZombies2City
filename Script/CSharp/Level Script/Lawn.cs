using Godot;
using Touch;
using Game;
using GameUI;
namespace Level{
public partial class Lawn : ColorRect
	{
		[Signal]
		public delegate void ME_JoinEventHandler(Lawn This);
		[Export] public Vector2I ArrayPosition = new Vector2I();
		[Export] public TouchPad pad;
		[Export] public Godot.Vector2 Array2D_Position = Godot.Vector2.Zero;
		public override void _Ready() {
			base._Ready();
			Get_GlobalNode.Get_Card_Data(GetTree()).Selected_Change += Card_Change;
			pad = GetNode<TouchPad>("TouchPad");
			pad.Focus_Joinvoid += focus_Join;
		}
		public void focus_Join()
		{
			Game.Level_Script.Lawn = this;
			EmitSignal("ME_Join",this);
		}
		public void Card_Change(Card card)
		{
			if (card == null)
			{
				for (int i = 0; i < GetChildCount(); i++)
				{
					if(i > 0)
					{
						GetChild(i).QueueFree();
					}
				}
				SelfModulate = new Color(0,0,0,0);
			}
			else
			{
				Color = new Color(1,1,1,0.2f);
				SelfModulate = new Color(1,1,1,1);
			}
		}
	}
}