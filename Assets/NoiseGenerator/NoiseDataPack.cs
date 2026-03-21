using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseDataPack", menuName = "ScriptableObjects/Noise/NoiseDataPack")]
public class NoiseDataPack : ScriptableObject
{
    [Serializable]
    public struct NoiseData
    {
        public bool active;
        public Material material;
        public Vector2 offset;
        public float scale;
        public float amplitude;
    }

    public List<NoiseData> datas;
}
