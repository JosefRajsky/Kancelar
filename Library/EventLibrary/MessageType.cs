﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EventLibrary
{
    public enum MessageType
    {
        [Description("Prikaz k obnove entity")]
        HealMe = 1000,
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
        #region Uzivatel
        [Description("Command: Vytvoření nového uživatele")]
        UzivatelCreate = 12,

        [Description("Command: Odstranění uživatele")]
        UzivatelRemove = 13,

        [Description("Command: Update uživatele")]
        UzivatelUpdate = 14,

        [Description("Command: Uživatel byl vytvořen")]
        UzivatelCreated = 15,

        [Description("Command: Uživatel byl odstraněn")]
        UzivatelRemoved = 16,

        [Description("Command: Uživatel byl upraven")]
        UzivatelUpdated = 17,
        #endregion
        #region Kalendar
        [Description("Command: Vytvoření nového kalendáře")]
        KalendarCreate = 18,       

        [Description("Command: Update kalendáře")]
        KalendarUpdate = 19,

        [Description("Event: Kalendář byl vytvořen")]
        KalendarCreated = 20,       

        [Description("Event: kalendář byl upraven")]
        KalendarUpdated = 21,
        #endregion


    }
}
