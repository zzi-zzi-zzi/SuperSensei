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

        private bool _attemptRepulse;
        [DefaultValue(false)]
        public bool AttemptRepulse
        {
            get
            {
                return _attemptRepulse;
            }
            set
            {
                _attemptRepulse = value;
                OnPropertyChanged("AttemptRepulse");
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