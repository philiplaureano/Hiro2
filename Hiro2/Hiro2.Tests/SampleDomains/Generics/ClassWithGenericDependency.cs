using System.Collections.Generic;

namespace Hiro2.Tests.SampleDomains.Generics
{
    public class ClassWithGenericDependency
    {
        private ICollection<string> _items;

        public ClassWithGenericDependency(ICollection<string> items)
        {
            _items = items;
        }
    }
}