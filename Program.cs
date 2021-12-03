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

            var process = Process.Start("notepad.exe");
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
                pattern.SetWindowVisualState(WindowVisualState.Maximized);

                var rectMain = (System.Windows.Rect)msPaintAutomationElement.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                var titleBar =  msPaintAutomationElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TitleBar));
                var rectTitleBar = (System.Windows.Rect)titleBar.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                
                double height = rectTitleBar.Height;

                double h = rectTitleBar.Bottom - rectTitleBar.Top;
                var titleBarName = titleBar.Current.Name;
                // titleBar.Current.n
            }
        }
    }
}
