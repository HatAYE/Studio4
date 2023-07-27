
using UnityEngine;

public class InstantiatePacket : BasePacket
{
    public string prefabName { get; private set; }
    public Vector3 position { get; private set; }
    public Quaternion rotation { get; private set; }
    public int prefabIndex { get; private set; }

    public InstantiatePacket() : base()
    {
        prefabName = string.Empty;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
    public InstantiatePacket(PlayerData player, int prefabIndex, string GameObjectID, string prefabName, Vector3 position, Quaternion rotation) : base(player, PackType.instantiate, GameObjectID)
    {
        this.prefabIndex = prefabIndex;
        this.prefabName = prefabName;
        this.position = position;
        this.rotation = rotation;
    }

    public byte[] Serialize()
    {
        BeginSerialization();

        writer.Write(prefabIndex);
        writer.Write(prefabName);
        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(position.z);

        writer.Write(rotation.x);
        writer.Write(rotation.y);
        writer.Write(rotation.z);
        writer.Write(rotation.w);

        return FinishSerialization();

    }
    public new InstantiatePacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);
        prefabIndex = reader.ReadInt32();
        prefabName = reader.ReadString();
        position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        rotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        return this;
    }
}