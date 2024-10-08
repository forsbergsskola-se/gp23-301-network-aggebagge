using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Units
{
    [CreateAssetMenu(fileName = "unit_", menuName = "SO/Unit", order = 0)]
    public class UnitSo : ScriptableObject
    {
        public int id;
        public string unitName;
        public Sprite sprite;
        public int cost;

        public bool mustBeInShop;
        
        [TextArea(5, 6)]
        public string tooltipText;

        [FormerlySerializedAs("physicalDamage")] [Header("Stats")]
        public int damage;
        public int goldGain;

        public UnitAttributeSo attribute;
    }
}