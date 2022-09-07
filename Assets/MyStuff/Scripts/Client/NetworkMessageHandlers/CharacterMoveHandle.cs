using System;
using UnityEngine;

public class CharacterMoveHandle : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.moveCharacter;
    }

    public void ReadMessage(Packet _packet)
    {
        int characterIdToMove = _packet.ReadInt();
        int numberOfPathCorners = _packet.ReadInt();
        MoveData[] moveData = new MoveData[numberOfPathCorners];
        for (int i = 0; i < numberOfPathCorners; i++)
        {
            moveData[i] = new MoveData()
            {
                Position = new Vector3(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat()),
                Rotation = _packet.ReadFloat(),
            };
        }
        ThreadManager.ExecuteOnMainThread(() =>
        {
            var characterMoving = GameManager.Instance.GetCharacter(characterIdToMove);
            if (characterMoving != null)
            {
                characterMoving.SetCharacterPath(moveData);
            }
        });
    }

    public void WriteMessage(Vector3 targetPosition)
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(targetPosition.x);
            _packet.Write(targetPosition.y);
            _packet.Write(targetPosition.z);

            ClientSend.SendTcpData(_packet);
        }
    }
}

public class MoveData
{
    public Vector3 Position { get; set; }
    public float Rotation { get; set; }
}
