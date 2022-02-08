using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsData : IData
{
    public int Size;
    public List<bool> Active = new List<bool>();
    public List<int> RubbishListCount = new List<int>();
    public List<byte> RubbishType = new List<byte>();
    public List<int> RubbishCount = new List<int>();
    public List<int> Coin = new List<int>();
    public List<int> ID = new List<int>();
    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) + sizeof(int) * RubbishListCount.Count + sizeof(int) * RubbishCount.Count
             + sizeof(int) * Coin.Count + sizeof(int) * ID.Count + sizeof(byte) * Active.Count + sizeof(byte) * RubbishType.Count];
        var offset = 0;
        offset += ByteConverter.AddToStream(Size, result, offset);

        for (int i = 0; i < Size; i++)
        {
            offset += ByteConverter.AddToStream(Active[i], result, offset);
            offset += ByteConverter.AddToStream(RubbishListCount[i], result, offset);
            for (int j = 0; j < RubbishListCount[i]; j++)
            {
                offset += ByteConverter.AddToStream(RubbishType[j], result, offset);
                offset += ByteConverter.AddToStream(RubbishCount[j], result, offset);
            }
            offset += ByteConverter.AddToStream(Coin[i], result, offset);
            offset += ByteConverter.AddToStream(ID[i], result, offset);
        }
        return result;
    }
    public static BuildingsData Deserialize(byte[] data)
    {
        var offset = 0;
        var result = new BuildingsData();
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Size);
        for (int i = 0; i < result.Size; i++)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out bool active);
            result.Active.Add(active);
            offset += ByteConverter.ReturnFromStream(data, offset, out int rubbish);
            result.RubbishListCount.Add(rubbish);
            for (int j = 0; j < rubbish; j++)
            {
                offset += ByteConverter.ReturnFromStream(data, offset, out byte type);
                result.RubbishType.Add(type);
                offset += ByteConverter.ReturnFromStream(data, offset, out int count);
                result.RubbishCount.Add(count);
            }
            offset += ByteConverter.ReturnFromStream(data, offset, out int coin);
            result.Coin.Add(coin);
            offset += ByteConverter.ReturnFromStream(data, offset, out int id);
            result.ID.Add(id);
        }
        return result;
    }
}
