using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommandHandler
{
    public enum MessageType
    {

        #region Template     

        [Description("Event: Temp byl vytvořen")]
        TempCreated = 1504,

        [Description("Event: Temp byl odstraněn")]
        TempRemoved = 1505,

        [Description("Event: Temp byl upraven")]
        TempUpdated = 1506,
        #endregion

        [Description("Prikaz k obnove entity")]
        ProvideHealingStream = 1000,
        [Description("Event k obnove entity")]
        HealingStreamProvided = 1001,
        [Description("Provedení exportu přítomnosti")]
        ExportPritomnost = 1002,
        [Description("Import seznamu uživatelských účtů")]
        ImportUzivatel = 1003,

        #region Dochazka
        [Description("Command: Vytvoření nové docházky")]
        DochazkaCreate = 0,

        [Description("Command: Odstranění docházky")]
        DochazkaRemove = 1,

        [Description("Command: Update docházky")]
        DochazkaUpdate = 2,

        [Description("Event: docházka byla vytvořena")]
        DochazkaCreated = 3,

        [Description("Event docházka byla odstraněna")]
        DochazkaRemoved = 4,

        [Description("Event")]
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

        #region Pritomnost
        [Description("Command: Vytvoření nového kalendáře")]
        PritomnostCreate = 22,

        [Description("Command: Update kalendáře")]
        PritomnostUpdate = 23,

        [Description("Event: Kalendář byl vytvořen")]
        PritomnostCreated = 24,

        [Description("Event: kalendář byl upraven")]
        PritomnostUpdated = 25,
        #endregion

        #region Aktivita
        AktivitaCreate = 26,     
        AktivitaRemove = 27,
        AktivitaUpdate = 28,    
        AktivitaCreated = 29,
        AktivitaRemoved = 30,
        AktivitaUpdated = 31,
        #endregion
        #region Cinnost
        CinnostCreate = 32,
        CinnostRemove = 33,
        CinnostUpdate = 34,
        CinnostCreated = 35,
        CinnostRemoved = 36,
        CinnostUpdated = 37,
        #endregion
        #region MailSender
        MailSenderCreate = 38,
        MailSenderRemove = 39,
        MailSenderUpdate = 40,
        MailSenderCreated = 41,
        MailSenderRemoved = 42,
        MailSenderUpdated = 43,
        #endregion     
        #region Mzdy
        MzdyCreate = 44,
        MzdyRemove = 45,
        MzdyUpdate = 46,
        MzdyCreated = 47,
        MzdyRemoved = 48,
        MzdyUpdated = 49,
        #endregion
        #region Nastaveni
        NastaveniCreate = 50,
        NastaveniRemove = 51,
        NastaveniUpdate = 52,
        NastaveniCreated = 53,
        NastaveniRemoved = 54,
        NastaveniUpdated = 55,
        #endregion     
        #region Opravneni
        OpravneniCreate = 56,
        OpravneniRemove = 57,
        OpravneniUpdate = 58,
        OpravneniCreated = 59,
        OpravneniRemoved = 60,
        OpravneniUpdated = 61,
        #endregion
        #region Soucast
        SoucastCreate = 62,
        SoucastRemove = 63,
        SoucastUpdate = 64,
        SoucastCreated = 65,
        SoucastRemoved = 66,
        SoucastUpdated = 67,
        #endregion
        #region Struktura
        StrukturaCreate = 68,
        StrukturaRemove = 69,
        StrukturaUpdate = 70,
        StrukturaCreated = 71,
        StrukturaRemoved = 72,
        StrukturaUpdated = 73,
        #endregion
        #region Ukol
        UkolCreate = 74,
        UkolRemove = 75,
        UkolUpdate = 76,
        UkolCreated = 77,
        UkolRemoved = 78,
        UkolUpdated = 79,
        #endregion
        #region Vykaz
        VykazCreate = 80,
        VykazRemove = 81,
        VykazUpdate = 82,
        VykazCreated = 83,
        VykazRemoved = 84,
        VykazUpdated = 85,
        #endregion


    }
}
