using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using CommandHandler;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uzivatel_Api.Repositories;

namespace Uzivatel_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UzivatelController : ControllerBase
    {
        private readonly IRepository _repository;
        //Description: Přiřazení repositáře pro zpracování operací a připojení DB
        public UzivatelController(IRepository repository)
        {
            _repository = repository;
          
        }
        //Description: Metoda pro získání konkrétního uživatele podle ID
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Uzivatel> Get(Guid id)
        {
            var response = await _repository.Get(id);  
            return response;
        }
        //Description: Metoda získání listu všech uživatelů
        [HttpGet]
        [Route("GetList")]
        public async Task<List<Uzivatel>> GetList() {  
            return await _repository.GetList();
        }
        //Description: Metoda přidání nového uživatele
        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandUzivatelCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }
        //Description: Metoda odstranění uživatele
        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandUzivatelRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        //Description: Metoda pro úpravu uživatele
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandUzivatelUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
