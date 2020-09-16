using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace WebSockets.Controllers
{
    [RoutePrefix("api/chat")]
    public class MySocketController : ApiController
    {


        [Route]
        [HttpGet]
        public HttpResponseMessage Connect()
        {
            //在服务器端接受Web Socket请求，传入的函数作为Web Socket的处理函数，待Web Socket建立后该函数会被调用，在该函数中可以对Web Socket进行消息收发
            HttpContext.Current.AcceptWebSocketRequest(ProcessRequest);

            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols); //构造同意切换至Web Socket的Response.    
        }

        public async Task ProcessRequest(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;//传入的context中有当前的web socket对象

            CancellationToken cancellationToken = new CancellationToken();

            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
                else
                {
                    string msg = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    string procMsg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "你发送了" + msg;                 
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(procMsg));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
                }
            }
        }
    }
}
