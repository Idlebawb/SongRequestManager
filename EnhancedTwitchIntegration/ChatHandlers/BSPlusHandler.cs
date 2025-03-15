using System;
using System.Linq;
//using System.Runtime.InteropServices;
using CP_SDK.Chat.Interfaces;
using Newtonsoft.Json;



namespace SongRequestManager.ChatHandlers
{
    public class BSPlusHandler : IChatHandler
    {
        bool initialized = false;
            
            

        public BSPlusHandler()
        {
            if (initialized) return;
            // create chat core instance
            // run twitch services
            try
            {
            CP_SDK.Chat.Service.Acquire();
            //_twitchService = ((CatCoreInstance)sc).RunTwitchServices();
            
            CP_SDK.Chat.Service.Multiplexer.OnTextMessageReceived += _services_OnTextMessageReceived;
         
            }
            catch (Exception e)
            {
                Plugin.Log($"Exception was caught when trying to send bot message. {e.ToString()}");
            }

            initialized = true;
        }

        private void _services_OnTextMessageReceived(IChatService service, IChatMessage msg)
        {
            ChatHandler.ParseMessage(ChatUser.FromJSON(JsonConvert.SerializeObject((IChatUser)msg.Sender)), msg.Message);
        }



        public ChatUser Self => new ChatUser(CP_SDK.Chat.Service.Multiplexer.Channels.First().Item2.Id, CP_SDK.Chat.Service.Multiplexer.Channels.First().Item2.Name, CP_SDK.Chat.Service.Multiplexer.Channels.First().Item2.Name, true, false, "#FFFFFF", null, false, false, false);
        
        public bool Connected => CP_SDK.Chat.Service.Multiplexer.Channels.Any(x => x.Item2 != null);


        public void Send(string message, bool isCommand = false)
        {
            if (string.IsNullOrEmpty(message)) return;
            try
            {
                CP_SDK.Chat.Service.BroadcastMessage(message);
                
            }
            catch (Exception e)
            {
                Plugin.Log($"Exception was caught when trying to send bot message. {e.ToString()}");
            }
        }
    }
}