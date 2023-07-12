using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Util
{
    public static byte[] SerializeVector3(Vector3 vector)
    {
        MemoryStream stream= new MemoryStream();
        BinaryWriter bw= new BinaryWriter(stream);
        bw.Write(vector.x);
        bw.Write(vector.y);
        bw.Write(vector.z);  
        return stream.ToArray();
    }
    public static Vector3 DeserializeVector3(byte[] data)
    {
        MemoryStream stream = new MemoryStream(data);
        BinaryReader br = new BinaryReader(stream);

        Vector3 vector= new Vector3();
        vector.x = br.ReadSingle();
        vector.y = br.ReadSingle();
        vector.z = br.ReadSingle();
        return vector;
    }
    void Update()
    {
        
    }

}
