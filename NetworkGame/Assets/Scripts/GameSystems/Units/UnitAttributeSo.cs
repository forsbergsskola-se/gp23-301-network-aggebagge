using UnityEngine;

namespace GameSystems.Units
{
    [CreateAssetMenu(fileName = "ua_", menuName = "SO/Attribute", order = 0)]
    public class UnitAttributeSo : ScriptableObject
    {
        public enum AttributeType
        {
            None,
            Curse,
            AntiCurse,
            Foresight
        }

        public string attributeName;
        public AttributeType type;
        public Sprite icon;
    }
}