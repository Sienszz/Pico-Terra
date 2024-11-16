using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Unity.VisualScripting;
using System;

public static class FirestoreManager 
{
    static string playerid = "player";
    static FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    public static void SaveBuilding(Building building)
    {
        DocumentReference documentReference = db.Collection("users").Document(playerid).Collection("buildings").Document(building.Id);
        documentReference.SetAsync(building);
    }

    public static void UpdateBuildingStatus(string buildingid, bool builded)
    {
        DocumentReference documentReference = db.Collection("users").Document(playerid).Collection("buildings").Document(buildingid);
        documentReference.UpdateAsync("isBuilded", builded).ContinueWithOnMainThread(task =>
        {

        });
    }
    public static void FetchBuildings(UILoader loader)
    {
        ArrayList buildings = new ArrayList();

         db.Collection("users").Document(playerid).Collection("buildings").GetSnapshotAsync().ContinueWithOnMainThread(task =>
         {
            foreach (DocumentSnapshot document in task.Result)
            {
                 buildings.Add(document.ConvertTo<Building>());
            }

            loader.PlaceExistingStructures(buildings);
         });
    }
    public static void FetchData(UILoader loader, Energy energy, Currency currency, Nature nature, Polution polution, Population population)
    {
        // Create a reference to the document
        DocumentReference docRef = db.Collection("users").Document(playerid);

        // Check if the document exists
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Debug.Log("document exist");
                    Query allDataQuery = docRef.Collection("userData");
                    allDataQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                    {
                        QuerySnapshot allDatasQuerySnapshot = task.Result;
                        foreach (DocumentSnapshot documentSnapshot in allDatasQuerySnapshot.Documents)
                        {
                            if (documentSnapshot.Id == "Energy")
                            {
                                energy.updateValue(documentSnapshot.ConvertTo<Energy>());

                            }
                            else if (documentSnapshot.Id == "Population")
                            {

                                population.updateValue(documentSnapshot.ConvertTo<Population>());
                            }
                            else if (documentSnapshot.Id == "Polution")
                            {
                                polution.updateValue(documentSnapshot.ConvertTo<Polution>());
                            }
                            else if (documentSnapshot.Id == "Currency")
                            {
                                currency.updateValue(documentSnapshot.ConvertTo<Currency>());
                            }
                            else if (documentSnapshot.Id == "Nature")
                            {
                                nature.updateValue(documentSnapshot.ConvertTo<Nature>());
                            }
                        }
                        loader.LoadDataOnOpen();
                    });
                }
                else
                {
                    Debug.Log("document not exist");
                    Dictionary<string, object> field = new Dictionary<string, object>
                    {
                        { "current_pollution_bar_limit", 0 },
                        { "sacrifice_karma_timestamp", DateTime.Now }
                    };
                    docRef.SetAsync(field);

                }
            }
            else
            {
                Debug.LogError("Error checking the document: " + task.Exception);
            }
        });
    }

    public static void SaveData(Energy energy, Currency currency, Nature nature, Polution polution, Population population)
    {
        db.Collection("users").Document(playerid).Collection("userData").Document("Energy").SetAsync(energy);
        db.Collection("users").Document(playerid).Collection("userData").Document("Currency").SetAsync(currency);
        db.Collection("users").Document(playerid).Collection("userData").Document("Nature").SetAsync(nature);
        db.Collection("users").Document(playerid).Collection("userData").Document("Polution").SetAsync(polution);
        db.Collection("users").Document(playerid).Collection("userData").Document("Population").SetAsync(population);
    }


    public static void DeleteBuilding(string documentId)
    {
        DocumentReference documentReference = db.Collection("users").Document(playerid).Collection("buildings3").Document(documentId);
        documentReference.DeleteAsync();
    }

    public static void SaveKarma(Karma karma)
    {
        db.Collection("users").Document(playerid).Collection("karma").Document(karma.Id).SetAsync(karma);
    }

    public static void SaveSacrificeKarmaTimestamp()
    {
        db.Collection("users").Document(playerid).UpdateAsync("sacrifice_karma_timestamp", DateTime.Now);
    }

    public static void UpdatePollutionBarLimit(int limit)
    {
        db.Collection("users").Document(playerid).UpdateAsync("current_pollution_bar_limit", limit);
    }

    public static void DeleteKarma(string documentId)
    {
        DocumentReference documentReference = db.Collection("users").Document(playerid).Collection("karma").Document(documentId);
        documentReference.DeleteAsync();
    }
}
