namespace GameSystems.Units
{
    public class UnitData
    {
        public int id;
        public int damage;
        public int goldGain;
        public UnitAttributeSo.AttributeType attributeType;

        public UnitData(int dmg, int gold, UnitAttributeSo.AttributeType atType)
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
            attributeType = unitSo.attribute.type;
        }
    }
}