using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class LevelManager : MonoBehaviour
{
    public float countDown;
    
    public Slider slider;

    public TextMeshProUGUI messageText;

    public Button nextBTN;
    public Button restartBTN;
    public Button menuBTN;

    public GameObject popup;

    private float elapsedTime;
    
    private bool isInLevel;

    private int stageIndex;

    private bool isGameEnd;


    private int starsEarned;
    private List<LevelData> levelDataList;

    public StageSelect stageSelect;

    private GameObject bugGameObject;

 
    void Start()
    {
        StartCoroutine(ChangeSliderValueOverTime());
        stageIndex = stageSelect.stageIndex;
        levelDataList = SaveGame.Load<List<LevelData>>("levelDataList");
        
        
        bugGameObject = FindObjectOfType<BugController>().gameObject;
        if (stageSelect.isContinue)
         {
             elapsedTime = SaveGame.Load<int>("timeLeft");
             bugGameObject.transform.position = SaveGame.Load<Vector3>("bugPosition");
             stageSelect.isContinue = false;
         }
         

    }
    

    IEnumerator ChangeSliderValueOverTime()
    {
         elapsedTime = 0f;
        float startValue = 0f;
        float endValue = 60f;

        while (elapsedTime < countDown && !isGameEnd)
        {
            // Calculate the new slider value using Lerp
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / countDown);

            // Set the slider value
            slider.value = newValue;

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;
        }

        GameEnd();

    }

    public void GameEnd()
    {
        isGameEnd = true;
        SaveGame.Save<bool>("isInLevel", isInLevel);
        StopCoroutine(ChangeSliderValueOverTime());
        popup.SetActive(true);
        if (elapsedTime  < 60)
        {
            if (elapsedTime < 15)
            {
                starsEarned = 3;
            }
            else if (elapsedTime < 40)
            {
                starsEarned = 2;
            }
            else
            {
                starsEarned = 1;
            }
            messageText.text = "Congratulations, You Won";
            
            int stageUnlocked = SaveGame.Load<int>("stageUnlocked");
            if (stageIndex == stageUnlocked)
            {
                ++stageUnlocked;
                SaveGame.Save<int>("stageUnlocked", stageUnlocked);
            }

            levelDataList[stageIndex].starsEarned = starsEarned;
            SaveGame.Save<List<LevelData>>("levelDataList", levelDataList);


        }
        else
        {
            nextBTN.interactable = false;
            messageText.text = "You lose, let's try again";

        }
    }

    public void NextSelect()
    {
        stageSelect.stageIndex++;
        SceneManager.LoadScene("InGame");
        Debug.Log("next clicked");

    }
    public void RestartSelect()
    {
        SceneManager.LoadScene("InGame");
        Debug.Log("restart clicked");

    }
    public void MenuSelect()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("menu clicked");
    }

    private void OnApplicationQuit()
    {
        if (!isGameEnd)
        {
            SaveMidGameData();
        }
    }

    public void BackToMenu()
    {
        SaveMidGameData();
        SceneManager.LoadScene("Menu");
    }

    void SaveMidGameData()
    {
        SaveGame.Save<bool>("isInLevel", !isGameEnd);
        SaveGame.Save<int>("isInLevelIndex", stageIndex);
        SaveGame.Save<Vector3>("bugPosition", bugGameObject.transform.position);
        SaveGame.Save<float>("timeLeft",elapsedTime);
    }
}
