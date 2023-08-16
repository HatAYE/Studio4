
using UnityEngine;

public class MovementPacket : BasePacket
{
    public Vector3 position { get; private set; }
    public int posIndex { get; private set; }

    public MovementPacket() : base()
    {
        position = Vector3.zero;
    }
    
    public MovementPacket( PlayerData player, Vector3 position, int posIndex, string GameObjectID) : base(player, PackType.movement, GameObjectID )
    {
        this.position = position;
        this.posIndex = posIndex;
    }

    public byte[] Serialize()
    {
        BeginSerialization();

        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(position.z);
        writer.Write(posIndex);
        return FinishSerialization();
    }

    public new MovementPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);

        position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        posIndex= reader.ReadInt32();

        return this;
    }
}
