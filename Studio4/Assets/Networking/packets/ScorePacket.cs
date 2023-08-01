using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePacket : BasePacket
{
    public int gameScore { get; private set; }

    public ScorePacket() : base()
    {
        gameScore = 0;
    }

    public ScorePacket(PlayerData player, string GameObjectID, int gameScore) : base(player, PackType.score, GameObjectID)
    {
        this.gameScore = gameScore;
    }

    public byte[] Serialize()
    {
        BeginSerialization();
        writer.Write(gameScore);
        return FinishSerialization();

    }

    public new ScorePacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);
        gameScore = reader.ReadInt32();
        return this;
    }
}
