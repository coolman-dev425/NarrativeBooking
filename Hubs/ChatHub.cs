using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helper;
using WebApplication1.Identity;
using WebApplication1.Models;

namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        private readonly BooksContext db;
        public ChatHub(BooksContext context)
        {
            db = context;
        }

        public async Task SendMessage(string toUser, string message)
        {
            if (toUser == Context.User.Identity.Name)
            {
                if (message == Common.MessageTypeConstants.GET_UNREAD_MESSAGE)
                    await GetUnreadMessage(toUser);
                else if (message == Common.MessageTypeConstants.DELETE_UNREAD_MESSAGE)
                {                    
                    await DeletUnreadMessage(toUser);
                }
            }
            else
            {
                MessageModel messageModel = new MessageModel
                {
                    From = Context.User.Identity.Name,
                    To = toUser,
                    SendTime = DateTime.Now,
                    Message = message
                };

                if (CacheHelper.Instance.IsOnline(toUser))
                {
                    await Clients.Groups(toUser, Context.User.Identity.Name).SendAsync("ReceiveMessage", messageModel);
                }
                else
                {
                    await Clients.Groups(Context.User.Identity.Name).SendAsync("ReceiveMessage", messageModel);
                    await AddOfflineMessage(messageModel);
                }
            }
            
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await Clients.Others.SendAsync("OnlineMessage", Context.User.Identity.Name);            
            CacheHelper.Instance.AddUserOnline(Context.User.Identity.Name);
            await Clients.Group(Context.User.Identity.Name).SendAsync("OnConnectedSuccess", Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await Clients.Others.SendAsync("OfflineMessage", Context.User.Identity.Name);
            CacheHelper.Instance.RemoveUserOnline(Context.User.Identity.Name);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task AddOfflineMessage(MessageModel message)
        {
            try
            {
                db.OfflineMessages.Add(message);
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task GetUnreadMessage(string userName)
        {
            await Task.Run(() => {
                IEnumerable<MessageModel> messages = db.OfflineMessages.Where(x => x.To == userName).OrderBy(x => x.SendTime).ToList();
                if (messages.Any())
                {
                    Clients.Group(userName).SendAsync("UnreadMessages", messages);                    
                }
            }); 
        }

        private async Task DeletUnreadMessage(string owerUser)
        {
            await Task.Run(() =>
            {
                IEnumerable<MessageModel> deletingMesages = db.OfflineMessages.Where(x => x.To == owerUser);
                db.OfflineMessages.RemoveRange(deletingMesages);
                db.SaveChanges();
            });
        }
    }
}
