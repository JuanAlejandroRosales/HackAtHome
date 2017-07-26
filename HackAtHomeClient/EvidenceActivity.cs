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
using HackAtHome.SAL;
using HackAtHome.CustomAdapters;

namespace HackAtHomeClient
{
    [Activity(Label = "Hack@Home", Icon = "@drawable/iconox")]
    public class EvidenceActivity : Activity
    {
        Complex Data;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Evidence);

            var FullName = Intent.GetStringExtra("FullName");
            var textViewName = FindViewById<TextView>(Resource.Id.textViewName);
            textViewName.Text = FullName;

            // Utilizar FragmentManager para recuperar el Fragmento
            Data = (Complex)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                // No ha sido almacenado, agregar el fragmento a la Activity
                Data = new Complex();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
                await ServiceEvidences();
            }


            var Token = Intent.GetStringExtra("Token");
            var listViewEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);
            
            listViewEvidences.Adapter = new EvidencesAdapter(
                this, Data.ListEvidences, Resource.Layout.ListItem, Resource.Id.textViewTitleEvidence, Resource.Id.textViewStatusEvidence);

            var buttonValidate = FindViewById<Button>(Resource.Id.buttonValidate);

            listViewEvidences.ItemClick += (sender, e) =>
            {
                var Intent = new Android.Content.Intent(this,
                   typeof(EvidenceDetalleActivity));

                var textViewTitleEvidence = FindViewById<TextView>(Resource.Id.textViewTitleEvidence);
                var textViewStatusEvidence = FindViewById<TextView>(Resource.Id.textViewStatusEvidence);
                var Position = e.Position;

                var IDEvidence = Data.ListEvidences[Position].EvidenceID;
                var TitleEvidence = Data.ListEvidences[Position].Title;
                var StatusEvidence = Data.ListEvidences[Position].Status;
                Intent.PutExtra("Token", Token);
                Intent.PutExtra("IDEvidence", IDEvidence);
                Intent.PutExtra("FullName", FullName);

                Intent.PutExtra("TitleEvidence", TitleEvidence);
                Intent.PutExtra("StatusEvidence", StatusEvidence);
                StartActivity(Intent);
            };

        }
        
        private async System.Threading.Tasks.Task ServiceEvidences()
        {
            var Token = Intent.GetStringExtra("Token");
            var ServiceGetEvidences = new GetEvidences();

            var GetListEvidences = await ServiceGetEvidences.GetEvidencesAsync(Token);

            Data.ListEvidences = GetListEvidences;
        }
    }
}