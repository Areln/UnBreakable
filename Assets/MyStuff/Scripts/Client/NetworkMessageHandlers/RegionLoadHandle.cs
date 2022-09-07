
using System.Collections.Generic;

public class RegionLoadHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.loadRegions;
	}

	public void ReadMessage(Packet _packet)
	{
		List<Coordinates> regionsToKeep = new List<Coordinates>();
		var numberOfRegions = _packet.ReadInt();
		for (int i = 0; i < numberOfRegions; i++)
		{
			var coords = new Coordinates()
			{
				X = _packet.ReadInt(),
				Y = _packet.ReadInt(),
			};
			GameManager.Instance.LoadRegion(coords);
			regionsToKeep.Add(coords);
		}

		GameManager.Instance.DestroyFarRegions(regionsToKeep);
	}
}
