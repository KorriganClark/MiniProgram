using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class Singleton<T> where T : class, new() 
    {
        public static T instance = null;
        public Singleton() { }

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