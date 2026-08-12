using Godot;


public partial class CanvasLayer : Godot.CanvasLayer
{
    public override void _Ready() {
        base._Ready();
        GD.Print(1);
        GetNode<Godot.Button>("Server").Pressed += Start_Server;
        GetNode<Godot.Button>("Player").Pressed += Client_Server;
        GetNode<Godot.Button>("Send").Pressed += Player_Send_Data;
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        NETDNS.Server.IPs = GetNode<Godot.LineEdit>("IP").Text;
        NETDNS.Server.Port = int.Parse(GetNode<Godot.LineEdit>("Port").Text);
        if (NETDNS.Server.Client != null)
        {
            GetNode<Label>("Community").Text = $"连接状态:{NETDNS.Server.Client.Connected}";
        }
        if (NETDNS.Server.TCPServer != null)
        {
            GetNode<Label>("Community2").Text = "当前状态:房主";
        }
        else
        {
            if (NETDNS.Server.Client != null && NETDNS.Server.Client.Connected){
            GetNode<Label>("Community2").Text = "当前状态:房员";
            }
            else
            {
                GetNode<Label>("Community2").Text = "当前状态:无";
            }
        }
    }
    public void Start_Server()
    {
        NETDNS.Server.Create_Server();
    }
    public void Client_Server()
    {
        NETDNS.Server.Join_Server();
    }
    public void Player_Send_Data()
    {
        NETDNS.Server.Player_Send_Data("Test");
    }
}
