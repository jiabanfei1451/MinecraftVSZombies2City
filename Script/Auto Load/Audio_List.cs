using Godot;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

/// <summary>
/// Audio_List是负责存储预加载的音乐与音效,预计之后推出分片式预加载
/// </summary>
public partial class Audio_List : Node
{
	[Export] public Godot.Collections.Array<Godot.Collections.Array> Muisc_List = new Godot.Collections.Array<Godot.Collections.Array>()
	{
		// 音乐名称
		new Godot.Collections.Array(){
			"主菜单",
			"选卡",
			"灾变行者"
		},
		// 音乐内部名
		new Godot.Collections.Array(){
			"MVZ2:Menu",
			"MVZ2:Select_Blueprint",
			"MVZ2_City:Cataclysm_Walker"
		},
		// 音乐资源
		new Godot.Collections.Array(){
			GD.Load<AudioStream>("uid://bdjsd8tb6k4mm"),
			GD.Load<AudioStream>("uid://xkigyx7w0icw"),
			GD.Load<AudioStream>("uid://dvs4erog4jnkg")
		}
	};
	[Export] public Godot.Collections.Array Souds_list = new Godot.Collections.Array()
	{
		// 音效名称
		new Godot.Collections.Array<String>()
		{
			
		},
		// 音效内部名
		new Godot.Collections.Array<String>()
		{
			
		},
		// 音效资源
		new Godot.Collections.Array<AudioStream>()
		{
			
		}
	};
	[Export] public float Muisc_Volume = 100;
	[Export] public float Souds_Volume = 100;
	/// <summary>
	/// 使用字符串获取音效
	/// </summary>
	/// <param name="What"></param>
	/// <returns></returns>
	public override void _Ready() {
		base._Ready();
		#region 添加音效
		Add_Souds("uid://doqxxw08vbj5i","好,准备,安放器械","MVZ2:Ready");
		#endregion
	}
	/// <summary>
	/// 添加音乐
	/// </summary>
	/// <param name="FilePath"></param>
	/// <param name="Name"></param>
	/// <param name="Internal_Name"></param>
	public void Add_Muisc(String FilePath,String Name,String Internal_Name)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		AudioStream Addstream = GD.Load<AudioStream>(FilePath);
		Muisc_List[0].Add(Name);
		Muisc_List[1].Add(Internal_Name);
		Muisc_List[2].Add(Addstream);
	}
	/// <summary>
	/// 添加音效
	/// </summary>
	/// <param name="FilePath"></param>
	/// <param name="Name"></param>
	/// <param name="Internal_Name"></param>
	public void Add_Souds(String FilePath,String Name,String Internal_Name)
	{
		AudioStream Addstream = GD.Load<AudioStream>(FilePath);
		if (FilePath == "")
		{
			throw(new ArgumentOutOfRangeException("路径不能是空的"));
		}
		if (Name == "")
		{
			throw(new ArgumentOutOfRangeException("名称不能是空的"));
		}
		if (Internal_Name == "")
		{
			throw(new ArgumentOutOfRangeException("内部名称不能是空的"));
		}
		((Godot.Collections.Array<String>)Souds_list[0]).Add(Name);
		((Godot.Collections.Array<String>)Souds_list[1]).Add(Internal_Name);
		((Godot.Collections.Array<AudioStream>)Souds_list[2]).Add(Addstream);
	}
	public AudioStream Get_Souds(String What)
	{
		int ListIndex = -1;
		if (What == "")
		{
			throw(new ArgumentOutOfRangeException("字符串不能为空!"));
		}
		AudioStream audio;
		Godot.Collections.Array<AudioStream> SoudsResearch = (Godot.Collections.Array<AudioStream>)Souds_list[2];
		if (What.Find("CH:") != -1)
		{
			ListIndex = ((Godot.Collections.Array<String>)Souds_list[0]).IndexOf(What[3..]);
		}
		else
		{
			ListIndex = ((Godot.Collections.Array<String>)Souds_list[1]).IndexOf(What);
		}
		audio = SoudsResearch[ListIndex];
		if (ListIndex == -1)
		{
			throw(new ArgumentOutOfRangeException("当前索引不存在！请检查是否存在相关的中文名与内部名"));
		}
		return audio;
	}
	/// <summary>
	/// 使用索引获取音效
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	public AudioStream Get_Souds(int Index)
	{
		Godot.Collections.Array<AudioStream> SoudsResearch = (Godot.Collections.Array<AudioStream>)Souds_list[2];
		AudioStream audio;
		audio = SoudsResearch[Index];
		return audio;
	}

}
