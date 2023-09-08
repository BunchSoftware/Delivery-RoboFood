using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntryPoint : MonoBehaviour, IInitializable, IDisposable
{
    public abstract void Initialize();
    public abstract void Dispose();

    ~EntryPoint()
    {
        Dispose();
    }
}
