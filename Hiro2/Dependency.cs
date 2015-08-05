using System;

namespace Hiro2
{
    /// <summary>
    /// Represents a service dependency.
    /// </summary>
    public class Dependency : IDependency
    {
        /// <summary>
        /// Initializes a new instance of the Dependency class.
        /// </summary>
        /// <param name="dependencyType"> service type.</param>
        public Dependency(Type dependencyType)
            : this(dependencyType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Dependency class.
        /// </summary>
        /// <param name="dependencyType"> service type.</param>
        /// <param name="name">The service ServiceName.</param>
        public Dependency(Type dependencyType, string name)
        {
            this.Name = name;
            this.DependencyType = dependencyType;
        }

        /// <summary>
        /// Gets the value indicating the ServiceName of the service itself.
        /// </summary>
        /// <value>The service ServiceName.</value>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating the service type.
        /// </summary>
        /// <value>The service type.</value>
        public Type DependencyType { get; }

        /// <summary>
        /// Computes the hash code using the <see cref="Name"/> and <see cref="DependencyType"/>.
        /// </summary>
        /// <returns>The hash code value.</returns>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(this.Name))
                return this.DependencyType.GetHashCode();

            return this.DependencyType.GetHashCode() ^ this.Name.GetHashCode();
        }

        /// <summary>
        /// Determines whether or not the current object is equal to the <paramref ServiceName="obj">other object.</paramref>
        /// </summary>
        /// <param ServiceName="obj">The object that will be compared with the current object.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, it will return <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var dependency = obj as IDependency;
            if (obj == null || dependency == null)
                return false;

            if (string.IsNullOrEmpty(this.Name))
                return DependencyType == dependency.DependencyType;

            return DependencyType == dependency.DependencyType && Name == dependency.Name;
        }

        public int CompareTo(IDependency other)
        {
            var areEqual = (other.Name == this.Name && other.DependencyType == this.DependencyType);
            if (areEqual)
                return 0;

            var otherHash = other.GetHashCode();
            var thisHash = this.GetHashCode();

            var isGreater = otherHash > thisHash;

            return isGreater ? 1 : -1;
        }

        /// <summary>
        /// Displays the dependency as a string.
        /// </summary>
        /// <returns>A string that displays the contents of the current dependency.</returns>
        public override string ToString()
        {
            var serviceName = string.IsNullOrEmpty(this.Name) ? "{NoName}" : this.Name;
            return $"Service ServiceName: {serviceName}, ServiceType: {this.DependencyType.Name}";
        }
    }
}