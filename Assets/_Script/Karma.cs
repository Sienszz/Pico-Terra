using Firebase.Firestore;
using System;

[FirestoreData]
public class Karma
{
    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public string PollutionType { get; set; }

    [FirestoreProperty]
    public DateTime TriggerDateTime { get; set; }

    [FirestoreProperty]
    public int PollutionAmount { get; set; }
}
