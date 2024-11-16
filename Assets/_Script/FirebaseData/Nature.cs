using Firebase.Firestore;
using System;

[FirestoreData]
public class Nature
{
    [FirestoreProperty]
    public float airNature { get; set; }
    [FirestoreProperty]
    public float waterNature { get; set; }
    [FirestoreProperty]
    public float soilNature { get; set; }
    [FirestoreProperty]
    public float soilNatureTemp { get; set; }
    [FirestoreProperty]
    public float waterNatureTemp { get; set; }
    [FirestoreProperty]
    public float airNatureTemp { get; set; }

    public void updateValue(Nature nature)
    {
        airNature = nature.airNature;
        waterNature = nature.waterNature;
        soilNature = nature.soilNature;

        airNatureTemp = nature.airNatureTemp;
        waterNatureTemp = nature.waterNatureTemp;
        soilNatureTemp = nature.soilNatureTemp;
    }
}
