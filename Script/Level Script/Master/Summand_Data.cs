using System.Collections.Generic;
using Level.Enum;
using MVZ2_City.Type;
using SaveData;
using Level.Object;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Godot;
using Level.Module;
using System;
using System.Text.Json;
using Game.Cheak;
namespace Data.Level;
/// <summary>
/// 生成数据
/// </summary>
public class Summand_Data
{
    /// <summary>
    /// 生成物体ID
    /// </summary>
    public List<List<MVZ2_City.Type.ID>> Summand_IDs {get;set;} = new List<List<ID>>(){};
    /// <summary>
    /// 生成数量
    /// </summary>
    public List<int> Summand_Number {get;set;} = new(){};
    /// <summary>
    /// 是否为大波
    /// </summary>
    public List<bool> Is_Wave {get;set;} = new(){};
    /// <summary>
    /// 当前进攻波标签
    /// </summary>
    public List<WaveTag> WaveTags {get;set;} = new(){};
    /// <summary>
    /// 提前下一波倒计时
    /// </summary>
    public List<float> Next_Wave_Timers {get;set;} = new(){};
    /// <summary>
    /// 是否等待所有怪物死亡
    /// </summary>
    public List<bool> Await_Is_Monster_Kills {get;set;} = new(){};
    /// <summary>
    /// 指定标签
    /// </summary>
    public List<MVZ2_City.Type.ID> Detection_IDs = new(){};
    /// <summary>
    /// 在场怪物
    /// </summary>
    public List<LevelObject> Present_Object {get;set;} = new(){};
    /// <summary>
    /// 当前进度
    /// </summary>
    public int Current_Summand_Value {get;set;} = 0;
    /// <summary>
    /// 最大进度
    /// </summary>
    public int Max_Summand_Value {get;set;} = 0;
    /// <summary>
    /// 循环模式
    /// </summary>
    public static MVZ2_City.Type.WhileMode whileMode {get;set;} = WhileMode._While;
    /// <summary>
    /// 启用延迟生成
    /// </summary>
    public bool delay_Summand {get;set;} = false;
    /// <summary>
    /// 延迟生成秒数
    /// </summary>
    public float delay_SummandTime {get;set;} = 0.02f;
    /// <summary>
    /// 生成草坪行范围
    /// </summary>
    public SaveData.Struct.Vector2i YPosition_Offset {get;set;} = new(){X=0,Y=4};
    /// <summary>
    /// 生成坐标
    /// </summary>
    public SaveData.Struct.Vector2 Summand_Position {get;set;} = new()
    {
        X = 80 * 9,
        Y = 0
    };
    /// <summary>
    /// 生成列表
    /// </summary>
    public List<ID> Summand_List {get;set;} = null;
    /// <summary>
    /// 当前波次标签
    /// </summary>
    public WaveTag Current_WaveTag {get;set;} = WaveTag.Normal;
    /// <summary>
    /// 最大生成数量
    /// </summary>
    public int Max_Summand_Number {get;set;} = -1;
    /// <summary>
    /// 已生成数量
    /// </summary>
    public int Current_Summand_Number {get;set;} = -1;
    /// <summary>
    /// 等待下一波倒计时
    /// </summary>
    public float Await_Next_Wave_Timer {get;set;} = 0;
    /// <summary>
    /// 最大计时时间
    /// </summary>
    public float Max_Next_Wave_Timer {get;set;} = 45;
    /// <summary>
    /// 生成中
    /// </summary>
    public bool Summand_ing {get;set;} = false;
    /// <summary>
    /// 排除空值时间
    /// </summary>
    public float Cheak_NullObject_Timer {get;set;} = 1;
    /// <summary>
    /// 等待是否怪物死亡
    /// </summary>
    public bool Await_AllMonster_IsKill {get;set;} = false;
    /// <summary>
    /// 是否为最后一波
    /// </summary>
    public bool Is_Final_Wave {get;set;} = false;
    /// <summary>
    /// 允许胜利,当前波标签为Boss时此标签为true可以直接胜利
    /// </summary>
    public bool Allow_victory {get;set;} = false;
    /// <summary>
    /// 随机值生成器
    /// </summary>
    public Godot.RandomNumberGenerator random = new();
    /// <summary>
    /// 种子
    /// </summary>
    public ulong Seed {get;set;} = 0;
    /// <summary>
    /// 随机生成器先前状态
    /// </summary>
    public ulong State {get;set;} = 0;
    public void Remove_present_Object(LevelObject Object)
    {
        Present_Object.Remove(Object);
    }
    /// <summary>
    /// 添加波次
    /// </summary>
    /// <param name="Add_ID">添加物体ID</param>
    /// <param name="Summand_Number">生成数量</param>
    /// <param name="waveTag">波次标签</param>
    /// <param name="Is_Wave">是否为大波</param>
    public void Add_Wave(Add_WaveData waveData)
    {
        Summand_IDs.Add(waveData.Add_ID);
        DEBUG.Info.Print(waveData.Add_ID.Count);
        Summand_Number.Add(waveData.Number);
        WaveTags.Add(waveData.waveTag);
        Is_Wave.Add(waveData.IsWave);
        Await_Is_Monster_Kills.Add(waveData.Await_Monster_Kill);
        if (waveData.Next_Wave_Timer == -1)
        {
            Next_Wave_Timers.Add(Max_Next_Wave_Timer);
        }
        else
        {
            Next_Wave_Timers.Add(waveData.Next_Wave_Timer);
        }
        DEBUG.Info.Print("已添加");
        Max_Summand_Value += 1;
    }
    /// <summary>
    /// 获取波次
    /// </summary>
    /// <param name="Index"></param>
    public void Get_Wave(int Index)
    {
        if (Index < 0){return;}
        if (Index >= Summand_IDs.Count){return;}
        Max_Summand_Number = Summand_Number[Index];
        Summand_List = Summand_IDs[Index];
        if (Index >= Summand_IDs.Count - 1)
        {
            Is_Final_Wave = true;
        }
        Current_WaveTag = WaveTags[Index];
        Await_AllMonster_IsKill = Await_Is_Monster_Kills[Index];
        Await_Next_Wave_Timer = Next_Wave_Timers[Index];
        if (Await_Next_Wave_Timer >= Max_Next_Wave_Timer)
        {
            Await_Next_Wave_Timer = Max_Next_Wave_Timer;
        }
    }
    public void Remove_Null_Object()
    {
        List<LevelObject> levelObjects = new();
        foreach(var i in Present_Object)
        {
            if (!Cheak.Cheak_Null<LevelObject>(i))
            {
                levelObjects.Add(i);
            }
        }
        Present_Object = levelObjects;
    }
    /// <summary>
    /// 循环减少时间
    /// </summary>
    /// <returns></returns>
    public async Task While_Reduce_Timer()
    {
        if (whileMode == WhileMode._While)
        {
            while (Await_Next_Wave_Timer > 0 && whileMode == WhileMode._While)
            {
                Await_Next_Wave_Timer -= 1f / 60f * (float)Engine.TimeScale;
                await Task.Delay((int)(1000 / 60));
            }
        }
        return ;
    }
    /// <summary>
    /// 检测场上剩余怪物死否死亡
    /// </summary>
    /// <returns></returns>
    public bool Cheak_AllMonster_Kill()
    {
        if (Present_Object.Count <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 读取随机生成器种子
    /// </summary>
    public void Load_Random()
    {
        random.Seed = Seed;
        random.State = State;
    }
    /// <summary>
    /// 保存随机生成器种子
    /// </summary>
    public void Save_Random()
    {
        Seed = random.Seed;
        State = random.State;
    }
    /// <summary>
    /// 创建随机种子
    /// </summary>
    public void Create_Random_Seed()
    {
        random.Seed = (ulong)new System.Random().Next(0,210000000);
    }
    /// <summary>
    /// 添加波次属性:
    /// add_ID Number 这些属性为必填项
    /// </summary>
    public struct Add_WaveData()
    {
        /// <summary>
        /// 添加波次ID
        /// </summary>
        public List<ID> Add_ID;
        /// <summary>
        /// 生成数量
        /// </summary>
        public int Number = 0;
        /// <summary>
        /// 波次标签
        /// </summary>
        public WaveTag waveTag = WaveTag.Normal;
        /// <summary>
        /// 是否为大波
        /// </summary>
        public bool IsWave = false;
        /// <summary>
        /// 是否等待所有怪物死亡
        /// </summary>
        public bool Await_Monster_Kill = true;
        /// <summary>
        /// 进入下一波计时
        /// </summary>
        public float Next_Wave_Timer = -1;
    }
}