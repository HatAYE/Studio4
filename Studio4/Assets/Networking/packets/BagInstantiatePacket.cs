using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInstantiatePacket : BasePacket
{
    public List <int> prefabIndex { get; private set; }

    public BagInstantiatePacket() : base()
    {
        prefabIndex = new List<int>();
    }
    public BagInstantiatePacket(PlayerData player, List<int> prefabIndex, string GameObjectID) : base(player, PackType.indexInstantiate, GameObjectID)
    {
        this.prefabIndex = prefabIndex;
    }

    public byte[] Serialize()
    {
        BeginSerialization();

        writer.Write(prefabIndex.Count);
        for (int i = 0; i < prefabIndex.Count; i++)
        {
            writer.Write(prefabIndex[i]);
        }
        return FinishSerialization();

    }
    public new BagInstantiatePacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);
        int listCount = reader.ReadInt32();
        prefabIndex = new List<int>(listCount);
        for (int i = 0; i < listCount; i++)
        {
            prefabIndex.Add(reader.ReadInt32());
        }
        return this;
    }
}
