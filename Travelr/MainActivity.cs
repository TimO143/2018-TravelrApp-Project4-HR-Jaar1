using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content;
using System;

namespace Travelr
{
    [Activity(Label = "Travelr", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class MainActivity : Activity
    {
        string btn_selected = "";
        public int mUserID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            TextView txtTitle = FindViewById<TextView>(Resource.Id.txtTitle);
            
            Typeface tf = Typeface.CreateFromAsset(Assets, "BOOKOS.TTF");
            txtTitle.SetTypeface(tf, TypefaceStyle.Bold);

            TextView txthelp = FindViewById<TextView>(Resource.Id.txtSendHelp2);
            Button btnNE = FindViewById<Button>(Resource.Id.btnNoordEuropa);
            Button btnOE = FindViewById<Button>(Resource.Id.btnOostEuropa);
            Button btnZE = FindViewById<Button>(Resource.Id.btnZuidEuropa);
            Button btnWE = FindViewById<Button>(Resource.Id.btnWestEuropa);
            ImageButton btnZoekLand = FindViewById<ImageButton>(Resource.Id.btnZoekLand);
            ImageButton btnBack = FindViewById<ImageButton>(Resource.Id.btnBack);
            ImageButton btnAccount = FindViewById<ImageButton>(Resource.Id.btnAccount);

            //logout & login tekst verandert als je bent ingelogd
            // voor favorieten kan je de bool LoginCheck gebruiken via regel 36 bool Logincheck = ...
            Button btnLoginLogout = FindViewById<Button>(Resource.Id.btnLoginLogout);
            bool LoginCheck = Intent.GetBooleanExtra("LoginCheck", false);
            int mUserID = Intent.GetIntExtra("mUserID", 0);
            string Username = Intent.GetStringExtra("Username");


            if (LoginCheck == false)
            {
                btnLoginLogout.Text = "Log in";
            }
            else
            {
                btnLoginLogout.Text = "log uit";
            }
            btnLoginLogout.Click += (object sender, EventArgs args) =>
            {
                Intent nextActivity = new Intent(this, typeof(LoginActivity));
                StartActivity(nextActivity);
            };

            txthelp.Click += (sender, e) => txthelp_Click(sender, e, Username, LoginCheck, mUserID);
            btnNE.Click += (sender, e) => BtnNE_Click(sender, e, btnNE, Username, LoginCheck, mUserID);
            btnOE.Click += (sender, e) => BtnOE_Click(sender, e, btnOE, Username, LoginCheck, mUserID);
            btnZE.Click += (sender, e) => BtnZE_Click(sender, e, btnZE, Username, LoginCheck, mUserID);
            btnWE.Click += (sender, e) => BtnWE_Click(sender, e, btnWE, Username, LoginCheck, mUserID);
            btnZoekLand.Click += (sender, e) => BtnZoekLand_Click(sender, e, LoginCheck, mUserID, Username);
            btnAccount.Click += (sender, e) => BtnAccount_Click(sender, e, mUserID, Username, LoginCheck);

        }

        private void txthelp_Click(object sender, System.EventArgs e, string Username, bool LoginCheck, int mUserID)
        {
            Intent nextActivity = new Intent(this, typeof(HelpActivity));
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
        private void BtnAccount_Click(object sender, EventArgs e, int mUserId, string Username, bool loginCheck)
        {
            if (loginCheck)
            {
                Intent nextActivity = new Intent(this, typeof(AccountActivity));
                nextActivity.PutExtra("UserID", mUserId);
                nextActivity.PutExtra("Username", Username);
                if (loginCheck == true)
                {
                    nextActivity.PutExtra("LoginCheck", true);
                }
                else
                {
                    nextActivity.PutExtra("LoginCheck", false);
                }
                StartActivity(nextActivity);
            }
            else
            {
                Toast.MakeText(this, "U moet eerst inloggen.", ToastLength.Short).Show();
            }

        }


        private void BtnZoekLand_Click(object sender, System.EventArgs e, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(ZoekLandActivity));
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

        public override void OnBackPressed()
        {
            //DO NOTHING
        }

        private void BtnWE_Click(object sender, System.EventArgs e, Button btnWE, string Username, bool LoginCheck, int mUserID)
        {
            btnWE.SetBackgroundColor(Color.Argb(255, 175, 83, 42));
            btn_selected = "West Europa";

            if (btn_selected != "")
            {
                Intent nextActivity = new Intent(this, typeof(LandenActivity));
                nextActivity.PutExtra("DeelEuropa", btn_selected);
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
                btn_reset();
            }

        }

        private void BtnZE_Click(object sender, System.EventArgs e, Button btnZE, string Username, bool LoginCheck, int mUserID)
        {
            btnZE.SetBackgroundColor(Color.Argb(255, 175, 83, 42));
            btn_selected = "Zuid Europa";
            if (btn_selected != "")
            {
                Intent nextActivity = new Intent(this, typeof(LandenActivity));
                nextActivity.PutExtra("DeelEuropa", btn_selected);
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
                btn_reset();
            }
        }

        private void BtnOE_Click(object sender, System.EventArgs e, Button btnOE, string Username, bool LoginCheck, int mUserID)
        {
            btnOE.SetBackgroundColor(Color.Argb(255, 175, 83, 42));
            btn_selected = "Oost Europa";
            if (btn_selected != "")
            {
                Intent nextActivity = new Intent(this, typeof(LandenActivity));
                nextActivity.PutExtra("DeelEuropa", btn_selected);
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
                btn_reset();
            }
        }

        private void BtnNE_Click(object sender, System.EventArgs e, Button btnNE, string Username, bool LoginCheck, int mUserID)
        {
            btnNE.SetBackgroundColor(Color.Argb(255, 175, 83, 42));
            btn_selected = "Noord Europa";
            if (btn_selected != "")
            {
                Intent nextActivity = new Intent(this, typeof(LandenActivity));
                nextActivity.PutExtra("DeelEuropa", btn_selected);
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
                btn_reset();
            }
        }

        public void btn_reset()
        {
            Button btnNE = FindViewById<Button>(Resource.Id.btnNoordEuropa);
            Button btnOE = FindViewById<Button>(Resource.Id.btnOostEuropa);
            Button btnZE = FindViewById<Button>(Resource.Id.btnZuidEuropa);
            Button btnWE = FindViewById<Button>(Resource.Id.btnWestEuropa);

            if (btn_selected == "Noord Europa")
            {
                btnOE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnZE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnWE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
            }
            if (btn_selected == "Oost Europa")
            {
                btnNE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnZE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnWE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
            }
            if (btn_selected == "Zuid Europa")
            {
                btnNE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnOE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnWE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
            }
            if (btn_selected == "West Europa")
            {
                btnNE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnOE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
                btnZE.SetBackgroundColor(Color.Argb(255, 194, 98, 55));
            }


        }
    }

}