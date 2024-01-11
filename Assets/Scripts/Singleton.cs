using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<T> ();
                if ( instance == null )
                {
                    GameObject obj = new GameObject ();
                    obj.name = typeof ( T ).Name;
                    instance = obj.AddComponent<T> ();
                }
            }
            return instance;
        }
    }
    
    private static T instance;

    protected virtual void Awake ()
    {
        if ( instance == null )
        {
            instance = this as T;
            Init();
        }
        else
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
    
    protected virtual void Init (){}
}