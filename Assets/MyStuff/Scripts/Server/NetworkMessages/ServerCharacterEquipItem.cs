
namespace Server.Networking
{
    public class ServerCharacterEquipItem : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CharacterEquipItem;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            int itemSlotIndex = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
                ServerPlayerInventory charInv = character.GetComponent<ServerPlayerInventory>();

                string itemInternalName = charInv.ServerGetInventoryItemFromIndex(itemSlotIndex);

                ItemEquipable item = ServerGameManager.Instance.SearchItems(itemInternalName).GetComponent<ItemEquipable>();

                // TODO: check if this is possible to do ex. stat check

                charInv.ServerEquipItem(item, itemSlotIndex);

                WriteMessage(_fromClientId, itemSlotIndex, character.GetInstanceID(), itemInternalName);
            });
        }
        void WriteMessage(int _fromClientId, int itemSlotIndex, int characterId, string itemInternalName)
        {
            //packet to fromclient
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(true);
                _packet.Write(itemSlotIndex);

                ServerSend.SendTcpDataAuthenticated(_fromClientId, _packet);
            }
            //packet to everyone else
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(false);
                _packet.Write(characterId);
                _packet.Write(itemInternalName);

                ServerSend.SendTcpDataToAllAuthenticated(_fromClientId, _packet);
            }
        }
    }
}
