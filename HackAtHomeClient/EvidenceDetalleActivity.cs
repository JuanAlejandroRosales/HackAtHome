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
using Android.Webkit;
using Android.Graphics;

namespace HackAtHomeClient
{
    [Activity(Label = "Hack@Home", Icon = "@drawable/iconox")]
    public class EvidenceDetalleActivity : Activity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EvidenceDetalle);

            var FullName = Intent.GetStringExtra("FullName");
            var textViewName = FindViewById<TextView>(Resource.Id.textViewName);
            textViewName.Text = FullName;

            var TitleEvidence = Intent.GetStringExtra("TitleEvidence");
            var textViewTitle = FindViewById<TextView>(Resource.Id.textViewTitle);
            textViewTitle.Text = TitleEvidence;

            var StatusEvidence = Intent.GetStringExtra("StatusEvidence");
            var textViewStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            textViewStatus.Text = StatusEvidence;

            var Token = Intent.GetStringExtra("Token");

            var IDEvidence = Intent.GetIntExtra("IDEvidence",0);
            
            var webViewDescriptionEvidence = FindViewById<WebView>(Resource.Id.webViewDescriptionEvidence);
            var imageViewEvidence = FindViewById<ImageView>(Resource.Id.imageViewEvidence);

            var ServiceGetEvidencesDetail = new HackAtHome.SAL.GetEvidenceByID();

            var GetEvidencesDetail = await ServiceGetEvidencesDetail.GetEvidenceByIDAsync(Token, IDEvidence);

            Koush.UrlImageViewHelper.SetUrlDrawable(imageViewEvidence, GetEvidencesDetail.Url);

            string WebViewContent = $"<div style='color:#BCBCBC'>{GetEvidencesDetail.Description}</div>";
            webViewDescriptionEvidence.LoadDataWithBaseURL(
                null, WebViewContent, "text/html", "utf-8", null);

            webViewDescriptionEvidence.SetBackgroundColor(Color.Black);
            
        }
    }
}