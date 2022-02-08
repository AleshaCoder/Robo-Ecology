using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementData : IData
{
    public int Size;
    public List<int> IDs = new List<int>();
    public List<int> CurrentLevels = new List<int>();
    public List<int> CurrrentRewards = new List<int>();
    public List<int> CurrrentFullness = new List<int>();
    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) + sizeof(int) * IDs.Count + sizeof(int) * CurrentLevels.Count
            + sizeof(int) * CurrrentRewards.Count + sizeof(int) * CurrrentFullness.Count];
        var offset = 0;

        offset += ByteConverter.AddToStream(Size, result, offset);
        for (int i = 0; i < Size; i++)
        {
            offset += ByteConverter.AddToStream(IDs[i], result, offset);
            offset += ByteConverter.AddToStream(CurrentLevels[i], result, offset);
            offset += ByteConverter.AddToStream(CurrrentRewards[i], result, offset);
            offset += ByteConverter.AddToStream(CurrrentFullness[i], result, offset);
        }
        return result;
    }

    public static AchievementData Deserialize(byte[] data)
    {
        var offset = 0;
        var result = new AchievementData();

        offset += ByteConverter.ReturnFromStream(data, offset, out result.Size);
        for (int i = 0; i < result.Size; i++)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out int id);
            result.IDs.Add(id);
            offset += ByteConverter.ReturnFromStream(data, offset, out int level);
            result.CurrentLevels.Add(level);
            offset += ByteConverter.ReturnFromStream(data, offset, out int reward);
            result.CurrrentRewards.Add(reward);
            offset += ByteConverter.ReturnFromStream(data, offset, out int fullness);
            result.CurrrentFullness.Add(fullness);
        }
        return result;
    }
}
