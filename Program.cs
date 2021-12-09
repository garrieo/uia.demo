using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows;

namespace uia_test_console
{
    class Program
    {
        static void Main(string[] args)
        {


            
            var snapResult = SnapLayoutAppSizeSupport.launchApplication(@"C:\Windows\System32\notepad.exe");

            var process = Process.Start("notepad.exe");
            var id = process.Id;
                int ct = 0;
            AutomationElement msPaintAutomationElement = null;
            while (process.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(100);
               // process.Refresh();
            }

            msPaintAutomationElement = AutomationElement.FromHandle(process.MainWindowHandle);
            if (msPaintAutomationElement != null)
            {
                
                var pattern = (WindowPattern)msPaintAutomationElement.GetCurrentPattern(WindowPattern.Pattern);
              //  pattern.SetWindowVisualState(WindowVisualState.Maximized);

                var rectMain = (System.Windows.Rect)msPaintAutomationElement.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                var titleBar =  msPaintAutomationElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TitleBar));
                var rectTitleBar = (System.Windows.Rect)titleBar.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                
                
                var buttonList = titleBar.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));

                var maxButton = buttonList.Cast<AutomationElement>().ToList().FirstOrDefault(x => x.Current.Name == "Maximize");

                var cursor = new Cursor(Cursor.Current.Handle);
                var maxButtonRectangle = (System.Windows.Rect)maxButton.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);


                Cursor.Position = new System.Drawing.Point((int)maxButtonRectangle.X + 2, (int)maxButtonRectangle.Y + 2);

                var comboCacheRequest = new CacheRequest();
                comboCacheRequest.Add(SelectionPattern.Pattern);
                comboCacheRequest.Add(SelectionPattern.SelectionProperty);
                comboCacheRequest.Add(AutomationElement.NameProperty);
                comboCacheRequest.TreeScope = TreeScope.Element | TreeScope.Descendants;

                msPaintAutomationElement = msPaintAutomationElement.GetUpdatedCache(comboCacheRequest);
               var panels = msPaintAutomationElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane));
                double height = rectTitleBar.Height;
                double h = rectTitleBar.Bottom - rectTitleBar.Top;
                var titleBarName = titleBar.Current.Name;
                // titleBar.Current.n
            }
        }
    }
}
