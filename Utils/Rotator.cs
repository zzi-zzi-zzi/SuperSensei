using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSaiyan.Utils
{
    class Rotator<T> : LinkedList<T>
    {
        private T[] _base;

        public Rotator(params T[] v)
        {
            _base = v;
            Reset();
        }

        public T Get()
        {
            var x = First.Value;
            RemoveFirst();
            AddLast(x);
            return x;
        }



        internal void Reset()
        {
            Clear();
            foreach(var x in _base)
            {
                AddLast(x);
            }
        }
    }
}
