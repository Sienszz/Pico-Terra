using Firebase.Firestore;
using System;

[FirestoreData]
public class Polution
{
    [FirestoreProperty]
    public float airPolution { get; set; }
    [FirestoreProperty]
    public float airPolutionIncome { get; set; }
    [FirestoreProperty]
    public float airPolutionTemp { get; set; }

    [FirestoreProperty]
    public float waterPolution { get; set; }
    [FirestoreProperty]
    public float waterPolutionIncome { get; set; }
    [FirestoreProperty]
    public float waterPolutionTemp { get; set; }

    [FirestoreProperty]
    public float soilPolution { get; set; }
    [FirestoreProperty]
    public float soilPolutionIncome { get; set; }
    [FirestoreProperty]
    public float soilPolutionTemp { get; set; }

    public void updateValue(Polution polution)
    {
        airPolution = polution.airPolution;
        airPolutionIncome = polution.airPolutionIncome;
        airPolutionTemp = polution.airPolutionTemp;

        waterPolution = polution.waterPolution;
        waterPolutionIncome = polution.waterPolutionIncome;
        waterPolutionTemp = polution.waterPolutionTemp;

        soilPolution = polution.soilPolution;
        soilPolutionIncome = polution.soilPolutionIncome;
        soilPolutionTemp = polution.soilPolutionTemp;
    }
}
