using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
    public List<ObjectData> PlaceObject;
    public List<ConstructionObject> constructionObject;
    public List<WasteManagement> wasteManagements;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; set; }

    [field: SerializeField]
    public string Type { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefeb { get; private set; }

    [field: SerializeField]
    public int LevelMainHall { get; private set; }

    [field: SerializeField]
    public float AirPolution { get; private set; }

    [field: SerializeField]
    public float SoilPolution { get; private set; }

    [field: SerializeField]
    public float WaterPolution { get; private set; }

    [field: SerializeField]
    public int Currency { get; set; }

    [field: SerializeField]
    public string Category { get; private set; }

    [field: SerializeField]
    public int PopulationNeed { get; private set; }

    [field: SerializeField]
    public int CurrencyNeed { get; private set; }

    [field: SerializeField]
    public float BuildingTime { get; private set; }

    [field: SerializeField]
    public int Food { get; private set; }

    [field: SerializeField]
    public int Mineral { get; private set; }

    [field: SerializeField]
    public int Electrictiy { get; private set; }

    [field: SerializeField]
    public int EnergyProduction { get; private set; }

    [field: SerializeField]
    public bool IsLevelMax { get; private set; }

    [field: SerializeField]
    public Sprite ImageBuilding { get; private set; }

    [field: SerializeField]
    public String BuildingDescription { get; private set; }

    [field: SerializeField]
    public String KendaraanNeed { get; set; }
}

[Serializable]
public class PlaceObject
{
    private static int nextID = 0;

    public int ID { get; private set; }

    public string Name { get; set; }

    public Image image { get; set; }

    public float AirPolution { get; set; }

    public float SoilPolution { get; set; }

    public float WaterPolution { get; set; }

    public int level { get; set; }

    public int count { get; set; }

    public PlaceObject()
    {
        ID = nextID;
        nextID++;
    }

}

[Serializable]
public class ConstructionObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public GameObject Prefeb { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
}

[Serializable]
public class WasteManagement
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public string AirPollution { get; private set; }

    [field: SerializeField]
    public string WaterPollution { get; private set; }

    [field: SerializeField]
    public string SoilPollution { get; private set; }

    [field: SerializeField]
    public Sprite Image { get; private set; }

    [field: SerializeField]
    public float CurrencyNeeds { get; private set; }

    [field: SerializeField]
    public string Type { get; private set; }
}