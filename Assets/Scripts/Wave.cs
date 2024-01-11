using System;
using UnityEngine;

[Serializable]
public class WaveElement
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int Count { get; private set; }
    
}

[CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves")]
public class Wave : ScriptableObject
{
    public WaveElement[] Characters;
}
