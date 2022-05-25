using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Inventory Item", menuName = "Item")]
public class _ScriptableObject : ScriptableObject
{
    public GameObject objectPrefab;
    public Sprite inventory_Sprite;
}
