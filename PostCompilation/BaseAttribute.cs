using System;

namespace PostCompilation
{
    public abstract class BaseAttribute : Attribute
    {
        public abstract void BeforeMethodAction();
        public abstract void AfterMethodAction();
    }
}
