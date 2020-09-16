using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using RabbitFactory;
using System.Text;

namespace WebSockets.Controllers
{
    
    public class MyScoketQLController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Connect(string userName)
        {
            //在服务器端接受Web Socket请求，传入的函数作为Web Socket的处理函数，待Web Socket建立后该函数会被调用，在该函数中可以对Web Socket进行消息收发
            HttpContext.Current.AcceptWebSocketRequest(ProcessRequest);

            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols); //构造同意切换至Web Socket的Response.    
        }

        public async Task ProcessRequest(AspNetWebSocketContext context)
        {
            CancellationToken cancellationToken = new CancellationToken();

            WebSocket socket = context.WebSocket;

            var username = context.QueryString["userName"];
            string procMsg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + username + ": 加入聊天室";

            await WebScoketsManage.SendMsg(procMsg, cancellationToken);

            WebScoketsManage.AddUserWs(username, socket);

            //连接状态
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    WebScoketsManage.RemoveUserWS(username);
                    string outMsg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + username + ": 退出聊天室";
                    await WebScoketsManage.SendMsg(outMsg, cancellationToken);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, cancellationToken);
                }
                else
                {
                    string msg = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    string sayMsg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") +"  "+ username + ":" + msg;
                    await WebScoketsManage.SendMsg(sayMsg, cancellationToken);
                }
            }
        }
    }
}
