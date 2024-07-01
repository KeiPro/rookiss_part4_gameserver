using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    internal class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAcceptHandler;

        public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;

            // 문지기 교육
            _listenSocket.Bind(endPoint);

            // 영업 시작
            // backlog : 최대 대기수
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // args에 대해서 초기화를 시켜주는 것이 중요하다.
            // 아래의 OnAcceptCompleted에서 args를 넘겨주고 있는데 이 args는 null이 아닌 어떤 데이터들이 들어가있는 형태이기 때문에..
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);

            // AcceptAsync를 호출했을 때,
            // 정말 운 좋게 동시에 클라이언트의 접속 요청이 pending없이 오게 된다면 OnAcceptCompleted()함수 호출.
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // TODO
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }

            // 여기까지 왔다는 것은 정상적으로 성공했다는 뜻이니,
            // 다음 소켓을 위해서 또 한 번 등록을 해주기 위해 호출하는 것.
            RegisterAccept(args);
        }
    }
}
