using System;

namespace Hiro2
{
    public interface IDependency
    {
        string Name { get; }
        Type DependencyType { get; }
    }
}