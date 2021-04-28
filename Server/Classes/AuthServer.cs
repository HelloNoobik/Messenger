using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SharedLibrary;
using SharedLibrary.Net;

namespace Server.Classes
{
    public class AuthServer : ServerBase
    {
        ResponseDispatcherAsync dispatcher;
        const string CONSOLE_PREFIX = "[Auth]: ";
        public AuthServer() : base() 
        {
            dispatcher = new ResponseDispatcherAsync();
            dispatcher.Add(Auth);

            socket.Bind(endPoint);
            socket.Listen(128);
        }

        public void Start() 
        {
            isActive = true;
            AcceptLoop();
        }

        public void Stop() 
        {
            isActive = false;
        }

        private async void AcceptLoop() 
        {
            await Task.Run(()=> 
            {
                Console.WriteLine(CONSOLE_PREFIX + "Ожидание подключений");
                try
                {
                    while (isActive)
                    {
                        Channel channel = new Channel(socket.Accept());
                        Console.WriteLine(CONSOLE_PREFIX + "Новое подключение");
                        SendResponse(channel);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is SocketException) Console.WriteLine(CONSOLE_PREFIX + "Ошибка подключения");
                    else Console.WriteLine(ex);
                }
            });
        }

        private async void SendResponse(Channel channel) 
        {
            await Task.Run(() => 
            {
                try
                {
                    RequestPocket request = (RequestPocket) channel.Recieve();
                    ResponsePocket response = dispatcher.Execute(request);
                    channel.Send(response);
                }
                catch (Exception ex) 
                {
                    if (ex.InnerException is SocketException) Console.WriteLine(CONSOLE_PREFIX + "Ошибка подключения");
                    else Console.WriteLine(ex);
                }
            });
        }

        #region Routes
        [Route("Auth")]
        private async Task<ResponsePocket> Auth(RequestPocket pocket) 
        {
            return await Task.Run(() => 
            {
                Console.WriteLine(CONSOLE_PREFIX + "Запрос авторизации");

                AuthRequest request = (AuthRequest)pocket.Request;
                AuthResponse response;
                
                Data.User user = db.Users.Where(c => c.Login == request.Login && c.Password == request.Password).Single();

                if (user != null)
                {
                    response = new AuthResponse(user.Id);
                }
                else response = new AuthResponse();

                return response.GetPocket();
            });
        }
        #endregion
    }
}
