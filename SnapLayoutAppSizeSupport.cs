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


                var maxButton = notePadSession.FindElementByName("Maximize");
                //var maxButton = notePadSession.FindElementByName("Restore");


                var action = new Actions(notePadSession);
                action.MoveToElement(maxButton).MoveByOffset(5, 5).Build().Perform();

                //System.Threading.Thread.Sleep(1000);

                //notePadSession.SwitchTo().Window(notePadSession.WindowHandles.First());
                var deskTopOptions = new AppiumOptions();
                deskTopOptions.AddAdditionalCapability("app", "Root");
                var deskTopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), deskTopOptions);

                var popHostElement = deskTopSession.FindElementByName("PopupHost");
                var popUpElement = popHostElement.FindElementByName("Popup");
                var snapLayOutElement = popUpElement.FindElementByName("Snap Layouts Menu");

                var windowSize = notePadSession.Manage().Window.Size;
                /*   
                 *   notePadSession.Manage().Window.Position
                 *   notePadSession.Manage().Window.Maximize()
                 *   notePadSession.Manage().Window.Size
                 *   snapLayOutElement.Size;
                   snapLayOutElement.Rect;
                   snapLayOutElement.Location; 

                 */

                var snapLayoutAction = new Actions(deskTopSession);

                var twoWindowSplitGroup = snapLayOutElement.FindElementByName("2 window split layout");
                var twoWindowSplitGroupButton = snapLayOutElement.FindElementByName("Left half, 50% width, 100% height");
                snapLayoutAction.Click(twoWindowSplitGroupButton).Build().Perform();

                var twoWindowLeftLayoutGroup = snapLayOutElement.FindElementByName("2 window focus on left layout");


                snapLayoutAction.MoveToElement(twoWindowLeftLayoutGroup).Build().Perform();


                snapLayoutAction.MoveToElement(twoWindowSplitGroup).Build().Perform();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
         
            return 0;
        }


    }
}
