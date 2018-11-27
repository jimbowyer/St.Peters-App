using System;
using Xamarin.Forms;
using stPetes.Services;
using stPetes.Droid.Services;
using Android.Content;
using Android.Widget;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Support.V4.App;
using Android;

[assembly: Dependency(typeof(PdFService))]

namespace stPetes.Droid.Services
{
    public class PdFService : Ipdf
    {
        public async Task<int> OpenPdf(string _sUrl)
        {
            const string sPerm = Manifest.Permission.WriteExternalStorage;
            if (Android.App.Application.Context.CheckSelfPermission(sPerm) != (int)Permission.Granted)
            {
                // We dont have permission, go ahead use stroage.
                Toast.MakeText(Android.App.Application.Context, "Please provide app permission to read/write storage to save bulletins", ToastLength.Long).Show();

                //try
                //{
                //    //*******
                //    Toast.MakeText(Android.App.Application.Context, "write denied", ToastLength.Short).Show();
                //    //ext storage permission is not granted. If necessary display rationale &request.
                //    //var activity = Android.App.Application.Context as MainActivity; //null why?
                //    Android.App.Activity activity = Android.App.Application.Context as MainActivity;
                //    //activity = (Android.App.Activity)Plugin.CurrentActivity.CrossCurrentActivity.Current;
                //    ActivityCompat.RequestPermissions(activity, new String[]{Manifest.Permission.WriteExternalStorage}, 0);
                //}
                //catch (Exception ex)
                //{
                //    Debug.Write(ex.Message);
                //    Toast.MakeText(Android.App.Application.Context, "Barfed trying to get file permissions", ToastLength.Short).Show();
                //}
            }

            //Copy the private file's data to the EXTERNAL PUBLIC storage location
            HttpClient webClient = new HttpClient();
            byte[] bytes = await webClient.GetByteArrayAsync(_sUrl);            
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/bulletin.pdf";
            File.WriteAllBytes(externalPath, bytes);
            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);

            //now open the bulleitn up in pdf reader:
            Android.Net.Uri uri = Android.Net.Uri.Parse("content:///" + externalPath);            
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "application/pdf");            
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Android.App.Application.Context.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Debug.Write( ex.Message);
                Toast.MakeText(Android.App.Application.Context, "No Application Available to View PDF", ToastLength.Short).Show();
            }
           
            return 1;           
        } //OpenPdf

    } //class
} //ns