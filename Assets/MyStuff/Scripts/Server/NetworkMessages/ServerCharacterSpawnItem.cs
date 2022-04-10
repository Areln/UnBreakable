
using System.Linq;

namespace Server.Networking
{
	public class ServerCharacterSpawnItem : IServerHandle
	{
		string[] AdminNames = new string[] { "yo", "CrimOudin", "Syzzurpp" };

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
						WriteMessage(_fromClientId, character, slotIndex.Value, character.GetComponent<ServerPlayerInventory>().AddToServerInventory(slotIndex.Value, itemInternalName, 1));
					}
				}
			});
		}

		public void WriteMessage(int clientId, ServerCharacterBrain character, int slotIndex, StorageData itemStorageData)
		{
			using (Packet _packet = new Packet(GetMessageId()))
			{
				_packet.Write(character.GetInstanceID());
				_packet.Write(slotIndex);
				_packet.Write(itemStorageData);

				ServerSend.SendTcpDataAuthenticated(clientId, _packet);
			}
		}
	}
}
