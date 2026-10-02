using Godot;
using System;
using System.IO;
using System.Reflection;

namespace Game.AutoLoad;
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
		Add_Muisc(Load.Load_External_Audio_File( Load.File_Type.ogg,"E:/UT入/ink/ink_bgm1.ogg"),"Ink_Phase1","File:ink");
		Add_Muisc("uid://dmsb1maakx8lg","绝命矿坑","MVZ2_City:KillStone_Final");
		Add_Souds("uid://doqxxw08vbj5i","好,准备,安放器械","MVZ2:Ready");
		Add_Souds("uid://d4ljnli5u46x6","受击","MVZ2:Splat_1");
		Add_Souds("uid://cj01m2icn8jfh","受击2","MVZ2:Splat_2");
		Add_Souds("uid://bqfk5s3oirsy1","受击3","MVZ2:Splat_3");
		Add_Souds("uid://b4lik07h6ep5d","僵尸死亡","MVZ2:Zombies_Kill");
		Add_Souds("uid://casidmhjb6xn8","拿起稿子","MVZ2:Shovel");
		Add_Souds("res://Sound/Souds/UI/card/buzzer.wav","警告","MVZ2:buzzer");
		Add_Souds("res://Sound/Souds/UI/card/Selected.ogg","选卡","MVZ2:Selected_Card");
		Add_Souds("uid://dqqmyotd4p22f","取消","MVZ2:Cancel");
		Add_Souds("res://Sound/Souds/Equipment/Landmine/Unearthed.ogg","地雷出土","MVZ2:Landmine_Unearthed");
		Add_Souds("res://Sound/Souds/Equipment/Landmine/Landmine Boom.ogg","地雷爆炸","MVZ2:Landmine_Boom");
		Add_Souds("res://Sound/Souds/misc/evocation.ogg","激发器械","MVZ2:evocation");
		Add_Souds("res://Sound/Souds/UI/Minecraft_UI/Button/MC_click.ogg","MC按钮按下","MVZ2:MC_Click");
		Add_Souds("res://Sound/Souds/UI/UpspeedUI/Add_Speed.wav","加速","MVZ2:Add_Speed");
		Add_Souds("res://Sound/Souds/UI/UpspeedUI/Recede_Speed.wav","减速","MVZ2:Recede_Speed");
			#region 草方块挖掘
		Add_Souds("res://Sound/Souds/Equipment Hit/Grass/grass1.wav","草被挖掘","MVZ2:Grass_mining");
		Add_Souds("res://Sound/Souds/Equipment Hit/Grass/grass2.wav","草被挖掘2","MVZ2:Grass_mining2");
		Add_Souds("res://Sound/Souds/Equipment Hit/Grass/grass3.wav","草被挖掘3","MVZ2:Grass_mining3");
			#endregion
			#region 泥块被挖掘
		Add_Souds("res://Sound/Souds/Equipment Hit/gravel/break1.ogg","泥块被挖掘","MVZ2:break_mining");
		Add_Souds("res://Sound/Souds/Equipment Hit/gravel/break2.ogg","泥块被挖掘2","MVZ2:break_mining2");
		Add_Souds("res://Sound/Souds/Equipment Hit/gravel/break3.ogg","泥块被挖掘3","MVZ2:break_mining3");
		Add_Souds("res://Sound/Souds/Equipment Hit/gravel/break4.ogg","泥块被挖掘4","MVZ2:break_mining4");
			#endregion
			#region 石质物体被挖掘
		Add_Souds("res://Sound/Souds/Equipment Hit/Stone Mining/stone1.ogg","石材质物体被挖掘","MVZ2:Stone_mining");
		Add_Souds("res://Sound/Souds/Equipment Hit/Stone Mining/stone2.ogg","石材质物体被挖掘2","MVZ2:Stone_mining2");
		Add_Souds("res://Sound/Souds/Equipment Hit/Stone Mining/stone3.ogg","石材质物体被挖掘3","MVZ2:Stone_mining3");
		Add_Souds("res://Sound/Souds/Equipment Hit/Stone Mining/stone4.ogg","石材质物体被挖掘4","MVZ2:Stone_mining4");
			#endregion
		Add_Souds("res://Sound/Souds/Bullet/Shoot.ogg","发射射弹","MVZ2:Shoot");
		Add_Souds("res://Sound/Souds/Equipment/Furnace/Stimulate production.ogg","激发生产","MVZ2:Stimulate_production");
		Add_Souds("res://Sound/Souds/Game/Level/Wave.ogg","一大波","MVZ2:Wave");
		Add_Souds("res://Sound/Souds/Game/Level/Final_Wave.ogg","最后一波","MVZ2:Final_Wave");
		Add_Souds("res://Sound/Souds/Item/RedStone/points.wav","拾取红石","MVZ2:Points");
		Add_Souds("res://Sound/Souds/Item/RedStone/RedStone_Output.ogg","产出红石","MVZ2:RedStone_Output");
		Add_Souds("res://Sound/Souds/UI/Level_Button/spring.ogg","关卡按钮按下","MVZ2:Spring");
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
	/// 直接添加音频
	/// </summary>
	/// <param name="FilePath"></param>
	/// <param name="Name"></param>
	/// <param name="Internal_Name"></param>
	public void Add_Muisc(AudioStream FilePath,String Name,String Internal_Name)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		Muisc_List[0].Add(Name);
		Muisc_List[1].Add(Internal_Name);
		Muisc_List[2].Add(FilePath);
	}
	/// <summary>
	/// 加载音频
	/// </summary>
	public static class Load
	{
		public enum File_Type
		{
			/// <summary>
			/// Wav音频文件
			/// </summary>
			wav = 0,
			/// <summary>
			/// Ogg音频文件
			/// </summary>
			ogg = 1,
			/// <summary>
			/// Mp3音频文件
			/// </summary>
			mp3 = 2
		}
		/// <summary>
		/// 加载外部音频
		/// </summary>
		/// <param name="fileType"></param>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		public static AudioStream Load_External_Audio_File(File_Type fileType,String FilePath)
		{
			AudioStream Stream = null;
			switch (fileType)
			{
				case File_Type.mp3:
				Stream = AudioStreamMP3.LoadFromFile(FilePath);
				break;
				case File_Type.ogg:
				Stream = AudioStreamOggVorbis.LoadFromFile(FilePath);
				break;
				case File_Type.wav:
				Stream = AudioStreamWav.LoadFromFile(FilePath);
				break;
			}
			return Stream;
		}
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
