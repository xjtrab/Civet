using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Civet.Client
{
    public class Client
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Client端启动");
            //同服务器端一样，需要创建一个socket
            Socket ss = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //这块主要就是把你想连接的目标主机IP地址进行一下解析，并存入IPAddress类型的一个实例当中
            IPAddress myIP = IPAddress.Parse("192.168.1.105");
            //这也同服务器是一个意思，把IP和端口号集成在一个网络端点中
            IPEndPoint ipe = new IPEndPoint(myIP, 11000);
            byte[] bMessage = null;
            string sMsg = "hello world";
            try
            {
                //这是客户端的一个方法，表示连接的对象就是参数的网络端点中的IP地址和端口号
                //但是注意这里不需要返回一个新的socket作为通信socket
                //而是进行连接的这个ss就是将来一直维持此次连接的socket，直到该通道关闭或断开
                ss.Connect(ipe);
                bMessage = System.Text.Encoding.UTF8.GetBytes(sMsg.ToCharArray());
                //send方法的返回值表示已发送到socket的字节数，就像我在server端说的那样
                //这个Demo的设计思路就是连通后，客户端先向服务器端发送一个信息
                for (int i = 0; i < 1000; i++)
                {
                    int count = ss.Send(bMessage);
                }

                //if (count > 0)
                //{
                //    while (true)
                //    {
                //        //bMessage = null;
                //        ss.Receive(bMessage);
                //        sMsg = System.Text.Encoding.UTF8.GetString(bMessage);
                //        Console.WriteLine("Server(" + DateTime.Now.ToFileTimeUtc() + "):" + sMsg);
                //        bMessage = System.Text.Encoding.UTF8.GetBytes(Console.ReadLine().ToCharArray());
                //        ss.Send(bMessage);
                //    }
                //}

                Console.ReadLine();
            }
            catch (ArgumentNullException ae)
            {

            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
            }
        }
    }
}
