
using System.IO;

public class BasePacket
{
    protected MemoryStream writeStream;
    protected BinaryWriter writer;
    protected MemoryStream readStream;
    protected BinaryReader reader;
    public PlayerData player { get; private set; }
    public string GameObjectID;
    public enum PackType
    {
        none, instantiate, destroy, animation
    }
    public PackType packType { get; private set; }
    public BasePacket()
    {
        player= new PlayerData("","");
        GameObjectID = "";
        packType = PackType.none;
    }
    protected BasePacket(PlayerData player,PackType packType, string GameObjectID)
    {
        this.player = player;
        this.packType = packType;
        this.GameObjectID = GameObjectID;
    }
    public void BeginSerialization()
    {
        writeStream = new MemoryStream();
        writer = new BinaryWriter(writeStream);
        writer.Write(player.playerID);
        writer.Write(player.playerName);
        writer.Write((int)packType);
        writer.Write(GameObjectID);
    }
    public byte[] FinishSerialization()
    {
        return writeStream.ToArray();
    }
    public BasePacket Deserialize(byte[] buffer)
    {
        readStream = new MemoryStream(buffer);
        reader= new BinaryReader(readStream);
        player= new PlayerData(reader.ReadString(), reader.ReadString());
        GameObjectID = reader.ReadString();
        packType = (PackType)reader.ReadInt32();
        return this;
    }
    protected void BeginDesrialize(byte[] buffer)
    {
        readStream = new MemoryStream(buffer);
        reader = new BinaryReader(readStream);
        player = new PlayerData(reader.ReadString(), reader.ReadString());
        GameObjectID = reader.ReadString();
        packType = (PackType)reader.ReadInt32();
    }

    protected void EndDesrialize()
    {

    }
}
