using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Simon.Models;
using Xamarin.Forms;

namespace Simon.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        #endregion

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        private const string DeviceTokenKey = "DeviceToken_key";
        private static readonly string DeviceTokenDefault = string.Empty;
        public static string DeviceToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(DeviceTokenKey, DeviceTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DeviceTokenKey, value);
            }
        }

        private const string MessageThreadUsersKey = "MessageThreadUsers_Key";
        public static ObservableCollection<MessageThread> MessageThreadUsersData
        {
            get
            {
                var value = AppSettings.GetValueOrDefault(MessageThreadUsersKey, string.Empty);
                if (string.IsNullOrEmpty(value))
                {
                    return new ObservableCollection<MessageThread>();
                }
                else
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<MessageThread>>(value);
                }
            }
            set
            {
                string data = string.Empty;
                if (value != null)
                {
                    data = JsonConvert.SerializeObject(value);
                }
                AppSettings.AddOrUpdateValue(MessageThreadUsersKey, data);
            }
        }

        public const string ApprovePageSelectedTabKey = "ApprovePageSelectedTab_key";
        private static readonly int ApprovePageDefaultSelectedTab = 0;
        public static int ApprovePageSelectedTabIndex
        {
            get
            {
                return AppSettings.GetValueOrDefault(ApprovePageSelectedTabKey, ApprovePageDefaultSelectedTab);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ApprovePageSelectedTabKey, value);
            }
        }

        public const string PartyNameKey = "PartyName_key";
        private static readonly string PartyNameDefault = null;
        public static string PartyName
        {
            get
            {
                return AppSettings.GetValueOrDefault(PartyNameKey, PartyNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PartyNameKey, value);
            }
        }

        public const string TypedMessageKey = "TypedMessage_key";
        private static readonly string TypedMessageDefault = string.Empty;
        public static string TypedMessage
        {
            get
            {
                return AppSettings.GetValueOrDefault(TypedMessageKey, TypedMessageDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TypedMessageKey, value);
            }
        }

        private const string MessageCountKey = "MessageCount_key";
        private static readonly int MessageCountDefault = 0;
        public static int MessageCount
        {
            get
            {
                return AppSettings.GetValueOrDefault(MessageCountKey, MessageCountDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(MessageCountKey, value);
            }
        }
    }
}