using System;
using System.Collections;
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
    [Activity(Label = "ZoekLandActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class ZoekLandActivity : Activity
    {
        private SearchView _sv;
        private ListView _lv;
        private ArrayList _al;
        private ArrayAdapter _adapter;
        private Button btnZoekBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ZoekLand);
            //GET LOGIN STATUS 
            bool LoginCheck = Intent.GetBooleanExtra("LoginCheck", false);
            int mUserID = Intent.GetIntExtra("mUserID", 0);
            string Username = Intent.GetStringExtra("Username");


            _lv = FindViewById<ListView>(Resource.Id.lv);
            _sv = FindViewById<SearchView>(Resource.Id.sv);

            addData();

            _adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, _al);

            _lv.Adapter = _adapter;

            _sv.QueryTextChange += _sv_QueryTextChange;
            _lv.ItemClick += (sender, e) => _lv_ItemClick(sender, e, LoginCheck, mUserID, Username);

            btnZoekBack = FindViewById<Button>(Resource.Id.btnZoekBack);
            btnZoekBack.Click += BtnZoekBack_Click;


            // SET TEXT COLOR & HINT TEXT COLOR

            SearchView sv = FindViewById<SearchView>(Resource.Id.sv);
            int id = sv.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            TextView tv = (TextView)sv.FindViewById(id);
            tv.SetTextColor(Android.Graphics.Color.ParseColor("#e77d4d"));  
            sv.SetQueryHint("Typ hier om een land te zoeken.");

            sv.OnActionViewExpanded();

        }

        private void BtnZoekBack_Click(object sender, EventArgs e)
        {
            this.Finish();
            
        }

        private void _lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(LandInfoActivity));
            nextActivity.PutExtra("landKeuze", _lv.GetItemAtPosition(e.Position).ToString());
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

        private void _sv_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            //FILTER OR SEARCH
            _adapter.Filter.InvokeFilter(e.NewText);
        }

        private void addData()
        {
            _al = new ArrayList();
            //QUERY GET ALLE LANDEN IN DE DATABASE
            _al.Add("Finland");
            _al.Add("Noorwegen");
            _al.Add("Zweden");
            _al.Add("Denemarken");
            _al.Add("Ierland");
            _al.Add("Ijsland");
            _al.Add("Rusland");
            _al.Add("Oekraine");
            _al.Add("Polen");
            _al.Add("Bulgarije");
            _al.Add("Slowakije");
            _al.Add("Tsjechie");
            _al.Add("Spanje");
            _al.Add("Italie");
            _al.Add("Portugal");
            _al.Add("Duitsland");
            _al.Add("Oostenrijk");
            _al.Add("Zwitserland");
            _al.Add("Nederland");
            _al.Add("Frankrijk");
            _al.Add("Belgie");
            _al.Add("Griekenland");
            _al.Add("Kroatie");
            _al.Add("Slovenie");
        }

    }
}