using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionData : IData
{
    public int Version;
    public byte[] Serialize()
    {
        var result = new byte[sizeof(int)];
        var offset = 0;
        offset += ByteConverter.AddToStream(Version, result, offset);
        return result;
    }

    public static VersionData Deserialize(byte[] data)
    {
        var offset = 0;
        var result = new VersionData();
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Version);
        return result;
    }
}
