using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Units
{
    [CreateAssetMenu(fileName = "unit_", menuName = "SO/Unit", order = 0)]
    public class UnitSo : ScriptableObject
    {
        public string unitName;
        public Sprite sprite;
        public int cost;

        [Header("Stats")]
        public int physicalDamage;
        public int goldGain;

        public UnitAttributeSo attribute;
    }
}