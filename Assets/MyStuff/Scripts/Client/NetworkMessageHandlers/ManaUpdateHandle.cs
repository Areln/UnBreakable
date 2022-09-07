
public class ManaUpdateHandle : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.manaUpdate;
    }

    public void ReadMessage(Packet _packet)
    {
        int characterId = _packet.ReadInt();
        int manaChange = _packet.ReadInt();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            var character = GameManager.Instance.GetCharacter(characterId);
            character.ChangeMana(manaChange);
        });
    }
}
