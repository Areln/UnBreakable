public class ItemConsumable : Item
{
    //public ItemType ItemType;
    //public string ItemName;
    //public string Description;
    //public Sprite ItemSprite;
    //public int MonetaryValue;
    public int UseCount;

    public virtual void OnConsume() 
    {
        UseCount -= 1;   
    }

}
