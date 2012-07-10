using System;
using PostCompilation.Extensions;

namespace PostCompilation.Configuration
{
    sealed class AppSettingsProperty<TValue> : AppSettingsPropertyBase
    {
        private readonly string key;
        private TValue value;
        private readonly TValue defaultValue;



        public TValue Value
        {
            get
            {
                if (value.IsDefault())
                {
                    var settings = AppSettings.Settings;
                    try
                    {
                        var configValue = settings[key].Value;
                        value = configValue.ChangeType<TValue>();
                    }
                    catch
                    {
                        value = defaultValue;
                    }
                }

                return value;
            }
        }



        public AppSettingsProperty(string key, TValue defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (defaultValue.IsDefault())
            {
                throw new ArgumentNullException("defaultValue");
            }

            this.key = key;
            value = default(TValue);
            this.defaultValue = defaultValue;
        }
    }
}
