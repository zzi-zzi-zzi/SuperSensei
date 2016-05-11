using Buddy.Common;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SuperSaiyan.Settings
{
    public class SuperSettings : JsonSettings, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        private static SuperSettings _instance;
        [JsonIgnore]
        internal static SuperSettings Instance { get { return _instance ?? (_instance = new SuperSettings()); } }

        public SuperSettings() : base(GetSettingsFilePath(SettingsPath, "SuperSaiyan.json"))
        {
        }

        private WarlockSettings _warlock;
        public WarlockSettings Warlock
        {
            get
            {
                return _warlock ?? (_warlock = new WarlockSettings());
            }
            set
            {
                _warlock = value;
                OnPropertyChanged("Warlock");
            }
        }


        /// <summary>
        /// Called when property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }


}