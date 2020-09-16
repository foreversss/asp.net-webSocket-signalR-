using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketNet
{
    class Program
    {
        static void Main(string[] args)
        {

            var server = new WebSocketServer("ws://127.0.0.1:8181");

            server.Start(socket =>
            {
                //打开连接
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    socket.Send("hello");
                };
                //关闭
                socket.OnClose = () => Console.WriteLine("Close!");

                //收到消息时
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    dynamic o = JsonConvert.DeserializeObject(message);

                    var pwd = o.pwd;

                    var uid = o.uid;
                    if (uid == "admin" && uid == "admin")
                    {
                        socket.Send("login success111111");
                    }
                    else
                    {
                        socket.Send("login fail");
                    }
                };
            });
            Console.ReadLine();
        }
    }

    public class user
    {
        public string uid { get; set; }
        public string pwd { get; set; }
    }
}
