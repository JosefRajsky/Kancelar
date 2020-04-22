using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailSender_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Mail>> GetList();
        Task<Mail> Get(Guid id);
        Task Add(CommandMailSenderCreate cmd);
        Task Update(CommandMailSenderUpdate cmd);
        Task Remove(CommandMailSenderRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
