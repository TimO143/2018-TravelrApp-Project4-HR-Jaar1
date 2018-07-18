using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Graphics;
using Android.Content;
using MySql.Data.MySqlClient;
using System.Data;

namespace Travelr
{
    [Activity(Label = "Travelr", MainLauncher = true, Theme = "@android:style/Theme.Black.NoTitleBar", Icon = "@drawable/TravelrIcon")]
    public class LoginActivity : Activity
    {
        // globals aangegeven met een m ervoor
        private Button mBtnRegistreer;
        private Button mBtnSkipLogin;
        private Button mBtnLogin;
        public bool LoginCheck;
        public int mUserID;

        public override void OnBackPressed()
        {

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);
            TextView txthelp = FindViewById<TextView>(Resource.Id.txtSendHelp);
            mBtnRegistreer = FindViewById<Button>(Resource.Id.btnregistreer);
            mBtnSkipLogin = FindViewById<Button>(Resource.Id.btnSkipLogin);
            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            TextView txtTitle = FindViewById<TextView>(Resource.Id.txtTitle);
            Typeface tf = Typeface.CreateFromAsset(Assets, "BOOKOS.TTF");
            txtTitle.SetTypeface(tf, TypefaceStyle.Bold);

            // gebruik invul velden als input bij inloggen
            EditText username = FindViewById<EditText>(Resource.Id.txtFirstName_login);
            EditText password = FindViewById<EditText>(Resource.Id.txtPassword_login);

            txthelp.Click += (sender, e) => Txthelp_Click(sender, e);

            mBtnRegistreer.Click += (object sender, EventArgs args) =>
            {

                Intent nextActivity = new Intent(this, typeof(RegistreerActivity));
                StartActivity(nextActivity);

            };


            mBtnSkipLogin.Click += (object sender, EventArgs args) =>
            {
                Intent nextActivity = new Intent(this, typeof(MainActivity));
                StartActivity(nextActivity);
            };

            mBtnLogin.Click += (object sender, EventArgs args) =>
            {
                MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
                if (username.Text.Length == 0 || password.Text.Length == 0)
                {
                    Toast.MakeText(this, "Vul gebruikersnaam en wachtwoord in", ToastLength.Short).Show();
                };
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    if (username.Text.Length > 0 && password.Text.Length > 0)
                    {
                        MySqlCommand cmd = new MySqlCommand("select * from tbl_userdata where userName=@username and userPassword=@password", con);
                        cmd.Parameters.AddWithValue("@username", username.Text);
                        cmd.Parameters.AddWithValue("@password", password.Text);
                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(ds);
                        con.Close();

                        bool loginSuc6 = (ds.Tables[0].Rows.Count > 0);
                        if (loginSuc6)
                        {
                            // GET USERID
                            MySqlCommand cmdGet_ID = new MySqlCommand("select userID from tbl_userdata where userName=@username", con);
                            cmdGet_ID.Parameters.AddWithValue("@username", username.Text);
                            MySqlDataAdapter daGetUser = new MySqlDataAdapter(cmdGet_ID);
                            DataSet dsGetUser = new DataSet();
                            daGetUser.Fill(dsGetUser);
                            DataRow dr = ds.Tables[0].Rows[0];
                            mUserID = Int32.Parse(dr["userID"].ToString());

                            con.Close();

                            // Geef LoginCheck, mUserId en Username door aan de andere activitieten.
                            Intent nextActivity = new Intent(this, typeof(MainActivity));
                            nextActivity.PutExtra("LoginCheck", true);
                            nextActivity.PutExtra("mUserID", mUserID);
                            nextActivity.PutExtra("Username", username.Text);
                            StartActivity(nextActivity);
                        }
                        else
                        {
                            Toast.MakeText(this, "Gebruikersnaam en/of wachtwoord is onjuist", ToastLength.Short).Show();
                        }


                    }


                };


            };


        }

        private void Txthelp_Click(object sender, System.EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(HelpActivity));
            StartActivity(nextActivity);
        }
    }
}

