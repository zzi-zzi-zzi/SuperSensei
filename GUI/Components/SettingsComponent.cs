using SuperSensei.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSensei.GUI.Components
{
    class SettingsComponent
    {
        private SuperSettings _settings;

        public SettingsComponent()
        {
            _settings = SuperSettings.Instance;
        }

        public WarlockSettings Warlock
        {
            get
            {
                return _settings.Warlock;
            }
        }

    }
}
