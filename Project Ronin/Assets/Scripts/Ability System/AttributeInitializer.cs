using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attribute Initializer", menuName = "ScriptableObjects/Attribute Initializer", order = 1)]
public class AttributeInitializer : ScriptableObject
{
    [System.Serializable]
    public struct FloatPair
    {
        public string name;
        public float value;
    }

    public List<FloatPair> floatInitials = new List<FloatPair>();
    public List<string> tagInitials = new List<string>();
}
