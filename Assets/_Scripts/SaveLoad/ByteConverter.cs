using System;
using UnityEngine;

public static class ByteConverter
{
    public static int AddToStream(byte source, byte[] destination, int offset)
    {
        destination[offset] = source;
        return 1;
    }
    public static int AddToStream(bool source, byte[] destination, int offset)
    {
        destination[offset] = Convert.ToByte(source);
        return 1;
    }
    public static int AddToStream(int source, byte[] destination, int offset)
    {
        byte byteOffset = 8;
        byte maxIntOffser = 24;
        for (int i = 0; i < 4; i++)
        {
            destination[offset + i] = (byte)(source >> maxIntOffser - byteOffset * i);
        }
        return 4;
    }
    public static int AddToStream(float source, byte[] destination, int offset)
    {
        int number = (int)Mathf.Round(source * 1000000);
        AddToStream(number, destination, offset);
        return 4;
    }

    public static int AddToStream(Vector3 source, byte[] destination, int offset)
    {
        AddToStream(source.x, destination, offset);
        AddToStream(source.y, destination, offset + 4);
        AddToStream(source.z, destination, offset + 8);
        return 12;
    }

    public static int ReturnFromStream(byte[] source, int offset, out byte destination)
    {
        destination = source[offset];
        return 1;
    }
    public static int ReturnFromStream(byte[] source, int offset, out bool destination)
    {
        destination = Convert.ToBoolean(source[offset]);
        return 1;
    }
    public static int ReturnFromStream(byte[] source, int offset, out int destination)
    {

        byte byteOffset = 8;
        byte maxIntOffser = 24;
        destination = 0;
        for (int i = 0; i < 4; i++)
        {
            destination |= (source[offset + i] << maxIntOffser - byteOffset * i);
        }
        return 4;
    }

    public static int ReturnFromStream(byte[] source, int offset, out float destination)
    {
        int number;
        ReturnFromStream(source, offset, out number);
        destination = number / 1000000.0f;
        return 4;
    }

    public static int ReturnFromStream(byte[] source, int offset, out Vector3 destination)
    {
        float x;
        float y;
        float z;
        ReturnFromStream(source, offset, out x);
        ReturnFromStream(source, offset + 4, out y);
        ReturnFromStream(source, offset + 8, out z);
        destination = new Vector3(x, y, z);
        return 12;
    }
}
