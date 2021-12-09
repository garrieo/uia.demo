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


                //notePadSession.Manage().Window.Maximize();
                //var maxWindowAction = new Actions(notePadSession);
                //maxWindowAction.SendKeys(Keys.Command + Keys.ArrowUp + Keys.Command).Build().Perform();


                var notePadSource = notePadSession.PageSource;


                //var maxButton = notePadSession.FindElementByName("Maximize");
                ////var maxButton = notePadSession.FindElementByName("Restore");


                //var action = new Actions(notePadSession);
                //action.MoveToElement(maxButton).MoveByOffset(5, 5).Build().Perform();

                //System.Threading.Thread.Sleep(1000);

                //notePadSession.SwitchTo().Window(notePadSession.WindowHandles.First());
                var deskTopOptions = new AppiumOptions();
                deskTopOptions.AddAdditionalCapability("app", "Root");
                var deskTopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), deskTopOptions);


                SnapToLocation(notePadSession,deskTopSession, "", Constants.SnapLayoutLeftHalf50);
                var windowSize50 = notePadSession.Manage().Window.Size;
                var windowLocation50 = notePadSession.Manage().Window.Position;

                SnapToLocation(notePadSession, deskTopSession, "", Constants.SnapLayoutRight40);

                var windowSize60 = notePadSession.Manage().Window.Size;
                var windowLocation60 = notePadSession.Manage().Window.Position;
                /*   
                 *   notePadSession.Manage().Window.Position
                 *   notePadSession.Manage().Window.Maximize()
                 *   notePadSession.Manage().Window.Size
                 *   snapLayOutElement.Size;
                   snapLayOutElement.Rect;
                   snapLayOutElement.Location; 

                 */



                var twoWindowSplitGroupPosition = notePadSession.Manage().Window.Position;
                var twoWindowSplitGroupSize = notePadSession.Manage().Window.Size;

              

                //  snapLayoutAction.MoveToElement(twoWindowLeftLayoutGroup).Build().Perform();
                //  snapLayoutAction.MoveToElement(twoWindowSplitGroup).Build().Perform();

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
            var maxButton = notePadSession.FindElementByName("Maximize");
            ///var maxButton = notePadSession.FindElementByName("Restore");


            var action = new Actions(notePadSession);
            action.MoveToElement(maxButton).MoveByOffset(5, 5).Build().Perform();

           

            deskTopSession.CloseApp();         deskTopSession.LaunchApp();
            var popHostElement = deskTopSession.FindElementByName("PopupHost");
            var popUpElement = popHostElement.FindElementByName("Popup");
            var snapLayOutElement = popUpElement.FindElementByName(Constants.SnapLayOutMenu);


            var snapLayoutAction = new Actions(deskTopSession);

            //var twoWindowSplitGroup = snapLayOutElement.FindElementByName("2 window split layout");
            var twoWindowSplitGroupButton = snapLayOutElement.FindElementByName(snapLayOutTarget);

            snapLayoutAction.Click(twoWindowSplitGroupButton).Build().Perform();
        }
    }


}
