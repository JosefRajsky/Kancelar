﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Udalost_Service.Entities;
using Udalost_Service.Repositories;

namespace Udalost_Service.Controllers
{
    
    //[ApiController]
    //[Route("[controller]")]
    //public class UdalostController : ControllerBase
    //{
    //    private readonly IUdalostRepository _udalostRepository;
    //    public UdalostController(IUdalostRepository udalostService)
    //    {
    //        _udalostRepository = udalostService;
    //    }

    //    [HttpGet]        
    //    [Route("Get")]
    //    public ActionResult<Udalost> Get(int id)
    //    {
    //        var result = _udalostRepository.Get(id);
    //        if (result == null)
    //        {
    //            return NotFound();
    //        }
    //        return result;
    //    }

    //    [HttpGet]
    //    [Route("GetList")]
    //    public ActionResult<List<Udalost>> GetList() {
    //        var result = _udalostRepository.GetList().ToList();
    //        if (result == null || !result.Any()){
    //            return NotFound();
    //        }
    //        return result;
    //    }
       
    //    [HttpPost]
    //    [Route("Update")]
    //    public ActionResult<bool> Update(Udalost udalost)
    //    {
    //        var result = _udalostRepository.Update(udalost);
           
    //        return result;
    //    }

    //    //[HttpPut]
    //    //[Route("Add")]
    //    //public ActionResult<Udalost> Add(Udalost udalost)
    //    //{
    //    //    var result = _udalostRepository.Add(udalost);
    //    //    if (result == null)
    //    //    {
    //    //        return NotFound();
    //    //    }
    //    //    return result;
    //    //}

    //    //[HttpDelete]
    //    //[Route("Delete")]
    //    //public ActionResult<bool> Delete(int id)
    //    //{
    //    //    return _udalostRepository.Delete(Convert.ToInt32(id));
    //    //}
    //}
}