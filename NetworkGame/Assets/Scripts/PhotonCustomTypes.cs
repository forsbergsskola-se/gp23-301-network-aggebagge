using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameSystems.Guild;
using GameSystems.Units;

public class PhotonCustomTypes
{
    public static void Register()
    {
        PhotonPeer.RegisterType(typeof(UnitData), (byte)'U', SerializeUnitData, DeserializeUnitData);
    }
    
    // Serialize UnitData into a byte array
    public static byte[] SerializeUnitData(object customObject)
    {
        UnitData unit = (UnitData)customObject;

        byte[] byteArray = new byte[16]; // Adjust size based on the number of fields you're sending

        int index = 0;
        Protocol.Serialize(unit.id, byteArray, ref index);
        Protocol.Serialize(unit.damage, byteArray, ref index);
        Protocol.Serialize(unit.goldGain, byteArray, ref index);
        Protocol.Serialize((int)unit.attributeType, byteArray, ref index); // Cast enum to int for serialization

        return byteArray;
    }

    // Deserialize byte array back into UnitData
    public static object DeserializeUnitData(byte[] data)
    {
        UnitData unit = new UnitData();

        int index = 0;
        Protocol.Deserialize(out unit.id, data, ref index);
        Protocol.Deserialize(out unit.damage, data, ref index);
        Protocol.Deserialize(out unit.goldGain, data, ref index);

        int attributeTypeValue;
        Protocol.Deserialize(out attributeTypeValue, data, ref index);
        unit.attributeType = (AttributeType)attributeTypeValue; // Cast back to enum

        return unit;
    }
}