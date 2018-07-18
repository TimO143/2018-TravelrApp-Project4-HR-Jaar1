using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Graphics;
using Android.OS;
using Android.Widget;
// deze twee nodig voor textview naar link te maken
using Android.Text.Method;
using Android.Text;
// style van hyperlinks aanpassen
using Android.Text.Style;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;

namespace Travelr
{
    [Activity(Label = "LandInfoActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class LandInfoActivity : Activity
    {
        DBHelper db;
        SQLiteDatabase sqliteDB;
        List<LandInformatie> lstLanden = new List<LandInformatie>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LandInfo);
            //GETDATA

            string landNaam = Intent.GetStringExtra("landKeuze");
            db = new DBHelper(this);
            sqliteDB = db.WritableDatabase;
            lstLanden = GetData();

            Thread p = new Thread(() => SetData(landNaam));
            p.Start();

            //SetData(landNaam);

            Thread t = new Thread(() => setTypeFace());
            t.Start();

            

            //GET LOGIN STATUS, USERID & USERNAME
            bool LoginCheck = Intent.GetBooleanExtra("LoginCheck", false);
            int mUserID = Intent.GetIntExtra("mUserID", 0);
            string Username = Intent.GetStringExtra("Username");
            

            ImageButton btnBack = FindViewById<ImageButton>(Resource.Id.btnBackLI);
            ImageButton btnHome = FindViewById<ImageButton>(Resource.Id.btnHome);
            ImageView imgLand = FindViewById<ImageView>(Resource.Id.imgLand);
            Button btnAddFavorieten = FindViewById<Button>(Resource.Id.btnAddFavorieten);

            

            setLandNaam(landNaam);

            //GET FAVORIETEN

            if (LoginCheck)
            {
                string[] Favorieten = getFavorieten(Username);

                if (Favorieten.Contains(landNaam))
                {
                    btnAddFavorieten.Text = "Verwijder uit favorieten";
                }
                else
                {
                    btnAddFavorieten.Text = "Voeg toe aan favorieten";
                }
            }
            else
            {
                btnAddFavorieten.Text = "Login om de favorieten functie te gebruiken.";
            }

            setLinks(landNaam);


            btnBack.Click += BtnBack_Click;
            btnHome.Click += (sender, e) => BtnHome_Click(sender, e, LoginCheck, mUserID, Username);
            btnAddFavorieten.Click += (sender, e) => BtnAddFavorieten_Click(sender, e, landNaam, btnAddFavorieten, LoginCheck, Username);


            string setAfbeelding = landNaam + "Info";
            int id = (int)typeof(Resource.Drawable).GetField(setAfbeelding).GetValue(null);
            imgLand.SetImageResource(id);

            ImageButton btnRecensie = FindViewById<ImageButton>(Resource.Id.btnRecensie);
            btnRecensie.Click += (sender, e) => BtnRecensie_Click(sender, e, LoginCheck, mUserID, landNaam, Username);

        }

        private void setTypeFace()
        {
            //Lettertypes
            Typeface opensansbold = Typeface.CreateFromAsset(Assets, "OpenSans-Bold.ttf");
            Typeface opensanslight = Typeface.CreateFromAsset(Assets, "OpenSans-Light.ttf");

            TextView txtLandVerhaal = FindViewById<TextView>(Resource.Id.txtLandVerhaal);

            TextView txtHoofdstadTitel = FindViewById<TextView>(Resource.Id.txtHoofdstadTitel);
            TextView txtHoofdstad = FindViewById<TextView>(Resource.Id.txtHoofdstad);

            TextView txtGerechtTitel = FindViewById<TextView>(Resource.Id.txtGerechtTitel);
            TextView txtGerecht1 = FindViewById<TextView>(Resource.Id.txtGerecht1);
            TextView txtGerecht2 = FindViewById<TextView>(Resource.Id.txtGerecht2);
            TextView txtGerecht3 = FindViewById<TextView>(Resource.Id.txtGerecht3);

            TextView txtBezienswaardigheidTitel = FindViewById<TextView>(Resource.Id.txtBezienswaardigheidTitel);
            TextView txtBezienswaardigheid1 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid1);
            TextView txtBezienswaardigheid2 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid2);
            TextView txtBezienswaardigheid3 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid3);
            TextView txtBezienswaardigheid4 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid4);
            TextView txtBezienswaardigheid5 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid5);

            TextView txtNatuurgebiedenTitel = FindViewById<TextView>(Resource.Id.txtNatuurgebiedenTitel);
            TextView txtNatuurgebied1 = FindViewById<TextView>(Resource.Id.txtNatuurgebied1);
            TextView txtNatuurgebied2 = FindViewById<TextView>(Resource.Id.txtNatuurgebied2);
            TextView txtNatuurgebied3 = FindViewById<TextView>(Resource.Id.txtNatuurgebied3);

            TextView txtHotelTitel = FindViewById<TextView>(Resource.Id.txtHotelTitel);
            TextView txtHotel1 = FindViewById<TextView>(Resource.Id.txtHotel1);
            TextView txtHotel2 = FindViewById<TextView>(Resource.Id.txtHotel2);

            TextView txtValutaTitel = FindViewById<TextView>(Resource.Id.txtValutaTitel);
            TextView txtValuta = FindViewById<TextView>(Resource.Id.txtValuta);

            txtLandVerhaal.SetTypeface(opensanslight, TypefaceStyle.Normal);

            txtHoofdstadTitel.SetTypeface(opensansbold, TypefaceStyle.Bold);
            txtHoofdstad.SetTypeface(opensanslight, TypefaceStyle.Bold);

            txtGerechtTitel.SetTypeface(opensansbold, TypefaceStyle.Bold);
            txtGerecht1.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtGerecht2.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtGerecht3.SetTypeface(opensanslight, TypefaceStyle.Bold);

            txtBezienswaardigheidTitel.SetTypeface(opensansbold, TypefaceStyle.Bold);
            txtBezienswaardigheid1.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtBezienswaardigheid2.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtBezienswaardigheid3.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtBezienswaardigheid4.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtBezienswaardigheid5.SetTypeface(opensanslight, TypefaceStyle.Bold);

            txtNatuurgebiedenTitel.SetTypeface(opensansbold, TypefaceStyle.Bold);
            txtNatuurgebied1.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtNatuurgebied2.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtNatuurgebied3.SetTypeface(opensanslight, TypefaceStyle.Bold);

            txtHotelTitel.SetTypeface(opensansbold, TypefaceStyle.Bold);
            txtHotel1.SetTypeface(opensanslight, TypefaceStyle.Bold);
            txtHotel2.SetTypeface(opensanslight, TypefaceStyle.Bold);

            txtValutaTitel.SetTypeface(opensansbold, TypefaceStyle.Bold);
            txtValuta.SetTypeface(opensanslight, TypefaceStyle.Bold);
        }

        private void BtnAddFavorieten_Click(object sender, EventArgs e, string landNaam, Button btnAddFavorieten, bool loginCheck, string Username)
        {
            string[] columns = { "userFavoriet1", "userFavoriet2", "userFavoriet3", "userFavoriet4", "userFavoriet5", "userFavoriet6" };
            bool removed = false;
            bool added = false;

            //GET EMPTY COLUMN ID
            if (loginCheck != false)
            {

                //GET FAVORIETEN


                string[] Favorieten = getFavorieten(Username);

                //VERWIJDER AAN FAVORIETEN
                if (btnAddFavorieten.Text == "Verwijder uit favorieten")
                {
                    //GET COLUMN ID
                    for (int i = 0; i < Favorieten.Length; i++)
                    {
                        if (Favorieten[i] == landNaam)
                        {
                            string selected_column = columns[i];
                            RemoveFavoriet(selected_column, btnAddFavorieten, landNaam, Username);
                            removed = true;
                            break;
                        }
                    }
                                      
                }
                //VOEG TOE AAN FAVORIETEN
                if (btnAddFavorieten.Text == "Voeg toe aan favorieten")
                {
                    //CHECK OF ALLE FAVORIETEN GEBRUIKT ZIJN.
                    if (Favorieten.Contains("geenFavoriet"))
                    {
                        for (int i = 0; i < Favorieten.Length; i++)
                        {

                            if (Favorieten[i] == "geenFavoriet")
                            {
                                string empty_column = columns[i];
                                AddFavoriet(empty_column, btnAddFavorieten, landNaam, Username);
                                added = true;
                                break;
                            }
                            

                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "U heeft het maximale aantal favorieten gebruikt.", ToastLength.Short).Show();
                        //GET EMPTY COLUMN ID


                    }

                }
            }

            if (added)
            {
                btnAddFavorieten.Text = "Verwijder uit favorieten";
                Toast.MakeText(this, "U heeft " + landNaam + " aan u favorieten toegevoegd", ToastLength.Short).Show();
            }
            if (removed)
            {
                btnAddFavorieten.Text = "Voeg toe aan favorieten";
                Toast.MakeText(this, "U heeft " + landNaam + " uit u favorieten verwijderd", ToastLength.Short).Show();
            }
            
            

        }

        private void RemoveFavoriet(string column, Button btnAddFavorieten, string landNaam, string Username)
        {
            //GET FAVORIETEN
            string[] Favorieten = getFavorieten(Username);
            // Verwijder Favoriet
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                    using (var conn = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8"))
                    {
                        conn.Open();

                        MySqlCommand cmd = new MySqlCommand("update tbl_userdata set "+ column +" = 'geenFavoriet' where userName=@username;", con);
                        cmd.Parameters.AddWithValue("@username", Username);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        

                    }
                }
            }
            finally
            {
                con.Close();


                //REFRESH ALLE COMMENTS
            }
        }
        private void AddFavoriet(string column, Button btnAddFavorieten, string landNaam, string Username)
        {
            //GET FAVORIETEN
            string[] Favorieten = getFavorieten(Username);
            // SET FAVORIET
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                    using (var conn = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8"))
                    {
                        conn.Open();

                        MySqlCommand cmd = new MySqlCommand("update tbl_userdata set " + column + "='" + landNaam + "' where userName=@username;", con);
                        cmd.Parameters.AddWithValue("@username", Username);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        

                    }
                }
            }
            finally
            {
                con.Close();


                //REFRESH ALLE COMMENTS
            }
        }

        private string[] getFavorieten(string username)
        {
            string[] columns = { "userFavoriet1", "userFavoriet2", "userFavoriet3", "userFavoriet4", "userFavoriet5", "userFavoriet6" };
            string[] Favorieten = new string[6];
            int counter = 0;
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                    using (var conn = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8"))
                    {
                        conn.Open();
                        foreach(string column in columns)
                        {
                            MySqlCommand cmdGet_Favo = new MySqlCommand("select " + column + " from tbl_userdata where userName=@username", con);
                            cmdGet_Favo.Parameters.AddWithValue("@username", username);
                            MySqlDataAdapter daGetFavo = new MySqlDataAdapter(cmdGet_Favo);
                            DataSet dsGetFavo = new DataSet();
                            daGetFavo.Fill(dsGetFavo);
                            DataRow dr = dsGetFavo.Tables[0].Rows[0];
                            string favoriet = dr[column].ToString();
                            Favorieten[counter] = favoriet;
                            counter += 1;
                        }


                        return Favorieten;
                    }
                }
                return Favorieten;
            }
            finally
            {
                con.Close();

                //REFRESH ALLE COMMENTS
            }
            
        }

        private List<LandInformatie> GetData()
        {
            ICursor selectData = sqliteDB.RawQuery("select * from LandInformatie", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    LandInformatie info = new LandInformatie();
                    info.LandNaam = selectData.GetString(selectData.GetColumnIndex("LandNaam"));
                    info.Hoofdstad = selectData.GetString(selectData.GetColumnIndex("Hoofdstad"));
                    info.Gerecht1 = selectData.GetString(selectData.GetColumnIndex("Gerecht1"));
                    info.Gerecht2 = selectData.GetString(selectData.GetColumnIndex("Gerecht2"));
                    info.Gerecht3 = selectData.GetString(selectData.GetColumnIndex("Gerecht3"));
                    info.Bezienswaardigheid1 = selectData.GetString(selectData.GetColumnIndex("Bezienswaardigheid1"));
                    info.Bezienswaardigheid2 = selectData.GetString(selectData.GetColumnIndex("Bezienswaardigheid2"));
                    info.Bezienswaardigheid3 = selectData.GetString(selectData.GetColumnIndex("Bezienswaardigheid3"));
                    info.Bezienswaardigheid4 = selectData.GetString(selectData.GetColumnIndex("Bezienswaardigheid4"));
                    info.Bezienswaardigheid5 = selectData.GetString(selectData.GetColumnIndex("Bezienswaardigheid5"));
                    info.Natuurgebied1 = selectData.GetString(selectData.GetColumnIndex("Natuurgebied1"));
                    info.Natuurgebied2 = selectData.GetString(selectData.GetColumnIndex("Natuurgebied2"));
                    info.Natuurgebied3 = selectData.GetString(selectData.GetColumnIndex("Natuurgebied3"));
                    info.Hotel1 = selectData.GetString(selectData.GetColumnIndex("Hotel1"));
                    info.Hotel2 = selectData.GetString(selectData.GetColumnIndex("Hotel2"));
                    info.Valuta = selectData.GetString(selectData.GetColumnIndex("Valuta"));
                    info.Description = selectData.GetString(selectData.GetColumnIndex("Descriptions"));
                    lstLanden.Add(info);
                }
                while (selectData.MoveToNext());
                selectData.Close();
            }

            return lstLanden;

        }

        private void SetData(string landNaam)
        {
            TextView txtLandNaam = FindViewById<TextView>(Resource.Id.txtLandNaam);
            TextView txtHoofdstad = FindViewById<TextView>(Resource.Id.txtHoofdstad);
            TextView txtGerecht1 = FindViewById<TextView>(Resource.Id.txtGerecht1);
            TextView txtGerecht2 = FindViewById<TextView>(Resource.Id.txtGerecht2);
            TextView txtGerecht3 = FindViewById<TextView>(Resource.Id.txtGerecht3);
            TextView txtBezienswaardigheid1 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid1);
            TextView txtBezienswaardigheid2 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid2);
            TextView txtBezienswaardigheid3 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid3);
            TextView txtBezienswaardigheid4 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid4);
            TextView txtBezienswaardigheid5 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid5);
            TextView txtNatuurgebied1 = FindViewById<TextView>(Resource.Id.txtNatuurgebied1);
            TextView txtNatuurgebied2 = FindViewById<TextView>(Resource.Id.txtNatuurgebied2);
            TextView txtNatuurgebied3 = FindViewById<TextView>(Resource.Id.txtNatuurgebied3);
            TextView txtHotel1 = FindViewById<TextView>(Resource.Id.txtHotel1);
            TextView txtHotel2 = FindViewById<TextView>(Resource.Id.txtHotel2);
            TextView txtValuta = FindViewById<TextView>(Resource.Id.txtValuta);
            TextView txtLandVerhaal = FindViewById<TextView>(Resource.Id.txtLandVerhaal);

            foreach (var item in lstLanden)
            {
                if (item.LandNaam == landNaam)
                {
                    txtLandNaam.Text = item.LandNaam;
                    txtHoofdstad.Text = item.Hoofdstad;
                    txtGerecht1.Text = item.Gerecht1;
                    txtGerecht2.Text = item.Gerecht2;
                    txtGerecht3.Text = item.Gerecht3;
                    txtBezienswaardigheid1.Text = item.Bezienswaardigheid1;
                    txtBezienswaardigheid2.Text = item.Bezienswaardigheid2;
                    txtBezienswaardigheid3.Text = item.Bezienswaardigheid3;
                    txtBezienswaardigheid4.Text = item.Bezienswaardigheid4;
                    txtBezienswaardigheid5.Text = item.Bezienswaardigheid5;
                    txtNatuurgebied1.Text = item.Natuurgebied1;
                    txtNatuurgebied2.Text = item.Natuurgebied2;
                    txtNatuurgebied3.Text = item.Natuurgebied3;
                    txtHotel1.Text = item.Hotel1;
                    txtHotel2.Text = item.Hotel2;
                    txtValuta.Text = item.Valuta;
                    txtLandVerhaal.Text = item.Description;
                }
            }
        }

        private void BtnRecensie_Click(object sender, EventArgs e, bool LoginCheck, int mUserID, string landNaam, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(RecensieActivity));
            nextActivity.PutExtra("mUserID", mUserID);
            nextActivity.PutExtra("landNaam", landNaam);
            nextActivity.PutExtra("Username", Username);

            if (LoginCheck == true)
            {
                nextActivity.PutExtra("LoginCheck", true);
            }
            else
            {
                nextActivity.PutExtra("LoginCheck", false);
            }
            StartActivity(nextActivity);
        }

        private void BtnHome_Click(object sender, EventArgs e, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            nextActivity.PutExtra("mUserID", mUserID);
            nextActivity.PutExtra("Username", Username);

            if (LoginCheck == true)
            {
                nextActivity.PutExtra("LoginCheck", true);
            }
            else
            {
                nextActivity.PutExtra("LoginCheck", false);
            }
            StartActivity(nextActivity);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        public void setLandNaam(string landNaam)
        {
            TextView txtLandNaam = FindViewById<TextView>(Resource.Id.txtLandNaam);
            if (landNaam == "Oekraine")
            {
                landNaam = "Oekraïne";
            }
            if (landNaam == "Tsjechie")
            {
                landNaam = "Tsjechië";
            }
            if (landNaam == "Belgie")
            {
                landNaam = "België";
            }
            if (landNaam == "Italie")
            {
                landNaam = "Italië";
            }
            if (landNaam == "Kroatie")
            {
                landNaam = "Kroatië";
            }
            if (landNaam == "Slovenie")
            {
                landNaam = "Slovenië";
            }
            txtLandNaam.Text = landNaam;
        }

        public override void OnBackPressed()
        {


        }

        private void setLinks(string landNaam)
        {
            //  Dit zijn de standaard waarden voor als een land geen link heeft gekregen
            string link_hoofdstad;
            link_hoofdstad = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_gerecht1;
            link_gerecht1 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_gerecht2;
            link_gerecht2 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_gerecht3;
            link_gerecht3 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_valuta;
            link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=EUR"; // deze link verwijst altijd al goed bij elke valuta
            string link_beziens1;
            link_beziens1 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_beziens2;
            link_beziens2 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_beziens3;
            link_beziens3 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_beziens4;
            link_beziens4 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_beziens5;
            link_beziens5 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_natuur1;
            link_natuur1 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_natuur2;
            link_natuur2 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_natuur3;
            link_natuur3 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_hotel1;
            link_hotel1 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde
            string link_hotel2;
            link_hotel2 = "https://www.google.com/"; // er moet een link zijn anders heeft link geen waarde


            foreach (var item in lstLanden)
            {
                if (item.LandNaam == landNaam)

                {
                    if (item.Valuta == "Zwitserse Frank (CHF)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=CHF";
                    }
                    if (item.Valuta == "Noorse kroon (NOK)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=NOK";
                    }
                    if (item.Valuta == "Grivna (UAH)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=UAH";
                    }
                    if (item.Valuta == "Bulgaarse Lev (BGN)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=BGN";
                    }
                    if (item.Valuta == "Zweedse kroon (SEK)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=SEK";
                    }
                    if (item.Valuta == "Deense Kroon (DKK)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=DKK";
                    }
                    if (item.Valuta == "Ijslandse kroon")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=ISK";
                    }
                    if (item.Valuta == "Tsjechië Kroon (CZK)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=CZK";
                    }
                    if (item.Valuta == "Kuna (HRK)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=HRK";
                    }
                    if (item.Valuta == "Russische roebel (RUB)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=RUB";
                    }
                    if (item.Valuta == "Poolse zloty (PLN)")
                    {
                        link_valuta = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=EUR&To=PLN";
                    }
                    // Wisselkoersen een link geven , hoef je niks voor te linken er is maar 1 link nodig
                    TextView valuta = FindViewById<TextView>(Resource.Id.txtValuta);
                    valuta.TextFormatted = Html.FromHtml(
                                    "<a href=\'" + link_valuta + "'>" + item.Valuta + "</a> ");     // deze methode is 'obsolete' maar kan nog wel gebruikt worden.

                    valuta.MovementMethod = LinkMovementMethod.Instance;  // dit gebruik je om er op te kunnen klikken

                    // links van hoofdsteden in Noord Europa
                    if (item.Hoofdstad == "Helsinki")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven+107,+3011+WN+Rotterdam/Helsinki,+Finland/@56.0993553,10.2272296,6z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x46920bc796210691:0xcd4ebd843be2f763!2m2!1d24.9383791!2d60.1698557";
                    }
                    if (item.Hoofdstad == "Oslo")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven+107,+3011+WN+Rotterdam/Oslo,+Norway/@55.8417953,4.333867,6z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x46416e61f267f039:0x7e92605fd3231e9a!2m2!1d10.7522454!2d59.9138688/";
                    }
                    if (item.Hoofdstad == "Stockholm")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven+107,+3011+WN+Rotterdam/Stockholm,+Sweden/@55.5468456,6.7745958,6z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x465f763119640bcb:0xa80d27d3679d7766!2m2!1d18.0685808!2d59.3293235";
                    }
                    if (item.Hoofdstad == "Kopenhagen")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven+107,+3011+WN+Rotterdam/Copenhagen,+Denmark/@53.7771582,6.2682032,7z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x4652533c5c803d23:0x4dd7edde69467b8!2m2!1d12.5683372!2d55.6760968";
                    }
                    if (item.Hoofdstad == "Dublin")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven+107,+3011+WN+Rotterdam/Dublin,+Ireland/@52.1226035,-2.9999704,7z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x48670e80ea27ac2f:0xa00c7a9973171a0!2m2!1d-6.2603097!2d53.3498053";
                    }
                    if (item.Hoofdstad == "Reykjavik")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven+107,+3011+WN+Rotterdam/Reykjav%C3%ADk,+Iceland/@58.5643167,-14.8589715,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x48d674b9eedcedc3:0xec912ca230d26071!2m2!1d-21.8174392!2d64.1265206";
                    }
                    // link voor hoofdsteden in Oost Europa
                    if (item.Hoofdstad == "Moskou")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Moscow,+Russia/@52.9586509,12.0821502,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x46b54afc73d4b0c9:0x3d44d6cc5757cf4c!2m2!1d37.6172999!2d55.755826";
                    }
                    if (item.Hoofdstad == "Kiev")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Kiev,+Ukraine/@50.8108712,8.5289074,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x40d4cf4ee15a4505:0x764931d2170146fe!2m2!1d30.5234!2d50.4501";
                    }
                    if (item.Hoofdstad == "Warschau")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Warsaw,+Poland/@52.0861344,8.24229,6z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x471ecc669a869f01:0x72f0be2a88ead3fc!2m2!1d21.0122287!2d52.2296756";
                    }
                    if (item.Hoofdstad == "Sofia")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Sofia,+Bulgaria/@47.0514156,4.9179283,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x40aa8682cb317bf5:0x400a01269bf5e60!2m2!1d23.3218675!2d42.6977082";
                    }
                    if (item.Hoofdstad == "Bratislava")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Bratislava,+Slovakia/@49.9706978,6.3087197,6z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x476c89360aca6197:0x631f9b82fd884368!2m2!1d17.1077478!2d48.1485965";
                    }
                    if (item.Hoofdstad == "Praag")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Prague,+Czechia/@51.1732767,5.0006682,6z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x470b939c0970798b:0x400af0f66164090!2m2!1d14.4378005!2d50.0755381";
                    }
                    // link voor hoofdsteden in Zuid Europa
                    if (item.Hoofdstad == "Madrid")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Madrid,+Spain/@45.7922038,-8.1808628,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0xd422997800a3c81:0xc436dec1618c2269!2m2!1d-3.7037902!2d40.4167754";
                    }
                    if (item.Hoofdstad == "Rome")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Rome,+Italy/@46.5711742,-1.395379,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x13258a111bd74ac3:0x3094f9ab2388100!2m2!1d12.4962352!2d41.9027008";
                    }
                    if (item.Hoofdstad == "Lissabon")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Lisbon,+Portugal/@44.9158617,-11.1876764,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0xd19331a61e4f33b:0x400ebbde49036d0!2m2!1d-9.1393366!2d38.7222524";
                    }
                    if (item.Hoofdstad == "Athene")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Athens,+Greece/@44.692855,5.1788208,5z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x14a1bd1f067043f1:0x2736354576668ddd!2m2!1d23.7275388!2d37.9838096";
                    }
                    if (item.Hoofdstad == "Zagreb")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Zagreb,+Croatia/@48.7861018,5.7433413,6z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x4765d692c902cc39:0x3a45249628fbc28a!2m2!1d15.9819189!2d45.8150108!3e4";
                    }
                    if (item.Hoofdstad == "Ljubljana")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Ljubljana,+Slovenia/@48.9071044,5.005309,6z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x476531f5969886d1:0x400f81c823fec20!2m2!1d14.5057515!2d46.0569465!3e4";
                    }
                    // link voor hoofdsteden in West Europa
                    if (item.Hoofdstad == "Amsterdam")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Amsterdam/@52.1166314,4.1330816,9z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x47c63fb5949a7755:0x6600fd4cb7c0af8d!2m2!1d4.8951679!2d52.3702157!3e0";
                    }
                    if (item.Hoofdstad == "Parijs")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Paris,+France/@50.3503667,1.5948412,7z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x47e66e1f06e2b70f:0x40b82c3688c9460!2m2!1d2.3522219!2d48.856614!3e0";
                    }
                    if (item.Hoofdstad == "Brussel")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Brussels,+Belgium/@51.381673,3.3895258,8z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x47c3a4ed73c76867:0xc18b3a66787302a7!2m2!1d4.3517103!2d50.8503396!3e0";
                    }
                    if (item.Hoofdstad == "Berlijn")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Berlin,+Germany/@51.9810135,6.7013235,7z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x47a84e373f035901:0x42120465b5e3b70!2m2!1d13.404954!2d52.5200066!3e0";
                    }
                    if (item.Hoofdstad == "Wenen")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Vienna,+Austria/@50.0165902,5.9423396,6z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x476d079e5136ca9f:0xfdc2e58a51a25b46!2m2!1d16.3738189!2d48.2081743!3e0";
                    }
                    if (item.Hoofdstad == "Bern")
                    {
                        link_hoofdstad = "https://www.google.com/maps/dir/Rotterdam+University+of+Applied+Sciences,+Wijnhaven,+Rotterdam/Bern,+Switzerland/@49.3505706,2.0394669,6z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x47c4335dd0e438b7:0xed54cfc9f8ebad3!2m2!1d4.4840313!2d51.9171985!1m5!1m1!1s0x478e39c0d43a1b77:0xcb555ffe0457659a!2m2!1d7.4474468!2d46.9479739!3e0";
                    }
                    // hoofdsteden een link geven
                    TextView hoofdstad = FindViewById<TextView>(Resource.Id.txtHoofdstad);
                    hoofdstad.TextFormatted = Html.FromHtml(
                                    "<a href=\'" + link_hoofdstad + "'>" + item.Hoofdstad + "</a> ");

                    hoofdstad.MovementMethod = LinkMovementMethod.Instance;

                    // gerecht1 een link geven Noord-Europa
                    if (item.Gerecht1 == "Guinness brood")
                    {
                        link_gerecht1 = "https://kampvuurkok.nl/guinness-bread/";
                    }
                    if (item.Gerecht1 == "Ärtsoppa med pannkakor")
                    {
                        link_gerecht1 = "https://www.thespruceeats.com/dried-pea-soup-artsoppa-in-swedish-2952931";
                    }
                    if (item.Gerecht1 == "Rakfisk")
                    {
                        link_gerecht1 = "https://www.atlasobscura.com/foods/rakfisk-norwegian-fermented-fish";
                    }
                    if (item.Gerecht1 == "Köttbullar")
                    {
                        link_gerecht1 = "https://www.196flavors.com/sweden-kottbullar-swedish-meatballs/";
                    }
                    if (item.Gerecht1 == "Knäckebrød")
                    {
                        link_gerecht1 = "http://www.geniuskitchen.com/recipe/swedish-crispbread-kn-ckebr-d-30989";
                    }
                    if (item.Gerecht1 == "Hákarl")
                    {
                        link_gerecht1 = "https://en.wikipedia.org/wiki/H%C3%A1karl";
                    }
                    // gerecht1 een link geven Oost-Europa
                    if (item.Gerecht1 == "Borsjtsj")
                    {
                        link_gerecht1 = "http://www.darmgezondheid.nl/voeding-en-leefstijl/darmgezonde-recepten/borsjtsj-russische-bietensoep/";
                    }
                    if (item.Gerecht1 == "Pierogi")
                    {
                        link_gerecht1 = "https://www.epicurious.com/recipes/food/views/pierogies-109296";
                    }
                    if (item.Gerecht1 == "Barszcz")
                    {
                        link_gerecht1 = "https://www.thespruceeats.com/polish-beet-borscht-soup-recipe-1137127";
                    }
                    if (item.Gerecht1 == "Moussaka")
                    {
                        link_gerecht1 = "https://www.bbc.com/food/recipes/moussaka_01004";
                    }
                    if (item.Gerecht1 == "Kapustnica")
                    {
                        link_gerecht1 = "https://www.allrecipes.com/recipe/38835/slovak-sauerkraut-christmas-soup/";
                    }
                    if (item.Gerecht1 == "Chlebicky")
                    {
                        link_gerecht1 = "https://www.196flavors.com/czech-republic-oblozene-chlebicky/";
                    }
                    // gerecht1 een link geven Zuid-Europa
                    if (item.Gerecht1 == "Paella")
                    {
                        link_gerecht1 = "https://www.allrecipes.com/recipe/84137/easy-paella/";
                    }
                    if (item.Gerecht1 == "Spaghetti")
                    {
                        link_gerecht1 = "https://www.bbcgoodfood.com/recipes/1502640/the-best-spaghetti-bolognese";
                    }
                    if (item.Gerecht1 == "Cataplana de marisco")
                    {
                        link_gerecht1 = "https://easyportugueserecipes.com/cataplana/";
                    }
                    if (item.Gerecht1 == "Tzatziki")
                    {
                        link_gerecht1 = "https://www.jamieoliver.com/news-and-features/features/how-to-make-tzatziki/";
                    }
                    if (item.Gerecht1 == "Burek")
                    {
                        link_gerecht1 = "https://www.smulweb.nl/recepten/burek";
                    }
                    if (item.Gerecht1 == "Štruklji")
                    {
                        link_gerecht1 = "https://www.amuse-your-bouche.com/slovenian-struklji-apple-dumplings-and-a-trip-to-ljubljana/";
                    }
                    // gerecht1 een link geven West-Europa
                    if (item.Gerecht1 == "Hutspot")
                    {
                        link_gerecht1 = "https://www.lekkerensimpel.com/hutspot/";
                    }
                    if (item.Gerecht1 == "Escargots")
                    {
                        link_gerecht1 = "https://www.cookingchanneltv.com/recipes/escargots-in-garlic-and-parsley-butter-2124879";
                    }
                    if (item.Gerecht1 == "Mosselen")
                    {
                        link_gerecht1 = "https://www.mosselen.nl/nl/recepten/";
                    }
                    if (item.Gerecht1 == "Bratwurst")
                    {
                        link_gerecht1 = "https://www.allrecipes.com/recipe/14524/wisconsin-bratwurst/";
                    }
                    if (item.Gerecht1 == "Apfelstrudel")
                    {
                        link_gerecht1 = "http://allrecipes.nl/recept/1686/apfelstrudel.aspx";
                    }
                    if (item.Gerecht1 == "Rösti")
                    {
                        link_gerecht1 = "https://www.myswitzerland.com/en/roesti.html";
                    }

                    TextView gerecht1 = FindViewById<TextView>(Resource.Id.txtGerecht1);
                    gerecht1.TextFormatted = Html.FromHtml(
                                    "<a href=\'" + link_gerecht1 + "'>" + item.Gerecht1 + "</a> ");

                    gerecht1.MovementMethod = LinkMovementMethod.Instance;

                    // gerecht2 een link geven Noord Europa
                    if (item.Gerecht2 == "Lobster lawyer")
                    {
                        link_gerecht2 = "http://www.geniuskitchen.com/recipe/dublin-lawyer-lobster-dublin-style-with-whiskey-and-cream-288456";
                    }
                    if (item.Gerecht2 == "Rapujuhlat")
                    {
                        link_gerecht2 = "https://en.wikipedia.org/wiki/Crayfish_party";
                    }
                    if (item.Gerecht2 == "Lutefisk")
                    {
                        link_gerecht2 = "https://whatscookingamerica.net/History/LutefiskHistory.htm";
                    }
                    if (item.Gerecht2 == "Kräftskiva")
                    {
                        link_gerecht2 = "https://visitsweden.com/crayfish-party/";
                    }
                    if (item.Gerecht2 == "Filmjølk")
                    {
                        link_gerecht2 = "https://www.culturesforhealth.com/learn/yogurt/counter-top-yogurt-starters-video/";
                    }
                    if (item.Gerecht2 == "Hardfiskur")
                    {
                        link_gerecht2 = "http://icecook.blogspot.com/2006/01/harfiskur-icelandic-hard-dried-fish_18.html";
                    }
                    // gerecht2 een link geven Oost Europa
                    if (item.Gerecht2 == "Kulich")
                    {
                        link_gerecht2 = "https://natashaskitchen.com/paska-easter-bread-recipe-kulich/";
                    }
                    if (item.Gerecht2 == "Kasja")
                    {
                        link_gerecht2 = "http://weekend.knack.be/lifestyle/culinair/recepten/balletjes-van-kasja-en-courgette/recipe-snack-839897.html";
                    }
                    if (item.Gerecht2 == "Zùr")
                    {
                        link_gerecht2 = "https://www.bonappetit.com/recipe/creamy-polenta";
                    }
                    if (item.Gerecht2 == "Apetitka")
                    {
                        link_gerecht2 = "http://en.receptite.com/recipe/apetitka-roasted-peppers";
                    }
                    if (item.Gerecht2 == "Lokše")
                    {
                        link_gerecht2 = "http://www.slovakcooking.com/2010/recipes/lokshe/";
                    }
                    if (item.Gerecht2 == "Bramborov polevka")
                    {
                        link_gerecht2 = "http://www.czechcookbook.com/potato-soup/";
                    }
                    // gerecht2 een link geven Zuid Europa
                    if (item.Gerecht2 == "Tortilla")
                    {
                        link_gerecht2 = "https://www.bbcgoodfood.com/recipes/spanish-tortilla";
                    }
                    if (item.Gerecht2 == "Pizza")
                    {
                        link_gerecht2 = "https://www.cntraveler.com/galleries/2014-12-01/10-best-pizzerias-in-italy-rome-naples-venice";
                    }
                    if (item.Gerecht2 == "Pastel de nata")
                    {
                        link_gerecht2 = "https://leitesculinaria.com/7759/recipes-pasteis-de-nata.html";
                    }
                    if (item.Gerecht2 == "Gyros")
                    {
                        link_gerecht2 = "https://www.bbcgoodfood.com/recipes/chicken-gyros";
                    }
                    if (item.Gerecht2 == "Blitva")
                    {
                        link_gerecht2 = "http://www.geniuskitchen.com/recipe/blitva-croatian-swiss-chard-dish-144344";
                    }
                    if (item.Gerecht2 == "Jota")
                    {
                        link_gerecht2 = "http://www.slovenia.si/visit/cuisine/recipes/jota-bean-and-pickled-turnips-soup/";
                    }
                    // gerecht2 een link geven West Europa 
                    if (item.Gerecht2 == "Pannenkoeken")
                    {
                        link_gerecht2 = "http://www.potsenmaeker.nl/";
                    }
                    if (item.Gerecht2 == "Friet")
                    {
                        link_gerecht2 = "http://www.lutosa.com/nl/bedrijf/dol-op-belgische-friet/";
                    }
                    if (item.Gerecht2 == "Flammkuchen")
                    {
                        link_gerecht2 = "https://uitpaulineskeuken.nl/2014/12/flammkuchen.html";
                    }
                    if (item.Gerecht2 == "Käzespätzle")
                    {
                        link_gerecht2 = "https://www.snowplaza.nl/weblog/1205-recept-kasespaetzle/";
                    }
                    if (item.Gerecht2 == "Alpenpasta")
                    {
                        link_gerecht2 = "https://www.indebergen.nl/weblog/10239-zwitsers-bergrecept-alplermagronen/";
                    }
                    if (item.Gerecht2 == "Foie gras")
                    {
                        link_gerecht2 = "https://www.dartagnan.com/how-to-sear-foie-gras.html";
                    }
                    TextView gerecht2 = FindViewById<TextView>(Resource.Id.txtGerecht2);
                    gerecht2.TextFormatted = Html.FromHtml(
                                    "<a href=\'" + link_gerecht2 + "'>" + item.Gerecht2 + "</a> ");

                    gerecht2.MovementMethod = LinkMovementMethod.Instance;



                    // gerecht3 een link geven Noord Europa
                    if (item.Gerecht3 == "Ierse stampot")
                    {

                        link_gerecht3 = "http://allrecipes.nl/recept/2860/colcannon--ierse-stamppot-.aspx";
                    }
                    if (item.Gerecht3 == "Leipäjuusto")
                    {
                        link_gerecht3 = "http://allrecipes.co.uk/recipe/42965/-leipajuusto--finnish-baked-cheese-.aspx";
                    }
                    if (item.Gerecht3 == "Pinnekjøt")
                    {
                        link_gerecht3 = "http://www.scandikitchen.co.uk/recipe-pinnekjott-traditional-norwegian-christmas-dinner/";
                    }
                    if (item.Gerecht3 == "Pyttipanna")
                    {
                        link_gerecht3 = "http://www.swedishfood.com/swedish-food-recipes-main-courses/194-pyttipanna";
                    }
                    if (item.Gerecht3 == "Smørrebrød")
                    {
                        link_gerecht3 = "https://www.saveur.com/how-to-make-smorrebrod#page-7";
                    }
                    if (item.Gerecht3 == "Slátur")
                    {
                        link_gerecht3 = "http://icelandmag.is/article/food-vikings-how-make-authentic-icelandic-delicacy-slatur-slaughter";
                    }
                    // gerecht3 een link geven Oost Europa
                    if (item.Gerecht3 == "Golubtsji")
                    {
                        link_gerecht3 = "https://natashaskitchen.com/golubtsi-recipe-a-classic-russian-food/";
                    }
                    if (item.Gerecht3 == "Kvas")
                    {
                        link_gerecht3 = "https://natashaskitchen.com/angelinas-easy-bread-kvas-recipe/";
                    }
                    if (item.Gerecht3 == "Kapusniak")
                    {
                        link_gerecht3 = "https://www.thespruceeats.com/polish-sauerkraut-soup-recipe-1137137";
                    }
                    if (item.Gerecht3 == "Tarator")
                    {
                        link_gerecht3 = "http://www.geniuskitchen.com/recipe/tarator-bulgarian-cold-cucumber-soup-62181";
                    }
                    if (item.Gerecht3 == "Granadír")
                    {
                        link_gerecht3 = "http://vegancooking.tk/en/main-course/granadir-pasta-with-potatoes/";
                    }
                    if (item.Gerecht3 == "Vepřo knedlo zelo")
                    {
                        link_gerecht3 = "https://eatyourworld.com/destinations/europe/czech_republic/prague/what_to_eat/vepoknedlozelo";
                    }
                    // gerecht3 een link geven Zuid Europa
                    if (item.Gerecht3 == "Gazpacho")
                    {
                        link_gerecht3 = "https://www.allrecipes.com/recipe/222331/chef-johns-gazpacho/";
                    }
                    if (item.Gerecht3 == "Lasagne")
                    {
                        link_gerecht3 = "https://www.tasteofhome.com/recipes/best-lasagna";
                    }
                    if (item.Gerecht3 == "Torrades")
                    {
                        link_gerecht3 = "http://www.francescakookt.nl/torrades-descalivada/";
                    }
                    if (item.Gerecht3 == "Moussaka")
                    {
                        link_gerecht3 = "https://www.bbc.com/food/recipes/moussaka_01004";
                    }
                    if (item.Gerecht3 == "Mazalice")
                    {
                        link_gerecht3 = "https://www.ajvar.nl/kroatische_recepten/mazalice/";
                    }
                    if (item.Gerecht3 == "Kranjska klobasa")
                    {
                        link_gerecht3 = "https://www.thespruceeats.com/slovenian-carniolian-sausage-recipe-1137407";
                    }
                    // gerecht3 een link geven West Europa
                    if (item.Gerecht3 == "Aardappelen")
                    {
                        link_gerecht3 = "http://www.voedingscentrum.nl/encyclopedie/aardappelen.aspx";
                    }
                    if (item.Gerecht3 == "Croque - monsieur")
                    {
                        link_gerecht3 = "https://www.marthastewart.com/334699/croque-monsieur";
                    }
                    if (item.Gerecht3 == "Waterzooi")
                    {
                        link_gerecht3 = "http://www.visitflanders.com/en/themes/flemish-food/flemish-dishes-and-specialities/flemish-dishes/ghent-waterzooi/";
                    }
                    if (item.Gerecht3 == "Kartoffelsalat")
                    {
                        link_gerecht3 = "http://www.geniuskitchen.com/recipe/bavarian-potato-salad-bayerischer-kartoffelsalat-425795";
                    }
                    if (item.Gerecht3 == "Kaiserschmarrn")
                    {
                        link_gerecht3 = "https://www.allrecipes.com/recipe/141136/kaiserschmarrn/";
                    }
                    if (item.Gerecht3 == "Raclette")
                    {
                        link_gerecht3 = "https://www.bbcgoodfood.com/recipes/raclette";
                    }
                    TextView gerecht3 = FindViewById<TextView>(Resource.Id.txtGerecht3);
                    gerecht3.TextFormatted = Html.FromHtml(
                                    "<a href=\'" + link_gerecht3 + "'>" + item.Gerecht3 + "</a> ");
                    gerecht3.MovementMethod = LinkMovementMethod.Instance;

                    // bezienswaardigheid1 een link geven Noord Europa
                    if (item.Bezienswaardigheid1 == "Kylemore Abbey")
                    {
                        link_beziens1 = "https://www.kylemoreabbey.com/";
                    }
                    if (item.Bezienswaardigheid1 == "Saimaameer")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Saimaameer";
                    }
                    if (item.Bezienswaardigheid1 == "Lofoten")
                    {
                        link_beziens1 = "https://en.wikipedia.org/wiki/Lofoten";
                    }
                    if (item.Bezienswaardigheid1 == "Gamla Stan")
                    {
                        link_beziens1 = "https://www.visitstockholm.com/see--do/attractions/gamla-stan/";
                    }
                    if (item.Bezienswaardigheid1 == "Tivoli")
                    {
                        link_beziens1 = "https://www.tivoli.dk/en/";
                    }
                    if (item.Bezienswaardigheid1 == "Blue Lagoon")
                    {
                        link_beziens1 = "https://www.bluelagoon.com/";
                    }
                    // bezienswaardigheid1 een link geven Oost Europa
                    if (item.Bezienswaardigheid1 == "Kremlin van Moskou")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Kremlin_van_Moskou";
                    }
                    if (item.Bezienswaardigheid1 == "Holenklooster van kiev")
                    {
                        link_beziens1 = "https://www.cityspotters.com/oekraine/kiev/pechersk-lavra-holenklooster";
                    }
                    if (item.Bezienswaardigheid1 == "Wieliczk-zoutmijn")
                    {
                        link_beziens1 = "https://www.bezoekkrakau.nl/bezienswaardigheden-krakau/zoutmijn-krakau/";
                    }
                    if (item.Bezienswaardigheid1 == "Borovets")
                    {
                        link_beziens1 = "https://en.wikipedia.org/wiki/Borovets";
                    }
                    if (item.Bezienswaardigheid1 == "Kasteel van Bratislava")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Kasteel_van_Bratislava";
                    }
                    if (item.Bezienswaardigheid1 == "Karelsburg")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Karelsbrug";
                    }
                    // bezienswaardigheid1 een link geven Zuid Europa
                    if (item.Bezienswaardigheid1 == "Garganta de los Infiernos")
                    {
                        link_beziens1 = "http://www.turismoextremadura.com/viajar/turismo/en/explora/Garganta-de-los-Infiernos-Nature-Reserve_1924203559/";
                    }
                    if (item.Bezienswaardigheid1 == "Colosseum")
                    {
                        link_beziens1 = "https://www.rome-museum.com/colosseum-palatino-roman-forum.php?gclid=EAIaIQobChMIwKWv_LfL2wIVYirTCh1NvAGkEAAYAiAAEgK53vD_BwE";
                    }
                    if (item.Bezienswaardigheid1 == "Porto(havenstad bekent om wijn)")
                    {
                        link_beziens1 = "https://www.landenweb.nl/portugal/porto/";
                    }
                    if (item.Bezienswaardigheid1 == "Akropolis van Athene")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Akropolis_van_Athene";
                    }
                    if (item.Bezienswaardigheid1 == "Nationaal park")
                    {
                        link_beziens1 = "https://www.allesoverkroatie.nl/nationaal-park-plitvice/";
                    }
                    if (item.Bezienswaardigheid1 == "De Vintgar Kloof en de Sum Waterval")
                    {
                        link_beziens1 = "https://kidseropuit.nl/wandelen-vintgar-kloof-met-kinderen/";
                    }
                    // bezienswaardigheid1 een link geven West Europa
                    if (item.Bezienswaardigheid1 == "Red light district")
                    {
                        link_beziens1 = "https://www.amsterdam.info/red-light-district/";
                    }
                    if (item.Bezienswaardigheid1 == "Disneyland parijs")
                    {
                        link_beziens1 = "http://www.disneylandparis.com/en-us/disney-hotels/?ecid=SEM_ild23_A_3b93d0fa-d303-458b-8a97-7af6ef1972ef&customid=173993972_1EAIaIQobChMIgPaO8bjL2wIVVBobCh2xLwHjEAAYASAAEgJ4-_D_BwE&customid2=1_c_173993972_8723594492_kwd-68609491";
                    }
                    if (item.Bezienswaardigheid1 == "Manneke pis")
                    {
                        link_beziens1 = "https://en.wikipedia.org/wiki/Manneken_Pis";
                    }
                    if (item.Bezienswaardigheid1 == "Zoo Berlin")
                    {
                        link_beziens1 = "https://www.zoo-berlin.de/en";
                    }
                    if (item.Bezienswaardigheid1 == "Wenen")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Wenen";
                    }
                    if (item.Bezienswaardigheid1 == "Meer van Genève")
                    {
                        link_beziens1 = "https://nl.wikipedia.org/wiki/Meer_van_Gen%C3%A8ve";
                    }

                    TextView beziens1 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid1);
                    beziens1.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_beziens1 + "'>" + item.Bezienswaardigheid1 + "</a> ");
                    beziens1.MovementMethod = LinkMovementMethod.Instance;

                    // bezienswaardigheid2 een link geven Noord Europa
                    if (item.Bezienswaardigheid2 == "Brú na Bóinne")
                    {
                        link_beziens2 = "http://www.worldheritageireland.ie/bru-na-boinne/";
                    }
                    if (item.Bezienswaardigheid2 == "Midzomerfeest")
                    {
                        link_beziens2 = "https://visitsweden.nl/midzomer/";
                    }
                    if (item.Bezienswaardigheid2 == "Preikestolen")
                    {
                        link_beziens2 = "https://www.visitnorway.com/places-to-go/fjord-norway/the-stavanger-region/listings-stavanger/preikestolen/185743/";
                    }
                    if (item.Bezienswaardigheid2 == "Vasa")
                    {
                        link_beziens2 = "https://www.vasamuseet.se/en";
                    }
                    if (item.Bezienswaardigheid2 == "Nyhavn")
                    {
                        link_beziens2 = "https://www.visitcopenhagen.com/copenhagen/nyhavn-gdk474735";
                    }
                    if (item.Bezienswaardigheid2 == "Gullfoss")
                    {
                        link_beziens2 = "http://gullfoss.is/";
                    }
                    // bezienswaardigheid2 een link geven Oost Europa
                    if (item.Bezienswaardigheid2 == "Baikalmeer")
                    {
                        link_beziens2 = "https://nl.wikipedia.org/wiki/Baikalmeer";
                    }
                    if (item.Bezienswaardigheid2 == "sint-sofiakathedraal")
                    {
                        link_beziens2 = "https://www.cityspotters.com/oekraine/kiev/st-sofia-kathedraal";
                    }
                    if (item.Bezienswaardigheid2 == "Auschwitz")
                    {
                        link_beziens2 = "http://auschwitz.org/en/";
                    }
                    if (item.Bezienswaardigheid2 == "Seven Rila Lakes")
                    {
                        link_beziens2 = "https://en.wikipedia.org/wiki/Seven_Rila_Lakes";
                    }
                    if (item.Bezienswaardigheid2 == "Spisský hard")
                    {
                        link_beziens2 = "https://en.wikipedia.org/wiki/Spi%C5%A1_Castle";
                    }
                    if (item.Bezienswaardigheid2 == "Praagse burcht")
                    {
                        link_beziens2 = "https://www.cityspotters.com/tsjechie/praag/praagse-burcht";
                    }
                    // bezienswaardigheid2 een link geven Zuid Europa
                    if (item.Bezienswaardigheid2 == "Bárdenas Reales")
                    {
                        link_beziens2 = "http://www.turismo.navarra.es/eng/organice-viaje/recurso/Patrimonio/3023/Parque-Natural-de-las-Bardenas-Reales.htm";
                    }
                    if (item.Bezienswaardigheid2 == "Toren van Pisa")
                    {
                        link_beziens2 = "https://nl.wikipedia.org/wiki/Toren_van_Pisa";
                    }
                    if (item.Bezienswaardigheid2 == "Douro")
                    {
                        link_beziens2 = "https://en.wikipedia.org/wiki/Douro";
                    }
                    if (item.Bezienswaardigheid2 == "Meteora")
                    {
                        link_beziens2 = "https://en.wikipedia.org/wiki/Meteora";
                    }
                    if (item.Bezienswaardigheid2 == "Hvar")
                    {
                        link_beziens2 = "http://www.hvarinfo.com/";
                    }
                    if (item.Bezienswaardigheid2 == "Kerk in het Meer van Bled")
                    {
                        link_beziens2 = "https://www.wiki-vakantie.nl/vakantielanden/slovenie/het-meer-van-bled-in";
                    }
                    // bezienswaardigheid2 een link geven West Europa
                    if (item.Bezienswaardigheid2 == "Efteling")
                    {
                        link_beziens2 = "https://www.efteling.com/nl";
                    }
                    if (item.Bezienswaardigheid2 == "Eifeltoren")
                    {
                        link_beziens2 = "https://www.toureiffel.paris/en";
                    }
                    if (item.Bezienswaardigheid2 == "Historische binnenstad van Brugge")
                    {
                        link_beziens2 = "https://www.bruggevoorbeginners.nl/";
                    }
                    if (item.Bezienswaardigheid2 == "Brandenburger Tor")
                    {
                        link_beziens2 = "https://www.visitberlin.de/en/brandenburg-gate";
                    }
                    if (item.Bezienswaardigheid2 == "Alpen")
                    {
                        link_beziens2 = "https://en.wikipedia.org/wiki/Alps";
                    }
                    if (item.Bezienswaardigheid2 == "Bodenmeer")
                    {
                        link_beziens2 = "https://nl.wikipedia.org/wiki/Bodenmeer";
                    }
                    TextView beziens2 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid2);
                    beziens2.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_beziens2 + "'>" + item.Bezienswaardigheid2 + "</a> ");
                    beziens2.MovementMethod = LinkMovementMethod.Instance;

                    // bezienswaardigheid3 een link geven Noord Europa
                    if (item.Bezienswaardigheid3 == "Guinness")
                    {
                        link_beziens3 = "https://en.wikipedia.org/wiki/Guinness";
                    }
                    if (item.Bezienswaardigheid3 == "Suomenlinna")
                    {
                        link_beziens3 = "https://www.suomenlinna.fi/en/";
                    }
                    if (item.Bezienswaardigheid3 == "Geirangerfjord")
                    {
                        link_beziens3 = "https://www.visitnorway.com/places-to-go/fjord-norway/the-geirangerfjord/";
                    }
                    if (item.Bezienswaardigheid3 == "Stockholms slot")
                    {
                        link_beziens3 = "https://www.kungligaslotten.se/english.html";
                    }
                    if (item.Bezienswaardigheid3 == "De kleine zeemeermin")
                    {
                        link_beziens3 = "https://nl.wikipedia.org/wiki/De_kleine_zeemeermin_(beeld)";
                    }
                    if (item.Bezienswaardigheid3 == "Pingvellir")
                    {
                        link_beziens3 = "http://www.thingvellir.is/english.aspx";
                    }
                    // bezienswaardigheid3 een link geven Oost Europa
                    if (item.Bezienswaardigheid3 == "Hermitage")
                    {
                        link_beziens3 = "http://www.hermitagemuseum.org/wps/portal/hermitage/?lng=nl";
                    }
                    if (item.Bezienswaardigheid3 == "Boekovel")
                    {
                        link_beziens3 = "https://en.wikipedia.org/wiki/Bukovel";
                    }
                    if (item.Bezienswaardigheid3 == "Wawel Castle")
                    {
                        link_beziens3 = "http://wawel.krakow.pl/en/";
                    }
                    if (item.Bezienswaardigheid3 == "Vitosja")
                    {
                        link_beziens3 = "https://www.bulgariaski.com/vitosha/index.shtml";
                    }
                    if (item.Bezienswaardigheid3 == "Kasteel van Devín")
                    {
                        link_beziens3 = "https://nl.wikipedia.org/wiki/Kasteel_van_Dev%C3%ADn";
                    }
                    if (item.Bezienswaardigheid3 == "Sint-Vituskathedraal")
                    {
                        link_beziens3 = "https://www.cityspotters.com/tsjechie/praag/sint-vituskathedraal";
                    }
                    // bezienswaardigheid3 een link geven Zuid Europa
                    if (item.Bezienswaardigheid3 == "Cabo de Gata")
                    {
                        link_beziens3 = "https://www.theguardian.com/travel/2017/apr/29/cabo-de-gata-desert-almeria-spain-hiking-beaches-hoilday#img-1";
                    }
                    if (item.Bezienswaardigheid3 == "San Marcoplein")
                    {
                        link_beziens3 = "https://www.cityspotters.com/italie/venetie/san-marcoplein-en-san-marco-basiliek";
                    }
                    if (item.Bezienswaardigheid3 == "Torre de Belém")
                    {
                        link_beziens3 = "http://www.torrebelem.gov.pt/en/index.php?s=white&pid=168";
                    }
                    if (item.Bezienswaardigheid3 == "Parthenon")
                    {
                        link_beziens3 = "https://en.wikipedia.org/wiki/Parthenon";
                    }
                    if (item.Bezienswaardigheid3 == "Korčula")
                    {
                        link_beziens3 = "https://www.allesoverkroatie.nl/korcula/";
                    }
                    if (item.Bezienswaardigheid3 == "Predjama Burcht")
                    {
                        link_beziens3 = "https://www.naturescanner.nl/europa/slovenie/kasteel-predjama";
                    }
                    // bezienswaardigheid3 een link geven West Europa
                    if (item.Bezienswaardigheid3 == "Anne frank huis")
                    {
                        link_beziens3 = "http://www.annefrank.org/nl/";
                    }
                    if (item.Bezienswaardigheid3 == "Louvre")
                    {
                        link_beziens3 = "https://www.louvre.fr/en";
                    }
                    if (item.Bezienswaardigheid3 == "Grote Markt van Brussel")
                    {
                        link_beziens3 = "https://nl.wikipedia.org/wiki/Grote_Markt_(Brussel)";
                    }
                    if (item.Bezienswaardigheid3 == "Kasteel Neuschwanstein")
                    {
                        link_beziens3 = "http://www.neuschwanstein.de/englisch/tourist/index.htm";
                    }
                    if (item.Bezienswaardigheid3 == "Schloss Schönbrunn")
                    {
                        link_beziens3 = "https://www.schoenbrunn.at/en/";
                    }
                    if (item.Bezienswaardigheid3 == "Lago Maggiore")
                    {
                        link_beziens3 = "http://www.illagomaggiore.com/en_US/home";
                    }
                    TextView beziens3 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid3);
                    beziens3.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_beziens3 + "'>" + item.Bezienswaardigheid3 + "</a> ");
                    beziens3.MovementMethod = LinkMovementMethod.Instance;

                    // bezienswaardigheid4 een link geven Noord Europa
                    if (item.Bezienswaardigheid4 == "Ierse Whiskey")
                    {
                        link_beziens4 = "https://www.dramtime.nl/Whisky-gebieden/Ierland";
                    }
                    if (item.Bezienswaardigheid4 == "Dorp van de Kerstman")
                    {
                        link_beziens4 = "https://santaclausvillage.info/";
                    }
                    if (item.Bezienswaardigheid4 == "Noordkaap")
                    {
                        link_beziens4 = "http://www.visitnordkapp.net/en/";
                    }
                    if (item.Bezienswaardigheid4 == "Skansen")
                    {
                        link_beziens4 = "http://www.skansen.se/en/";
                    }
                    if (item.Bezienswaardigheid4 == "Amalienborg")
                    {
                        link_beziens4 = "https://www.visitcopenhagen.com/copenhagen/amalienborg-palace-gdk492887";
                    }
                    if (item.Bezienswaardigheid4 == "Jökulsárlón")
                    {
                        link_beziens4 = "https://guidetoiceland.is/travel-iceland/drive/jokulsarlon";
                    }
                    // bezienswaardigheid4 een link geven Oost Europa
                    if (item.Bezienswaardigheid4 == "Rode plein")
                    {
                        link_beziens4 = "https://nl.wikipedia.org/wiki/Rode_Plein";
                    }
                    if (item.Bezienswaardigheid4 == "Khreshchatyk")
                    {
                        link_beziens4 = "https://www.visitkievukraine.com/sights/khreschatyk/";
                    }
                    if (item.Bezienswaardigheid4 == "Stare Miasto")
                    {
                        link_beziens4 = "https://en.wikipedia.org/wiki/Krak%C3%B3w_Old_Town";
                    }
                    if (item.Bezienswaardigheid4 == "Perperikon")
                    {
                        link_beziens4 = "https://www.atlasobscura.com/places/perperikon";
                    }
                    if (item.Bezienswaardigheid4 == "St. MArtin's Cathedral")
                    {
                        link_beziens4 = "https://en.wikipedia.org/wiki/St_Martin%27s_Cathedral,_Bratislava";
                    }
                    if (item.Bezienswaardigheid4 == "Wenceslausplein")
                    {
                        link_beziens4 = "http://www.reisvormen.nl/tsjechie/wenceslausplein-praag.htm";
                    }
                    // bezienswaardigheid4 een link geven Zuid Europa
                    if (item.Bezienswaardigheid4 == "Sevilla")
                    {
                        link_beziens4 = "https://en.wikipedia.org/wiki/Seville";
                    }
                    if (item.Bezienswaardigheid4 == "Amalfikust")
                    {
                        link_beziens4 = "https://www.travelta.nl/opreisgaan/2016/09/amalfikust-tips-bezienswaardigheden-en-ervaringen/";
                    }
                    if (item.Bezienswaardigheid4 == "Palácio da Pena")
                    {
                        link_beziens4 = "https://www.visitportugal.com/en/content/palacio-nacional-da-pena";
                    }
                    if (item.Bezienswaardigheid4 == "Akropolismuseum")
                    {
                        link_beziens4 = "http://www.theacropolismuseum.gr/en";
                    }
                    if (item.Bezienswaardigheid4 == "Brač")
                    {
                        link_beziens4 = "https://en.wikipedia.org/wiki/Bra%C4%8D";
                    }
                    if (item.Bezienswaardigheid4 == "Krizna Cave")
                    {
                        link_beziens4 = "https://krizna-jama.si/nl/";
                    }
                    // bezienswaardigheid4 een link geven West Europa
                    if (item.Bezienswaardigheid4 == "Keukenhof")
                    {
                        link_beziens4 = "https://keukenhof.nl/nl/";
                    }
                    if (item.Bezienswaardigheid4 == "Versailles")
                    {
                        link_beziens4 = "http://en.chateauversailles.fr/";
                    }
                    if (item.Bezienswaardigheid4 == "Bierbrouwerijen")
                    {
                        link_beziens4 = "http://www.dekoninck.be/nl/brouwerijbezoek/de-experience?gclid=EAIaIQobChMIse2rtNvL2wIVxUAbCh0RAg5wEAAYASAAEgL5jPD_BwE";
                    }
                    if (item.Bezienswaardigheid4 == "Loreley in de Rijnoever")
                    {
                        link_beziens4 = "https://nl.wikipedia.org/wiki/Loreley";
                    }
                    if (item.Bezienswaardigheid4 == "Krimmler Wasserfälle")
                    {
                        link_beziens4 = "http://www.wasserfaelle-krimml.at/en/";
                    }
                    if (item.Bezienswaardigheid4 == "Matterhorn")
                    {
                        link_beziens4 = "https://en.wikipedia.org/wiki/Matterhorn";
                    }
                    TextView beziens4 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid4);
                    beziens4.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_beziens4 + "'>" + item.Bezienswaardigheid4 + "</a> ");
                    beziens4.MovementMethod = LinkMovementMethod.Instance;

                    // bezienswaardigheid5 een link geven Noord Europa
                    if (item.Bezienswaardigheid5 == "Ring of Kerry")
                    {
                        link_beziens5 = "http://www.theringofkerry.com/";
                    }
                    if (item.Bezienswaardigheid5 == "Markt van Helsinki")
                    {
                        link_beziens5 = "https://www.tripadvisor.nl/ShowUserReviews-g189934-d314921-r329242339-Old_Market_Hall-Helsinki_Uusimaa.html";
                    }
                    if (item.Bezienswaardigheid5 == "Bryggen")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Bryggen";
                    }
                    if (item.Bezienswaardigheid5 == "Stadhuis van Stockholm")
                    {
                        link_beziens5 = "http://www.stockholm.se/OmStockholm/Stadshuset/";
                    }
                    if (item.Bezienswaardigheid5 == "Rosenborg")
                    {
                        link_beziens5 = "http://www.kongernessamling.dk/rosenborg/";
                    }
                    if (item.Bezienswaardigheid5 == "Golden Circle")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Golden_Circle_(Iceland)";
                    }
                    // bezienswaardigheid5 een link geven Oost Europa
                    if (item.Bezienswaardigheid5 == "Kathedraal van de oorbede van Moeders Gods")
                    {
                        link_beziens5 = "https://nl.wikipedia.org/wiki/Kathedraal_van_de_Voorbede_van_de_Moeder_Gods_(Moskou)";
                    }
                    if (item.Bezienswaardigheid5 == "Golden Gate, Kiev")
                    {
                        link_beziens5 = "https://www.atlasobscura.com/places/golden-gate-of-kyiv";
                    }
                    if (item.Bezienswaardigheid5 == "Sukiennice")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Sukiennice_Museum";
                    }
                    if (item.Bezienswaardigheid5 == "Nationaal Historisch Museum")
                    {
                        link_beziens5 = "https://www.stedentripsofia.nl/musea/nationaal-historisch-museum/";
                    }
                    if (item.Bezienswaardigheid5 == "Most SNP")
                    {
                        link_beziens5 = "http://www.welcometobratislava.eu/ufo-bridge-tower/";
                    }
                    if (item.Bezienswaardigheid5 == "Josefov")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Josefov_Fortress";
                    }
                    // bezienswaardigheid5 een link geven Zuid Europa
                    if (item.Bezienswaardigheid5 == "Bilbao")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Bilbao";
                    }
                    if (item.Bezienswaardigheid5 == "Trevifontein")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Trevi_Fountain";
                    }
                    if (item.Bezienswaardigheid5 == "Castelo de São Jorge")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/S%C3%A3o_Jorge_Castle";
                    }
                    if (item.Bezienswaardigheid5 == "Knossos")
                    {
                        link_beziens5 = "https://www.grieksegids.nl/kreta/knossos.php";
                    }
                    if (item.Bezienswaardigheid5 == "Nationaal park Krka")
                    {
                        link_beziens5 = "http://www.np-krka.hr/en/";
                    }
                    if (item.Bezienswaardigheid5 == "Grotten van Postojna")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Postojna_Cave";
                    }
                    // bezienswaardigheid5 een link geven West Europa
                    if (item.Bezienswaardigheid5 == "Rijksmuseum")
                    {
                        link_beziens5 = "https://www.rijksmuseum.nl/en";
                    }
                    if (item.Bezienswaardigheid5 == "des Champs-Élysées")
                    {
                        link_beziens5 = "https://www.viator.com/Paris-attractions/Avenue-des-Champs-Elysees/d479-a582?pref=02&mcid=28353&supag=43900205886&supkl=kl&supsc=s&supai=240653133296&supap=1t1&supdv=c&supnt=g&supti=dsa-369040346314&suplp=9064071&supli=&gclid=EAIaIQobChMIyKakr9zL2wIVibgbCh3AUAJQEAAYASAAEgJ_-_D_BwE";
                    }
                    if (item.Bezienswaardigheid5 == "In Flanders Fields Museum")
                    {
                        link_beziens5 = "http://www.inflandersfields.be/en";
                    }
                    if (item.Bezienswaardigheid5 == "Mainau")
                    {
                        link_beziens5 = "https://en.wikipedia.org/wiki/Mainau";
                    }
                    if (item.Bezienswaardigheid5 == "Der Dachstein")
                    {
                        link_beziens5 = "https://www.derdachstein.at/en";
                    }
                    if (item.Bezienswaardigheid5 == "Vierwoudenstrekenmeer")
                    {
                        link_beziens5 = "https://www.stopandstare.nl/vierwoudstrekenmeer-zwitserland/";
                    }
                    TextView beziens5 = FindViewById<TextView>(Resource.Id.txtBezienswaardigheid5);
                    beziens5.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_beziens5 + "'>" + item.Bezienswaardigheid5 + "</a> ");
                    beziens5.MovementMethod = LinkMovementMethod.Instance;

                    // Natuurgebied1 een link geven Noord Europa
                    if (item.Natuurgebied1 == "Ring of Kerry")
                    {
                        link_natuur1 = "http://www.theringofkerry.com/";
                    }
                    if (item.Natuurgebied1 == "Pyynikki Park")
                    {
                        link_natuur1 = "https://en.wikipedia.org/wiki/Pyynikki";
                    }
                    if (item.Natuurgebied1 == "Oslo Fjord")
                    {
                        link_natuur1 = "https://www.visitoslo.com/en/activities-and-attractions/boroughs/oslo-fjord/";
                    }
                    if (item.Natuurgebied1 == "Nationale Park Fulufjället")
                    {
                        link_natuur1 = "http://www.zweden.com/cms/index.php?option=com_content&view=article&id=238:fulufjaellet&catid=59:nationale-parken&Itemid=165";
                    }
                    if (item.Natuurgebied1 == "Park Thy")
                    {
                        link_natuur1 = "https://www.visitdenmark.com/north-jutland/nature/thy-national-park-0";
                    }
                    if (item.Natuurgebied1 == "Gullfoss")
                    {
                        link_natuur1 = "http://gullfoss.is/";
                    }
                    // Natuurgebied1 een link geven Oost Europa
                    if (item.Natuurgebied1 == "Camp Garden")
                    {
                        link_natuur1 = "https://www.flickr.com/photos/marcofieber/16677945723";
                    }
                    if (item.Natuurgebied1 == "Sofiyivka Park")
                    {
                        link_natuur1 = "https://en.wikipedia.org/wiki/Arboretum_Sofiyivka";
                    }
                    if (item.Natuurgebied1 == "Palmiarnia Miejska")
                    {
                        link_natuur1 = "https://www.inyourpocket.com/katowice/Gliwice-Palm-House_73442v";
                    }
                    if (item.Natuurgebied1 == "Prohodna Cave")
                    {
                        link_natuur1 = "https://en.wikipedia.org/wiki/Prohodna";
                    }
                    if (item.Natuurgebied1 == "Koliba")
                    {
                        link_natuur1 = "https://www.tripadvisor.nl/Attraction_Review-g274924-d3253194-Reviews-Koliba-Bratislava_Bratislava_Region.html";
                    }
                    if (item.Natuurgebied1 == "Divoka Sarka")
                    {
                        link_natuur1 = "https://en.wikipedia.org/wiki/Divok%C3%A1_%C5%A0%C3%A1rka";
                    }
                    // Natuurgebied1 een link geven Zuid Europa
                    if (item.Natuurgebied1 == "Natuurpark Las Lagunas de Ruidera")
                    {
                        link_natuur1 = "http://www.masspanje.nl/index.php/denia-stad-costa-blanca/87-frontpage/622-natuurpark-lagunas-de-ruidera";
                    }
                    if (item.Natuurgebied1 == "Dolomieten")
                    {
                        link_natuur1 = "https://nl.wikipedia.org/wiki/Dolomieten";
                    }
                    if (item.Natuurgebied1 == "Praia da Marinha(strand)")
                    {
                        link_natuur1 = "http://www.algarvetips.com/beaches/lagoa/praia-da-marinha/";
                    }
                    if (item.Natuurgebied1 == "Keri Caves")
                    {
                        link_natuur1 = "http://www.zanteisland.com/en/keri-caves-zante.php";
                    }
                    if (item.Natuurgebied1 == "Lopud Island")
                    {
                        link_natuur1 = "https://www.croatiatraveller.com/southern_dalmatia/Elaphiti/Lopud.htm";
                    }
                    if (item.Natuurgebied1 == "Skocjan Caves Park")
                    {
                        link_natuur1 = "http://www.park-skocjanske-jame.si/en/";
                    }
                    // Natuurgebied1 een link geven West Europa
                    if (item.Natuurgebied1 == "Oostvaardersplassen")
                    {
                        link_natuur1 = "https://www.staatsbosbeheer.nl/Natuurgebieden/oostvaardersplassen";
                    }
                    if (item.Natuurgebied1 == "Pyreneeën(bergen)")
                    {
                        link_natuur1 = "https://nl.wikipedia.org/wiki/Pyrenee%C3%ABn";
                    }
                    if (item.Natuurgebied1 == "Tillegembos")
                    {
                        link_natuur1 = "https://www.westtoer.be/nl/doen/provinciedomein-tillegembos";
                    }
                    if (item.Natuurgebied1 == "Montafon")
                    {
                        link_natuur1 = "https://www.montafon.at/en/mountain-experiences/snow/ski-snowboard";
                    }
                    if (item.Natuurgebied1 == "Furka Pass")
                    {
                        link_natuur1 = "https://en.wikipedia.org/wiki/Furka_Pass";
                    }
                    if (item.Natuurgebied1 == "Nationaal park Berchtesgaden")
                    {
                        link_natuur1 = "https://www.beieren.nu/beieren-nationaal-park-berchtesgaden";
                    }
                    TextView natuur1 = FindViewById<TextView>(Resource.Id.txtNatuurgebied1);
                    natuur1.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_natuur1 + "'>" + item.Natuurgebied1 + "</a> ");
                    natuur1.MovementMethod = LinkMovementMethod.Instance;

                    // Natuurgebied2 een link geven Noord Europa
                    if (item.Natuurgebied2 == "Cliffs of Moher")
                    {
                        link_natuur2 = "https://www.cliffsofmoher.ie/";
                    }
                    if (item.Natuurgebied2 == "Nuuksio National Park")
                    {
                        link_natuur2 = "http://www.nationalparks.fi/nuuksionp";
                    }
                    if (item.Natuurgebied2 == "Tromso Fjords")
                    {
                        link_natuur2 = "https://www.tripadvisor.nl/Attraction_Review-g190475-d1208984-Reviews-Tromso_Fjords-Tromso_Troms_Northern_Norway.html";
                    }
                    if (item.Natuurgebied2 == "Sarek National Park")
                    {
                        link_natuur2 = "https://www.sverigesnationalparker.se/en/choose-park---list/sarek-national-park/";
                    }
                    if (item.Natuurgebied2 == "Park Mols Bjerge")
                    {
                        link_natuur2 = "https://www.visitdenmark.nl/nl/oost-jutland/natuur/nationaal-park-mols-bjerge";
                    }
                    if (item.Natuurgebied2 == "Langjokull Glacier")
                    {
                        link_natuur2 = "https://guidetoiceland.is/travel-iceland/drive/langjokull";
                    }
                    // Natuurgebied2 een link geven Oost Europa
                    if (item.Natuurgebied2 == "Dream Gardens Park")
                    {
                        link_natuur2 = "https://www.tripadvisor.com.au/ShowUserReviews-g635883-d6502226-r336392268-Dream_Gardens_Park-Abakan_Republic_of_Khakassia_Siberian_District.html";
                    }
                    if (item.Natuurgebied2 == "Ternopil Pond")
                    {
                        link_natuur2 = "https://en.wikipedia.org/wiki/Ternopil_Pond";
                    }
                    if (item.Natuurgebied2 == "Ogrody Kapias")
                    {
                        link_natuur2 = "http://translate.google.com/translate?js=n&sl=pl&tl=gb&u=https%3A%2F%2Fwww.kapias.pl%2F";
                    }
                    if (item.Natuurgebied2 == "Park Vrana")
                    {
                        link_natuur2 = "https://en.wikipedia.org/wiki/Vrana_Palace";
                    }
                    if (item.Natuurgebied2 == "Belianska Cave")
                    {
                        link_natuur2 = "https://www.slovakia.com/caves/belianska-cave/";
                    }
                    if (item.Natuurgebied2 == "Snezka")
                    {
                        link_natuur2 = "https://en.wikipedia.org/wiki/Sn%C4%9B%C5%BEka";
                    }
                    // Natuurgebied2 een link geven Zuid Europa
                    if (item.Natuurgebied2 == "Fuente Dé")
                    {
                        link_natuur2 = "https://www.debestemmingswijzer.nl/home/bestemmingen/galicie-asturie-cantabrie-tips/bezienswaardigheden-asturie-cantabrie/fuente-de/";
                    }
                    if (item.Natuurgebied2 == "Toscane")
                    {
                        link_natuur2 = "https://nl.wikipedia.org/wiki/Toscane";
                    }
                    if (item.Natuurgebied2 == "Serra da Estrela(bergen)")
                    {
                        link_natuur2 = "https://en.wikipedia.org/wiki/Serra_da_Estrela";
                    }
                    if (item.Natuurgebied2 == "Reptisland")
                    {
                        link_natuur2 = "https://www.inspirock.com/greece/melidoni/reptisland-a5445069301";
                    }
                    if (item.Natuurgebied2 == "Omis and Cetina River")
                    {
                        link_natuur2 = "http://www.omisinfo.com/omis/omis-nature/cetina-river.htm";
                    }
                    if (item.Natuurgebied2 == "Vrsic Pass - Julian Alps")
                    {
                        link_natuur2 = "https://forgetsomeday.com/vrsic-pass/";
                    }
                    // Natuurgebied2 een link geven West Europa
                    if (item.Natuurgebied2 == "Groene Hart")
                    {
                        link_natuur2 = "https://www.groenehart.nl/";
                    }
                    if (item.Natuurgebied2 == "Bretagne")
                    {
                        link_natuur2 = "http://www.bretagne.nl/";
                    }
                    if (item.Natuurgebied2 == "Hoge Venen")
                    {
                        link_natuur2 = "https://nl.wikipedia.org/wiki/Hoge_Venen";
                    }
                    if (item.Natuurgebied2 == "Nationaal park Hainich")
                    {
                        link_natuur2 = "https://nl.wikipedia.org/wiki/Nationaal_Park_Hainich";
                    }
                    if (item.Natuurgebied2 == "Bludenz")
                    {
                        link_natuur2 = "https://en.wikipedia.org/wiki/Bludenz";
                    }
                    if (item.Natuurgebied2 == "Meer van Luzern")
                    {
                        link_natuur2 = "https://www.myswitzerland.com/nl-nl/rond-het-meer-van-luzern.html";
                    }
                    TextView natuur2 = FindViewById<TextView>(Resource.Id.txtNatuurgebied2);
                    natuur2.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_natuur2 + "'>" + item.Natuurgebied2 + "</a> ");
                    natuur2.MovementMethod = LinkMovementMethod.Instance;

                    // Natuurgebied3 een link geven Noord Europa
                    if (item.Natuurgebied3 == "Wicklow Mountains")
                    {
                        link_natuur3 = "http://www.wicklowmountainsnationalpark.ie/";
                    }
                    if (item.Natuurgebied3 == "Repovesi National Park")
                    {
                        link_natuur3 = "http://www.nationalparks.fi/repovesinp";
                    }
                    if (item.Natuurgebied3 == "Kjerag")
                    {
                        link_natuur3 = "https://www.visitnorway.com/places-to-go/fjord-norway/the-stavanger-region/listings-stavanger/kjerag/185744/";
                    }
                    if (item.Natuurgebied3 == "Stockholm Canals")
                    {
                        link_natuur3 = "https://www.tripadvisor.nl/Attraction_Review-g189852-d2298827-Reviews-Stockholm_Canals-Stockholm.html";
                    }
                    if (item.Natuurgebied3 == "Park Waddenzee")
                    {
                        link_natuur3 = "https://www.visitdenmark.nl/nl/west-jutland/natuur/nationaal-park-waddenzee";
                    }
                    if (item.Natuurgebied3 == "Strokkur")
                    {
                        link_natuur3 = "https://guidetoiceland.is/travel-iceland/drive/strokkur";
                    }
                    // Natuurgebied3 een link geven Oost Europa
                    if (item.Natuurgebied3 == "Kurortny Park")
                    {
                        link_natuur3 = "https://www.tripadvisor.nl/Attraction_Review-g940874-d9737779-Reviews-Kurortny_Park-Truskavets_Lviv_Oblast.html";
                    }
                    if (item.Natuurgebied3 == "Botanical Gardens")
                    {
                        link_natuur3 = "https://en.wikipedia.org/wiki/Botanical_garden";
                    }
                    if (item.Natuurgebied3 == "Errant Rocks")
                    {
                        link_natuur3 = "https://blogs.transparent.com/polish/have-you-visited-errant-rocks-in-poland-also-known-as-bledne-skaly/";
                    }
                    if (item.Natuurgebied3 == "Boyana Waterfall")
                    {
                        link_natuur3 = "http://www.sofia-guide.com/attraction/boyana-waterfall/";
                    }
                    if (item.Natuurgebied3 == "Janosikove Diery")
                    {
                        link_natuur3 = "https://ecobnb.com/itinerary/janosikove-diery-one-of-the-most-beautiful-gorges-in-slovakia";
                    }
                    if (item.Natuurgebied3 == "Franciscan Garden")
                    {
                        link_natuur3 = "https://franciscangardens.com/";
                    }
                    // Natuurgebied3 een link geven Zuid Europa
                    if (item.Natuurgebied3 == "Las Médulas")
                    {
                        link_natuur3 = "https://en.wikipedia.org/wiki/Las_M%C3%A9dulas";
                    }
                    if (item.Natuurgebied3 == "Amalfikust")
                    {
                        link_natuur3 = "https://www.travelvalley.nl/ontspanning/de-amalfikust-een-droomweg-om-te-rijden-in-italie";
                    }
                    if (item.Natuurgebied3 == "Algarve")
                    {
                        link_natuur3 = "https://en.wikipedia.org/wiki/Algarve";
                    }
                    if (item.Natuurgebied3 == "Voidomatis River")
                    {
                        link_natuur3 = "http://www.visitgreece.gr/en/activities/water_sports/rafting/rafting_on_a_paradise_river";
                    }
                    if (item.Natuurgebied3 == "Marjan")
                    {
                        link_natuur3 = "https://en.wikipedia.org/wiki/Marjan,_Split";
                    }
                    if (item.Natuurgebied3 == "Soca Valley")
                    {
                        link_natuur3 = "https://www.off-the-path.com/en/soca-valley-slovenia/";
                    }
                    // Natuurgebied3 een link geven West Europa
                    if (item.Natuurgebied3 == "Veluwe")
                    {
                        link_natuur3 = "https://www.visitveluwe.nl/";
                    }
                    if (item.Natuurgebied3 == "Chauvet(grot)")
                    {
                        link_natuur3 = "https://en.wikipedia.org/wiki/Chauvet_Cave";
                    }
                    if (item.Natuurgebied3 == "Ardennen")
                    {
                        link_natuur3 = "https://nl.wikipedia.org/wiki/Ardennen";
                    }
                    if (item.Natuurgebied3 == "Zwarte Woud")
                    {
                        link_natuur3 = "https://nl.wikipedia.org/wiki/Zwarte_Woud";
                    }
                    if (item.Natuurgebied3 == "Lech Zürs am Arlberg")
                    {
                        link_natuur3 = "https://www.lechzuers.com/skiing";
                    }
                    if (item.Natuurgebied3 == "Giessbach Falls")
                    {
                        link_natuur3 = "https://www.giessbach.ch/en/giessbach-falls-nature-park.html";
                    }
                    TextView natuur3 = FindViewById<TextView>(Resource.Id.txtNatuurgebied3);
                    natuur3.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_natuur3 + "'>" + item.Natuurgebied3 + "</a> ");
                    natuur3.MovementMethod = LinkMovementMethod.Instance;

                    // Hotel1 een link geven Noord Europa
                    if (item.Hotel1 == "The Pier Hotel")
                    {
                        link_hotel1 = "https://www.milsomhotels.com/the-pier/?utm_source=GoogleMyBusiness&utm_medium=ThePier";
                    }
                    if (item.Hotel1 == "Klaus K Hotel")
                    {
                        link_hotel1 = "https://www.klauskhotel.com/en/";
                    }
                    if (item.Hotel1 == "Radisson Blu Hotel Alesund")
                    {
                        link_hotel1 = "https://www.radissonblu.com/en/hotel-alesund?facilitatorId=CSOSEO&csref=org_gmb_sk_en_sn_ho_AESZH";
                    }
                    if (item.Hotel1 == "Hjalmar")
                    {
                        link_hotel1 = "http://www.hotellhjalmar.se/boka.html";
                    }
                    if (item.Hotel1 == "Hotel Opus Horsens")
                    {
                        link_hotel1 = "https://hotelopushorsens.dk/en#anchor-hoteltilbud";
                    }
                    if (item.Hotel1 == "Hellissandur")
                    {
                        link_hotel1 = "https://www.tripadvisor.com/Hotel_Review-g1185010-d1189013-Reviews-Hotel_Hellissandur-Hellissandur_Snaefellsbaer_West_Region.html";
                    }
                    // Hotel1 een link geven Oost Europa
                    if (item.Hotel1 == "Standart")
                    {
                        link_hotel1 = "https://www.standarthotel.com/en/";
                    }
                    if (item.Hotel1 == "Classic")
                    {
                        link_hotel1 = "https://www.booking.com/hotel/ua/a-classic-a.en-gb.html?aid=356989;label=gog235jc-hotel-XX-ua-aNclassicNa-unspec-nl-com-L%3Aen-O%3AwindowsS10-B%3Achrome-N%3AXX-S%3Abo-U%3AXX-H%3As;sid=a786ac432d33c89b6bea703923f3e76f;dist=0&sb_price_type=total&type=total&";
                    }
                    if (item.Hotel1 == "Globus")
                    {
                        link_hotel1 = "http://globus.krakowhotels.net/en/";
                    }
                    if (item.Hotel1 == "Regnum Bansko Hotel & Spa")
                    {
                        link_hotel1 = "https://www.booking.com/hotel/bg/regnum-bansko-aparthotel-and-spa.en-gb.html?aid=356989;label=gog235jc-hotel-XX-bg-regnumNbanskoNaparthotelNandNspa-unspec-nl-com-L%3Aen-O%3AwindowsS10-B%3Achrome-N%3AXX-S%3Abo-U%3AXX-H%3As;sid=a786ac432d33c89b6bea703923f3e76f;dist=0&sb_price_type=total&type=total&";
                    }
                    if (item.Hotel1 == "NH Bratislava Gate One")
                    {
                        link_hotel1 = "https://www.nh-hotels.com/hotel/nh-bratislava-gate-one";
                    }
                    if (item.Hotel1 == "Mandarin Oriental Prague")
                    {
                        link_hotel1 = "https://www.mandarinoriental.com/prague/mala-strana/luxury-hotel?kw=mandarin-oriental-prague_e&htl=MOPRG&eng=Google_EN&src=PPC&gclid=EAIaIQobChMInN2Hm_LO2wIVmp3VCh2ScgDKEAAYASAAEgKp1vD_BwE";
                    }
                    // Hotel1 een link geven Zuid Europa
                    if (item.Hotel1 == "Hotel Arc La Rambla")
                    {
                        link_hotel1 = "https://www.booking.com/hotel/es/larc.en-gb.html?aid=311984;label=hotel-90161-es-kZxpGpWYBrE1PQaepgAnuQS258577781454%3Apl%3Ata%3Ap1%3Ap2%3Aac%3Aap1t1%3Aneg%3Afi%3Atikwd-6478266527%3Alp9064071%3Ali%3Adec%3Adm;sid=a786ac432d33c89b6bea703923f3e76f;dest_id=-372490;dest_type=city;dist=0;hapos=1;hpos=1;room1=A%2CA;sb_price_type=total;srepoch=1528832901;srfid=2d2c301c436d6647931d9328def1ee434709ec76X1;srpvid=542d8b426b5e0283;type=total;ucfs=1&#hotelTmpl";
                    }
                    if (item.Hotel1 == "ApartHotel Dei Mercanti")
                    {
                        link_hotel1 = "http://www.aparthoteldeimercanti.com/?lang=en";
                    }
                    if (item.Hotel1 == "Upon Lisbon Residences")
                    {
                        link_hotel1 = "https://uponlisbon.com/en/";
                    }
                    if (item.Hotel1 == "Proteas Blu Resort")
                    {
                        link_hotel1 = "http://www.proteasbluresort.gr/";
                    }
                    if (item.Hotel1 == "Sol Stella Apartments")
                    {
                        link_hotel1 = "https://www.melia.com/en/hotels/croacia/umag/sol-stella-apartments/index.html";
                    }
                    if (item.Hotel1 == "Piran")
                    {
                        link_hotel1 = "https://hotel-piran.si/en/";
                    }
                    // Hotel1 een link geven West Europa
                    if (item.Hotel1 == "Holiday Inn Express Rotterdam")
                    {
                        link_hotel1 = "https://www.ihg.com/holidayinnexpress/hotels/us/en/rotterdam/rtmcs/hoteldetail";
                    }
                    if (item.Hotel1 == "Hotel Odyssey by Elegancia")
                    {
                        link_hotel1 = "https://hotelodysseyparis.com/en/index.html?&trkid=V3ADW263561_19293443473_kwd-78130212384__244140422155_g_c__&atrkid=V3ADWC103E03A_19293443473_kwd-78130212384__244140422155_g_c___1t2&gclid=EAIaIQobChMIvJ2E_vLO2wIVSfhRCh0xnw-XEAAYAiAAEgKRjPD_BwE";
                    }
                    if (item.Hotel1 == "Van der Valk Hotel Antwerpen")
                    {
                        link_hotel1 = "https://www.vandervalkantwerpen.be/en";
                    }
                    if (item.Hotel1 == "NH Dortmund")
                    {
                        link_hotel1 = "https://www.nh-hotels.com/hotel/nh-dortmund";
                    }
                    if (item.Hotel1 == "NH Collection Wien Zentrum")
                    {
                        link_hotel1 = "https://www.nh-hotels.com/hotel/nh-collection-wien-zentrum?campid=8435708&ct=287135536&gclid=EAIaIQobChMI9aWRufPO2wIVQ4jVCh0V7QhXEAAYASAAEgL7n_D_BwE&dclid=CJax2rrzztsCFQiLdwodySwCgQ";
                    }
                    if (item.Hotel1 == "NH Geneva City")
                    {
                        link_hotel1 = "http://nh-rex.hotels-geneva.org/en/";
                    }
                    TextView Hotel1 = FindViewById<TextView>(Resource.Id.txtHotel1);
                    Hotel1.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_hotel1 + "'>" + item.Hotel1 + "</a> ");
                    Hotel1.MovementMethod = LinkMovementMethod.Instance;

                    // Hotel2 een link geven Noord Europa
                    if (item.Hotel2 == "Clayton Hotel Galway")
                    {
                        link_hotel2 = "https://www.claytonhotelgalway.ie/";
                    }
                    if (item.Hotel2 == "GLO Hotel Art")
                    {
                        link_hotel2 = "https://www.glohotels.fi/en/hotels/glo-art";
                    }
                    if (item.Hotel2 == "Scandic Lillehammer Hotel")
                    {
                        link_hotel2 = "https://www.scandichotels.se/hotell/norge/lillehammer/scandic-lillehammer-hotel";
                    }
                    if (item.Hotel2 == "Scandic Crown")
                    {
                        link_hotel2 = "https://www.scandichotels.com/crown";
                    }
                    if (item.Hotel2 == "Danmark")
                    {
                        link_hotel2 = "https://www.tripadvisor.com/Hotel_Review-s1-g189541-d206754-Reviews-Hotel_Danmark-Copenhagen_Zealand.html";
                    }
                    if (item.Hotel2 == "Icelandair Reykjavik Natura")
                    {
                        link_hotel2 = "https://www.icelandairhotels.com/en/hotels/natura";
                    }
                    // Hotel2 een link geven Oost Europa
                    if (item.Hotel2 == "Sovietsky")
                    {
                        link_hotel2 = "https://www.tripadvisor.nl/Hotel_Review-g298484-d299905-Reviews-Sovietsky_Historical_Hotel-Moscow_Central_Russia.html";
                    }
                    if (item.Hotel2 == "Radisson Blu Resort, Bukovel")
                    {
                        link_hotel2 = "https://www.radissonblu.com/en/resort-bukovel";
                    }
                    if (item.Hotel2 == "Cracowdays Apartments")
                    {
                        link_hotel2 = "https://cracowdays.com/";
                    }
                    if (item.Hotel2 == "Global Ville Apartcomplex")
                    {
                        link_hotel2 = "https://www.booking.com/hotel/bg/golden-view-apartcomplex.en-gb.html?aid=356989;label=gog235jc-hotel-XX-bg-goldenNviewNapartcomplex-unspec-nl-com-L%3Aen-O%3AwindowsS10-B%3Achrome-N%3AXX-S%3Abo-U%3AXX-H%3As;sid=a786ac432d33c89b6bea703923f3e76f;dist=0&sb_price_type=total&type=total&";
                    }
                    if (item.Hotel2 == "Hotel Devín")
                    {
                        link_hotel2 = "https://www.hoteldevin.sk/en/?r=4285012&gclid=EAIaIQobChMItZjK-PPO2wIV2PhRCh0qfwB5EAAYAiAAEgJwAPD_BwE";
                    }
                    if (item.Hotel2 == "Barceló Brno Palace")
                    {
                        link_hotel2 = "https://www.barcelo.com/en-gb/barcelo-hotels/hotels/czech-republic/brno/barcelo-brno-palace/";
                    }
                    // Hotel2 een link geven Zuid Europa
                    if (item.Hotel2 == "Capri by Fraser Barcelona")
                    {
                        link_hotel2 = "https://barcelona.capribyfraser.com/en";
                    }
                    if (item.Hotel2 == "NH Collection Giustiniano")
                    {
                        link_hotel2 = "https://www.nh-hotels.com/hotel/nh-collection-roma-giustiniano?campid=8435708&ct=287135536&gclid=EAIaIQobChMI6JvJnfTO2wIVgobVCh1Fww_cEAAYASAAEgLVnPD_BwE&dclid=CIPIwKL0ztsCFROKdwodj2UM2g";
                    }
                    if (item.Hotel2 == "Sao Rafael Atlântico")
                    {
                        link_hotel2 = "https://www.saorafaelatlantico.com/en/";
                    }
                    if (item.Hotel2 == "Myconian Kyma Design")
                    {
                        link_hotel2 = "https://www.myconiankyma.gr/?gclid=EAIaIQobChMIveKdsvTO2wIVhgrTCh1oqAqxEAAYASAAEgJq4PD_BwE";
                    }
                    if (item.Hotel2 == "Piazza Heritage Hotel")
                    {
                        link_hotel2 = "http://piazza-heritagehotel.com/";
                    }
                    if (item.Hotel2 == "Hotel Lev")
                    {
                        link_hotel2 = "https://www.union-hotels.eu/en/hotel-lev/";
                    }
                    // Hotel2 een link geven West Europa
                    if (item.Hotel2 == "Central Station")
                    {
                        link_hotel2 = "https://www.ihg.com/holidayinnexpress/hotels/gb/en/rotterdam/rtmcs/hoteldetail";
                    }
                    if (item.Hotel2 == "Hotel Le Nouveau Monde")
                    {
                        link_hotel2 = "https://www.hotel-le-nouveau-monde.fr/en/";
                    }
                    if (item.Hotel2 == "De Keyser Hotel")
                    {
                        link_hotel2 = "http://dekeyserhotel.be/";
                    }
                    if (item.Hotel2 == "NH Düsseldorf City")
                    {
                        link_hotel2 = "https://www.nh-hotels.com/hotel/nh-duesseldorf-city";
                    }
                    if (item.Hotel2 == "NH Danube City")
                    {
                        link_hotel2 = "https://www.nh-hotels.com/hotel/nh-danube-city";
                    }
                    if (item.Hotel2 == "Hotel Century")
                    {
                        link_hotel2 = "http://century.hotel-geneva-center.com/en/";
                    }
                    TextView Hotel2 = FindViewById<TextView>(Resource.Id.txtHotel2);
                    Hotel2.TextFormatted = Html.FromHtml(
                                        "<a href=\'" + link_hotel2 + "'>" + item.Hotel2 + "</a> ");
                    Hotel2.MovementMethod = LinkMovementMethod.Instance;

                }
            }
        }
    }

}
