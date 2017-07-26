using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "Hack@Home", MainLauncher = true, Icon = "@drawable/iconox")]
    public class MainActivity : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var buttonValidate = FindViewById<Button>(Resource.Id.buttonValidate);

            buttonValidate.Click += (sender, e) =>
            {
                Validate();
                
            };
            
        }

        private async void Validate()
        {
            var ServiceClient = new HackAtHome.SAL.Autenticate();

            var editTextEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            var editTextPassword = FindViewById<EditText>(Resource.Id.editTextPassword);

            string StudentEmail = editTextEmail.Text;
            string Password = editTextPassword.Text;
            
            var Result = await ServiceClient.AutenticateAsync(
                StudentEmail, Password);

            var Intent = new Android.Content.Intent(this,
                   typeof(EvidenceActivity));

            if (Result.Status == HackAtHome.Entities.Status.Success)
            {
                Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
                AlertDialog Alert = Builder.Create();
                Alert.SetTitle("Conexión Exitosa");
                Alert.SetIcon(Resource.Drawable.android);
                Alert.SetMessage(
                $"{Result.Status}\n{Result.FullName}\n{Result.Token}");
                Alert.SetButton("Ok", (s, ev) => {
                    Intent.PutExtra("FullName", $"{Result.FullName}");
                    Intent.PutExtra("Token", $"{Result.Token}");
                    EnviarEvidencia();
                    StartActivity(Intent);
                });
                Alert.Show();
                


            }
            else if(Result.Status == HackAtHome.Entities.Status.Error)
            {
                Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
                AlertDialog Alert = Builder.Create();
                Alert.SetTitle("Ingrese sus datos");
                Alert.SetIcon(Resource.Drawable.android);
                Alert.SetMessage(
                $"{Result.Status}\n{Result.FullName}\n{Result.Token}");
                Alert.SetButton("Ok", (s, ev) => { });
                Alert.Show();
            }
            else if (Result.Status == HackAtHome.Entities.Status.InvalidUserOrNotInEvent)
            {
                Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
                AlertDialog Alert = Builder.Create();
                Alert.SetTitle("Datos Incorrectos");
                Alert.SetIcon(Resource.Drawable.android);
                Alert.SetMessage(
                $"{Result.Status}\n{Result.FullName}\n{Result.Token}");
                Alert.SetButton("Ok", (s, ev) => { });
                Alert.Show();
            }
        }

        public async void EnviarEvidencia()
        {
            var editTextEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            string StudentEmail = editTextEmail.Text;
            var MicrosoftEvidence = new LabItem
            {
                Email = StudentEmail,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString(
                    ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };
            var MicrosoftClient = new MicrosoftServiceClient();
            await MicrosoftClient.SendEvidence(MicrosoftEvidence);
        }
    }
}

