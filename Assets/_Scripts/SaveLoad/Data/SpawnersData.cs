using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersData : IData
{
    public int Size;
    public List<Vector3> Positions = new List<Vector3>();
    public List<byte> Types = new List<byte>();
    public List<bool> CanMove = new List<bool>();
    public List<bool> Active = new List<bool>();
    public List<int> PointsCount = new List<int>();
    public List<Vector3> Points = new List<Vector3>();
    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) + sizeof(int) * 3 * Positions.Count +
            sizeof(byte) * Types.Count + sizeof(byte) * CanMove.Count +
            sizeof(int) * PointsCount.Count + sizeof(int) * 3 * Points.Count + sizeof(byte) * Active.Count];
        var offset = 0;
        offset += ByteConverter.AddToStream(Size, result, offset);
        for (int i = 0; i < Size; i++)
        {
            offset += ByteConverter.AddToStream(Positions[i], result, offset);
            offset += ByteConverter.AddToStream(Types[i], result, offset);
            offset += ByteConverter.AddToStream(CanMove[i], result, offset);
            offset += ByteConverter.AddToStream(PointsCount[i], result, offset);
            for (int j = 0; j < PointsCount[i]; j++)
            {
                offset += ByteConverter.AddToStream(Points[j], result, offset);
            }
            offset += ByteConverter.AddToStream(Active[i], result, offset);
        }
        return result;
    }

    public static SpawnersData Deserialize(byte[] data)
    {
        var offset = 0;
        var result = new SpawnersData();
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Size);
        for (int i = 0; i < result.Size; i++)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out Vector3 position);
            result.Positions.Add(position);
            offset += ByteConverter.ReturnFromStream(data, offset, out byte type);
            result.Types.Add(type);
            offset += ByteConverter.ReturnFromStream(data, offset, out bool canMove);
            result.CanMove.Add(canMove);
            offset += ByteConverter.ReturnFromStream(data, offset, out int count);
            result.PointsCount.Add(count);
            for (int j = 0; j < result.PointsCount[i]; j++)
            {
                offset += ByteConverter.ReturnFromStream(data, offset, out Vector3 point);
                result.Points.Add(point);
            }
            offset += ByteConverter.ReturnFromStream(data, offset, out bool active);
            result.Active.Add(active);
        }
        //Debug.Log(result.Size);
        return result;
    }
}
