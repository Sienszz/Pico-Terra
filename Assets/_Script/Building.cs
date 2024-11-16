using Firebase.Firestore;
using System;

[FirestoreData]
public class Building
{
    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public string Category { get; set; }

    [FirestoreProperty]
    public float PositionX { get; set; }

    [FirestoreProperty]
    public float PositionY { get; set; }

    [FirestoreProperty]
    public float PositionZ { get; set; }

    [FirestoreProperty]
    public string ModelName { get; set; }

    [FirestoreProperty]
    public int WasteManagementLevel { get; set; }
    
    [FirestoreProperty]
    public DateTime TimeStamp { get; set; }

    [FirestoreProperty]
    public bool isBuilded { get; set; }

    [FirestoreProperty]
    public string KendaraanName { get; set; }
}
