using System.Security;

namespace ADPhotoPrep.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {
        public bool Dirty { get; private set; }

        private SecureString _password = null;

        public string SecurePassword
        {
            get
            {
                if (_password == null)
                {
                    if (string.IsNullOrEmpty(Password))
                        return null;

                    var v = Password.DecryptString();
                    if (v == null || v.Length == 0)
                        return null;
                    _password = v;
                }
                return _password.ToInsecureString();
            }
            set
            {
                if (value == null)
                {
                    _password = null;
                    Password = null;
                }
                else
                {
                    _password = value.ToSecureString();
                    Password = _password.EncryptString();
                }
            }
        }

        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
            Dirty = true;
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            Dirty = false;
        }

        public static void AutoUpgrade()
        {
            var conf = Default;
            if (conf.UpgradeNeeded)
            {
                conf.Upgrade();
                conf.Save();
                conf.Reload();
                if (conf.UpgradeNeeded)
                {
                    conf.UpgradeNeeded = false;
                    conf.Save();
                }
            }
        }
    }
}
