using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Civet.Server
{
    public class Server
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Server端启动");
            //创建一个socket对象,三个参数分别代表：
            //AddressFamily.InterNetwork: IPV4协议，SocketType.Stream：流类型，ProtocolType.Tcp:TCP方式连接
            Socket ss = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //获取当前机器的IP地址，在C#中用IPAddress这个类去存放IP地址
            //foreach (var item in Dns.GetHostAddressesAsync(Dns.GetHostName()).Result)
            //{
            //    Console.WriteLine(item.ToString());
            //}
            IPAddress ipa = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result[3];

            //IPEndPoint是用来把IP地址和端口号集成在一起的一个类型，在C#中叫做“网络端点”
            //我这里假设服务器开的端口号是11000，这个是自己随意设置的。不是唯一
            IPEndPoint iep = new IPEndPoint(ipa, 11000);

            //之前创建的Socket与我们本机的IP和所设的端口号绑定
            ss.Bind(iep);

            //进行监听，也就是说我们开始侦听网络上对本机IP和11000这个端口进行连接的信息了
            //参数列表官方定义为：挂起连接队列的最大长度。这个稍后单独说。
            ss.Listen(50);

            byte[] bMessage = new byte[1024 * 10];
            string sMsg = "Can I help you ?";
            //当ss这个用于监听的socket收到一个连接请求之后，会接受对方请求，并建立一个新的连接
            //而新的这个s就是接下来用于真正通信的socket了。
            Socket s = ss.Accept();
            while (true)
            {
                try
                {
                    //bMessage = System.Text.Encoding.BigEndianUnicode.GetBytes(sMsg.ToCharArray());
                    //s.Send(bMessage);
                    //bMessage = null;
                    //顾名思义啦，这是一个接收信息的方法，把通过网络传过来的流存入byte数组中去。
                    //之所以把它写在这里是因为我的设计之初是，当socket连通成功之后，Client端会首先给Server端发一个信息。
                    s.Receive(bMessage);
                    //用于把byte转成string的一个方法，注意我用的是Unicode，通常我们还可以用UTF8，ASCII编解码
                    //之前最坑的是当时不知道在C#下如何转，在网上看到了一个用BigEndianUnicode编码的，
                    //当时没有想太多就使了，结果就是解出来各种乱码的样子，这块跟大伙提一下
                    sMsg = System.Text.Encoding.UTF8.GetString(bMessage);
                    Console.WriteLine("Client(" + DateTime.Now.ToString() + "):" + sMsg);
                    //接下来就是输入一个字符串，并把其转成byte数组，然后Send出去。
                    //bMessage = System.Text.Encoding.UTF8.GetBytes(Console.ReadLine().ToCharArray());
                    //s.Send(bMessage);
                }
                catch (System.Exception ex)
                {

                }
            }

        }
    }
}
