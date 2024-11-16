using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridVisualizationGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject gridVisu;
    [SerializeField] private Vector2Int size;

    [SerializeField] private SwitchingLevels mainHall;

    private void Start()
    {
        GenerateTiles(mainHall.current_level);

    }

    public void GenerateTiles(int mainHallLevel)
    {
        if (mainHallLevel == 1)
        {
            tilePrefab.transform.localScale = new Vector3(2.5f, 5, 1);
            gridVisu.transform.localScale = new Vector3(0.2f, 1, 0.4f);
            size.x = 3;
            size.y = 3;
        }
        else if (mainHallLevel == 2)
        {
            tilePrefab.transform.localScale = new Vector3(2.5f, 3.3333f, 1);
            gridVisu.transform.localScale = new Vector3(0.3f, 1, 0.4f);
            size.x = 4;
            size.y = 3;
        }
        else if (mainHallLevel == 3)
        {
            tilePrefab.transform.localScale = new Vector3(2, 3.3333f, 1);
            gridVisu.transform.localScale = new Vector3(0.3f, 1, 0.5f);
            size.x = 4;
            size.y = 4;
        }
        else if (mainHallLevel == 4)
        {
            tilePrefab.transform.localScale = new Vector3(1.6667f, 2.5f, 1);
            gridVisu.transform.localScale = new Vector3(0.4f, 1, 0.6f);
            size.x = 5;
            size.y = 4;
        }
        else if (mainHallLevel == 5)
        {
            tilePrefab.transform.localScale = new Vector3(1.42857f, 2.5f, 1);
            gridVisu.transform.localScale = new Vector3(0.4f, 1, 0.7f);
            size.x = 5;
            size.y = 5;
        }
        else if (mainHallLevel == 6)
        {
            tilePrefab.transform.localScale = new Vector3(1.42857f, 2f, 1);
            gridVisu.transform.localScale = new Vector3(0.5f, 1, 0.7f);
            size.x = 6;
            size.y = 5;
        }
        else if (mainHallLevel == 7)
        {
            tilePrefab.transform.localScale = new Vector3(1.25f, 2, 1);
            gridVisu.transform.localScale = new Vector3(0.5f, 1, 0.8f);
            size.x = 6;
            size.y = 6;
        }
        else if (mainHallLevel == 8)
        {
            tilePrefab.transform.localScale = new Vector3(1.25f, 1.6667f, 1);
            gridVisu.transform.localScale = new Vector3(0.6f, 1, 0.8f);
            size.x = 8;
            size.y = 6;
        }
        else if (mainHallLevel == 9)
        {
            tilePrefab.transform.localScale = new Vector3(1.1111f, 1.6667f, 1);
            gridVisu.transform.localScale = new Vector3(0.6f, 1, 0.9f);
            size.x = 8;
            size.y = 7;
        }
        else if (mainHallLevel == 10)
        {
            tilePrefab.transform.localScale = new Vector3(1, 1.25f, 1);
            gridVisu.transform.localScale = new Vector3(0.8f, 1, 1);
            size.x = 10;
            size.y = 8;
        }
        else if (mainHallLevel == 11)
        {
            tilePrefab.transform.localScale = new Vector3(1, 1, 1);
            gridVisu.transform.localScale = new Vector3(1, 1, 1);
            size.x = 12;
            size.y = 9;
        }


        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var coordinates = new List<Vector3Int>();

        for (int x = -size.x / 2; x <= size.x / 2; x++)
        {
            var offset1 = x % 2 == 0 ? 1 : 0;
            var offset = 1;
            if (mainHallLevel == 3)
            {
                offset1 = 0;
                offset = x % 2 == 0 ? 0 : 1;
            }
            else if (mainHallLevel == 5 || mainHallLevel == 6 || mainHallLevel == 9)
            {
                offset1 = 1;
                offset = x % 2 == 0 ? 1 : 2;
            }
            else if (mainHallLevel == 7)
            {
                offset = 1;
            }
            else if (mainHallLevel == 11)
            {
                offset1 = 1;
                offset = 1;
            }
            for (int y = -size.y / 2 - offset; y <= size.y / 2 + offset1; y++)
            {
                coordinates.Add(new Vector3Int(y, x, 0));
            }
        }


        foreach (var coordinate in coordinates)
        {
            var position = grid.GetCellCenterWorld(coordinate);
            var rotation = Quaternion.Euler(90, 0, 90);

            GameObject spawned = Instantiate(tilePrefab, position, rotation, transform);
        }
    }
}
