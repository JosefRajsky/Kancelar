using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EventLibrary
{
    public enum MessageType
    {
        #region Dochazka
        [Description("Command: Vytvoření nové docházky")]
        DochazkaCreate = 0,

        [Description("Command: Odstranění docházky")]
        DochazkaRemove = 1,

        [Description("Command: Update docházky")]
        DochazkaUpdate = 2,

        [Description("Command: Vytvoření nové docházky")]
        DochazkaCreated = 3,

        [Description("Command: Odstranění docházky")]
        DochazkaRemoved = 4,

        [Description("Command: Update docházky")]
        DochazkaUpdated = 5,
        #endregion
        #region Udalost

        [Description("Command: Založení nové události")]
        UdalostCreate = 6,

        [Description("Command: Odstranění události")]
        UdalostRemove = 7,

        [Description("Command: Update události")]
        UdalostUpdate = 8,

        [Description("Command: Založení nové události")]
        UdalostCreated = 9,

        [Description("Command: Odstranění události")]
        UdalostRemoved = 10,

        [Description("Command: Update události")]
        UdalostUpdated = 11,
        #endregion



    }
}
