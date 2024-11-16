using Firebase.Firestore;
using System;

[FirestoreData]
public class Energy
{
    [FirestoreProperty]
    public int maxElectricity { get; set; }
    [FirestoreProperty]
    public int maxMineral { get; set; }
    [FirestoreProperty]
    public int maxFood { get; set; }

    [FirestoreProperty]
    public int usedElectricity { get; set; }
    [FirestoreProperty]
    public int usedMineral { get; set; }
    [FirestoreProperty]
    public int usedFood { get; set; }
    public void updateValue(Energy energyData)
    {
        maxElectricity = energyData.maxElectricity;
        maxMineral = energyData.maxMineral;
        maxFood = energyData.maxFood;
        usedElectricity = energyData.usedElectricity;
        usedMineral = energyData.usedMineral;
        usedFood = energyData.usedFood;
    }
}
