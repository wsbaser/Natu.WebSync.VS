using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynSpike.SessionWeb.Models {
    public interface IComponentInstance : ICodeModelWithId {
        string FieldName { get; }
        string Name { get; }
        Scss.Scss RootSelector { get; }
        string ComponentType { get; }
        IEnumerable<string> ConstructorParams { get; }
    }

    public class ComponentInstanceComparer : IEqualityComparer<IComponentInstance>
    {
        public bool Equals(IComponentInstance x, IComponentInstance y)
        {
            //Check whether the objects are the same object. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether the properties are equal. 
            return x != null && y != null &&
                   string.Equals(x.FieldName, y.FieldName) &&
                   string.Equals(x.Name, y.Name) &&
                   Scss.Scss.Equals(x.RootSelector, y.RootSelector) &&
                   string.Equals(x.ComponentType, y.ComponentType) &&
                   x.ConstructorParams.SequenceEqual(y.ConstructorParams);
        }

        public int GetHashCode(IComponentInstance obj)
        {
            return obj.GetHashCode();
        }
    }

}