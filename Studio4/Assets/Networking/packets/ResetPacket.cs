using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPacket : BasePacket
{
    public ResetPacket() : base()
    {

    }
    public ResetPacket(PlayerData player, string GameObjectID) : base(player, PackType.reset, GameObjectID)
    {

    }

    public byte[] Serialize()
    {
        BeginSerialization();
        return FinishSerialization();

    }
    public new ResetPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);
        return this;
    }
}
