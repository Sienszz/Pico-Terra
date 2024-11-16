using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StructureBuilder
{
    public static GameObject GetSubCategoryContainer(string category, string subCategory)
    {
        GameObject emptyGameObject = GameObject.Find(subCategory + "Container");

        if (emptyGameObject == null)
        {
            emptyGameObject = new GameObject();
            emptyGameObject.name = subCategory + "Container";
            emptyGameObject.transform.parent = GameObject.Find(category).transform;
        }
        return emptyGameObject;
    }

    public static string GetStructureTag(string category)
    {
        if (category == "Energy" || category == "Nature")
        {
            return "UnaffectableStructure";
        }
        else
        {
            return "AffectableStructure";
        }
    }
    
}