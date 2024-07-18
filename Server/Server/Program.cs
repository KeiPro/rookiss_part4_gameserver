﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Session;
using ServerCore;


namespace Server
{ 

    internal class Program
    {
        static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            // 도메인을 하나 등록한 다음에 이 이름에 해당하는 ip를 찾아낼 수 있게끔 해주는 시스템.
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            // 코드 종료만 안되게 무한루프를 돌림.
            while (true)
            {
                Room.Push(() => Room.Flush());
                Thread.Sleep(250);
                ;
            }
        }
    }
}
