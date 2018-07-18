using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace Travelr
{
    [Activity(Label = "RecensieActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class RecensieActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Recensie);

            bool LoginCheck = Intent.GetBooleanExtra("LoginCheck", false);
            int mUserID = Intent.GetIntExtra("mUserID", 0);
            string landNaam = Intent.GetStringExtra("landNaam");
            string Username = Intent.GetStringExtra("Username");

            

            // Buttons
            ImageButton backBtn = FindViewById<ImageButton>(Resource.Id.btnRecensieBack);
            ImageButton homeBtn = FindViewById<ImageButton>(Resource.Id.btnRecensieHome);
            Button btnBevestig = FindViewById<Button>(Resource.Id.btnBevestig);

            //RECENSIE PLAATSEN VIEWS
            TextView txtRGebruikersNaam = FindViewById<TextView>(Resource.Id.txtRGebruikersNaam);
            EditText txtRecensie = FindViewById<EditText>(Resource.Id.txtRecensie);
            RatingBar rbRecensie = FindViewById<RatingBar>(Resource.Id.rbRecensie);

            backBtn.Click += BackBtn_Click;
            homeBtn.Click += (sender, e) => HomeBtn_Click(sender, e, LoginCheck, mUserID, Username);
            btnBevestig.Click += (s, e) => BtnBevestig_Click(s, e, txtRecensie, rbRecensie, mUserID, landNaam);

            laadRecensies(landNaam);
            if(Username != null)
            {
                txtRGebruikersNaam.Text = Username;
            }
            else
            {
                txtRGebruikersNaam.Text = "Gast";
            }
            
        }

        private void BtnBevestig_Click(object sender, EventArgs e, EditText txtRecensie, RatingBar rbRecensie,
            int mUserID, string landNaam)
        {

            if(mUserID != 0)
            {
                if(rbRecensie.Rating != 0)
                {
                    if (txtRecensie.Text.Length > 15)
                    {
                        if(txtRecensie.Text.Length < 500)
                        {
                            // RECENSIE PLAATSEN
                            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
                            try
                            {
                                if (con.State == ConnectionState.Closed)
                                {
                                    con.Open();

                                    using (var conn = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8"))
                                    {
                                        conn.Open();

                                        string sql = "INSERT INTO tbl_recensies (userID, recensie_text, country_name, rating) VALUES(" + mUserID + ", '" + txtRecensie.Text.ToString() + "', '" + landNaam + "', '" + rbRecensie.Rating.ToString() + "');";
                                        MySqlCommand command = new MySqlCommand(sql, con);
                                        MySqlDataReader rdr = command.ExecuteReader();

                                    }
                                }
                            }
                            finally
                            {
                                con.Close();
                                rbRecensie.Rating = 0;
                                txtRecensie.Text = "";
                                Toast.MakeText(this, "U recensie is geplaatst.", ToastLength.Short).Show();
                                laadRecensies(landNaam);
                                //REFRESH ALLE COMMENTS
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, "U kunt maximaal 500 tekens gebruiken.", ToastLength.Short).Show();
                        }
                        
                    }
                    else
                    {
                        Toast.MakeText(this, "U moet minimaal 15 tekens gebruiken.", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Selecteer het aantal sterren die u wilt geven.", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "U Moet inloggen om een recensie te plaatsen.", ToastLength.Short).Show();
            }

        }

        private void laadRecensies(string landNaam)
        {
            TextView txtGeenRecensie = FindViewById<TextView>(Resource.Id.txtGeenRecensie);
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");

            if (con.State == ConnectionState.Closed)
            {
                con.Open();


                // GET USERID
                MySqlCommand cmdGet_Recensie = new MySqlCommand("SELECT * FROM tbl_recensies where country_name=@landnaam;", con);
                cmdGet_Recensie.Parameters.AddWithValue("@landnaam", landNaam);
                MySqlDataAdapter daGetUser = new MySqlDataAdapter(cmdGet_Recensie);
                DataSet dsRecensies = new DataSet();
                daGetUser.Fill(dsRecensies);
                //GET ROWS
                int rowAmount = dsRecensies.Tables[0].Rows.Count;

                if(rowAmount >= 15)
                {
                    rowAmount = 15;
                }
                // FILL IN ALL REVIEWS
                if(rowAmount != 0)
                {
                    txtGeenRecensie.Visibility = ViewStates.Gone;
                    for (int i = 0; i < rowAmount; i++)
                    {
                        if (rowAmount == 0)
                        {
                            setEmptyReviews();
                        }
                        else
                        {
                            
                            DataRow dr = dsRecensies.Tables[0].Rows[i];
                            int UserId = Int32.Parse(dr["userID"].ToString());
                            string RecensieText = dr["recensie_text"].ToString();
                            int Rating = Int32.Parse(dr["rating"].ToString());
                            setReviews(UserId, RecensieText, Rating, i, rowAmount);
                        }

                    }
                }
                else
                {
                    setEmptyReviews();
                }
                
               
                con.Close();  
            }
        }

        private void setEmptyReviews()
        {
            TextView[] txtNaamArray = getTxtNaamArray();
            RatingBar[] rbRArray = getRatingBarArray();
            TextView[] txtTextArray = getTxtTextArray();

            for (int i = 0; i < 15; i++)
            {

                txtNaamArray[i].Visibility = ViewStates.Gone;
                rbRArray[i].Visibility = ViewStates.Gone;
                txtTextArray[i].Visibility = ViewStates.Gone;
                
            }
        }
        private void setReviews(int UserId, string RecensieText, int Rating, int counter, int rowAmount)
        {

            TextView[] txtNaamArray = getTxtNaamArray();
            RatingBar[] rbRArray = getRatingBarArray();
            TextView[] txtTextArray = getTxtTextArray();
            //GET USERNAME

            txtNaamArray[counter].Text = getUserName(UserId);
            rbRArray[counter].Rating = Rating;
            txtTextArray[counter].Text = RecensieText;
            

 
            for (int i = 0; i < 15; i++)
            {

                if(i < rowAmount)
                {
                    txtNaamArray[i].Visibility = ViewStates.Visible;
                    rbRArray[i].Visibility = ViewStates.Visible;
                    rbRArray[i].IsIndicator = true;
                    txtTextArray[i].Visibility = ViewStates.Visible;
                }
                else
                {
                    txtNaamArray[i].Visibility = ViewStates.Gone;
                    rbRArray[i].Visibility = ViewStates.Gone;
                    txtTextArray[i].Visibility = ViewStates.Gone;
                }

            }

        }

        private TextView[] getTxtNaamArray()
        {
            TextView txtNaamR1 = FindViewById<TextView>(Resource.Id.txtNaamR1);
            TextView txtNaamR2 = FindViewById<TextView>(Resource.Id.txtNaamR2);
            TextView txtNaamR3 = FindViewById<TextView>(Resource.Id.txtNaamR3);
            TextView txtNaamR4 = FindViewById<TextView>(Resource.Id.txtNaamR4);
            TextView txtNaamR5 = FindViewById<TextView>(Resource.Id.txtNaamR5);
            TextView txtNaamR6 = FindViewById<TextView>(Resource.Id.txtNaamR6);
            TextView txtNaamR7 = FindViewById<TextView>(Resource.Id.txtNaamR7);
            TextView txtNaamR8 = FindViewById<TextView>(Resource.Id.txtNaamR8);
            TextView txtNaamR9 = FindViewById<TextView>(Resource.Id.txtNaamR9);
            TextView txtNaamR10 = FindViewById<TextView>(Resource.Id.txtNaamR10);
            TextView txtNaamR11 = FindViewById<TextView>(Resource.Id.txtNaamR11);
            TextView txtNaamR12 = FindViewById<TextView>(Resource.Id.txtNaamR12);
            TextView txtNaamR13 = FindViewById<TextView>(Resource.Id.txtNaamR13);
            TextView txtNaamR14 = FindViewById<TextView>(Resource.Id.txtNaamR14);
            TextView txtNaamR15 = FindViewById<TextView>(Resource.Id.txtNaamR15);
            TextView[] txtNaamArray = { txtNaamR1, txtNaamR2, txtNaamR3, txtNaamR4, txtNaamR5, txtNaamR6, txtNaamR7,
                txtNaamR8, txtNaamR9, txtNaamR10, txtNaamR11, txtNaamR12, txtNaamR13, txtNaamR14, txtNaamR15 };
            return txtNaamArray;
        }
        private TextView[] getTxtTextArray()
        {
            TextView txtTextR1 = FindViewById<TextView>(Resource.Id.txtTextR1);
            TextView txtTextR2 = FindViewById<TextView>(Resource.Id.txtTextR2);
            TextView txtTextR3 = FindViewById<TextView>(Resource.Id.txtTextR3);
            TextView txtTextR4 = FindViewById<TextView>(Resource.Id.txtTextR4);
            TextView txtTextR5 = FindViewById<TextView>(Resource.Id.txtTextR5);
            TextView txtTextR6 = FindViewById<TextView>(Resource.Id.txtTextR6);
            TextView txtTextR7 = FindViewById<TextView>(Resource.Id.txtTextR7);
            TextView txtTextR8 = FindViewById<TextView>(Resource.Id.txtTextR8);
            TextView txtTextR9 = FindViewById<TextView>(Resource.Id.txtTextR9);
            TextView txtTextR10 = FindViewById<TextView>(Resource.Id.txtTextR10);
            TextView txtTextR11 = FindViewById<TextView>(Resource.Id.txtTextR11);
            TextView txtTextR12 = FindViewById<TextView>(Resource.Id.txtTextR12);
            TextView txtTextR13 = FindViewById<TextView>(Resource.Id.txtTextR13);
            TextView txtTextR14 = FindViewById<TextView>(Resource.Id.txtTextR14);
            TextView txtTextR15 = FindViewById<TextView>(Resource.Id.txtTextR15);
            TextView[] txtTextArray = { txtTextR1, txtTextR2, txtTextR3, txtTextR4, txtTextR5, txtTextR6, txtTextR7, txtTextR8,
                txtTextR9, txtTextR10, txtTextR11, txtTextR12, txtTextR13, txtTextR14, txtTextR15 };

            return txtTextArray;
        }
        private RatingBar[] getRatingBarArray()
        {
            RatingBar rbR1 = FindViewById<RatingBar>(Resource.Id.rbR1);
            RatingBar rbR2 = FindViewById<RatingBar>(Resource.Id.rbR2);
            RatingBar rbR3 = FindViewById<RatingBar>(Resource.Id.rbR3);
            RatingBar rbR4 = FindViewById<RatingBar>(Resource.Id.rbR4);       
            RatingBar rbR5 = FindViewById<RatingBar>(Resource.Id.rbR5);         
            RatingBar rbR6 = FindViewById<RatingBar>(Resource.Id.rbR6);
            RatingBar rbR7 = FindViewById<RatingBar>(Resource.Id.rbR7);            
            RatingBar rbR8 = FindViewById<RatingBar>(Resource.Id.rbR8);        
            RatingBar rbR9 = FindViewById<RatingBar>(Resource.Id.rbR9);           
            RatingBar rbR10 = FindViewById<RatingBar>(Resource.Id.rbR10);           
            RatingBar rbR11 = FindViewById<RatingBar>(Resource.Id.rbR11);          
            RatingBar rbR12 = FindViewById<RatingBar>(Resource.Id.rbR12);           
            RatingBar rbR13 = FindViewById<RatingBar>(Resource.Id.rbR13);           
            RatingBar rbR14 = FindViewById<RatingBar>(Resource.Id.rbR14);           
            RatingBar rbR15 = FindViewById<RatingBar>(Resource.Id.rbR15);
            RatingBar[] ratingBarArray = { rbR1, rbR2, rbR3, rbR4, rbR5, rbR6, rbR7, rbR8, rbR9, rbR10, rbR11, rbR12, rbR13, rbR14, rbR15 };

            return ratingBarArray;
           
        }

        private string getUserName(int UserID)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=travelr;User Id=koster3travelr;Password=Travelr2018;charset=utf8");
            string Username;

            con.Open();


            // GET USERID
            MySqlCommand cmdGet_UM = new MySqlCommand("SELECT userName FROM tbl_userdata WHERE tbl_userdata.userID = @UserID", con);
            cmdGet_UM.Parameters.AddWithValue("@UserID", UserID);
            MySqlDataAdapter daGetUser = new MySqlDataAdapter(cmdGet_UM);
            DataSet dsGetUser = new DataSet();
            daGetUser.Fill(dsGetUser);
            DataRow dr = dsGetUser.Tables[0].Rows[0];
            Username = dr["UserName"].ToString();
            con.Close();


            return Username;
        }

        private void HomeBtn_Click(object sender, EventArgs e, bool LoginCheck, int mUserID, string Username)
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));
            nextActivity.PutExtra("mUserID", mUserID);
            nextActivity.PutExtra("Username", Username);

            if (LoginCheck == true)
            {
                nextActivity.PutExtra("LoginCheck", true);
            }
            else{
                nextActivity.PutExtra("LoginCheck", false);
            }

            StartActivity(nextActivity);
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

    }
}