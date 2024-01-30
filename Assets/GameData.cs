using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData 
{
    public int stageIndex;
    public int stars;
    public int stageUnlocked;

    public GameData(int index)
    {
        stageIndex = index;
        stars = 0;
    }
}
