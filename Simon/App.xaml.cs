using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using DLToolkit.Forms.Controls;
using Plugin.PushNotification;
using Simon.Helpers;
using Simon.Models;
using Simon.ServiceHandler;
using Simon.ViewModel;
using Simon.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Simon
{
    public partial class App : Application
    {
        public static string ScreenTitle { get; set; } = string.Empty;

        public static UserListModel SelectedUserData { get; set; }
        public static ObservableCollection<messages> MessageThreadData { get; set; }
        public static int buttonClick;

        public static bool isFromKeyboardDoneButton { get; set; } = false;
        public static bool isFromAddParticipantPage { get; set; } = false;

        public static bool isSelectRead { get; set; } = false;
        public static bool isSelectUnRead { get; set; } = false;
        public static bool AsceDsce { get; set; } = true;
        public static bool AsceDsceName { get; set; } = true;
        public static bool isFirstTime { get; set; } = false;

        public static string ReadUnread { get; set; } = string.Empty;
        public static string OrderByText { get; set; } = string.Empty;
        public static string SelectedTitle { get; set; } = string.Empty;
        public static string selectedName { get; set; } = string.Empty;
        public static string ApprovalSelectedTitle { get; set; } = string.Empty;

        public static string tempFile;
        public static int selectedPageId;
        BaseViewModel bindingContext;

        public App()
        {
            InitializeComponent();

            //tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
            //File.AppendAllText(App.tempFile, "Add log to File");
            //Debug.WriteLine("File Name====" + App.tempFile);

            FlowListView.Init();
            LayoutService.Init();

            XF.Material.Forms.Material.Init(this);
            // MainPage = new MainPage();
            //App.Current.Properties["REFRESHTOKEN"] =token;
            NavigationPage navPage = new NavigationPage
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.Black
            };
            //MainPage = new NavigationPage(new MainPage())
            //{
            //    BarTextColor = Color.White,
            //    BarBackgroundColor= Color.FromHex("#002343")
            //};

            MainPage = new NavigationPage(new LoginPage())
            {
                BarTextColor = Color.Black,
                BarBackgroundColor = Color.White
            };
            bindingContext = (BaseViewModel)this.BindingContext;
            App.Current.Resources["MsgCount"] = "0";
            //MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            Settings.DeviceToken = CrossPushNotification.Current.Token;

            tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
            File.AppendAllText(tempFile, "\n\n" + Settings.DeviceToken);
            Debug.WriteLine("File Name====" + tempFile);

            CrossPushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                Debug.WriteLine($"TOKEN : {p.Token}");

                tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
                File.AppendAllText(tempFile, "\n\n" + p.Token);
                Debug.WriteLine("File Name====" + tempFile);
            };

            Debug.WriteLine($"TOKEN: {CrossPushNotification.Current.Token}");
            Console.WriteLine("Token " + CrossPushNotification.Current.Token);

            CrossPushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    Debug.WriteLine("Received");

                    Debug.WriteLine("Received");
                    tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
                    File.AppendAllText(tempFile, "\n\nNotification Received....");
                    Debug.WriteLine("File Name====" + tempFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };

            CrossPushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                Debug.WriteLine("Opened");
                tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFile.txt");
                File.AppendAllText(tempFile, "\n\nNotification Opened....");
                Debug.WriteLine("File Name====" + tempFile);
                foreach (var data in p.Data)
                {
                    Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            CrossPushNotification.Current.OnNotificationAction += (s, p) =>
            {
                Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossPushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                Debug.WriteLine("Dismissed");
            };
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
    }
}
