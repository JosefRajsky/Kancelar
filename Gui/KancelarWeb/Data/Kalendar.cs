//using AllWan.Services.Uzivatel;
//using IntranetVZ_Kancelar2.Controllers;
//using IntranetVZ_Kancelar2.DAL.Entities;
//using IntranetVZ_Kancelar2.DAL.Services;
//using System;
//using System.Collections.Generic;
//using System.Data.Linq;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace IntranetVZ_Kancelar2.Generators
//{
//    public class Kalendar
//    {

//        public List<UdalostTyp> UdalostTypList { get; set; }
//        public List<StromNastaveni> vybraneOsoby { get; set; }
//        public List<StromNastaveni> OsobyWrite { get; set; }
//        public IUzivatelService UzivatelService { get; set; }
//        public KancelarService KancelarService { get; set; }


//        public string GenerujKalendar(int mod, int rok, int mesic, bool edit, int? HotovostId, KancelarService kancelarService, IUzivatelService uzivatelService)
//        {
//            UzivatelService = uzivatelService;
//            KancelarService = kancelarService;
//            vybraneOsoby = KancelarService.StromGetSelected();
//            var resultTable = new Table();
//            UdalostTypList = new List<UdalostTyp>();
//            UdalostTypList.AddRange(kancelarService.GetUdalostTypList());

//            if (HotovostId != 0 & HotovostId != null)
//            {
//                var HotovostZarazeniList = KancelarService.HotovostZarazeniList().Where(h => h.HotovostTypId == HotovostId);
//                var osobyHotovost = new List<StromNastaveni>();
//                foreach (var item in HotovostZarazeniList)
//                {
//                    var Osoba = vybraneOsoby.FirstOrDefault(o => o.VybranyUzivatelId == item.Uzivatel.ActualId);
//                    if (Osoba != null)
//                    {
//                        osobyHotovost.Add(Osoba);
//                    }
//                }
//                vybraneOsoby = osobyHotovost;
//            }
//            switch (mod)
//            {
//                case 1:

//                    resultTable = GenerateCal_MonthInRow(mesic, rok, edit);
//                    break;
//                case 2:

//                    resultTable = GenerateCal_Year(rok, edit);
//                    break;
//                default:

//                    resultTable = GenerateCal_MonthInRow(mesic, rok, edit);
//                    break;
//            }
//            StringBuilder sb = new StringBuilder();
//            StringWriter sw = new StringWriter();
//            HtmlTextWriter htw = new HtmlTextWriter(sw);
//            htw.Flush();
//            resultTable.RenderControl(htw);
//            string htmlString = sw.ToString();
//            return htmlString;
//        }
//        public Table GenerateCal_MonthInRow(int month, int year, bool editable)
//        {
//            var resultTable = new Table();
//            resultTable.ID = "KalendarTable";
//            resultTable.CssClass = "table table-bordered";
//            resultTable.Attributes.Add("data-year", year.ToString());
//            resultTable.Attributes.Add("data-month", month.ToString());
//            resultTable.Attributes.Add("data-editable", editable.ToString());
//            resultTable.Attributes.Add("data-mode", 1.ToString());
//            var daysInMonth = DateTime.DaysInMonth(year, month);


//            var topRow = new TableRow();
//            topRow.CssClass = "bg-default";
//            var topRowCell = new TableCell()
//            {
//                Text = string.Format("<h4>{0} {1}</h4>", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year),
//                HorizontalAlign = HorizontalAlign.Center,
//                ColumnSpan = daysInMonth + 1

//            };
//            topRowCell.Attributes.Add("style", "text-transform: capitalize;");
//            topRow.Cells.Add(topRowCell);
//            resultTable.Rows.Add(topRow);
//            var tempSoucastId = 0;
//            //TODO: Rozlišit právo Read / write
//            var writeList = KancelarService.GetOsobyWrite();

//            foreach (var u in vybraneOsoby.OrderBy(s => s.SoucastId))
//            {
//                if (u.VybranyUzivatelId != 0)
//                {
//                    var uzivatel = UzivatelService.GetUzivatel(Convert.ToInt16(u.VybranyUzivatelId));
//                    u.isReadOnly = !writeList.Where(w => w.UzivatelId == u.VybranyUzivatelId).Any();

//                    var soucastId = u.SoucastId;
//                    if (tempSoucastId != soucastId)
//                    {
//                        tempSoucastId = Convert.ToInt32(soucastId);
//                        var soucastRow = new TableRow();
//                        var soucastCell = new TableCell()
//                        {
//                            Text = uzivatel.SoucastNazev,
//                            ColumnSpan = daysInMonth + 1,
//                            CssClass = "bg-info"

//                        };
//                        soucastRow.Cells.Add(soucastCell);
//                        resultTable.Rows.Add(soucastRow);
//                        resultTable.Rows.Add(GetHeaderRow_Month(daysInMonth, year, month));
//                    }
//                    var MonthRow = new TableRow();
//                    var nameCell = new TableCell()
//                    {
//                        Text = uzivatel.KratkeJmeno,

//                    };
//                    MonthRow.Cells.Add(nameCell);
//                    //TODO: Predat informaci jestli je read / write pravo na osobu / nastaven filtr na hotovost
//                    for (int dayIndex = 1; dayIndex <= daysInMonth; dayIndex++)
//                    {
//                        //var dCell = GetDayCell(daysInMonth, year, month, dayIndex, (u.isReadOnly) ? false : editable, uzivatel.UzivatelId);
//                        var dCell = GetDayCell(daysInMonth, year, month, dayIndex, !u.isReadOnly, uzivatel.UzivatelId);
//                        MonthRow.Cells.Add(dCell);
//                    }
//                    resultTable.Rows.Add(MonthRow);
//                }
//            }
//            if (!vybraneOsoby.Any())
//            {
//                var emptyRow = new TableRow();
//                var emptyCell = new TableCell()
//                {
//                    Text = "Není vybrán žádný uživatel",
//                };
//                emptyCell.HorizontalAlign = HorizontalAlign.Center;
//                emptyRow.Controls.Add(emptyCell);
//                resultTable.Controls.Add(emptyRow);
//            }
//            return resultTable;
//        }
//        public TableRow GetHeaderRow_Month(int den, int rok, int mesic)
//        {
//            var headerRow = new TableRow();
//            headerRow.Cells.Add(new TableCell());
//            for (int dayIndex = 1; dayIndex <= den; dayIndex++)
//            {
//                var day = new DateTime(rok, mesic, dayIndex);

//                var dc = new TableCell()
//                {
//                    Text = dayIndex.ToString(),
//                    ToolTip = day.ToShortDateString()
//                };


//                headerRow.Cells.Add(dc);
//            }
//            return headerRow;
//        }
//        public Table GenerateCal_Year(int year, bool editable)
//        {
//            var resultTable = new Table();
//            resultTable.ID = "KalendarTable";
//            resultTable.CssClass = "table table-bordered";
//            resultTable.Attributes.Add("data-year", year.ToString());
//            resultTable.Attributes.Add("data-month", DateTime.Today.Month.ToString());
//            resultTable.Attributes.Add("data-editable", editable.ToString());
//            resultTable.Attributes.Add("data-mode", 2.ToString());
//            var daysInRow = 32;
//            var headCell = new TableCell()
//            {
//                CssClass = "bg-default",
//                Text = string.Format("<h4>{0}</h4>", year.ToString()),
//                ColumnSpan = daysInRow + 1,
//                HorizontalAlign = HorizontalAlign.Center
//            };

//            var headRow = new TableRow();
//            headRow.Controls.Add(headCell);
//            resultTable.Controls.Add(headRow);


//            var writeList = KancelarService.GetOsobyWrite();
//            foreach (var u in vybraneOsoby.OrderBy(s => s.SoucastId))
//            {
//                if (u.VybranyUzivatelId != 0)
//                {


//                    u.isReadOnly = !writeList.Where(w => w.UzivatelId == u.VybranyUzivatelId).Any();
//                    var topRow = new TableRow();
//                    topRow.CssClass = "bg-success";
//                    var topRowCell = new TableCell()
//                    {
//                        Text = string.Format("<h4>{0}</h4>", UzivatelService.GetUzivatel(Convert.ToInt16(u.VybranyUzivatelId)).KratkeJmeno),
//                        ColumnSpan = daysInRow + 1,
//                        HorizontalAlign = HorizontalAlign.Center,
//                    };
//                    topRow.Cells.Add(topRowCell);
//                    resultTable.Rows.Add(topRow);
//                    resultTable.Rows.Add(GetHeaderRow_Year(daysInRow));




//                    for (int m = 1; m <= 12; m++)
//                    {
//                        var daysInMonth = DateTime.DaysInMonth(year, m);
//                        var MonthRow = new TableRow();
//                        var nameCell = new TableCell()
//                        {
//                            Text = string.Format("{0}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)),
//                        };
//                        MonthRow.Cells.Add(nameCell);

//                        for (int d = 1; d < daysInRow; d++)
//                        {
//                            var dCell = GetDayCell(daysInMonth, year, m, d, (u.isReadOnly) ? false : editable, Convert.ToInt16(u.VybranyUzivatelId));
//                            MonthRow.Cells.Add(dCell);
//                        }
//                        resultTable.Rows.Add(MonthRow);
//                    }
//                }
//            }

//            return resultTable;
//        }
//        public TableCell GetDayCell(int daysInMonth, int r, int m, int d, bool editable, int uzivatelId)
//        {
//            var dCell = new TableCell();
//            if (d > daysInMonth)
//            {
//                dCell.Text = string.Empty;
//                dCell.BackColor = System.Drawing.Color.LightGray;
//            }
//            else
//            {
//                var day = new DateTime(r, m, d);
//                dCell.ID = string.Format("{0}-{1}-{2}_{3}", r, m, d, uzivatelId);
              
//                var UdalostDayList = KancelarService.GetUdalostOsobaDenList(uzivatelId, day);
//                dCell.CssClass = "dayCell";
//                dCell.Attributes.Add("style", "padding:0px;height:100%;");
//                var EventTable = new Table();
//                EventTable.CssClass = "table table-condensed eventTable";
//                //TODO rozlišit jestli je na osobu právo READ/Write nebo nastaven filtr na HOTOVOST
//                if (editable)
//                {
//                    var selectorCell = new Panel() { CssClass = "selector glyphicon glyphicon-unchecked" };
//                    selectorCell.Attributes.Add("data-DefaultClass", selectorCell.CssClass);
//                    selectorCell.Attributes.Add("data-SelectedClass", "selector glyphicon glyphicon-check");
//                    selectorCell.Attributes.Add("style", "display: none;");
//                    dCell.Controls.Add(selectorCell);
//                    dCell.Attributes.Add("onClick", "DaySelectChanged('" + dCell.ID + "');");
//                }
              
//                var hw = UdalostDayList.FirstOrDefault(da => da.Datum == day && da.Uzivatel.ActualId == uzivatelId & da.isHotovost == true);

//                if (hw != null)
//                {
//                    var hotovostCell = new Panel();
//                    hotovostCell.ToolTip = day.ToShortDateString();
//                    hotovostCell.CssClass = "cellHotovostPrubeh";
//                    //if (day == hw.DatumOd)
//                    if (hw.MasterUdalostId == 0)

//                    {
//                        if (string.IsNullOrEmpty(hw.CasStart))
//                        {
//                            hotovostCell.ToolTip = day.ToShortDateString();
//                            hotovostCell.CssClass = "cellHotovostPrubeh";
//                        }
//                        else
//                        {
//                            hotovostCell.ToolTip = string.Format("{0} od {1}", day.ToShortDateString(), hw.CasStart);
//                            hotovostCell.CssClass = "cellHotovostStart";
//                        }

//                    }
//                    else
//                    //if (day == hw.DatumDo)

//                    {
//                        if (string.IsNullOrEmpty(hw.CasKonec))
//                        {
//                            hotovostCell.ToolTip = day.ToShortDateString();
//                            hotovostCell.CssClass = "cellHotovostPrubeh";
//                        }
//                        else
//                        {
//                            hotovostCell.ToolTip = string.Format("{0} do {1}", day.ToShortDateString(), hw.CasKonec);
//                            hotovostCell.CssClass = "cellHotovostKonec";
//                        }
//                    }
//                    hotovostCell.Attributes.Add("data-toggle", "modal");
//                    hotovostCell.Attributes.Add("data-target", "#EditUdalost");
//                    hotovostCell.Attributes.Add("onClick", string.Format("javascript:UdalostDetail('{0}');", ((hw.MasterUdalostId == 0) ? hw.UdalostId : hw.MasterUdalostId)));
//                    dCell.Controls.Add(hotovostCell);
//                }
//                if (UdalostDayList.Any())
//                {
//                    foreach (var item in UdalostDayList)
//                    {
//                        var EventTable_Row = new TableRow();

//                        var EventTable_Cell = new TableCell();
//                        EventTable_Cell.CssClass = "eventCell";
//                        EventTable_Cell.Attributes.Add("data-toggle", "modal");
//                        EventTable_Cell.Attributes.Add("data-target", "#EditUdalost");
//                        EventTable_Cell.Attributes.Add("onClick", string.Format("javascript:UdalostDetail('{0}');", ((item.MasterUdalostId == 0) ? item.UdalostId : item.MasterUdalostId)));
//                        var uTyp = UdalostTypList.FirstOrDefault(ut => ut.UdalostTypId == item.UdalostTypId);
//                        if (uTyp != null)
//                        {
//                            EventTable_Cell.Text = uTyp.Zkratka;
//                            if (item.Necelodenni)
//                            {
//                                EventTable_Cell.Attributes.Add("style", string.Format("background-color: #{0};background-image: linear-gradient(30deg, #{0} 50%, white 50%)", uTyp.BarvaPozadi));
//                            }
//                            else
//                            {
//                                EventTable_Cell.BackColor = (!string.IsNullOrEmpty(uTyp.BarvaPozadi)) ? System.Drawing.ColorTranslator.FromHtml("#" + uTyp.BarvaPozadi) : System.Drawing.Color.Black;
//                            }

//                            EventTable_Cell.ForeColor = (!string.IsNullOrEmpty(uTyp.BarvaPisma)) ? System.Drawing.ColorTranslator.FromHtml("#" + uTyp.BarvaPisma) : System.Drawing.Color.Black;
//                            EventTable_Cell.ToolTip = uTyp.Popis;
//                        }
//                        EventTable_Row.Controls.Add(EventTable_Cell);
//                        EventTable.Controls.Add(EventTable_Row);
//                    }
//                }
//                var isWeekend = (day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday);
//                var isToday = (day == DateTime.Today);
//                var svatek = IsSvatek(day);
//                var isSvatek = svatek != string.Empty;
//                dCell.CssClass = (isSvatek) ? "cellSvatek" : (isToday) ? "cellDnes" : (isWeekend) ? "cellVikend" : string.Empty;
//                dCell.Attributes.Add("data-DefaultClass", dCell.CssClass);
//                dCell.ToolTip = (isSvatek) ? svatek : day.ToShortDateString();
//                if (editable | UdalostDayList.Any()) dCell.Controls.Add(EventTable);

//            }



//            return dCell;
//        }
//        public TableRow GetHeaderRow_Year(int days)
//        {
//            var headerRow = new TableRow();
//            headerRow.Cells.Add(new TableCell());
//            for (int dayIndex = 1; dayIndex < days; dayIndex++)
//            {
//                var dc = new TableCell()
//                {
//                    Text = dayIndex.ToString(),
//                };
//                headerRow.Cells.Add(dc);
//            }
//            return headerRow;
//        }
//        public string IsSvatek(DateTime datum)
//        {
//            var svatek = string.Empty;

//            if (datum == DateTime.Parse("30.3.2018") ||
//                datum == DateTime.Parse("19.4.2019") ||
//                datum == DateTime.Parse("10.4.2020") ||
//                datum == DateTime.Parse("2.4.2021") ||
//                datum == DateTime.Parse("15.4.2022") ||
//                datum == DateTime.Parse("7.4.2023") ||
//                datum == DateTime.Parse("29.3.2024") ||
//                datum == DateTime.Parse("18.4.2025"))
//            {
//                svatek = "Velký pátek";
//            }

//            // Pohyblivé velikonoce
//            if (2199 >= datum.Year && datum.Year >= 1800)
//            {
//                // Vyjímky
//                if (datum.Year == 1954 || datum.Year == 1981)
//                {
//                    if (datum.Year == 1954 && datum.Month == 4 && datum.Day == 12)
//                    {
//                        svatek = "Velikonoční pondělí (Vyjímka)";
//                    }
//                    if (datum.Year == 1981 && datum.Month == 4 && datum.Day == 13)
//                    {
//                        svatek = "Velikonoční pondělí (Vyjímka)";
//                    }
//                    if (datum.Year == 1954 && datum.Month == 4 && datum.Day == 9)
//                    {
//                        svatek = "Velký pátek (Vyjímka)";
//                    }
//                    if (datum.Year == 1981 && datum.Month == 4 && datum.Day == 10)
//                    {
//                        svatek = "Velký pátek (Vyjímka)";
//                    }
//                }

//                // Výpočet
//                else
//                {
//                    var m = 0;
//                    var n = 0;

//                    if (1899 >= datum.Year && datum.Year >= 1800)
//                    {
//                        m = 23;
//                        n = 4;
//                    }
//                    if (1999 >= datum.Year && datum.Year >= 1900)
//                    {
//                        m = 24;
//                        n = 5;
//                    }
//                    if (2099 >= datum.Year && datum.Year >= 2000)
//                    {
//                        m = 24;
//                        n = 5;
//                    }
//                    if (2199 >= datum.Year && datum.Year >= 2100)
//                    {
//                        m = 24;
//                        n = 6;
//                    }

//                    var a = datum.Year % 19; // Po 19 letech se měsíční cyklus opakuje ve stejné dny
//                    var b = datum.Year % 4; // Cyklus opakování přestupných let
//                    var c = datum.Year % 7; // Dorovnání dne v týdnu
//                    var d = (m + 19 * a) % 30;
//                    var e = (n + 2 * b + 4 * c + 6 * d) % 7;

//                    int den;
//                    int mesic;

//                    var f = 22 + d + e;
//                    if (f < 31)
//                    {
//                        mesic = 3;
//                        den = f + 1;
//                    }
//                    else
//                    {
//                        f = d + e - 9;
//                        f = (f > 25) ? f - 7 : f;
//                        mesic = 4;
//                        den = f + 1;
//                    }

//                    if (datum.Month == mesic && datum.Day == den)
//                    {
//                        svatek = "Velikonoční pondělí";
//                    }

//                    if (datum.Month == (mesic) && datum.Day == (den - 3) && datum.Year >= 2016)
//                    {
//                        svatek = "Velký pátek";
//                    }
//                }
//            }

//            // Statické svátky
//            if (datum.Month == 1 && datum.Day == 1)
//            {
//                svatek = "Nový rok";
//            }
//            if (datum.Month == 5 && datum.Day == 1)
//            {
//                svatek = "Svátek práce";
//            }
//            if (datum.Month == 5 && datum.Day == 8)
//            {
//                svatek = "Den vítězství";
//            }
//            if (datum.Month == 7 && datum.Day == 5)
//            {
//                svatek = "Den slovanských věrozvěstů Cyrila a Metoděje";
//            }
//            if (datum.Month == 7 && datum.Day == 6)
//            {
//                svatek = "Den upálení mistra Jana Husa";
//            }
//            if (datum.Month == 9 && datum.Day == 28)
//            {
//                svatek = "Den české státnosti";
//            }
//            if (datum.Month == 10 && datum.Day == 28)
//            {
//                svatek = "Den vzniku samostatného československého státu";
//            }
//            if (datum.Month == 11 && datum.Day == 17)
//            {
//                svatek = "Den boje za svobodu a demokracii";
//            }
//            if (datum.Month == 12 && datum.Day == 24)
//            {
//                svatek = "Štědrý den";
//            }
//            if (datum.Month == 12 && datum.Day == 25)
//            {
//                svatek = "1. Svátek vánoční";
//            }
//            if (datum.Month == 12 && datum.Day == 26)
//            {
//                svatek = "2. Svátek vánoční";
//            }
//            return svatek;
//        }

//        public TableRow GetHeaderRow_Week(int den, int rok, int mesic)
//        {


//            var datum = new DateTime(rok, mesic, den);

//            var c = CultureInfo.CurrentCulture;
//            var weekNumber = c.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
//            var firstDay = c.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                                                  
//            var headerRow = new TableRow();
//            headerRow.Cells.Add(new TableCell());
//            for (int dayIndex = 1; dayIndex == 7; dayIndex++)
//            {
//                var day = new DateTime(rok, mesic, dayIndex);

//                var dc = new TableCell()
//                {
//                    Text = dayIndex.ToString(),
//                    ToolTip = day.ToShortDateString()
//                };


//                headerRow.Cells.Add(dc);
//            }
//            return headerRow;

//        }

//        public TableRow GetHeaderRow_Day(int den, int rok, int mesic)
//        {
//            var headerRow = new TableRow();
//            headerRow.Cells.Add(new TableCell());
//            for (int dayIndex = 1; dayIndex <= den; dayIndex++)
//            {
//                var day = new DateTime(rok, mesic, dayIndex);

//                var dc = new TableCell()
//                {
//                    Text = dayIndex.ToString(),
//                    ToolTip = day.ToShortDateString()
//                };


//                headerRow.Cells.Add(dc);
//            }
//            return headerRow;

//        }


//    }



//}