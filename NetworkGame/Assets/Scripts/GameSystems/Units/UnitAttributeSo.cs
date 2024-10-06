using UnityEngine;

namespace GameSystems.Units
{
    public enum AttributeType
    {
        None,
        Curse,
        AntiCurse,
        Foresight,
        FullParty,
        Royalty,
        Scaling
    }
    
    [CreateAssetMenu(fileName = "ua_", menuName = "SO/Attribute", order = 0)]
    public class UnitAttributeSo : ScriptableObject
    {
        public string attributeName;
        public AttributeType type;
        public Sprite icon;
    }
}