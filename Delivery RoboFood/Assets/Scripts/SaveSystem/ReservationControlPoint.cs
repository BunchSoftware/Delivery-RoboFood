using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservationControlPoint : MonoBehaviour
{
    [SerializeField] private ReservationManager reservationManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && reservationManager != null)
        {
            reservationManager.AutoReservation();
        }
    }
}
