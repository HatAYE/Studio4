
using UnityEngine;

public class MovementPacket : BasePacket
{
    public Vector2 position { get; private set; }
    public int posIndex { get; private set; }

    public MovementPacket() : base()
    {
        position = Vector2.zero;
    }
    
    public MovementPacket( PlayerData player, Vector2 position, int posIndex, string GameObjectID) : base(player, PackType.movement, GameObjectID )
    {
        this.position = position;
        this.posIndex = posIndex;
    }

    public byte[] Serialize()
    {
        BeginSerialization();

        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(posIndex);
        return FinishSerialization();
    }

    public new MovementPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);

        position = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        posIndex= reader.ReadInt32();

        return this;
    }
}
