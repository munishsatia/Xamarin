using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace dialNumberAndroid
{
    [Activity(Label = "Call-Demo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            TextView txtNumber = FindViewById<TextView>(Resource.Id.txtNumber);
            Button btnCall = FindViewById<Button>(Resource.Id.btnCall);
            //btnCall.Enabled = false;
            btnCall.Click += (object sender, System.EventArgs e) =>
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + txtNumber.Text);

                callDialog.SetNeutralButton("Call", delegate {
                    // Create intent to dial phone
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + txtNumber.Text));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

        }

       
    }
}

