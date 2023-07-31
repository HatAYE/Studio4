
using UnityEngine;

public class MovementPacket : BasePacket
{
    public Vector3 position { get; private set; }

    public MovementPacket() : base()
    {
        position = Vector3.zero;
    }
    
    public MovementPacket( PlayerData player, string GameObjectID, Vector3 position) : base(player, PackType.movement, GameObjectID )
    {
        this.position = position;
    }

    public byte[] Serialize()
    {
        BeginSerialization();

        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(position.z);

        return FinishSerialization();
    }

    public new MovementPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);

        position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        return this;
    }
}
