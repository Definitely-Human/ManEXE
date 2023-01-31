using UnityEngine;

namespace ManExe.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        private static bool _mApplicationIsQuitting = false;

        public static T GetInstance()
        {
            //if (_mApplicationIsQuitting) { return null; }

            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }

        /* IMPORTANT!!! To use Awake in a derived class you need to do it this way
     * protected override void Awake()
     * {
     *     base.Awake();
     *     //Your code goes here
     * }
     * */

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this as T)
            {
                Destroy(gameObject);
            }
            else { DontDestroyOnLoad(gameObject); }
        }

        private void OnApplicationQuit()
        {
            _mApplicationIsQuitting = true;
        }
    }
}