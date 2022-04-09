namespace Server.Networking
{
	public class ServerCancelActionHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.cancelAction;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			ThreadManager.ExecuteOnMainThread(() =>
			{
				ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
				WriteMessage(character.GetInstanceID(), character.CancelAll());
			});
		}

		public void WriteMessage(int playerCharacterId, bool wasAbilityCanceled)
		{
			using (Packet _packet = new Packet(GetMessageId()))
			{
				_packet.Write(playerCharacterId);
				_packet.Write(wasAbilityCanceled);

				ServerSend.SendTcpDataToAllAuthenticated(_packet);
			}
		}
	}
}
