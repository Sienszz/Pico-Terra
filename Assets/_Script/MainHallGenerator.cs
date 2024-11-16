using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MainHallGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab1;
    [SerializeField] private GameObject tilePrefab2;
    [SerializeField] private GameObject tilePrefab3;
    [SerializeField] private Grid grid;
    [SerializeField] private PreviewSystem preview;
    private int selectedObjectIndex = -1;
    private GridData buildingData;
    private List<GameObject> placedGameObjects = new();

    private List<MainHallData> mainHallDataList = new List<MainHallData>();
    

    public GridData BuildingData
    {
        get { return buildingData; }
    }


    private void Start()
    {
        buildingData = new();
        GenerateMainHall();
        
    }

    private void GenerateMainHall()
    {
        var coordinates = new List<Vector3Int>();
        coordinates.Add(new Vector3Int(0, 0, 0));
        coordinates.Add(new Vector3Int(1, 0, 0));
        coordinates.Add(new Vector3Int(Mathf.RoundToInt(0.5f), 1, 0));

        var mainHallVector = new List<GameObject>();
        mainHallVector.Add(tilePrefab1);
        mainHallVector.Add(tilePrefab2);
        mainHallVector.Add(tilePrefab3);

        int i = 0;
       
        foreach (var coordinate in coordinates)
        {
            var position = grid.GetCellCenterWorld(coordinate);
            var rotation = Quaternion.Euler(0, 60, 0);
            Vector2Int size = new Vector2Int(1, 1);
            GameObject spawned = Instantiate(mainHallVector[i], position, rotation, transform);
            
            // Tambahkan data posisi ke dalam list
            mainHallDataList.Add(new MainHallData(coordinate, size, selectedObjectIndex + 1, placedGameObjects.Count - 1));
            selectedObjectIndex++;
            placedGameObjects.Add(spawned);
            GridData selectedData = buildingData;
            selectedData.AddObjectAt(coordinate,
            size,
            selectedObjectIndex,
            placedGameObjects.Count - 1, "mainhall");

            preview.UpdatePosition(position, false);
            i++;
        }
        //i = 0;
    }

    [Serializable]
    public class MainHallData
    {
        public Vector3Int coordinate;
        public Vector2Int objectSize;
        public int ID;
        public int placedObjectIndex;

        public MainHallData(Vector3Int coordinate,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex)
        {
            this.coordinate = coordinate;
            this.objectSize = objectSize;
            this.ID = ID;
            this.placedObjectIndex = placedObjectIndex;
        }
    }
}
