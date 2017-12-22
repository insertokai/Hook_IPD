using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace GlobalHook
{
    class HookMonitor
    {
        private readonly List<Keys> _keysBuff = new List<Keys>();
        private readonly Event _event = new Event();
        private readonly Keys[] _eventKeyses = {Keys.Tab, Keys.E};
        private readonly Keys[] _showKeyses = {Keys.Tab, Keys.Q};
        private Logger _logger;
        private HookForm _hookForm;
        private IKeyboardMouseEvents _mGlobalHook;

        public void Init(HookForm hookForm, SettingAdapter adapter)
        {
            _logger = new Logger(adapter);
            _hookForm = hookForm;
            _mGlobalHook = Hook.GlobalEvents();
            _mGlobalHook.KeyDown += KeyDownHandler;
            _mGlobalHook.KeyUp += KeyUpHandler;
            _mGlobalHook.MouseDown += MouseDownHandler;
            _mGlobalHook.MouseUp += MouseUpHandler;

        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (_keysBuff.Contains(e.KeyCode)) return;
            _keysBuff.Add(e.KeyCode);
            _logger.LogKey(e.KeyCode.ToString(), true);
            CheckShortcuts();
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            _keysBuff.Remove(e.KeyCode);
            _logger.LogKey(e.KeyCode.ToString(), false);
        }

        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            _logger.LogMouse(e.X, e.Y, e.Button.ToString(), true);
        }

        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            _logger.LogMouse(e.X, e.Y, e.Button.ToString(), false);
        }

        private void CheckShortcuts()
        {
            var hits = _showKeyses.Count(key => _keysBuff.Contains(key));
            if(hits == _showKeyses.Length) _hookForm.ShowApp();
            hits = _eventKeyses.Count(key => _keysBuff.Contains(key));
            if (hits == _eventKeyses.Length) _event.EventHandler();
        }
    }
}
