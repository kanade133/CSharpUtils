using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;

        public CommonEqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }
        public bool Equals(T x, T y)
        {
            return keySelector(x).Equals(keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return keySelector(obj).GetHashCode();
        }
    }
}
