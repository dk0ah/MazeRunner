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
   public GameObject stagePrefab;
   private int stageUnlocked;
   
   public Button continueButton;
   public Button closeButton;
   public GameObject popUp;
   public StageSelect stageSelect;
   public List<LevelData> levelDataList;

   public TextMeshProUGUI starsSumText;
   private int starsSum;

   private void Start()
   {
      
      Debug.Log("Data location : " + Application.persistentDataPath);

       stageUnlocked = SaveGame.Load<int>("stageUnlocked");
      Debug.Log("stage unlocked: "+stageUnlocked);

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

      stageSelect.isContinue = false;
      StageGenerate();

      if (SaveGame.Load<bool>("isInLevel"))
      {
         popUp.SetActive(true);
      }
   }
   
 

   void StageGenerate()
   {
      int gridWidth = 4;
      int gridHeight = Mathf.CeilToInt(999f / gridWidth);

      for (int y = 0; y < gridHeight; y++)
      {
         for (int x = 0; x < gridWidth; x++)
         {
            int i;

            if (y % 2 == 0)
            {
               // Even row, fill left to right
               i = y * gridWidth + x;
            }
            else
            {
               // Odd row, fill right to left
               i = (y + 1) * gridWidth - 1 - x;
            }

            if (i < 999)
            {
               GameObject stage = Instantiate(stagePrefab, gameObject.transform);
               Stage stageComp = stage.GetComponentInChildren<Stage>();
               stageComp.stageIndex = i;
               stageComp.stageStarAmount = levelDataList[i].starsEarned;
               starsSum += stageComp.stageStarAmount;

               if (i <= stageUnlocked)
               {
                  stageComp.stageIsUnlocked = true;
               }
               else
               {
                  stageComp.stageIsUnlocked = false;
               }

               // Check if the current index follows the specified pattern for activating hLineGameObject
               if ((i - 7) % 8 == 0 ||  i % 8 == 0)
               {
                  stageComp.hLineGameObject.SetActive(true);
               }
               if (i % 4 == 3)
               {
                  stageComp.vLineGameObject.SetActive(true);
               }
            }
         }
      }

      starsSumText.text = starsSum.ToString();
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
