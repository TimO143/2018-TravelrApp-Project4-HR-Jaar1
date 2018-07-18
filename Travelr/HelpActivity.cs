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

namespace Travelr
{
    [Activity(Label = "HelpActivity" ,Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class HelpActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Help);

            ImageButton btnBack = FindViewById<ImageButton>(Resource.Id.btnBackLI);

            btnBack.Click += BtnBack_Click;

            Typeface opensansbold = Typeface.CreateFromAsset(Assets, "OpenSans-Bold.ttf");
            Typeface opensanslight = Typeface.CreateFromAsset(Assets, "OpenSans-Light.ttf");

            TextView txtHulpContact = FindViewById<TextView>(Resource.Id.txtHulpContact);
            txtHulpContact.SetTypeface(opensansbold, TypefaceStyle.Bold);
            TextView txtHulppass = FindViewById<TextView>(Resource.Id.txtHulppass);
            txtHulppass.SetTypeface(opensansbold, TypefaceStyle.Bold);
            TextView txtHulpHome = FindViewById<TextView>(Resource.Id.txtHulpHome);
            txtHulpHome.SetTypeface(opensansbold, TypefaceStyle.Bold);
            TextView txtHulpTerug = FindViewById<TextView>(Resource.Id.txtHulpTerug);
            txtHulpTerug.SetTypeface(opensansbold, TypefaceStyle.Bold);
            TextView txtHulpRecensie = FindViewById<TextView>(Resource.Id.txtHulpRecensie);
            txtHulpRecensie.SetTypeface(opensansbold, TypefaceStyle.Bold);
            TextView txtHulpZoeken = FindViewById<TextView>(Resource.Id.txtHulpZoeken);
            txtHulpZoeken.SetTypeface(opensansbold, TypefaceStyle.Bold);
            TextView txthulpinformatie = FindViewById<TextView>(Resource.Id.txthulpinformatie);
            txthulpinformatie.SetTypeface(opensansbold, TypefaceStyle.Bold);



            TextView txtHulpContactInfo = FindViewById<TextView>(Resource.Id.txtHulpContactInfo);
            txtHulpContactInfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtHulppassinfo = FindViewById<TextView>(Resource.Id.txtHulppassinfo);
            txtHulppassinfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtHulpHomeInfo = FindViewById<TextView>(Resource.Id.txtHulpHomeInfo);
            txtHulpHomeInfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtHulpTerugInfo = FindViewById<TextView>(Resource.Id.txtHulpTerugInfo);
            txtHulpTerugInfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtHulpRecensieInfo = FindViewById<TextView>(Resource.Id.txtHulpRecensieInfo);
            txtHulpRecensieInfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txtHulpZoekenInfo = FindViewById<TextView>(Resource.Id.txtHulpZoekenInfo);
            txtHulpZoekenInfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
            TextView txthulplandinfo = FindViewById<TextView>(Resource.Id.txthulplandinfo);
            txthulplandinfo.SetTypeface(opensanslight, TypefaceStyle.Normal);
        }

       

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        public override void OnBackPressed()
        {
            //DO NOTHING
        }
    }

}