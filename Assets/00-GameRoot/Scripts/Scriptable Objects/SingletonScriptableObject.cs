using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T _instance = null;
    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                if(results.Length == 0)
                {
                    Debug.Log("Singleton not created");
                    return null;
                }
                if (results.Length > 1)
                {
                    Debug.Log("There are multiple singletons of Game Data");
                    return null;
                }

                _instance = results[0];
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }

            return _instance;
        }
    }

   
}
