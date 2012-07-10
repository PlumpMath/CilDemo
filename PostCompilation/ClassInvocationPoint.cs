using System;

namespace PostCompilation
{
    [Flags]
    public enum ClassInvocationPoint
    {
        None = 0x000,
        PrivateConstructors = 0x001,
        ProtectedConstructors = 0x002,
        PublicConstructors = 0x004,
        PrivateMethods = 0x008,
        ProtectedMethods = 0x010,
        PublicMethods = 0x020,
        PrivateProperties = 0x040,
        ProtectedProperties = 0x080,
        PublicProperties = 0x100,
        AllConstructors = 0x007,
        AllMethods = 0x038,
        AllProperties = 0x1C0,
        All = 0x1FF
    }
}
