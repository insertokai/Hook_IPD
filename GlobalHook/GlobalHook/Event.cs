using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalHook
{
    class Event
    {
        public void EventHandler()
        {
            Process.Start("microsoft.windows.camera:");
        }
    }
}
