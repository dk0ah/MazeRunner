using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseGridLayout : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        if (gridLayoutGroup != null)
        {
            FillGridWithZigzagPattern();
        }
        else
        {
            Debug.LogError("GridLayoutGroup component not found!");
        }
    }

    void FillGridWithZigzagPattern()
    {
        int gridWidth = gridLayoutGroup.constraintCount;
        int gridHeight = transform.childCount / gridWidth;

        int x = 0;
        int y = 0;
        bool moveRight = true;
        int index = 1;

        List<Transform> children = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        for (int i = 0; i < children.Count; i++)
        {
            children[i].GetComponentInChildren<Text>().text = index.ToString();

            index++;

            if (moveRight)
            {
                if (x + 1 < gridWidth)
                {
                    x = x + 1;
                }
                else
                {
                    moveRight = false;
                    y = y + 1;
                }
            }
            else
            {
                if (x - 1 >= 0)
                {
                    x = x - 1;
                }
                else
                {
                    moveRight = true;
                    y = y + 1;
                }
            }
        }
    }
}