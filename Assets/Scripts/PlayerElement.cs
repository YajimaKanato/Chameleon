using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerElement", menuName = "Scriptable Objects/Element/PlayerElement")]
public class PlayerElement : ScriptableObject
{
    [Header("ElementData")]
    public List<ElementData> _elementData;

    [System.Serializable]
    public class ElementData
    {
        public enum Element
        {
            Red,
            Blue,
            Yellow,
            Purple,
            Green,
            Orange,
            White
        }

        [Header("Element")]
        public Element _element;

        [Header("ChargePower")]
        public float _chargePower;

        [Header("TonguePower")]
        public float _tonguePower;

        [Header("TongueRange")]
        public float _tongueRange;
    }
}
