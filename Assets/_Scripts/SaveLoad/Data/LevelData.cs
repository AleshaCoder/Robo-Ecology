using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : IData
{
    public int Size;
    public List<int> ID = new List<int>();
    public List<bool> IsUnlocked = new List<bool>();
    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) + sizeof(int) * ID.Count + sizeof(byte) * IsUnlocked.Count];
        int offset = 0;
        offset += ByteConverter.AddToStream(Size, result, offset);
        for (int i = 0; i < Size; i++)
        {
            offset += ByteConverter.AddToStream(ID[i], result, offset);
            offset += ByteConverter.AddToStream(IsUnlocked[i], result, offset);
        }
        return result;
    }

    public static LevelData Deserialize(byte[] data)
    {
        int offset = 0;
        var result = new LevelData();
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Size);
        for (int i = 0; i < result.Size; i++)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out int id);
            result.ID.Add(id);
            offset += ByteConverter.ReturnFromStream(data, offset, out bool unlocked);
            result.IsUnlocked.Add(unlocked);
        }
        return result;
    }
}
