using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KancelarWeb.Data;
using KancelarWeb.ViewModels;

namespace KancelarWeb
{
    public class Index2Model : PageModel
    {
        private readonly KancelarWeb.Data.KancelarWebContext _context;

        public Index2Model(KancelarWeb.Data.KancelarWebContext context)
        {
            _context = context;
        }

        public IList<UzivatelModel> UzivatelModel { get;set; }

        public async Task OnGetAsync()
        {
            UzivatelModel = await _context.UzivatelModel.ToListAsync();
        }
    }
}
