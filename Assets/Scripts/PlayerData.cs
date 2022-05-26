using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData

{
    public int totalFinishedLevelNumber;
    public float money;
    public bool isNormalModeUnlocked;
    public bool isHardModeUnlocked;

    public PlayerData(GameManager gameManager)
    {
        totalFinishedLevelNumber = gameManager.totalFinishedLevelNumber;
        money = gameManager.money;
        isNormalModeUnlocked = gameManager.isNormalModeUnlocked;
        isHardModeUnlocked = gameManager.isHardModeUnlocked;
        //money = 0;
    }
}
