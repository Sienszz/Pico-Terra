using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConstructionSystem : MonoBehaviour
{
    public void ReplaceStructure(GameObject oldStructure, ObjectData newStructure)
    {
        GameObject newObject = Instantiate(newStructure.Prefeb);
        newObject.transform.position = oldStructure.transform.position;
        newObject.name = Guid.NewGuid().ToString();

        Building addedBuilding = new Building
        {
            PositionX = oldStructure.transform.position.x,
            PositionY = oldStructure.transform.position.y,
            PositionZ = oldStructure.transform.position.z,
            ModelName = newStructure.Name,
            Id = newObject.name,
            Category = newStructure.Category
        };

        DeleteStructure(oldStructure);
        FirestoreManager.SaveBuilding(addedBuilding);
    }

    private void DeleteStructure(GameObject structure)
    {
        FirestoreManager.DeleteBuilding(structure.name);
        Destroy(structure);
    }

}
