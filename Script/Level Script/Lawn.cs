using Godot;
using System;
namespace Level{
public partial class Lawn : ColorRect
	{
		[Signal]
		public delegate void ME_JoinEventHandler(ColorRect This);
		[Signal]
		public delegate void ME_ExitEventHandler(ColorRect This);
		[Export] public TouchPad pad;
		public override void _Ready() {
			base._Ready();
			pad = GetNode<TouchPad>("TouchPad");
			pad.Focus_Joinvoid += focus_Join;
			pad.Focus_Exitvoid += focus_exit;
		}
		public void focus_Join()
		{
			EmitSignal("ME_Join",this);
		}
		public void focus_exit()
		{
			EmitSignal("ME_Exit",this);
		}
	}
}