using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Simon.Controls;
using Simon.Helpers;
using Simon.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Simon.ServiceHandler
{
    public static class LayoutService
    {
        public static void Init()
        {
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    if (DeviceInfo.Version < Version.Parse("11.0"))
            //    {
            //        App.Current.Resources["SafeAreaPadding"] = new Thickness(0, 20, 0, 0);
            //    }
            //    else
            //    {
            //        App.Current.Resources["SafeAreaPadding"] = new Thickness(0, 0, 0, 0);
            //    }
            //}

            SessionService.BaseFooterItems = new ObservableCollection<FooterModel>();

            SessionService.BaseFooterItems.Add(new FooterModel()
            {
                Id = 0,
                Name = "Home",
                IsSelected = true,
                SelectedImage = "home_active",
                UnSelectedImage = "home",
                MsgCount = 0,
                isMsgBadgeVisible = false
            });
            SessionService.BaseFooterItems.Add(new FooterModel()
            {
                Id = 1,
                Name = "Deal",
                IsSelected = false,
                SelectedImage = "deal_active",
                UnSelectedImage = "deal",
                MsgCount = 0,
                isMsgBadgeVisible = false
            });
            SessionService.BaseFooterItems.Add(new FooterModel()
            {
                Id = 2,
                Name = "Message",
                IsSelected = false,
                SelectedImage = "message_active",
                UnSelectedImage = "message",
                MsgCount = Settings.MessageCount,
                isMsgBadgeVisible = (Settings.MessageCount > 0) ? true : false
            });
            SessionService.BaseFooterItems.Add(new FooterModel()
            {
                Id = 3,
                Name = "Approve",
                IsSelected = false,
                SelectedImage = "approve_active",
                UnSelectedImage = "approve",
                MsgCount = 0,
                isMsgBadgeVisible = false
            });

            /*if (SettingsService.LoggedInUser.Id > 0)
            {
                SessionService.BaseFooterItems.All((arg) => {
                    if (arg.Id == 2)
                    {
                        arg.IsSelected = true;
                    }
                    else
                    {
                        arg.IsSelected = false;
                    }
                    return true;
                });
                SessionService.SelectedFooterItem = SessionService.BaseFooterItems[1];
            }
            else
            {
                SessionService.BaseFooterItems.All((arg) => {
                    if (arg.Id == 1)
                    {
                        arg.IsSelected = true;
                    }
                    else
                    {
                        arg.IsSelected = false;
                    }
                    return true;
                });
                SessionService.SelectedFooterItem = SessionService.BaseFooterItems[0];
            }*/

        }

        public static double sizeConvertAsPerDevice(double size)
        {
            try
            {
                if (size <= 0) { return 0; }
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                    if (Device.RuntimePlatform == Device.iOS)
                    {

                        int d = 0;
                        if (mainDisplayInfo.Height <= 1136)
                        {
                            d = 5;
                        }
                        else if (mainDisplayInfo.Height <= 1334)
                        {
                            d = 7;
                        }
                        else
                        {
                            return size;
                        }

                        var half = size / d;
                        size = size - half;
                    }
                    else
                    {
                        return size;
                    }
                }
                else
                {
                    var half = size / 2;
                    size = size + half;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return size;
        }

        public static Thickness MarginPaddingConvertAsPerDevice(double left, double top, double right, double bottom)
        {
            try
            {
                if (Device.Idiom == TargetIdiom.Phone)
                {

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                        int d = 0;
                        if (mainDisplayInfo.Height <= 1136)
                        {
                            d = 5;
                        }
                        else if (mainDisplayInfo.Height <= 1334)
                        {
                            d = 7;
                        }
                        else
                        {
                            return new Thickness(left, top, right, bottom);
                        }

                        if (left > 0)
                        {
                            var half = left / d;
                            left = left - half;
                        }
                        else if (left < 0)
                        {
                            var half = Math.Abs(left) / d;
                            left = -(Math.Abs(left) - half);
                        }

                        if (top > 0)
                        {
                            var half = top / d;
                            top = top - half;
                        }
                        else if (top < 0)
                        {
                            var half = Math.Abs(top) / d;
                            top = -(Math.Abs(top) - half);
                        }

                        if (right > 0)
                        {
                            var half = right / d;
                            right = right - half;
                        }
                        else if (right < 0)
                        {
                            var half = Math.Abs(right) / d;
                            right = -(Math.Abs(right) - half);
                        }

                        if (bottom > 0)
                        {
                            var half = bottom / d;
                            bottom = bottom - half;
                        }
                        else if (bottom < 0)
                        {
                            var half = Math.Abs(bottom) / d;
                            bottom = -(Math.Abs(bottom) - half);
                        }
                    }
                }
                else
                {
                    if (left > 0)
                    {
                        var half = left / 2;
                        left = left + half;
                    }
                    else if (left < 0)
                    {
                        var half = Math.Abs(left) / 2;
                        left = -(Math.Abs(left) + half);
                    }

                    if (top > 0)
                    {
                        var half = top / 2;
                        top = top + half;
                    }
                    else if (top < 0)
                    {
                        var half = Math.Abs(top) / 2;
                        top = -(Math.Abs(top) + half);
                    }

                    if (right > 0)
                    {
                        var half = right / 2;
                        right = right + half;
                    }
                    else if (right < 0)
                    {
                        var half = Math.Abs(right) / 2;
                        right = -(Math.Abs(right) + half);
                    }

                    if (bottom > 0)
                    {
                        var half = bottom / 2;
                        bottom = bottom + half;
                    }
                    else if (bottom < 0)
                    {
                        var half = Math.Abs(bottom) / 2;
                        bottom = -(Math.Abs(bottom) + half);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new Thickness(left, top, right, bottom);
        }

        #region Font Size

        public static Double FontSize08 { get { return sizeConvertAsPerDevice(8); } }
        public static Double FontSize10 { get { return sizeConvertAsPerDevice(10); } }
        public static Double FontSize11 { get { return sizeConvertAsPerDevice(11); } }
        public static Double FontSize12 { get { return sizeConvertAsPerDevice(12); } }
        public static Double FontSize13 { get { return sizeConvertAsPerDevice(13); } }
        public static Double FontSize14 { get { return sizeConvertAsPerDevice(14); } }
        public static Double FontSize15 { get { return sizeConvertAsPerDevice(15); } }
        public static Double FontSize16 { get { return sizeConvertAsPerDevice(16); } }
        public static Double FontSize17 { get { return sizeConvertAsPerDevice(17); } }
        public static Double FontSize18 { get { return sizeConvertAsPerDevice(18); } }
        public static Double FontSize19 { get { return sizeConvertAsPerDevice(19); } }
        public static Double FontSize20 { get { return sizeConvertAsPerDevice(20); } }
        public static Double FontSize22 { get { return sizeConvertAsPerDevice(22); } }
        public static Double FontSize24 { get { return sizeConvertAsPerDevice(24); } }
        public static Double FontSize26 { get { return sizeConvertAsPerDevice(26); } }

        #endregion

        #region View Height Width

        public static Double HeightWidth1 { get { return sizeConvertAsPerDevice(1); } }
        public static Double HeightWidth3 { get { return sizeConvertAsPerDevice(3); } }
        public static Double HeightWidth10 { get { return sizeConvertAsPerDevice(10); } }
        public static Double HeightWidth12 { get { return sizeConvertAsPerDevice(12); } }
        public static Double HeightWidth15 { get { return sizeConvertAsPerDevice(15); } }
        public static Double HeightWidth16 { get { return sizeConvertAsPerDevice(16); } }
        public static Double HeightWidth20 { get { return sizeConvertAsPerDevice(20); } }
        public static Double HeightWidth22 { get { return sizeConvertAsPerDevice(22); } }
        public static Double HeightWidth25 { get { return sizeConvertAsPerDevice(25); } }
        public static Double HeightWidth30 { get { return sizeConvertAsPerDevice(30); } }
        public static Double HeightWidth40 { get { return sizeConvertAsPerDevice(40); } }
        public static Double HeightWidth45 { get { return sizeConvertAsPerDevice(45); } }
        public static Double HeightWidth50 { get { return sizeConvertAsPerDevice(50); } }
        public static Double HeightWidth55 { get { return sizeConvertAsPerDevice(55); } }
        public static Double HeightWidth60 { get { return sizeConvertAsPerDevice(60); } }
        public static Double HeightWidth65 { get { return sizeConvertAsPerDevice(65); } }
        public static Double HeightWidth70 { get { return sizeConvertAsPerDevice(70); } }
        public static Double HeightWidth75 { get { return sizeConvertAsPerDevice(75); } }
        public static Double HeightWidth76 { get { return sizeConvertAsPerDevice(76); } }
        public static Double HeightWidth80 { get { return sizeConvertAsPerDevice(80); } }
        public static Double HeightWidth85 { get { return sizeConvertAsPerDevice(85); } }
        public static Double HeightWidth90 { get { return sizeConvertAsPerDevice(90); } }
        public static Double HeightWidth96 { get { return sizeConvertAsPerDevice(96); } }
        public static Double HeightWidth100 { get { return sizeConvertAsPerDevice(100); } }
        public static Double HeightWidth108 { get { return sizeConvertAsPerDevice(108); } }
        public static Double HeightWidth120 { get { return sizeConvertAsPerDevice(120); } }
        public static Double HeightWidth130 { get { return sizeConvertAsPerDevice(130); } }
        public static Double HeightWidth140 { get { return sizeConvertAsPerDevice(140); } }
        public static Double HeightWidth150 { get { return sizeConvertAsPerDevice(150); } }
        public static Double HeightWidth160 { get { return sizeConvertAsPerDevice(160); } }
        public static Double HeightWidth170 { get { return sizeConvertAsPerDevice(170); } }
        public static Double HeightWidth180 { get { return sizeConvertAsPerDevice(180); } }
        public static Double HeightWidth300 { get { return sizeConvertAsPerDevice(300); } }

        #endregion

        #region View Spacing

        public static Double Spacing03 { get { return sizeConvertAsPerDevice(3); } }
        public static Double Spacing05 { get { return sizeConvertAsPerDevice(5); } }
        public static Double Spacing10 { get { return sizeConvertAsPerDevice(10); } }
        public static Double Spacing15 { get { return sizeConvertAsPerDevice(15); } }
        public static Double Spacing20 { get { return sizeConvertAsPerDevice(20); } }
        public static Double Spacing30 { get { return sizeConvertAsPerDevice(30); } }
        public static Double Spacing40 { get { return sizeConvertAsPerDevice(40); } }
        public static Double Spacing50 { get { return sizeConvertAsPerDevice(50); } }
        public static Double Spacing60 { get { return sizeConvertAsPerDevice(60); } }
        #endregion

        #region Corner Radius

        public static Single CornerRadius04 { get { return (float)sizeConvertAsPerDevice(4); } }
        public static Single CornerRadius10 { get { return (float)sizeConvertAsPerDevice(10); } }
        public static Single CornerRadius15 { get { return (float)sizeConvertAsPerDevice(15); } }
        public static Single CornerRadius20 { get { return (float)sizeConvertAsPerDevice(20); } }
        public static Single CornerRadius22 { get { return (float)sizeConvertAsPerDevice(22); } }
        public static Single CornerRadius25 { get { return (float)sizeConvertAsPerDevice(25); } }
        public static Single CornerRadius30 { get { return (float)sizeConvertAsPerDevice(30); } }
        public static Single CornerRadius40 { get { return (float)sizeConvertAsPerDevice(40); } }
        public static Single CornerRadius45 { get { return (float)sizeConvertAsPerDevice(45); } }
        public static Single CornerRadius50 { get { return (float)sizeConvertAsPerDevice(50); } }
        public static Single CornerRadius55 { get { return (float)sizeConvertAsPerDevice(55); } }
        public static Single CornerRadius60 { get { return (float)sizeConvertAsPerDevice(60); } }

        public static int ButtonCornerRadius05 { get { return (int)sizeConvertAsPerDevice(5); } }
        public static int ButtonCornerRadius10 { get { return (int)sizeConvertAsPerDevice(10); } }
        public static int ButtonCornerRadius25 { get { return (int)sizeConvertAsPerDevice(25); } }

        #endregion

        #region Grid Height Width
        public static GridLength GridHeightWidth1 { get { return sizeConvertAsPerDevice(1); } }
        public static GridLength GridHeightWidth3 { get { return sizeConvertAsPerDevice(3); } }
        public static GridLength GridHeightWidth10 { get { return sizeConvertAsPerDevice(10); } }
        public static GridLength GridHeightWidth20 { get { return sizeConvertAsPerDevice(20); } }
        public static GridLength GridHeightWidth30 { get { return sizeConvertAsPerDevice(30); } }
        public static GridLength GridHeightWidth40 { get { return sizeConvertAsPerDevice(40); } }
        public static GridLength GridHeightWidth50 { get { return sizeConvertAsPerDevice(50); } }
        public static GridLength GridHeightWidth60 { get { return sizeConvertAsPerDevice(60); } }
        public static GridLength GridHeightWidth80 { get { return sizeConvertAsPerDevice(80); } }
        public static GridLength GridHeightWidth100 { get { return sizeConvertAsPerDevice(100); } }
        public static GridLength GridHeightWidth120 { get { return sizeConvertAsPerDevice(120); } }
        public static GridLength GridHeightWidth130 { get { return sizeConvertAsPerDevice(130); } }
        public static GridLength GBCallGridHeight50 { get { return sizeConvertAsPerDevice(50); } }

        #endregion

        #region View Margin Padding

        public static Thickness MarginPadding02 { get { return MarginPaddingConvertAsPerDevice(02, 02, 02, 02); } }
        public static Thickness MarginPadding04 { get { return MarginPaddingConvertAsPerDevice(04, 04, 04, 04); } }
        public static Thickness MarginPadding05 { get { return MarginPaddingConvertAsPerDevice(05, 05, 05, 05); } }
        public static Thickness MarginPadding10 { get { return MarginPaddingConvertAsPerDevice(10, 10, 10, 10); } }
        public static Thickness MarginPadding12 { get { return MarginPaddingConvertAsPerDevice(12, 12, 12, 12); } }
        public static Thickness MarginPadding15 { get { return MarginPaddingConvertAsPerDevice(15, 15, 15, 15); } }
        public static Thickness MarginPadding20 { get { return MarginPaddingConvertAsPerDevice(20, 20, 20, 20); } }
        public static Thickness MarginPadding30 { get { return MarginPaddingConvertAsPerDevice(30, 30, 30, 30); } }

        public static Thickness MarginPadding00_05_00_00 { get { return MarginPaddingConvertAsPerDevice(00, 05, 00, 00); } }
        public static Thickness MarginPadding05_00_05_00 { get { return MarginPaddingConvertAsPerDevice(05, 00, 05, 00); } }
        public static Thickness MarginPadding05_10_05_10 { get { return MarginPaddingConvertAsPerDevice(05, 10, 05, 10); } }
        public static Thickness MarginPadding10_05_10_05 { get { return MarginPaddingConvertAsPerDevice(10, 05, 10, 05); } }
        public static Thickness MarginPadding10_00_10_00 { get { return MarginPaddingConvertAsPerDevice(10, 00, 10, 00); } }
        public static Thickness MarginPadding10_10_10_00 { get { return MarginPaddingConvertAsPerDevice(10, 10, 10, 00); } }
        public static Thickness MarginPadding10_00_10_10 { get { return MarginPaddingConvertAsPerDevice(10, 00, 10, 10); } }
        public static Thickness MarginPadding00_00_10_00 { get { return MarginPaddingConvertAsPerDevice(00, 00, 10, 00); } }
        public static Thickness MarginPadding00_10_00_10 { get { return MarginPaddingConvertAsPerDevice(00, 10, 00, 10); } }
        public static Thickness MarginPadding10_00_00_00 { get { return MarginPaddingConvertAsPerDevice(10, 00, 00, 00); } }
        public static Thickness MarginPadding00_00_00_10 { get { return MarginPaddingConvertAsPerDevice(00, 00, 00, 10); } }
        public static Thickness MarginPadding12_00_12_00 { get { return MarginPaddingConvertAsPerDevice(12, 00, 12, 00); } }

        public static Thickness MarginPadding15_15_15_10 { get { return MarginPaddingConvertAsPerDevice(15, 15, 15, 10); } }
        public static Thickness MarginPadding15_00_00_00 { get { return MarginPaddingConvertAsPerDevice(15, 00, 00, 00); } }
        public static Thickness MarginPadding15_00_15_00 { get { return MarginPaddingConvertAsPerDevice(15, 00, 15, 00); } }
        public static Thickness MarginPadding00_00_15_00 { get { return MarginPaddingConvertAsPerDevice(00, 00, 15, 00); } }
        public static Thickness MarginPadding15_00_15_10 { get { return MarginPaddingConvertAsPerDevice(15, 00, 15, 10); } }
        public static Thickness MarginPadding00_00_00_15 { get { return MarginPaddingConvertAsPerDevice(00, 00, 00, 15); } }

        public static Thickness MarginPadding20_00_20_00 { get { return MarginPaddingConvertAsPerDevice(20, 00, 20, 00); } }
        public static Thickness MarginPadding00_00_20_00 { get { return MarginPaddingConvertAsPerDevice(00, 00, 20, 00); } }
        public static Thickness MarginPadding00_20_00_00 { get { return MarginPaddingConvertAsPerDevice(00, 20, 00, 00); } }
        public static Thickness MarginPadding00_20_00_20 { get { return MarginPaddingConvertAsPerDevice(00, 20, 00, 20); } }
        public static Thickness MarginPadding20_20_20_00 { get { return MarginPaddingConvertAsPerDevice(20, 20, 20, 00); } }
        public static Thickness MarginPadding20_00_00_00 { get { return MarginPaddingConvertAsPerDevice(20, 00, 00, 00); } }
        public static Thickness MarginPadding00_00_00_20 { get { return MarginPaddingConvertAsPerDevice(00, 00, 00, 20); } }
        public static Thickness MarginPadding20_10_20_10 { get { return MarginPaddingConvertAsPerDevice(20, 10, 20, 10); } }
        public static Thickness MarginPadding20_10_20_00 { get { return MarginPaddingConvertAsPerDevice(20, 10, 20, 00); } }
        public static Thickness MarginPadding20_00_20_10 { get { return MarginPaddingConvertAsPerDevice(20, 00, 20, 10); } }
        public static Thickness MarginPadding20_05_10_05 { get { return MarginPaddingConvertAsPerDevice(20, 05, 10, 05); } }
        public static Thickness MarginPadding20_07_20_07 { get { return MarginPaddingConvertAsPerDevice(20, 07, 20, 07); } }

        public static Thickness MarginPadding30_00_00_00 { get { return MarginPaddingConvertAsPerDevice(30, 00, 00, 00); } }
        public static Thickness MarginPadding00_30_00_00 { get { return MarginPaddingConvertAsPerDevice(00, 30, 00, 00); } }
        public static Thickness MarginPadding00_30_00_30 { get { return MarginPaddingConvertAsPerDevice(00, 30, 00, 30); } }
        public static Thickness MarginPadding30_00_30_00 { get { return MarginPaddingConvertAsPerDevice(30, 00, 30, 00); } }
        public static Thickness MarginPadding30_10_30_10 { get { return MarginPaddingConvertAsPerDevice(30, 10, 30, 10); } }
        public static Thickness MarginPadding30_00_30_20 { get { return MarginPaddingConvertAsPerDevice(30, 00, 30, 20); } }
        public static Thickness MarginPadding30_20_30_00 { get { return MarginPaddingConvertAsPerDevice(30, 20, 30, 00); } }

        public static Thickness MarginPadding00_50_00_00 { get { return MarginPaddingConvertAsPerDevice(00, 50, 00, 00); } }

        public static Thickness MarginPadding60_15_60_15 { get { return MarginPaddingConvertAsPerDevice(60, 15, 60, 15); } }

        public static Thickness MarginPadding0_100_0_0 { get { return MarginPaddingConvertAsPerDevice(0, 100, 0, 0); } }
        public static Thickness MarginPadding0_120_0_0 { get { return MarginPaddingConvertAsPerDevice(0, 120, 0, 0); } }
        public static Thickness MarginPadding0_200_0_0 { get { return MarginPaddingConvertAsPerDevice(0, 200, 0, 0); } }

        public static Thickness MarginPaddingm5_00_00_00 { get { return MarginPaddingConvertAsPerDevice(-5, 00, 00, 00); } }
        public static Thickness MarginPadding0_m05_0_0 { get { return MarginPaddingConvertAsPerDevice(0, -5, 0, 0); } }
        public static Thickness MarginPadding0_m10_0_0 { get { return MarginPaddingConvertAsPerDevice(0, -10, 0, 0); } }
        public static Thickness MarginPadding0_m20_0_0 { get { return MarginPaddingConvertAsPerDevice(0, -20, 0, 0); } }
        public static Thickness MarginPaddingm20_0_0_0 { get { return MarginPaddingConvertAsPerDevice(-20, 0, 0, 0); } }
        public static Thickness MarginPadding0_0_m20_0 { get { return MarginPaddingConvertAsPerDevice(0, 0, -20, 0); } }
        public static Thickness MarginPadding30_m05_0_0 { get { return MarginPaddingConvertAsPerDevice(30, -5, 0, 0); } }

        public static Thickness MarginPadding20_m05_0_m05 { get { return MarginPaddingConvertAsPerDevice(20, -5, 0, -5); } }
        public static Thickness MarginPadding20_m07_0_m07 { get { return MarginPaddingConvertAsPerDevice(20, -7, 0, -7); } }
        public static Thickness MarginPadding0_0_0_m40 { get { return MarginPaddingConvertAsPerDevice(0, 0, 0, -40); } }

        public static Thickness MarginPadding0_m10_00_00 { get { return MarginPaddingConvertAsPerDevice(0, -10, 0, 0); } }
        public static Thickness MarginPadding0_m10_m20_m10 { get { return MarginPaddingConvertAsPerDevice(0, -10, -20, -10); } }
        public static Thickness MarginPaddingm20_m10_0_m10 { get { return MarginPaddingConvertAsPerDevice(-20, -10, 0, -10); } }

        public static Thickness MarginPadding15_0_0_0 { get { return MarginPaddingConvertAsPerDevice(15, 0, 0, 0); } }
        public static Thickness MarginPadding15_0_0_m100 { get { return MarginPaddingConvertAsPerDevice(15, 0, 0, -100); } }

        #endregion

        #region FlowList RowHeight 

        public static int RowHeight60 { get { return (int)sizeConvertAsPerDevice(60); } }

        #endregion

    }
}
