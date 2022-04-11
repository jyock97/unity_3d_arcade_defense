using System;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "Structures", menuName = "Models/Structures")]
public class SO_Structures : ScriptableObject
{
    public GameObject[] structures;
    public int[] prices;

    public void OnValidate()
    {
        if (structures.Length != prices.Length)
        {
            throw new Exception("Structures array and Prices array doesnt have the same length");
        }
    }
}
