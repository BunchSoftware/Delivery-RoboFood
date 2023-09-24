using System;
using System.Collections.Generic;

[Serializable]
public class ApplicationData
{
    public List<ReservationElementUIData> reservationElementUIData;
    public PlayerData playerData;
    public int currentReservationElementUIData = -1;
    public bool isFirstStart = true;

    public ApplicationData()
    {
        reservationElementUIData = new List<ReservationElementUIData>();
    }
}
