using UnityEngine;


[CreateAssetMenu(fileName = "New Material Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int id;
    public GameObject Item_Prefab;
    public int Cost;

    public Sprite TuckImage;
}
