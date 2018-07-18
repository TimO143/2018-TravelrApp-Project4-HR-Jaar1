using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net.Attributes;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Travelr
{
    class LandInformatie
    {
        [PrimaryKey]
        public string LandNaam { get; set; }
        public string Hoofdstad { get; set; }
        public string Gerecht1 { get; set; }
        public string Gerecht2 { get; set; }
        public string Gerecht3 { get; set; }
        public string Bezienswaardigheid1 { get; set; }
        public string Bezienswaardigheid2 { get; set; }
        public string Bezienswaardigheid3 { get; set; }
        public string Bezienswaardigheid4 { get; set; }
        public string Bezienswaardigheid5 { get; set; }
        public string Natuurgebied1 { get; set; }
        public string Natuurgebied2 { get; set; }
        public string Natuurgebied3 { get; set; }
        public string Hotel1 { get; set; }
        public string Hotel2 { get; set; }
        public string Valuta { get; set; }
        public string Description { get; set; }
    }
}