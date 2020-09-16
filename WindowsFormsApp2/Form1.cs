using SuperWebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {    
        //socke服务
        private WebSocketServer ws = null;

        private static ConcurrentDictionary<string, WebSocketSession> _userWSDic = new ConcurrentDictionary<string, WebSocketSession>();

        public Form1()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            WebSocketConnect();
        }
        //建立连接
        public void WebSocketConnect()
        {
            try
            {
                
                ws = new WebSocketServer();

                if (!ws.Setup(this.txtIpurl.Text, Convert.ToInt32(this.txtprot.Text)))
                {
                    MessageBox.Show("开启服务器失败");
                    return;
                }
                if (!ws.Start())
                {
                    MessageBox.Show("开启服务器失败");
                    return;
                }

                MessageBox.Show("开启成功");

                //注册事件
                ws.NewSessionConnected += WsNewSessionConnected;
                ws.NewMessageReceived += WsNewMessageReceived;
                ws.SessionClosed += WsSessionClosed;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);             
            }
        }

        //连接事件
        public void WsNewSessionConnected(WebSocketSession session)
        {
            var path = session.Path.Split('?');

            var userName = path[1];

            _userWSDic[userName] = session;

            string procMsg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")+" " + userName + ": 加入聊天室";

            foreach (var item in _userWSDic.Values)
            {
                item.Send(procMsg);
            }     
        }

        //收到消息事件
        public void WsNewMessageReceived(WebSocketSession session, string values)
        {
            var path = session.Path.Split('?');
            var userName = path[1];
            var msgs = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "  " + userName + ":" + values;
            foreach (var item in _userWSDic.Values)
            { 
                item.Send(msgs);
            }
        }

        public void WsSessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            //MessageBox.Show("客户端已关闭:" + value);
            var path = session.Path.Split('?');
            var userName = path[1];
            var msgs = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") +" "+ userName + ": 退出聊天室";
            foreach (var item in _userWSDic.Values)
            {
                item.Send(msgs);
            }

            WebSocketSession socketSession = null;
            _userWSDic.TryRemove(userName, out socketSession);
        }
    }
}
