using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");    

        if (objs.Length > 1)
        {
            for (int i = 0; i < objs.Length - 1; i++)
            {
                Destroy(objs[i].gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
