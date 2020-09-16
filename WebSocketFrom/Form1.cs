
using Fleck;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebSocketFrom
{
    public partial class Form1 : Form
    {

       
        private delegate void FlushClient(); //代理   
    
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

                    
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8181");               
            server.Start(socket =>
            {
                //连接事件
                socket.OnOpen = () =>
                {
                    FlushClient fc = new FlushClient(text);
                    this.BeginInvoke(fc);//调用代理                  
                    socket.Send(string.Format("{0}{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), "已连接"));
                };
                socket.OnClose = () => MessageBox.Show("Close!");
                socket.OnMessage = (message) =>
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    this.txtLog.Text = this.txtLog.Text + message;
                    //逻辑处理 
                    socket.Send("服务端已接收到消息拉！！！！！");
                };
            });            
        }
        public void text()
        {
            this.txtLog.Text = this.txtLog.Text+ string.Format("{0}{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), "已连接");
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
           
        }
    }
}
