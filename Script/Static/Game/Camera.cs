
using System.Threading.Tasks;
using Godot;

namespace Game.Static;
public static class Camera
{
    /// <summary>
    /// 镜头震动
    /// </summary>
    public static async void Camera_Vibration(Godot.Vector2 offset,int whileNumber,float AwaitTime)
    {
        for (int i = 0;i < whileNumber;i++){
            float tx = Game.Get.Random.NextFloat_32(0,offset.X);
            float x = -Game.Get.Random.NextFloat_32(0,offset.X) + tx;
            float ty = Game.Get.Random.NextFloat_32(0,offset.Y);
            float y = -Game.Get.Random.NextFloat_32(0,offset.Y) + ty;
            Game.Level_Script.Level_Object.Camera2D_Offset = new Godot.Vector2(x,y);
            await Task.Delay((int)(AwaitTime * 1000 / Engine.TimeScale));
        }
        Game.Level_Script.Level_Object.Camera2D_Offset = new Godot.Vector2(0,0);
    }
}