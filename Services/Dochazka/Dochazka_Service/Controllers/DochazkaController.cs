using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dochazka_Service.Entities;
using Dochazka_Service.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dochazka_Service.Controllers
{
    
    //[ApiController]
    //[Route("[controller]")]
    //public class DochazkaController : ControllerBase
    //{
    //    private readonly IDochazkaRepository _dochazkaRepository;
    //    public DochazkaController(IDochazkaRepository dochazkaService)
    //    {
    //        _dochazkaRepository = dochazkaService;
    //    }
    //    [HttpGet]        
    //    [Route("Get")]
    //    public ActionResult<Dochazka> Get(int id)
    //    {
    //        var result = _dochazkaRepository.Get(id);
    //        if (result == null)
    //        {
    //            return NotFound();
    //        }
    //        return result;
    //    }
    //    [HttpGet]
    //    [Route("GetList")]
    //    public ActionResult<List<DochazkaModel>> List() {
    //        var result = _dochazkaRepository.GetList().ToList();
    //        if (result == null || !result.Any()){
    //            return NotFound();
    //        }
    //        //TODO: WORKAROUND
    //        var dochazkaList = new List<DochazkaModel>();
    //        foreach (var item in result)
    //        {
    //            var dochazka = new DochazkaModel()
    //            {
    //                Id = item.Id,
    //                Prichod = item.Prichod,
    //                Datum = new DateTime(item.Tick),
    //                UzivatelId = item.UzivatelId,
    //                UzivatelCeleJmeno = "test"
    //            };
    //            dochazkaList.Add(dochazka);
    //        }
    //        return dochazkaList;
    //    }

    //    //[HttpPut]
    //    //[Route("Add")]
    //    //public ActionResult<Dochazka> Add(DochazkaModel model)       {

    //    //    var dochazka = new Dochazka() {
    //    //        Rok = model.Datum.Year,
    //    //        Mesic = model.Datum.Month,
    //    //        Den = model.Datum.Day,
    //    //        DenTydne =(int) model.Datum.DayOfWeek,
    //    //        UzivatelId = model.UzivatelId,
    //    //        Tick = model.Datum.Ticks,
    //    //        Prichod = model.Prichod,
    //    //    };


    //    //    var result = _dochazkaRepository.Add(dochazka);
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
    //    //   return _dochazkaRepository.Delete(Convert.ToInt32(id));   
    //    //}

    //}
}
