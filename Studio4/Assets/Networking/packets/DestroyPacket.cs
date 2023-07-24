using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPacket : BasePacket
{
    public int itemID { get; private set; }

    public DestroyPacket() : base()
    {
        itemID = -1;
    }

    public DestroyPacket(PlayerData player, string GameObjectID, int objectId) : base(player, PackType.destroy, GameObjectID)
    {
        this.itemID = objectId;
    }

    public byte[] Serialize()
    {
        BeginSerialization();
        writer.Write(itemID);
        return FinishSerialization();
    }

    public new DestroyPacket Deserialize(byte[] buffer)
    {
        Deserialize(buffer);
        itemID = reader.ReadInt32();
        return this;
    }
}
