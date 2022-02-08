using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData : IData
{
    public int Money;
    public int Speed, MagnetStrength, RubbishCapacity;
    public int MagnetUpgrade, SpeedUpgrade;

    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) * 6];
        var offset = 0;
        offset += ByteConverter.AddToStream(Money, result, offset);
        offset += ByteConverter.AddToStream(Speed, result, offset);
        offset += ByteConverter.AddToStream(RubbishCapacity, result, offset);
        offset += ByteConverter.AddToStream(MagnetStrength, result, offset);
        offset += ByteConverter.AddToStream(MagnetUpgrade, result, offset);
        offset += ByteConverter.AddToStream(SpeedUpgrade, result, offset);

        return result;
    }

    public static PlayerData Deserialize(byte[] data)
    {
        var offset = 0;
        var result = new PlayerData();
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Money);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Speed);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.RubbishCapacity);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.MagnetStrength);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.MagnetUpgrade);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.SpeedUpgrade);
        return result;
    }
}
public interface IData
{
    byte[] Serialize();

}