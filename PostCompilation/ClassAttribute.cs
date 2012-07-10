using System;

namespace PostCompilation
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class ClassAttribute : BaseAttribute
    {
        public ClassInvocationPoint ClassInvocationPoint { get; set; }
        public MethodInvocationPoint MethodInvocationPoint { get; set; }

        protected ClassAttribute()
        {
            ClassInvocationPoint = ClassInvocationPoint.All;
            MethodInvocationPoint = MethodInvocationPoint.MethodBoundary;
        }
    }
}
