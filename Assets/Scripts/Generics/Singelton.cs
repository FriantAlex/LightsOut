using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Generics.Singelton
{

}
public class Singelton<T> : MonoBehaviour where T : Singelton<T>
{
    private static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                // Get an array of all objects of type to check if singelton pattern is valid
                T[] singeltons = FindObjectsOfType(typeof(T)) as T[];
                if (singeltons.Length != 0)
                {
                    // if only one is found we have a valid singelton pattern.
                    if (singeltons.Length == 1)
                    {
                        Debug.Log($"Instance found for type of {typeof(T).Name}");
                        m_instance = singeltons[0];
                        return m_instance;
                    }
                    else
                    {
                        Debug.LogError("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying all extra copies");
                        singeltons.ToList().RemoveAt(0); // remove the first element as this should be the only valid one
                        foreach (T extra in singeltons)
                        {
                            Destroy(extra.gameObject);
                        }
                    }
                }
            }
            return m_instance;
        }
        set
        {
            m_instance = value as T;
        }
    }

    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        Debug.Log($"Init for {typeof(T).Name}");
        DontDestroyOnLoad(gameObject);
    }
}
