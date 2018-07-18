using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Travelr
{
    [Activity(Label = "AccountActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class AccountActivity : Activity
    {
        private Button btnVeranderwachtwoord;
        public EditText txtgetoldpass;
        public EditText txtgetnewpass;
        public string oudWachtwoord;
        public string mUsername;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account);

            int mUserID = Intent.GetIntExtra("UserID", 0);
            string mUsername = Intent.GetStringExtra("Username");
            bool LoginCheck = Intent.GetBooleanExtra("LoginCheck", false);


            Thread p = new Thread(() => setTypeFace(mUsername, mUserID));
            p.Start();



            ImageButton btnBackLI = FindViewById<ImageButton>(Resource.Id.btnBackLI);
            ImageButton btnFavo1 = FindViewById<ImageButton>(Resource.Id.btnFavo1);
            ImageButton btnFavo2 = FindViewById<ImageButton>(Resource.Id.btnFavo2);
            ImageButton btnFavo3 = FindViewById<ImageButton>(Resource.Id.btnFavo3);
            ImageButton btnFavo4 = FindViewById<ImageButton>(Resource.Id.btnFavo4);
            ImageButton btnFavo5 = FindViewById<ImageButton>(Resource.Id.btnFavo5);
            ImageButton btnFavo6 = FindViewById<ImageButton>(Resource.Id.btnFavo6);

            

            btnVeranderwachtwoord = FindViewById<Button>(Resource.Id.btnVeranderwachtwoord);

            //BUTTON CLICKS
            btnVeranderwachtwoord.Click += btnVeranderwachtwoord_Click;
            btnBackLI.Click += BtnAccountBack_Click;
            string[] Favorieten = getFavorieten(mUsername);
            drawFavorieten(mUsername, Favorieten);


            btnFavo1.Click += (sender,e) => BtnFavo1_Click(sender, e, Favorieten, mUserID, mUsername, LoginCheck);
            btnFavo2.Click += (sender, e) => BtnFavo2_Click(sender, e, Favorieten, mUserID, mUsername, LoginCheck);
            btnFavo3.Click += (sender, e) => BtnFavo3_Click(sender, e, Favorieten, mUserID, mUsername, LoginCheck);
            btnFavo4.Click += (sender, e) => BtnFavo4_Click(sender, e, Favorieten, mUserID, mUsername, LoginCheck);
            btnFavo5.Click += (sender, e) => BtnFavo5_Click(sender, e, Favorieten, mUserID, mUsername, LoginCheck);
            btnFavo6.Click += (sender, e) => BtnFavo6_Click(sender, e, Favorieten, mUserID, mUsername, LoginCheck);

        }

        private void setTypeFace(string mUsername, int mUserID)
        {
            Typeface opensansbold = Typeface.CreateFromAsset(Assets, "OpenSans-Bold.ttf");
            Typeface opensanslight = Typeface.CreateFromAsset(Assets, "OpenSans-Light.ttf");
            TextView txttitle = FindViewById<TextView>(Resource.Id.txtTitle);
            TextView txtUsername = FindViewById<TextView>(Resource.Id.txtUsername);
            txtUsername.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtgetUsername = FindViewById<TextView>(Resource.Id.txtgetUsername);
            txtgetUsername.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtUserID = FindViewById<TextView>(Resource.Id.txtUserID);
            txtUserID.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtgetUserID = FindViewById<TextView>(Resource.Id.txtgetUserID);
            txtgetUserID.SetTypeface(opensanslight, TypefaceStyle.Normal);

            // Setting strings
            txtgetUsername.Text = mUsername;
            txtgetUserID.Text = mUserID.ToString();
        }

        private void BtnFavo6_Click(object sender, EventArgs e, string[] Favorieten, int mUserId, string Username, bool LoginCheck)
        {
            string landNaam = Favorieten[5];
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("mUserID", mUserId);
            nextActivity.PutExtra("Username", Username);
            nextActivity.PutExtra("landKeuze", landNaam);
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

        private void BtnFavo5_Click(object sender, EventArgs e, string[] Favorieten, int mUserId, string Username, bool LoginCheck)
        {
            string landNaam = Favorieten[4];
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("mUserID", mUserId);
            nextActivity.PutExtra("Username", Username);
            nextActivity.PutExtra("landKeuze", landNaam);
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

        private void BtnFavo4_Click(object sender, EventArgs e, string[] Favorieten, int mUserId, string Username, bool LoginCheck)
        {
            string landNaam = Favorieten[3];
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("mUserID", mUserId);
            nextActivity.PutExtra("Username", Username);
            nextActivity.PutExtra("landKeuze", landNaam);
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

        private void BtnFavo3_Click(object sender, EventArgs e, string[] Favorieten, int mUserId, string Username, bool LoginCheck)
        {
            string landNaam = Favorieten[2];
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("mUserID", mUserId);
            nextActivity.PutExtra("Username", Username);
            nextActivity.PutExtra("landKeuze", landNaam);
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

        private void BtnFavo2_Click(object sender, EventArgs e, string[] Favorieten, int mUserId, string Username, bool LoginCheck)
        {
            string landNaam = Favorieten[1];
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("mUserID", mUserId);
            nextActivity.PutExtra("Username", Username);
            nextActivity.PutExtra("landKeuze", landNaam);
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

        private void BtnFavo1_Click(object sender, EventArgs e, string[] Favorieten, int mUserId, string Username, bool LoginCheck)
        {
            string landNaam = Favorieten[0];
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("mUserID", mUserId);
            nextActivity.PutExtra("Username", Username);
            nextActivity.PutExtra("landKeuze", landNaam);
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

        private void btnVeranderwachtwoord_Click(object sender, EventArgs e)
        {
            string gebruikersnaam = Intent.GetStringExtra("Username");
            EditText txtgetnewpass = FindViewById<EditText>(Resource.Id.txtgetnewpass);
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
            // GET USERID
            MySqlCommand cmdGet_Pass = new MySqlCommand("select userPassword, userName from tbl_userdata where userName=@username", con);
            cmdGet_Pass.Parameters.AddWithValue("@username", gebruikersnaam);
            MySqlDataAdapter daGetUser = new MySqlDataAdapter(cmdGet_Pass);
            DataSet dsGetPass = new DataSet();
            daGetUser.Fill(dsGetPass);
            DataRow dr = dsGetPass.Tables[0].Rows[0];

            oudWachtwoord = (dr[0].ToString());
            con.Close();
            EditText txtgetoldpass = FindViewById<EditText>(Resource.Id.txtgetoldpass);
            TextView txtgetUsername = FindViewById<TextView>(Resource.Id.txtgetUsername);
            if ((txtgetoldpass.Text) == (oudWachtwoord.ToString()))
            {
                // Nieuw wachtwoord
                MySqlConnection con2 = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
                try
                {
                    if (con2.State == ConnectionState.Closed)
                    {
                        con2.Open();

                        using (var conn = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8"))
                        {
                            conn.Open();

                            string sql = "UPDATE tbl_userdata SET userPassword = '" + (txtgetnewpass.Text.ToString()) + "' WHERE userName = '" + (txtgetUsername.Text.ToString()) + "';";
                            MySqlCommand command = new MySqlCommand(sql, con2);
                            MySqlDataReader rdr = command.ExecuteReader();
                            conn.Close();
                        }
                    }
                }
                finally
                {
                    con2.Close();
                    Toast.MakeText(this, "Wachtwoord gewijzigd!", ToastLength.Long).Show();
                }



            }
        }
        private void drawFavorieten(string username, string[] Favorieten)
        {
            ImageButton btnFavo1 = FindViewById<ImageButton>(Resource.Id.btnFavo1);
            ImageButton btnFavo2 = FindViewById<ImageButton>(Resource.Id.btnFavo2);
            ImageButton btnFavo3 = FindViewById<ImageButton>(Resource.Id.btnFavo3);
            ImageButton btnFavo4 = FindViewById<ImageButton>(Resource.Id.btnFavo4);
            ImageButton btnFavo5 = FindViewById<ImageButton>(Resource.Id.btnFavo5);
            ImageButton btnFavo6 = FindViewById<ImageButton>(Resource.Id.btnFavo6);

            TextView txtFavo1 = FindViewById<TextView>(Resource.Id.txtFavo1);
            TextView txtFavo2 = FindViewById<TextView>(Resource.Id.txtFavo2);
            TextView txtFavo3 = FindViewById<TextView>(Resource.Id.txtFavo3);
            TextView txtFavo4 = FindViewById<TextView>(Resource.Id.txtFavo4);
            TextView txtFavo5 = FindViewById<TextView>(Resource.Id.txtFavo5);
            TextView txtFavo6 = FindViewById<TextView>(Resource.Id.txtFavo6);

            ImageButton[] buttons = { btnFavo1, btnFavo2, btnFavo3, btnFavo4, btnFavo5, btnFavo6 };
            TextView[] txtFavos = { txtFavo1, txtFavo2, txtFavo3, txtFavo4, txtFavo5, txtFavo6 };


            int counter = 0;
            foreach(string favoriet in Favorieten)
            {
                
                if (favoriet != "geenFavoriet")
                {
                    string mDrawableName = favoriet + "Info";
                    int id = (int)typeof(Resource.Drawable).GetField(mDrawableName).GetValue(null);
                    buttons[counter].SetImageResource(id);
                    txtFavos[counter].Text = Favorieten[counter];
                    
                }
                else
                {
                    buttons[counter].Visibility = ViewStates.Gone;
                    txtFavos[counter].Visibility = ViewStates.Gone;
                }
                counter += 1;
            }
            
            

            
        }

        private void BtnAccountBack_Click(object sender, EventArgs e)
        {
            this.Finish();
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
                        foreach (string column in columns)
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
    }

}