using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Stage : MonoBehaviour
{
    public Sprite Stage0StarSprite;
    public Sprite Stage1StarSprite;
    public Sprite Stage2StarSprite;
    public Sprite Stage3StarSprite;
    public Sprite StageLockSprite;
    
    

    public Image backgroundIMG;
    public GameObject hLineGameObject;
    public GameObject vLineGameObject;

    public TextMeshProUGUI stageIndexText;
    public int stageIndex;
    public int stageStarAmount;
    public bool stageIsUnlocked;

    public bool isVisible;
    private RectTransform rectTransform;

    [SerializeField] private StageSelect stageSelect;

   

    private void Start()
    {
        stageIndexText.text = stageIndex.ToString();
        switch (stageStarAmount)
        {
            case 0: 
                backgroundIMG.sprite = Stage0StarSprite;
                break;
            case 1:
                backgroundIMG.sprite = Stage1StarSprite;
                break;
            case 2:
                backgroundIMG.sprite = Stage2StarSprite;
                break;
            case 3:
                backgroundIMG.sprite = Stage3StarSprite;
                break;
        }

        if (!stageIsUnlocked)
        {
            backgroundIMG.sprite = StageLockSprite;
            GetComponent<Button>().interactable = false;
        }

        rectTransform = GetComponent<RectTransform>();
    }
   
    public void SelectStage()
    {
        SceneManager.LoadScene("InGame");
        Debug.Log("selectt stage: "+stageIndex);
        stageSelect.stageIndex = stageIndex;
    }
}
