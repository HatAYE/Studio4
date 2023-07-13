using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class BasePacket
{
    protected MemoryStream writeStream;
    protected BinaryWriter writer;
    protected MemoryStream readStream;
    protected BinaryReader reader;
    PlayerData player;
    protected enum PackType
    {
        none, instantiate, destroy, animation
    }
    protected PackType packType;
    public BasePacket()
    {
        player= new PlayerData("","");
        packType = PackType.none;
    }
    protected BasePacket(PlayerData player,PackType packType)
    {
        this.player = player;
        this.packType = packType;
    }
    public void BeginSerialization()
    {
        writeStream = new MemoryStream();
        writer = new BinaryWriter(writeStream);
        writer.Write(player.playerID);
        writer.Write(player.playerName);
        writer.Write((int)packType);
    }
    public byte[] FinishSerialization()
    {
        return writeStream.ToArray();
    }
    public void Deserialize(byte[] buffer)
    {
        readStream = new MemoryStream(buffer);
        reader= new BinaryReader(readStream);
        player= new PlayerData(reader.ReadString(), reader.ReadString());
        packType= (PackType)reader.ReadInt32();
    }

}
