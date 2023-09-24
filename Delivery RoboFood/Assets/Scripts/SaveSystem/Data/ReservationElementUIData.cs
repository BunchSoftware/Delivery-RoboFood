using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReservationElementUIData :IDisposable
{
    public int indexReservationElementUI = -1;
    public bool isFull = false;
    public string dateCreation = "";
    public string path = "";

    public void Dispose()
    {
        indexReservationElementUI = -1;
        isFull = false;
        dateCreation = "";
        path = "";
    }

    ~ReservationElementUIData()
    {
        Dispose();
    }
}
