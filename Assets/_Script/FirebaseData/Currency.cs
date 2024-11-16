using Firebase.Firestore;
using System;

[FirestoreData]
public class Currency
{
    [FirestoreProperty]
    public int currency { get; set; } = 1000;
    [FirestoreProperty]
    public int currencyIncome{ get; set; }

    public void updateValue(Currency curr)
    {
        currency = curr.currency;
        currencyIncome = curr.currencyIncome;
    }
}
