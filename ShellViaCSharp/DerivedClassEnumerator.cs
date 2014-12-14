using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ShellViaCSharp
{
    class DerivedClassEnumerator<T>: IEnumerable<T> where T: class
    {
        public IEnumerator<T> GetEnumerableOfType<T>() where T : class
        {
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                yield return (T)Activator.CreateInstance(type);            
            }

        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerableOfType<T>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
