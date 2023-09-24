using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayEntryPoint : EntryPoint
{
    [SerializeField] private CarController carController;

    public override void Dispose()
    {
        
    }

    public override void Initialize()
    {
        carController.Initialize();
    }
}
