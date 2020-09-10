using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using DLToolkit.Forms.Controls;
using Foundation;
using Plugin.PushNotification;
using Simon.Helpers;
using UIKit;
using Xamarin;
using Xamarin.Forms;

namespace Simon.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        //public IAuthorizationFlowSession CurrentAuthorizationFlow { get; set; }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Forms.SetFlags(new[]
            {
                "Expander_Experimental"
            });

            global::Xamarin.Forms.Forms.Init();
           
            Rg.Plugins.Popup.Popup.Init();
            XF.Material.iOS.Material.Init();
 
            IQKeyboardManager.SharedManager.Enable = true;
            IQKeyboardManager.SharedManager.EnableAutoToolbar = true;
            IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;
            IQKeyboardManager.SharedManager.PreviousNextDisplayMode = IQPreviousNextDisplayMode.AlwaysHide;

            FlowListView.Init();

            LoadApplication(new App());

            PushNotificationManager.Initialize(options, true);
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            var result = base.FinishedLaunching(app, options);
            app.KeyWindow.TintColor = UIColor.Gray;

            // Color of the tabbar background:
            UITabBar.Appearance.BarTintColor = UIColor.FromRGB(247, 247, 247);

            // Color of the selected tab text color:
            UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    TextColor = UIColor.FromRGB(0, 122, 255)
                },
                UIControlState.Selected);

            // Color of the unselected tab icon & text:
            UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    TextColor = UIColor.FromRGB(146, 146, 146)
                },
                UIControlState.Normal);
            //UITabBar.Appearance.TintColor = UIColor.Blue;


            return result;
        }

        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    byte[] result = new byte[deviceToken.Length];
        //    Marshal.Copy(deviceToken.Bytes, result, 0, (int)deviceToken.Length);
        //    var token = BitConverter.ToString(result).Replace("-", "");

        //    App.tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
        //    File.AppendAllText(App.tempFile, "\n\nRegisteredForRemoteNotifications call \n\niOS Token : " + token);
        //    Debug.WriteLine("File Name====" + App.tempFile);

        //    System.Diagnostics.Debug.WriteLine($"TOKEN : {Settings.DeviceToken}");
        //    Settings.DeviceToken = token;
        //    PushNotificationManager.DidRegisterRemoteNotifications(token);
        //    UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        //}

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            App.tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
            File.AppendAllText(App.tempFile, "\n\nRegisteredForRemoteNotifications call \n\niOS Token : " + deviceToken);
            Debug.WriteLine("File Name====" + App.tempFile);

            PushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            PushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }
        
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {

            PushNotificationManager.DidReceiveMessage(userInfo);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url,string sourceApplication, NSObject annotation)
        {
            return false;
        }
    }
}