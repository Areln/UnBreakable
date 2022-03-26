
using System.Linq;

namespace Server.Networking
{
    public class ServerCharacterSpawnItem : IServerHandle
    {
        string[] AdminNames = new string[] {"yo", "CrimOudin", "Syzzurpp"};

        public int GetMessageId()
        {
            return (int)Packets.CharacterSpawnItem;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            string itemInternalName = _packet.ReadString();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);

            if (AdminNames.Any(x => x.Equals(character.characterName, System.StringComparison.OrdinalIgnoreCase)))
                {
                    int? slotIndex = character.GetComponent<ServerPlayerInventory>().FindFirstOpenItemSlot();

                    if (slotIndex != null)
                    {
                        character.GetComponent<ServerPlayerInventory>().AddToServerInventory(slotIndex.Value, itemInternalName);
                        WriteMessage(_fromClientId, character, slotIndex.Value, itemInternalName);
                    }
                }
            });
        }

        public void WriteMessage(int clientId, ServerCharacterBrain character, int slotIndex, string itemInternalName)
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(character.GetInstanceID());
                _packet.Write(slotIndex);
                _packet.Write(itemInternalName);

                ServerSend.SendTcpDataAuthenticated(clientId, _packet);
            }
        }
    }
}
