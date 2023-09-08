using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BootstrapEntryPoint : MonoBehaviour
{
    [SerializeField] private List<EntryPoint> entryPoints = new List<EntryPoint>();

    private void Start()
    {
        foreach (EntryPoint entryPoint in entryPoints)
        {
            entryPoint.Initialize();
        }
    }
}
