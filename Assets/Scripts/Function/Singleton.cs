using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    protected static T instance;
    public static T Instance
    {
        get
        {

            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(T)) as T;
            }

            return instance;
        }
    }
}
