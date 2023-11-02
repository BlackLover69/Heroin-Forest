using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            // If this is the first instance, mark it as created and do not destroy it
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            // If a duplicate is found, destroy it
            Destroy(this.gameObject);
        }
    }
}
