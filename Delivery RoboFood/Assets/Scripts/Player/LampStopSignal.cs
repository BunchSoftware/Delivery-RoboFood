using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampStopSignal : MonoBehaviour
{
    [SerializeField] private List<GameObject> Ordinary;
    [SerializeField] private List<GameObject> Red;
    [SerializeField] private GameObject light;

    private void Start()
    {
        foreach (GameObject gameObject in Red)
            gameObject.SetActive(false);
        light.SetActive(false);

        foreach (GameObject gameObject in Ordinary)
            gameObject.SetActive(true);
    }

    public void ChangeActiveStopSignalLamp()
    {
        if (light.activeInHierarchy)
            light.SetActive(false);
        else
            light.SetActive(true);

        foreach (GameObject gameObject in Ordinary)
        {
            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in Red)
        {
            if(gameObject.activeInHierarchy)
                gameObject.SetActive(false);
            else 
                gameObject.SetActive(true);
        }
    }
}
