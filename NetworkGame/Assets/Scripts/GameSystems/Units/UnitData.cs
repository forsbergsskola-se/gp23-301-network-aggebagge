using System;
using ExitGames.Client.Photon;

namespace GameSystems.Units
{
    
    public class UnitData
    {
        public int id;
        public int damage;
        public int goldGain;
        public AttributeType attributeType;
        
        public UnitData(UnitSo unitSo)
        {
            id = unitSo.id;
            damage = unitSo.damage;
            goldGain = unitSo.goldGain;
            
            attributeType = unitSo.attribute == null? AttributeType.None : unitSo.attribute.type;
        }

        public UnitData() {}
        
        // Convert UnitData to a Hashtable for Photon
        public Hashtable ToHashtable()
        {
            Hashtable data = new Hashtable();
            data["id"] = id;
            data["damage"] = damage;
            data["goldGain"] = goldGain;
            data["attributeType"] = (int)attributeType; // Store as an int since enums can be tricky
            return data;
        }

        // Create UnitData from a Hashtable
        public static UnitData FromHashtable(Hashtable data)
        {
            return new UnitData
            {
                id = (int)data["id"],
                damage = (int)data["damage"],
                goldGain = (int)data["goldGain"],
                attributeType = (AttributeType)(int)data["attributeType"] // Cast the int back to the enum
            };
        }
        
    }
}