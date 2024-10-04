using System;

namespace GameSystems.Units
{
    
    public class UnitData
    {
        public int id;
        public int damage;
        public int goldGain;
        public AttributeType attributeType;

        public UnitData(int dmg, int gold, AttributeType atType)
        {
            damage = dmg;
            goldGain = gold;
            attributeType = atType;
        }
        
        public UnitData(UnitSo unitSo)
        {
            id = unitSo.id;
            damage = unitSo.damage;
            goldGain = unitSo.goldGain;
            
            attributeType = unitSo.attribute == null? AttributeType.None : unitSo.attribute.type;
        }

        public UnitData()
        {
        }
    }
}