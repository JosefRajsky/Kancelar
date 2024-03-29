﻿


using CommandHandler;
using Pritomnost_Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pritomnost_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Pritomnost>> GetList();
        Task<Pritomnost> Get(Guid id);
        Task LastEventCheck(Guid eventId, Guid entityId);
   
        Task RequestEvents(Guid? entityId);
        Task CreateByAktivita(EventAktivitaCreated evt);
        Task UpdateByAktivita(EventAktivitaUpdated evt);
        Task DeleteByAktivita(EventAktivitaRemoved evt);

        Task CreateByUzivatel(EventUzivatelCreated evt);       
        Task UpdateByUzivatel(EventUzivatelUpdated evt);        
        Task DeleteByUzivatel(EventUzivatelRemoved evt);
    }
}
