using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Travelr

{
    [Activity(Label = "LandenActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class LandenActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Landen);
            //GET LOGIN STATUS
            bool LoginCheck = Intent.GetBooleanExtra("LoginCheck", false);
            int mUserID = Intent.GetIntExtra("mUserID", 0);
            string Username = Intent.GetStringExtra("Username");



            string deelEuropa = Intent.GetStringExtra("DeelEuropa");
            List<string> landen;
            string[] landKeuzes;

            // TEXT
            TextView txtEuropadeel = FindViewById<TextView>(Resource.Id.txtEuropaDeel);
            TextView txtAantalLanden = FindViewById<TextView>(Resource.Id.txtAantalLanden);

            // BUTTONS
            ImageButton btnBack = FindViewById<ImageButton>(Resource.Id.btnBack);
            ImageButton btnLandKeuze1 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze1);
            ImageButton btnLandKeuze2 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze2);
            ImageButton btnLandKeuze3 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze3);
            ImageButton btnLandKeuze4 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze4);
            ImageButton btnLandKeuze5 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze5);
            ImageButton btnLandKeuze6 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze6);


            btnBack.Click += BtnBack_Click;


            txtEuropadeel.Text = deelEuropa;
            landen = SetLanden(deelEuropa);

            string landKeuze1 = landen[0];
            string landKeuze2 = landen[1];
            string landKeuze3 = landen[2];
            string landKeuze4 = landen[3];
            string landKeuze5 = landen[4];
            string landKeuze6 = landen[5];

            landKeuzes = new string[] { landKeuze1, landKeuze2, landKeuze3, landKeuze4, landKeuze5, landKeuze6 };
            setNamen(landKeuzes);

            btnLandKeuze1.Click += (sender, e) => BtnLandKeuze1_Click(sender, e, landKeuze1, LoginCheck, mUserID, Username);
            btnLandKeuze2.Click += (sender, e) => BtnLandKeuze2_Click(sender, e, landKeuze2, LoginCheck, mUserID, Username);
            btnLandKeuze3.Click += (sender, e) => BtnLandKeuze3_Click(sender, e, landKeuze3, LoginCheck, mUserID, Username);
            btnLandKeuze4.Click += (sender, e) => BtnLandKeuze4_Click(sender, e, landKeuze4, LoginCheck, mUserID, Username);
            btnLandKeuze5.Click += (sender, e) => BtnLandKeuze5_Click(sender, e, landKeuze5, LoginCheck, mUserID, Username);
            btnLandKeuze6.Click += (sender, e) => BtnLandKeuze6_Click(sender, e, landKeuze6, LoginCheck, mUserID, Username);

            txtAantalLanden.Text = landen.Count.ToString() + " resultaten";
        }

        private void BtnLandKeuze6_Click(object sender, EventArgs e, string landKeuze, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", landKeuze);
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

        private void BtnLandKeuze5_Click(object sender, EventArgs e, string landKeuze, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", landKeuze);
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

        private void BtnLandKeuze4_Click(object sender, EventArgs e, string landKeuze, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", landKeuze);
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

        private void BtnLandKeuze3_Click(object sender, EventArgs e, string landKeuze, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", landKeuze);
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

        private void BtnLandKeuze2_Click(object sender, EventArgs e, string landKeuze, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", landKeuze);
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

        private void BtnLandKeuze1_Click(object sender, EventArgs e, string landKeuze, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", landKeuze);
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

        private List<string> SetLanden(string deelEuropa)
        {
            List<string> landen = new List<string>();
            if (deelEuropa == "Noord Europa")
            {
                landen.Add("Finland");
                landen.Add("Noorwegen");
                landen.Add("Zweden");
                landen.Add("Denemarken");
                landen.Add("Ierland");
                landen.Add("Ijsland");
            }
            if (deelEuropa == "Oost Europa")
            {
                landen.Add("Rusland");
                landen.Add("Oekraine");
                landen.Add("Polen");
                landen.Add("Bulgarije");
                landen.Add("Slowakije");
                landen.Add("Tsjechie");

            }
            if (deelEuropa == "Zuid Europa")
            {
                landen.Add("Spanje");
                landen.Add("Italie");
                landen.Add("Portugal");
                landen.Add("Griekenland");
                landen.Add("Kroatie");
                landen.Add("Slovenie");
            }
            if (deelEuropa == "West Europa")
            {
                //Query voeg landen toe
                landen.Add("Nederland");
                landen.Add("Frankrijk");
                landen.Add("Belgie");
                landen.Add("Duitsland");
                landen.Add("Oostenrijk");
                landen.Add("Zwitserland");

            }

            DrawLanden(landen);
            return landen;
        }

        private void DrawLanden(List<string> landen)
        {
            ImageButton btnLandKeuze1 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze1);
            ImageButton btnLandKeuze2 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze2);
            ImageButton btnLandKeuze3 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze3);
            ImageButton btnLandKeuze4 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze4);
            ImageButton btnLandKeuze5 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze5);
            ImageButton btnLandKeuze6 = FindViewById<ImageButton>(Resource.Id.btnLandKeuze6);

            ImageButton[] btnLanden = { btnLandKeuze1, btnLandKeuze2, btnLandKeuze3, btnLandKeuze4, btnLandKeuze5, btnLandKeuze6 };

            int counter = 0;
            foreach (string land in landen)
            {
                string mDrawableName = land;
                int id = (int)typeof(Resource.Drawable).GetField(mDrawableName).GetValue(null);
                btnLanden[counter].SetImageResource(id);
                counter += 1;
            }
        }

        public void setNamen(string[] landKeuzes)
        {
            TextView txtLandKeuze1 = FindViewById<TextView>(Resource.Id.txtLandKeuze1);
            TextView txtLandKeuze2 = FindViewById<TextView>(Resource.Id.txtLandKeuze2);
            TextView txtLandKeuze3 = FindViewById<TextView>(Resource.Id.txtLandKeuze3);
            TextView txtLandKeuze4 = FindViewById<TextView>(Resource.Id.txtLandKeuze4);
            TextView txtLandKeuze5 = FindViewById<TextView>(Resource.Id.txtLandKeuze5);
            TextView txtLandKeuze6 = FindViewById<TextView>(Resource.Id.txtLandKeuze6);

            // Trema's goed zetten
            int counter = 0;
            foreach(string land in landKeuzes)
            {
                if(land == "Oekraine")
                {
                    landKeuzes[counter] = "Oekraïne";
                }
                if(land == "Tsjechie")
                {
                    landKeuzes[counter] = "Tsjechië";
                }
                if (land == "Belgie")
                {
                    landKeuzes[counter] = "België";
                }
                if (land == "Italie")
                {
                    landKeuzes[counter] = "Italië";
                }
                if (land == "Kroatie")
                {
                    landKeuzes[counter] = "Kroatië";
                }
                if (land == "Slovenie")
                {
                    landKeuzes[counter] = "Slovenië";
                }
                counter += 1;
            }

            txtLandKeuze1.Text = landKeuzes[0];
            txtLandKeuze2.Text = landKeuzes[1];
            txtLandKeuze3.Text = landKeuzes[2];
            txtLandKeuze4.Text = landKeuzes[3];
            txtLandKeuze5.Text = landKeuzes[4];
            txtLandKeuze6.Text = landKeuzes[5];
        }

        public override void OnBackPressed()
        {

        }
    }
}