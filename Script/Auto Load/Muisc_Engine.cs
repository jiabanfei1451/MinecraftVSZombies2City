using Godot;
using My_Csharp_Node;
using System;
using System.IO;

namespace Game.AutoLoad;
/// <summary>
/// 音乐引擎
/// </summary>
public partial class Muisc_Engine : Node
{
	/// <summary>
	/// 播放状态
	///  Noplay = 无节点正在播放
	///  Play_Audio_1 = Audio_1正在播放音乐
	///  Play_Audio_2 = Audio_2正在播放音乐
	/// </summary>
	public enum Play_Status
	{
		NoPlay = 0,
		Play_Audio_1 = 1,
		Play_Audio_2 = 2
	}

	/// <summary>
	/// 播放节点_1
	/// </summary>
	[ExportGroup("Node")]
	[Export] public Audio_Plus Audio_1 = null;
	/// <summary>
	/// 播放节点_2
	/// </summary>
	[Export] public Audio_Plus Audio_2 = null;
	/// <summary>
	/// 淡入淡出时长
	/// </summary>
	[ExportGroup("Variant")]
	[Export] public float Fade_Time = 1;
	/// <summary>
	/// 音乐ID
	/// </summary>
	[Export] public String MuiscId = "0";
	/// <summary>
	/// 当前音乐Id
	/// </summary>
	[Export] public string Current_MuiscID = "MVZ2:Null";
	[Export] public Play_Status Status = Play_Status.NoPlay;
	public override void _Ready() {
		base._Ready();
		Initialization();
		new_playMuisc(MuiscId);
	}
	/// <summary>
	/// 初始化
	/// </summary>
	public void Initialization()
	{
		foreach (Node node in GetChildren())
		{
			node.QueueFree();
		}
		Audio_1 = new Audio_Plus();
		Audio_2 = new Audio_Plus();
		Audio_1.Autoplay = false;
		Audio_2.Autoplay = false;
		Audio_1.Stream = null;
		Audio_2.Stream = null;
		Audio_1.Add_Volume = 0;
		Audio_2.Add_Volume = 0;
		Audio_2.AudioList_Object = Game.Get_GlobalNode.Get_Audio_List(GetTree());
		Audio_1.AudioList_Object = Game.Get_GlobalNode.Get_Audio_List(GetTree());
		Audio_1.fade_Time = Fade_Time;
		Audio_2.fade_Time = Fade_Time;
		AddChild(Audio_2);
		AddChild(Audio_1);
	}
	/// <summary>
	/// 播放新音乐
	/// </summary>
	/// <param name="MuiscID"></param>
	public void new_playMuisc(String MuiscID)
	{
		switch (Status)
		{
			case Play_Status.NoPlay:
				Audio_1.Muisc_ID = MuiscID;
				Audio_1.Fade_Join();
				Audio_1.Autoplay = true;
				Audio_1.Playing = true;
				Status = Play_Status.Play_Audio_1;
				break;
			case Play_Status.Play_Audio_1:
				Audio_1.Fade_Exit();
				Audio_2.Fade_Join();
				Audio_2.Muisc_ID = MuiscID;
				Audio_2.Autoplay = true;
				Audio_2.Playing = true;
				Status = Play_Status.Play_Audio_2;
				break;
			case Play_Status.Play_Audio_2:
				Audio_2.Fade_Exit();
				Audio_1.Muisc_ID = MuiscID;
				Audio_1.Fade_Join();
				Audio_1.Autoplay = true;
				Audio_1.Playing = true;
				Status = Play_Status.Play_Audio_2;
				break;
		}
	}
}
