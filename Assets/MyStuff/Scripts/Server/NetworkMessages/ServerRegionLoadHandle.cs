using System.Collections.Generic;

namespace Server.Networking
{
	public class ServerRegionLoadHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.loadRegions;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			throw new System.NotImplementedException();
		}

		public void WriteMessage(int _toClient, IList<Coordinates> coordinatesToLoad)
		{
			using (Packet _packet = new Packet((int)Packets.loadRegions))
			{
				_packet.Write(coordinatesToLoad.Count);
				foreach(var coord in coordinatesToLoad)
				{
					_packet.Write(coord.X);
					_packet.Write(coord.Y);
				}

				ServerSend.SendTcpData(_toClient, _packet);
			}
		}
	}
}
