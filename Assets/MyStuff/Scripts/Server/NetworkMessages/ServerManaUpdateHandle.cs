
namespace Server.Networking
{
    public class ServerManaUpdateHandle : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.manaUpdate;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            throw new System.NotImplementedException();
        }

        public void WriteMessage(int characterId, int manaChange)
        {
            using (var packet = new Packet(GetMessageId()))
            {
                packet.Write(characterId);
                packet.Write(manaChange);

                ServerSend.SendTcpDataToAllAuthenticated(packet);
            }
        }
    }
}
