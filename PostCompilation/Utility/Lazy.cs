using System;

namespace PostCompilation.Utility
{
    sealed class Lazy<TValue>
    {
        private readonly Func<TValue> initializer; 

        public bool IsInitialized { get; private set; }

        private TValue value;
        public TValue Value
        {
            get
            {
                if (!IsInitialized)
                {
                    value = initializer();
                    IsInitialized = true;
                }

                return value;
            }
            set
            {
                IsInitialized = true;
                this.value = value;
            }
        }



        public Lazy(Func<TValue> initializer)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException("initializer");
            }

            this.initializer = initializer;
            IsInitialized = false;
            value = default(TValue);
        }
    }
}
