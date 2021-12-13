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

namespace uia_test_console
{
    public class SnapLayoutAppSizeSupport
    {
        public static int launchApplication(string path)
        {
            try
            {
                AppiumOptions options = new AppiumOptions();
                options.AddAdditionalCapability("app", @"C:\Windows\System32\notepad.exe");

                var notePadSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);


               notePadSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2000);
                //var maxWindowAction = new Actions(notePadSession);
                //maxWindowAction.SendKeys(Keys.Command + Keys.ArrowUp + Keys.Command).Build().Perform();


                //var maxButton = notePadSession.FindElementByName("Maximize");
                ////var maxButton = notePadSession.FindElementByName("Restore");

                var deskTopOptions = new AppiumOptions();
                deskTopOptions.AddAdditionalCapability("app", "Root");
                var deskTopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), deskTopOptions);


                deskTopSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2000);


                SnapToLocation(notePadSession, deskTopSession, "", Constants.SnapLayoutLeftHalf50);
                var windowSize50 = notePadSession.Manage().Window.Size;
                var windowLocation50 = notePadSession.Manage().Window.Position;
                LogToConsole(windowSize50, windowLocation50, Constants.SnapLayoutLeftHalf50);

                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);


                SnapToLocation(notePadSession, deskTopSession, "", Constants.SnapLayoutRight40);
                var windowSize60 = notePadSession.Manage().Window.Size;
                var windowLocation60 = notePadSession.Manage().Window.Position;
                LogToConsole(windowSize60, windowLocation60, Constants.SnapLayoutRight40);
                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);

                SnapToLocation(notePadSession, deskTopSession, "3 window focus on left layout", Constants.SnapLayoutTopRightQuarter);
                var windowSizequarterTop = notePadSession.Manage().Window.Size;
                var windowLocationquarterTop = notePadSession.Manage().Window.Position;
                LogToConsole(windowSizequarterTop, windowLocationquarterTop, Constants.SnapLayoutTopRightQuarter);

                System.Threading.Thread.Sleep(Constants.TimeBetweenSnaps);

                SnapToLocation(notePadSession, deskTopSession, "4 window grid layout", Constants.SnapLayoutBottomRightQuarter);
                var windowSizeQuarterBottom = notePadSession.Manage().Window.Size;
                var windowLocationQuarterBottom = notePadSession.Manage().Window.Position;
                LogToConsole(windowSizeQuarterBottom, windowLocationQuarterBottom, Constants.SnapLayoutBottomRightQuarter);

                notePadSession.FindElementByName("Maximize").Click();

                var notePadFullScreenDimensi = notePadSession.Manage().Window.Size;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            return 0;
        }

        public static void SnapToLocation(WindowsDriver<WindowsElement> notePadSession,
            WindowsDriver<WindowsElement> deskTopSession, string snapLayoutParent, string snapLayOutTarget)
        {
           // notePadSession.CloseApp(); notePadSession.LaunchApp();
            var maxButton = notePadSession.FindElementByName("Maximize");

            var action = new Actions(notePadSession);
            action.MoveToElement(maxButton).MoveByOffset(5, 5).Build().Perform();



            deskTopSession.CloseApp(); deskTopSession.LaunchApp();

            var popHostElement = deskTopSession.FindElementByName("PopupHost");
            var popUpElement = popHostElement.FindElementByName("Popup");
            var snapLayOutElement = popUpElement.FindElementByName(Constants.SnapLayOutMenu);

            AppiumWebElement snapLayOutButton;
            var snapLayoutActionMove = new Actions(deskTopSession);
            var snapLayoutActionClick = new Actions(deskTopSession);

            if (String.IsNullOrEmpty(snapLayoutParent))
                snapLayOutButton = snapLayOutElement.FindElementByName(snapLayOutTarget);
            else
            {
                var parent = snapLayOutElement.FindElementByName(snapLayoutParent);
                snapLayOutButton = parent.FindElementByName(snapLayOutTarget);
            }


            snapLayoutActionMove.MoveToElement(snapLayOutButton).Build().Perform();
            snapLayoutActionClick.Click(snapLayOutButton).Build().Perform();
        }


        private static void LogToConsole(System.Drawing.Size size, System.Drawing.Point location, string element)
        {
            Console.WriteLine($"SnapType [{element}] *** Size(w, h) = {size.Width}-{size.Height}  ***  Location(x, y) = {location.X}-{location.Y}");
            Console.WriteLine();
        }
    }


}
