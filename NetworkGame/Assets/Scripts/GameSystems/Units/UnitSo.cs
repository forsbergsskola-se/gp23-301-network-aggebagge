using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Units
{
    [CreateAssetMenu(fileName = "unit_", menuName = "SO/Unit", order = 0)]
    public class UnitSo : ScriptableObject
    {
        public enum Action
        {
            None
        }
        
        public string unitName;
        public Sprite sprite;
        public int cost;

        
        [Header("Stats")]
        public int physicalDamage;
        public int defense;
        public int spellDamage;
        [FormerlySerializedAs("gold")] public int goldGain;
        
        
        public int curse;
        public int antiCurse;

        public Action action;
    }
}