using System.ComponentModel;
using System.Runtime.Serialization;

namespace SuperSaiyan.Settings
{
    public class WarlockSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _attemptQuell;

        [DefaultValue(false)]
        public bool AttemptQuell
        {
            get
            {
                return _attemptQuell;
            }
            set
            {
                _attemptQuell = value;
                OnPropertyChanged("AttemptRepulse");
            }
        }

        private bool _useThrall;

        [DefaultValue(true)]
        public bool UseThrall
        {
            get
            {
                return _useThrall;
            }
            set
            {
                _useThrall = value;
                OnPropertyChanged("UseThrall");
            }
        }

        private int _superDelay;
        [DefaultValue(10)]
        public int SuperDelay
        {
            get
            {
                return _superDelay;
            }
            set
            {
                _superDelay = value;
                OnPropertyChanged("SuperDelay");
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