using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EventLibrary
{
    public class EventEnum
    {
        public enum MessageType
        {
            [Description("Command: Vytvoření nové docházky")]
            DochazkaCreate = 0,

            [Description("Command: Odstranění docházky")]
            DochazkaRemove = 1,

            [Description("Command: Update docházky")]
            DochazkaUpdate = 2,

            [Description("Command: Založení nové události")]
            UdalostCreate = 3,

            [Description("Command: Odstranění události")]
            UdalostRemove = 4,

            [Description("Command: Update události")]
            UdalostUpdate = 5,

        }
    }
}
