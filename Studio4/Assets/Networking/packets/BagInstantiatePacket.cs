using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInstantiatePacket : BasePacket
{
    public List <int> prefabIndex { get; private set; }
    public List<string> objectIDs { get; private set; }

    public BagInstantiatePacket() : base()
    {
        prefabIndex = new List<int>();
        objectIDs = new List<string>();
    }
    public BagInstantiatePacket(PlayerData player, List<int> prefabIndex, List<string> objectIDs, string GameObjectID) : base(player, PackType.indexInstantiate, GameObjectID)
    {
        this.prefabIndex = prefabIndex;
        this.objectIDs = objectIDs;
    }

    public byte[] Serialize()
    {
        BeginSerialization();

        writer.Write(prefabIndex.Count);
        writer.Write(objectIDs.Count);
        for (int i = 0; i < prefabIndex.Count; i++)
        {
            writer.Write(prefabIndex[i]);
            writer.Write(objectIDs[i]);
        }
        return FinishSerialization();

    }
    public new BagInstantiatePacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);
        int listCount = reader.ReadInt32();
        int IDListCount= reader.ReadInt32();
        prefabIndex = new List<int>(listCount);
        objectIDs = new List<string>(listCount);
        for (int i = 0; i < listCount; i++)
        {
            prefabIndex.Add(reader.ReadInt32());
            objectIDs.Add(reader.ReadString());
        }
        return this;
    }
}
