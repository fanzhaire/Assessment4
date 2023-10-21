using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public Vector2 gridSize = new Vector2(1, 1);
    public Button generateButton;
    public GameObject[] objectsToHide;
    public Button[] buttonsToHide;
    public TextMeshProUGUI[] textsToHide;

    private int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    private Quaternion GetRotationForCoordinates(int i, int j)
    {
        Vector2Int currentCoord = new Vector2Int(j, i);

        Vector2Int[] rotate90 =
        {
        new Vector2Int(0,1), new Vector2Int(0,2), new Vector2Int(0,3), new Vector2Int(0,4),
        new Vector2Int(0,5), new Vector2Int(0,6), new Vector2Int(0,7), new Vector2Int(0,8),
        new Vector2Int(0,9), new Vector2Int(2,3), new Vector2Int(2,4), new Vector2Int(2,7),
        new Vector2Int(7,4), new Vector2Int(5,10), new Vector2Int(5,11), new Vector2Int(5,12),
        new Vector2Int(7,7), new Vector2Int(7,8), new Vector2Int(7,9), new Vector2Int(7,10),
        new Vector2Int(7,11), new Vector2Int(7,12), new Vector2Int(7,13), new Vector2Int(8,7),
        new Vector2Int(8,8), new Vector2Int(8,9), new Vector2Int(8,11), new Vector2Int(8,12),
        new Vector2Int(10,14), new Vector2Int(10,13), new Vector2Int(13,1),
        new Vector2Int(13,2), new Vector2Int(13,3), new Vector2Int(11,3), new Vector2Int(13,4),
        new Vector2Int(13,8), new Vector2Int(13,9), new Vector2Int(5,3), new Vector2Int(7,3),
        new Vector2Int(10,7), new Vector2Int(13,10)
    };
        Vector2Int[] rotate180 =
        {
        new Vector2Int(5,4), new Vector2Int(5,13), new Vector2Int(5,7), new Vector2Int(8,13),
        new Vector2Int(11,4), new Vector2Int(11,10)
    };
        Vector2Int[] rotate270 =
        {
        new Vector2Int(5,2), new Vector2Int(5,6), new Vector2Int(5,9), new Vector2Int(8,6),
        new Vector2Int(11,3), new Vector2Int(11,8), new Vector2Int(13,7), new Vector2Int(11,2),
        new Vector2Int(11,9)
    };

        if (System.Array.Exists(rotate90, coord => coord.Equals(currentCoord)))
            return Quaternion.Euler(0, 0, 90);
        if (System.Array.Exists(rotate180, coord => coord.Equals(currentCoord)))
            return Quaternion.Euler(0, 0, 180);
        if (System.Array.Exists(rotate270, coord => coord.Equals(currentCoord)))
            return Quaternion.Euler(0, 0, 270);


        return Quaternion.identity;
    }

    void Start()
    {
        generateButton.onClick.AddListener(GenerateMap);
    }
    void GenerateMap()
    {
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }

        foreach (Button btn in buttonsToHide)
        {
            btn.gameObject.SetActive(false);
        }
        foreach (TextMeshProUGUI txt in textsToHide)
        {
            txt.gameObject.SetActive(false);
        }

        float totalWidth = levelMap.GetLength(1) * gridSize.x;
        float totalHeight = levelMap.GetLength(0) * gridSize.y;

        float startX = -totalWidth / 2 + gridSize.x / 2;
        float startY = totalHeight / 2 - gridSize.y / 2;


        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                int tileIndex = levelMap[i, j];
                if (tileIndex != 0)
                {
                    Quaternion rotation = GetRotationForCoordinates(i, j);
                    Instantiate(tilePrefabs[tileIndex - 1], new Vector3(startX + j * gridSize.x, startY - i * gridSize.y, 0), rotation, transform);
                }
            }

        }
    }



 }