using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool
{
    public ObjectType Type;
    public int Count;
    public GameObject Prefab;
    public Transform Container;
    public Queue<GameObject> Queue = new();
}