using System.IO;
using System.Text;
using UnityEngine;

namespace NetworkingLibrary
{
    public class BasePacket
    {
        public string gameObjectID;
        public enum PacketType
        {
            unknown = -1,
            none,
            Position,
            Rotation,
            Instantiation,
            ID,
            PlayerLobbyPacket,
            ServerLobbyPacket,
            ScenePacket,
            PlayerInMainScenePacket,
            Destruction,
            Message
        }

        public PacketType packetType { get; private set; }

        public ushort packetSize { get; private set; }

        protected MemoryStream writeMemoryStream;
        protected MemoryStream readMemoryStream;
        protected BinaryWriter binaryWriter;
        protected BinaryReader binaryReader;

        public BasePacket()
        {
            packetType = PacketType.none;
        }
        public BasePacket(PacketType _packetType, string gameObjectID = "")
        {
            packetType = _packetType;
            this.gameObjectID = gameObjectID;
        }

        public byte[] Serialize()
        {
            writeMemoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(writeMemoryStream);
            binaryWriter.Write(packetSize);
            binaryWriter.Write((int)packetType);
            binaryWriter.Write(gameObjectID);
            return writeMemoryStream.ToArray();
        }
        public BasePacket Deserialize(byte[] dataToDeserialize, int index)
        {
            try
            {
                readMemoryStream = new MemoryStream(dataToDeserialize);
                readMemoryStream.Seek(index, SeekOrigin.Begin);
                binaryReader = new BinaryReader(readMemoryStream);
                packetSize = binaryReader.ReadUInt16();
                packetType = (PacketType)binaryReader.ReadInt32();
                gameObjectID = binaryReader.ReadString();

                return this;
            }
            catch (EndOfStreamException ex)
            {
                Debug.LogError(ex);
            }
            return null;
        }

        protected void FinishSerialization()
        {
            packetSize = (ushort)writeMemoryStream.Length;
            binaryWriter.Seek(-packetSize, SeekOrigin.Current);
            binaryWriter.Write(packetSize);
        }
    }
}
