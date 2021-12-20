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
            int finalScore = 0;
            try
            {

                AppiumOptions options = new AppiumOptions();
                options.AddAdditionalCapability("app", path);
                options.AddAdditionalCapability("deviceName", "WindowsPC");


                var appSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);


                appSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

                var deskTopOptions = new AppiumOptions();
                deskTopOptions.AddAdditionalCapability("app", "Root");
                var deskTopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), deskTopOptions);


                appSession.Manage().Window.Maximize();
                var notePadFullScreenDimensi = appSession.Manage().Window.Size;

                appSession.Manage().Window.Size = new System.Drawing.Size { Height = 540, Width = 960 };
                deskTopSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);

                int totalScore = 0;
               
                int paneCount = 0;
                SnapToLocation(appSession, deskTopSession, "Snap Layouts Menu", MeasurementType.Half, out paneCount);
                var windowSize50 = appSession.Manage().Window.Size;
                var windowLocation50 = appSession.Manage().Window.Position;
                LogToConsole(windowSize50, windowLocation50, Constants.SnapLayoutLeftHalf50);

                bool isSixPaneLayout = paneCount == 6;
                if (IsAppDimensionValid(windowSize50, notePadFullScreenDimensi, MeasurementType.Half, isSixPaneLayout))
                    totalScore += 1;

                if (IsAppLocationValid(windowLocation50, notePadFullScreenDimensi, SnapLocation.Left))
                    totalScore += 1;

                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);



                SnapToLocation(appSession, deskTopSession, "3 window focus on left layout", MeasurementType.OneThird, out paneCount);
                var windowSizeThirdTop = appSession.Manage().Window.Size;
                var windowLocationThird = appSession.Manage().Window.Position;
                LogToConsole(windowSizeThirdTop, windowLocationThird, Constants.SnapLayoutTopRightQuarter);
                if (IsAppDimensionValid(windowSizeThirdTop, notePadFullScreenDimensi, MeasurementType.OneThird, isSixPaneLayout))
                    totalScore += 1;

                SnapLocation location = isSixPaneLayout ? SnapLocation.Center : SnapLocation.Left;
                if (IsAppLocationValid(windowLocationThird, notePadFullScreenDimensi, location))
                    totalScore += 1;

                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);

                SnapToLocation(appSession, deskTopSession, "4 window grid layout", MeasurementType.Quarter, out paneCount);
                var windowSizeQuarterBottom = appSession.Manage().Window.Size;
                var windowLocationQuarterBottom = appSession.Manage().Window.Position;
                if (IsAppDimensionValid(windowSizeQuarterBottom, notePadFullScreenDimensi, MeasurementType.Quarter, isSixPaneLayout))
                    totalScore += 1;


                if (IsAppLocationValid(windowLocationQuarterBottom, notePadFullScreenDimensi, SnapLocation.BottomRight))
                    totalScore += 1;
                LogToConsole(windowSizeQuarterBottom, windowLocationQuarterBottom, Constants.SnapLayoutBottomRightQuarter);

                if (totalScore == 6)
                    finalScore = 100;
                else if (totalScore >= 0 && totalScore < 6)
                    finalScore = 50;
                else
                    finalScore = 0;


                deskTopSession.CloseApp();
                appSession.CloseApp();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            return finalScore;
        }

        public static void SnapToLocation(WindowsDriver<WindowsElement> appSession,
            WindowsDriver<WindowsElement> deskTopSession, string snapLayoutParent, MeasurementType snapLayOutTarget, out int paneCount)
        {
            // notePadSession.CloseApp(); notePadSession.LaunchApp();
            WindowsElement maxButton;
            WindowsElement closeButton;
            try
            {
                closeButton = appSession.FindElementByAccessibilityId("Close");
            }
            catch
            {
                closeButton = appSession.FindElementByName("Close");
            }
            var action = new Actions(appSession);
            action.MoveToElement(closeButton).MoveByOffset(-closeButton.Size.Width, 0).Build().Perform();


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
            if (snapLayOutTarget == MeasurementType.OneThird)
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
                    if (Math.Abs(appSize.Width * 2 - fullScreenSize.Width) <= 20 && Math.Abs(appSize.Height - fullScreenSize.Height) <= 20)
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
                    if (!isSixPaneLayout && (Math.Abs(appSize.Width * 2 - fullScreenSize.Width) <= 20 && Math.Abs(appSize.Height - fullScreenSize.Height) <= 20))
                        result = true;
                    else if (isSixPaneLayout && (Math.Abs(appSize.Width * 3 - fullScreenSize.Width) <= 20 && (Math.Abs(appSize.Height - fullScreenSize.Height) <= 20)))
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
                    if (Math.Abs(appLocation.X - fullScreenSize.Width * 0.33) <= 20 && appLocation.Y == 0)
                        result = true;
                    else
                        result = false;
                    break;
                case SnapLocation.BottomRight:
                    if (Math.Abs(appLocation.X - fullScreenSize.Width * 0.5) <= 20 && Math.Abs(appLocation.Y - fullScreenSize.Height * 0.5) <= 20)
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
