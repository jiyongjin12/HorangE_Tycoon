using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Material Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int id;
    public GameObject Item_Prefab;
}
