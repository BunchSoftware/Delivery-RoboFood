using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageReservation : MonoBehaviour
{
    [SerializeField][HideInInspector] public List<ReservationElementUI> reservationElements;

    public void Initialization()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            reservationElements.Add(transform.GetChild(i).GetComponent<ReservationElementUI>());
        }
    }
}
