using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T instance;

    private void Awake()
    {
        if (instance==null)
        {
            instance = GetComponent<T>();
        }
        else
        {
            print("Double of type '" + typeof(T) + "' found, suppressing object " + gameObject.name+"...");
            Destroy(gameObject);
        }
    }
}
