using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Xamarin.Forms;

using Android.Util;
using System.Collections.Generic;
//using Android.Gms.Common;
//using Firebase.Iid;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Rg.Plugins.Popup.Services;
using Plugin.PushNotification;
using Plugin.Permissions;
using Xamarin.Forms.Platform.Android;

namespace Simon.Droid
{
    [Activity(Label = "Simon", Icon = "@mipmap/ic_launcher", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        static readonly string TAG = "MainActivity";
        //internal static readonly string CHANNEL_ID = "my_notification_channel";
        //internal static readonly int NOTIFICATION_ID = 100;
        private static bool _hasEverInited;
        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;

            Forms.SetFlags(new[]
            {
                "Expander_Experimental"
            });

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            //string refreshedToken=null;

            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);
            FlowListView.Init();
            Rg.Plugins.Popup.Popup.Init(this, bundle);

            if (!_hasEverInited)
            {
                global::Xamarin.Forms.Forms.Init(this, bundle);
                XF.Material.Droid.Material.Init(this, bundle);
                Rg.Plugins.Popup.Popup.Init(this, bundle);
                //LoadApplication(new App());
                //IsPlayServicesAvailable(); //You can use this method to check if play services are available.
                //CreateNotificationChannel();
                Task.Run(() =>
                {
                    //    //instanceid.DeleteInstanceId();
                    //var instanceid = FirebaseInstanceId.Instance;
                    //Log.Debug("TAG", "{0} {1}", instanceid.Token, instanceid.GetToken(this.GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope));

                    //refreshedToken = instanceid.Token;
                });

//                if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
//                {
//                    Window.SetStatusBarColor(Android.Graphics.Color.Black);
//                    Window.Attributes.LayoutInDisplayCutoutMode = LayoutInDisplayCutoutMode.ShortEdges;
//                }

//                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
//                {
//                    PushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";
//                    PushNotificationManager.DefaultNotificationChannelName = "General";
//                }
//                PushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.High;

//#if DEBUG
//                PushNotificationManager.Initialize(this, false);
//#else
//            PushNotificationManager.Initialize(this,false);
//#endif

                LoadApplication(new App());

                PushNotificationManager.ProcessIntent(this, Intent);
                PushNotificationManager.IconResource = Resource.Drawable.ic_stat_appstore;
                PushNotificationManager.Color = Color.FromHex("#000000").ToAndroid();
                //LoadApplication(new App(refreshedToken));
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            PushNotificationManager.ProcessIntent(this, intent);
            PushNotificationManager.IconResource = Resource.Drawable.ic_stat_appstore;
            PushNotificationManager.Color = Color.FromHex("#000000").ToAndroid();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override async void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                await PopupNavigation.Instance.PopAsync();
            }
        }

        //public bool IsPlayServicesAvailable()
        //{
        //    int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
        //    if (resultCode != ConnectionResult.Success)
        //    {
        //        if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
        //        { }  //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
        //        else
        //        {
        //            // msgText.Text = "This device is not supported";
        //            Finish();
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        //msgText.Text = "Google Play Services is available.";
        //        return true;
        //    }
        //}
        //void CreateNotificationChannel()
        //{
        //    if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        //    {
        //        // Notification channels are new in API 26 (and not a part of the
        //        // support library). There is no need to create a notification
        //        // channel on older versions of Android.
        //        return;
        //    }

        //    var channel = new NotificationChannel(CHANNEL_ID,
        //                                          "FCM Notifications",
        //                                          NotificationImportance.Default)
        //    {

        //        Description = "Firebase Cloud Messages appear in this channel"
        //    };

        //    var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
        //    notificationManager.CreateNotificationChannel(channel);
        //}
    }
}