using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonShared
{
    public abstract class LightSingleton<T> where T : new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
