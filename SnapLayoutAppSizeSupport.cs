using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uia_test_console
{
    public class SnapLayoutAppSizeSupport
    {
        public static int launchApplication(string path)
        {
            try
            {
                
                AppiumOptions options = new AppiumOptions();
                options.AddAdditionalCapability("app", path);


                var notePadSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);


                notePadSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2000);
                //var maxWindowAction = new Actions(notePadSession);
                //maxWindowAction.SendKeys(Keys.Command + Keys.ArrowUp + Keys.Command).Build().Perform();


                //var maxButton = notePadSession.FindElementByName("Maximize");
                ////var maxButton = notePadSession.FindElementByName("Restore");

                var deskTopOptions = new AppiumOptions();
                deskTopOptions.AddAdditionalCapability("app", "Root");
                var deskTopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), deskTopOptions);


                notePadSession.Manage().Window.Maximize();
                var notePadFullScreenDimensi = notePadSession.Manage().Window.Size;

                notePadSession.Manage().Window.Size = new System.Drawing.Size { Height = 540, Width = 960 };
                deskTopSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2000);


                int paneCount = 0;
                SnapToLocation(notePadSession, deskTopSession, "Snap Layouts Menu", MeasurementType.Half, out paneCount);
                var windowSize50 = notePadSession.Manage().Window.Size;
                var windowLocation50 = notePadSession.Manage().Window.Position;
                LogToConsole(windowSize50, windowLocation50, Constants.SnapLayoutLeftHalf50);

                bool isSixPaneLayout = paneCount == 6;
                var isHalfSizeValid = IsAppDimensionValid(windowSize50, notePadFullScreenDimensi, MeasurementType.Half, isSixPaneLayout);
                var isHalfLocationValid = IsAppLocationValid(windowLocation50, notePadFullScreenDimensi, SnapLocation.Left);
                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);

                                               

                SnapToLocation(notePadSession, deskTopSession, "3 window focus on left layout", MeasurementType.OneThird, out paneCount);
                var windowSizeThirdTop = notePadSession.Manage().Window.Size;
                var windowLocationThird = notePadSession.Manage().Window.Position;
                LogToConsole(windowSizeThirdTop, windowLocationThird, Constants.SnapLayoutTopRightQuarter);
                var isThirdSizeValid = IsAppDimensionValid(windowSizeThirdTop, notePadFullScreenDimensi, MeasurementType.OneThird, isSixPaneLayout);
                SnapLocation location = isSixPaneLayout ? SnapLocation.Center : SnapLocation.Left;
                var isThirdLocationValid = IsAppLocationValid(windowLocationThird, notePadFullScreenDimensi,location);

                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);

                SnapToLocation(notePadSession, deskTopSession, "4 window grid layout",MeasurementType.Quarter, out paneCount);
                var windowSizeQuarterBottom = notePadSession.Manage().Window.Size;
                var windowLocationQuarterBottom = notePadSession.Manage().Window.Position;
                var iQuarterSizeValid = IsAppDimensionValid(windowSizeQuarterBottom, notePadFullScreenDimensi, MeasurementType.Quarter, isSixPaneLayout);
                var isQuarterLocationValid = IsAppLocationValid(windowLocationQuarterBottom, notePadFullScreenDimensi, SnapLocation.BottomRight);
                LogToConsole(windowSizeQuarterBottom, windowLocationQuarterBottom, Constants.SnapLayoutBottomRightQuarter);

                //notePadSession.FindElementByName("Maximize").Click();


                deskTopSession.CloseApp(); deskTopSession.LaunchApp();
                var deskTopSize = deskTopSession.Manage();//.Window.Size;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            return 0;
        }

        public static void SnapToLocation(WindowsDriver<WindowsElement> notePadSession,
            WindowsDriver<WindowsElement> deskTopSession, string snapLayoutParent, MeasurementType snapLayOutTarget, out int paneCount)
        {
            // notePadSession.CloseApp(); notePadSession.LaunchApp();
            var maxButton = notePadSession.FindElementByName("Maximize");



            var action = new Actions(notePadSession);
            //action.SendKeys(Keys.Command + "z" + Keys.Command).Build().Perform();
            action.MoveToElement(maxButton).MoveByOffset(5, 5).Build().Perform();


            System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);

            deskTopSession.CloseApp(); deskTopSession.LaunchApp();

            var popHostElement = deskTopSession.FindElementByName("PopupHost");
            var popUpElement = popHostElement.FindElementByName("Popup");
            var snapLayOutMenuElement = popUpElement.FindElementByName(Constants.SnapLayOutMenu);


            var collection = snapLayOutMenuElement.FindElementsByClassName("NamedContainerAutomationPeer");
            paneCount = collection.Count();

            AppiumWebElement snapLayOutButton = null;
            var snapLayoutActionMove = new Actions(deskTopSession);
            var snapLayoutActionClick = new Actions(deskTopSession);

            if (snapLayOutTarget == MeasurementType.Half)
            {
                var parent = snapLayOutMenuElement.FindElementByName(snapLayoutParent);
                snapLayOutButton = parent.FindElementByName(Constants.SnapLayoutLeftHalf50);
            }
            if(snapLayOutTarget == MeasurementType.OneThird)
            {
                if (collection.Count == 6)
                {
                    snapLayoutParent = "3 window split layout";
                    var parent = snapLayOutMenuElement.FindElementByName(snapLayoutParent);
                    snapLayOutButton = parent.FindElementByName(Constants.SnapLayoutOneThird);
                }
                else
                {
                    snapLayoutParent = "3 window focus on left layout";
                    var parent = snapLayOutMenuElement.FindElementByName(snapLayoutParent);
                    snapLayOutButton = parent.FindElementByName(Constants.SnapLayoutLeftHalf50);
                }
            }
            if (snapLayOutTarget == MeasurementType.Quarter)
            {
                var parent = snapLayOutMenuElement.FindElementByName(snapLayoutParent);
                snapLayOutButton = parent.FindElementByName(Constants.SnapLayoutBottomRightQuarter);
            }


            snapLayoutActionMove.MoveToElement(snapLayOutButton).Build().Perform();
            snapLayoutActionClick.Click(snapLayOutButton).Build().Perform();
        }


        private static void LogToConsole(System.Drawing.Size size, System.Drawing.Point location, string element)
        {
            Console.WriteLine($"SnapType [{element}] *** Size(w, h) = {size.Width}-{size.Height}  ***  Location(x, y) = {location.X}-{location.Y}");
            Console.WriteLine();
        }

        private static bool IsAppDimensionValid(System.Drawing.Size appSize, System.Drawing.Size fullScreenSize, MeasurementType measurementType, bool isSixPaneLayout)
        {
            bool result;
            switch (measurementType)
            {
                case MeasurementType.Half:
                    if (Math.Abs(appSize.Width * 2 - fullScreenSize.Width) <=20 && Math.Abs(appSize.Height - fullScreenSize.Height) <= 20)
                        result = true;
                    else
                        result = false;
                    break;
                case MeasurementType.Quarter:
                    if (Math.Abs(appSize.Width * 2 - fullScreenSize.Width) <= 20 && Math.Abs(appSize.Height * 2 - fullScreenSize.Height) <= 20)
                        result = true;
                    else
                        result = false;
                    break;
                case MeasurementType.OneThird:
                    if (!isSixPaneLayout && (Math.Abs(appSize.Width * 2 - fullScreenSize.Width) <= 20 && Math.Abs(appSize.Height - fullScreenSize.Height) <=20))
                        result = true;
                    else if(isSixPaneLayout && (Math.Abs(appSize.Width * 0.33 - fullScreenSize.Width) <=20 && (Math.Abs(appSize.Height - fullScreenSize.Height) <=20)))
                        result = true;
                    else
                        result = false;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        private static bool IsAppLocationValid(System.Drawing.Point appLocation, System.Drawing.Size fullScreenSize, SnapLocation snapLocation)
        {
            bool result;
            switch (snapLocation)
            {
                case SnapLocation.Left:
                    if (appLocation.X == 0 && appLocation.Y == 0)
                        result = true;
                    else
                        result = false;
                    break;
                case SnapLocation.Center:
                    if (Math.Abs(appLocation.X * 0.33 - fullScreenSize.Width) <= 20 && appLocation.Y == 0)
                        result = true;
                    else
                        result = false;
                    break;
                case SnapLocation.BottomRight:
                    if (Math.Abs(appLocation.X * 1.5 - fullScreenSize.Width) <=20 && Math.Abs(appLocation.Y * 1.5 - fullScreenSize.Height) <= 20)
                        result = true;
                    else
                        result = false;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
    }


}
