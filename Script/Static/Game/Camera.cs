
using System.Threading.Tasks;
using Godot;
using PVZEngine.Module;
using PVZEngine.Static;
namespace Game.Static;
public static class Camera
{
    /// <summary>
    /// 镜头震动
    /// </summary>
    public static async void Camera_Vibration(Godot.Vector2 offset,int whileNumber,float AwaitTime)
    {
        for (int i = 0;i < whileNumber;i++){
            float tx = Random.NextFloat_32(0,offset.X);
            float x = -Random.NextFloat_32(0,offset.X) + tx;
            float ty = Random.NextFloat_32(0,offset.Y);
            float y = -Random.NextFloat_32(0,offset.Y) + ty;
            Level_Script.Level_Object.Camera2D_Offset = new Godot.Vector2(x,y);
            await Task.Delay((int)(AwaitTime * 1000 / Engine.TimeScale));
        }
        Level_Script.Level_Object.Camera2D_Offset = new Godot.Vector2(0,0);
    }
}