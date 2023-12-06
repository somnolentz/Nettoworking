using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkingLibrary;
using System.IO;
using System.Text;

public class MessagePacket : BasePacket
{
    //initializing
    public string Message { get; private set; }
    //setting a const to initialize 
    public MessagePacket(string message, string gameObjectID = "") : base(PacketType.Message, gameObjectID) //overwriting const of base
    {
        Message = message; //initializing and setting the message string
    }

    public MessagePacket()
    {
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(Message);
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }
    public new MessagePacket Deserialize(byte[] dataToDeserialize, int index)
    {
        try
        {
            base.Deserialize(dataToDeserialize, index);
            Message = binaryReader.ReadString();
            return this;
        }
        catch (EndOfStreamException ex)
        {
            Debug.LogError(ex);
            return null;
        }
    }
}