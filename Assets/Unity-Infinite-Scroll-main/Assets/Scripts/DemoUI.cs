using System.Collections.Generic;
using DynamicScrollRect;
using UnityEngine;

public class DemoUI : MonoBehaviour
{
    [SerializeField] private ScrollContent _content = null;

    [SerializeField] private int _itemCount = 50;
    private int row;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        List<ScrollItemData> contentDatas = new List<ScrollItemData>();

        int columns = 4; // Number of columns

        for (int row = 0; row < _itemCount / columns; row++)
        {
            // Determine whether the row is odd or even
            bool isOddRow = (row % 2 != 0);

            // Add items in ascending order for even rows, descending for odd rows
            for (int col = 0; col < columns; col++)
            {
                int index = isOddRow ? (row + 1) * columns - col - 1 : row * columns + col;
                contentDatas.Add(new ScrollItemData(index));
            }
        }    
        _content.InitScrollContent(contentDatas);
    }
}