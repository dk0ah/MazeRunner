using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class StageGenerator : MonoBehaviour
{
   private int stageUnlocked;
   
   public GameObject popUp;
   public StageSelect stageSelect;
   public List<LevelData> levelDataList;

   public TextMeshProUGUI starsSumText;
   private int starsSum;

   private void Start()
   {
      
      Debug.Log("Data location : " + Application.persistentDataPath);

     if (!SaveGame.Load<bool>("hasPopulateStageData", false))
     {
        levelDataList = new List<LevelData>();
        for (int i = 0; i < 999; i++)
        {
           levelDataList.Add(new LevelData(i, 0));
        }
        SaveGame.Save<List<LevelData>>("levelDataList", levelDataList);
        SaveGame.Save("hasPopulateStageData", true);

     }
     levelDataList = SaveGame.Load<List<LevelData>>("levelDataList");
     for (int i = 0; i < levelDataList.Count; i++)
     {
        starsSum += levelDataList[i].starsEarned;
     }

     starsSumText.text = starsSum.ToString();
     
      stageSelect.isContinue = false;
      
       

      if (SaveGame.Load<bool>("isInLevel"))
      {
         popUp.SetActive(true);
      }
   }
   
 

  public void LastLevelContinue()
   {
      int lastLevelIndex = SaveGame.Load<int>("isInLevelIndex");
      stageSelect.stageIndex = lastLevelIndex;
      stageSelect.isContinue = true;
      SceneManager.LoadScene("InGame");
   }

   public void ClosePopUp()
   {
      popUp.SetActive(false);
      SaveGame.Save<bool>("isInLevel", false);
   }
   
   
    
}
