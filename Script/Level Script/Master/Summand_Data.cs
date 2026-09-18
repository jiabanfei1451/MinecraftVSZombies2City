namespace Level.Static;
/// <summary>
/// 生成数据
/// </summary>
public static class Summand_Data
{
    /// <summary>
    /// 等待怪物死亡
    /// </summary>
    public static bool Await_AllMonster_Kill = false;
    /// <summary>
    /// 下一波等待时间
    /// </summary>
    public static double Await_Next_Time = 0;
    /// <summary>
    /// 最后一波
    /// </summary>
    public static bool Finale_Wave = false;
    /// <summary>
    /// 此关有Boss波,暂时不可触发过关战利品请求
    /// </summary>
    public static bool Boss_Wave = false;
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Initialization()
    {
        Await_AllMonster_Kill = false;
        Finale_Wave = false;
        Await_Next_Time = 0;
        Boss_Wave = false;
    }   
}