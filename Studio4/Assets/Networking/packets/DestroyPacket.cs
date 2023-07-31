using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPacket : BasePacket
{
    public string GameObjectID { get; private set; }

    public DestroyPacket() : base()
    {
        GameObjectID = string.Empty;
    }

    public DestroyPacket(PlayerData player, string GameObjectID) : base(player, PackType.destroy, GameObjectID)
    {
        this.GameObjectID = GameObjectID;
    }

    public byte[] Serialize()
    {
        BeginSerialization();
        writer.Write(GameObjectID);
        return FinishSerialization();

    }

    public new DestroyPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);
        GameObjectID = reader.ReadString();
        return this;
    }
}
