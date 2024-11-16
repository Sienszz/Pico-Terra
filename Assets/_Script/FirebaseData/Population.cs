using Firebase.Firestore;
using System;

[FirestoreData]
public class Population
{
    [FirestoreProperty]
    public int housePopulation { get; set; } = 4;
    [FirestoreProperty]
    public int maxPopulation { get; set; } = 4;
    [FirestoreProperty]
    public int currPopulation { get; set; } = 4;
    [FirestoreProperty]
    public int usedPopulation { get; set; }
    [FirestoreProperty]
    public int populationDiff{ get; set; }

    public void updateValue(Population population)
    {
        housePopulation = population.housePopulation;
        maxPopulation = population.maxPopulation;
        currPopulation = population.currPopulation;
        usedPopulation = population.usedPopulation;
        populationDiff = population.populationDiff;
    }
}
