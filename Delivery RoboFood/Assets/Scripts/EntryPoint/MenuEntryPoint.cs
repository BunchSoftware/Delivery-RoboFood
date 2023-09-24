using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuEntryPoint : EntryPoint
{
    [SerializeField] private CarCanvas carCanvas;

    public override void Dispose()
    {
        
    }

    public override void Initialize()
    {
        carCanvas.Initialize();
    }
}
