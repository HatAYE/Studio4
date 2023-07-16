using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPacket : BasePacket
{
    public int objectId { get; private set; }

    public DestroyPacket() : base()
    {
        objectId = -1;
    }

    public DestroyPacket(PlayerData player, int objectId) : base(player, PackType.destroy)
    {
        this.objectId = objectId;
    }

    public byte[] Serialize()
    {
        BeginSerialization();
        writer.Write(objectId);
        return FinishSerialization();
    }

    public new DestroyPacket Deserialize(byte[] buffer)
    {
        Deserialize(buffer);
        objectId = reader.ReadInt32();
        return this;
    }
}
