using System.Collections;
using System.Collections.Generic;

namespace Hiro2.Tests.SampleDomains.ConstructorResolution
{
    public class ClassWithMultipleResolvableConstructors : ISampleInterface
    {
        public ClassWithMultipleResolvableConstructors(ICollection<string> arg1)
        {
        }
        public ClassWithMultipleResolvableConstructors(ICollection<string> arg1, ICollection<string> arg2)
        {            
        }
    }
}