using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

namespace Travelr
{
    [Activity(Label = "Travelr-sign", Theme = "@android:style/Theme.Black.NoTitleBar")]
    class RegistreerActivity : Activity
    {
        // globals aangegeven met een m ervoor
        private Button mBtnRegistreerd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Registreer);
            CustomDialog customDialog = new CustomDialog(this);
            ImageButton btnBack = FindViewById<ImageButton>(Resource.Id.btnBack);
            TextView txtSignUpTitle = FindViewById<TextView>(Resource.Id.txtSignUpTitle);
            Typeface tf = Typeface.CreateFromAsset(Assets, "BOOKOS.TTF");
            EditText input_firstname = FindViewById<EditText>(Resource.Id.txtFirstName);
            EditText input_email = FindViewById<EditText>(Resource.Id.txtEmail);
            EditText input_password = FindViewById<EditText>(Resource.Id.txtPassword);
            txtSignUpTitle.SetTypeface(tf, TypefaceStyle.Bold);
            btnBack.Click += BtnBack_Click;
            mBtnRegistreerd = FindViewById<Button>(Resource.Id.btnRegistreerd);
            mBtnRegistreerd.Click += (object sender, EventArgs args) =>
            {
                //Intent nextActivity = new Intent(this, typeof(LoginActivity));
                //StartActivity(nextActivity);
                MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                        using (var conn = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8"))
                        {
                            conn.Open();
                            using (var cmd = new MySqlCommand("SELECT COUNT(userName) FROM tbl_userdata GROUP BY userName having userName = '" + input_firstname.Text + "' UNION ALL SELECT COUNT(userEmail) FROM tbl_userdata GROUP BY userEmail having userEmail = '" + input_email.Text + "'; ", conn))
                            {
                                int existingUserAmount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (existingUserAmount == 0)
                                {
                                    if (input_firstname.Text.Length > 0 && input_password.Text.Length > 0 && input_email.Text.Length > 0)
                                    {
                                        string sql = "INSERT INTO `tbl_userdata` (userName, userPassword, userEmail) SELECT '" + input_firstname.Text + "', '" + input_password.Text + "', '" + input_email.Text + "' FROM DUAL WHERE NOT EXISTS(SELECT * FROM `tbl_userdata` WHERE userName = '" + input_firstname.Text + "') LIMIT 1;";
                                        MySqlCommand command = new MySqlCommand(sql, con);
                                        MySqlDataReader rdr = command.ExecuteReader();

                                        Toast.MakeText(this, "U bent Geregistreerd", ToastLength.Long).Show();
                                        Intent nextActivity = new Intent(this, typeof(LoginActivity));

                                        StartActivity(nextActivity);

                                    }

                                } // deze weg
                                else
                                {
                                    customDialog.Show();
                                }

                            } // deze weg
                        }
                    }
                } // hier eentje bij
                catch (MySqlException ex)
                {
                    input_firstname.Text = ex.ToString();
                }
                finally
                {
                    con.Close();
                }
            };

        }
        private void BtnBack_Click(object sender, EventArgs e)
        {

            Intent nextActivity = new Intent(this, typeof(LoginActivity));
            StartActivity(nextActivity);
        }

        public override void OnBackPressed()
        {
        }
    }
}