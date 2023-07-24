using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationPacket : BasePacket
{
    public string animationName { get; private set; }

    public AnimationPacket() : base()
    {
        animationName = string.Empty;
    }

    public AnimationPacket(PlayerData player, string GameObjectID, string animationName) : base(player, PackType.animation, GameObjectID)
    {
        this.animationName = animationName;
    }

    public byte[] Serialize()
    {
        BeginSerialization();
        writer.Write(animationName);
        return FinishSerialization();
    }

    public new AnimationPacket Deserialize(byte[] buffer)
    {
        Deserialize(buffer);
        animationName = reader.ReadString();
        return this;
    }
}
