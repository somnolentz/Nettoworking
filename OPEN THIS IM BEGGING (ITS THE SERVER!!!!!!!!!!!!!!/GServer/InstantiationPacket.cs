using NetworkingLibrary;
using System.Text;
using UnityEngine;

public class InstantiationPacket : BasePacket
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
    public string OwnershipID;

    public InstantiationPacket()
    {
        prefabName = string.Empty;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public InstantiationPacket(string _prefabName, Vector3 _position, Quaternion _rotation, string gameObjectID = "", string ownershipID ="")
        : base(PacketType.Instantiation, gameObjectID)
    {
        prefabName = _prefabName;
        position = _position;
        rotation = _rotation;
        OwnershipID = ownershipID;

    }

    public new byte[] Serialize()
    {
        base.Serialize();

        binaryWriter.Write(prefabName);

        binaryWriter.Write(position.x);
        binaryWriter.Write(position.y);
        binaryWriter.Write(position.z);

        binaryWriter.Write(rotation.x);
        binaryWriter.Write(rotation.y);
        binaryWriter.Write(rotation.z);
        binaryWriter.Write(rotation.w);
        binaryWriter.Write((ushort)OwnershipID.Length);
        binaryWriter.Write(Encoding.UTF8.GetBytes(OwnershipID));

        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new InstantiationPacket Deserialize(byte[] dataToDeserialize, int index)
    {
        base.Deserialize(dataToDeserialize, index);

        prefabName = binaryReader.ReadString();
        position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        ushort ownershipIDLength = binaryReader.ReadUInt16();
        byte[] ownershipIDBytes = binaryReader.ReadBytes(ownershipIDLength);
        OwnershipID = Encoding.UTF8.GetString(ownershipIDBytes);

        return this;
    }
}