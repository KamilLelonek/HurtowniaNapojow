using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
    public static class ControlsPlugins
    {
        public static void PerformClick(this Button button)
        {
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        public static void RebindContext(this FrameworkElement control, object data)
        {
            control.DataContext = null;
            control.DataContext = data;
        }
    }
}