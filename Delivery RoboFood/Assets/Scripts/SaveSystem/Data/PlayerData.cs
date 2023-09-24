using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int level;
    public int countCard;
    public int countAnswer;
    public float[] position;

    public PlayerData(Player player)    
    {
        position = new float[3]
        {
        player.transform.localPosition.x,
        player.transform.localPosition.y,
        player.transform.localPosition.z,
        };
    }

    public void FillDataToPlayer(Player player)
    {
        player.transform.position
            = new Vector3(
            position[0],
            position[1],
            position[2]
            );
    }
}
