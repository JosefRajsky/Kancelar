

using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Kalendar_Api.Repositories
{
    public interface IListener
    {

        void AddCommand(string message);
        void AddAsync(CommandKalendarCreate message);            
        void Update(CommandKalendarUpdate message);



    }
}
