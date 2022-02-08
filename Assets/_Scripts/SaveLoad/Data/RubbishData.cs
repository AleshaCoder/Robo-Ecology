using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishData : IData
{
    public int Size;
    public List<byte> Type = new List<byte>();
    public List<Vector3> Position = new List<Vector3>();
    public List<bool> Hiden = new List<bool>();

    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) + sizeof(byte) * Type.Count + sizeof(int) * 3 * Position.Count + 
            sizeof(byte) * Hiden.Count];
        var offset = 0;
        offset += ByteConverter.AddToStream(Size, result, offset);
        for (int i = 0; i < Size; i++)
        {
            offset += ByteConverter.AddToStream(Type[i], result, offset);
            offset += ByteConverter.AddToStream(Position[i], result, offset);
            offset += ByteConverter.AddToStream(Hiden[i], result, offset);
        }
        return result;
    }

    public static RubbishData Deserialize(byte[] data)
    {
        var offset = 0;
        var result = new RubbishData();
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Size);
        for (int i = 0; i < result.Size; i++)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out byte type);
            result.Type.Add(type);
            offset += ByteConverter.ReturnFromStream(data, offset, out Vector3 position);
            result.Position.Add(position);
            offset += ByteConverter.ReturnFromStream(data, offset, out bool hiden);
            result.Hiden.Add(hiden);
        }
        return result;
    }
}
