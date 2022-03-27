using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerPlayerInventory : MonoBehaviour
    {
        public Transform EquipmentHolder;

        public ItemArmor EquippedHelmetPiece;
        public Transform HeadTransform;

        public ItemArmor EquippedChestPiece;
        public Transform ChestPieceTransform;

        public ItemArmor EquippedGlovePiece;
        public Transform GlovePieceTransform;

        public ItemArmor EquippedLegPiece;
        public Transform LegPieceTransform;

        public ItemArmor EquippedNecklacePiece;
        public Transform NecklacePieceTransform;

        public ItemArmor EquippedRing1Piece;
        public Transform Ring1PieceTransform;

        public ItemArmor EquippedRing2Piece;
        public Transform Ring2PieceTransform;

        public ItemWeapon EquippedMainHandWeapon;
        public Transform MainHandWeaponTransform;

        public ItemWeapon EquippedOffHandWeapon;
        public Transform OffHandWeaponTransform;

        //array 
        public List<Item> InventoryItems = new List<Item>();

        string[] serverInventory;
        internal Dictionary<ArmorType, ItemEquipable> serverArmorEquipSlots = new Dictionary<ArmorType, ItemEquipable>();
        internal Dictionary<WeaponType, ItemEquipable> serverWeaponEquipSlots = new Dictionary<WeaponType, ItemEquipable>();

        void Awake()
        {
            serverInventory = new string[25];
        }

        // Server Functions
        #region 
        
        public void DropAllItemsFromInventory() 
        {
            
        }

        public void AddToServerInventory(int index, string internalItemName) 
        {
            serverInventory[index] = internalItemName;
        }
        public string ServerGetInventoryItemFromIndex(int index) 
        {
            return serverInventory[index];
        }
        public void ServerEquipItem(ItemEquipable item, int? slotIndex = null)
        {
            ServerUnEquipItem(item, slotIndex);

            if (typeof(ItemArmor).IsAssignableFrom(item.GetType()))
            {
                serverArmorEquipSlots.Add(item.GetComponent<ItemArmor>().ArmorType, Instantiate(item, EquipmentHolder).GetComponent<ItemEquipable>());
            }
            else if (typeof(ItemWeapon).IsAssignableFrom(item.GetType()))
            {
                serverWeaponEquipSlots.Add(item.GetComponent<ItemWeapon>().WeaponType, Instantiate(item, EquipmentHolder).GetComponent<ItemEquipable>());
            }
        }
        public void ServerEquipItem(ArmorType armorType, string internalName) 
        {
            ServerUnEquipItem(armorType);
            serverArmorEquipSlots.Add(armorType, Instantiate(GameManager.Instance.GetItem(internalName), EquipmentHolder).GetComponent<ItemEquipable>());
        }
        public void ServerEquipItem(WeaponType weaponType, string internalName)
        {
            ServerUnEquipItem(weaponType);
            serverWeaponEquipSlots.Add(weaponType, Instantiate(GameManager.Instance.GetItem(internalName), EquipmentHolder).GetComponent<ItemEquipable>());
        }
        public void ServerUnEquipItem(ItemEquipable item, int? slotIndex = null) 
        {
            if (typeof(ItemArmor).IsAssignableFrom(item.GetType()))
            {
                ServerUnEquipItem(item.GetComponent<ItemArmor>().ArmorType, slotIndex);
            }
            else if (typeof(ItemWeapon).IsAssignableFrom(item.GetType()))
            {
                ServerUnEquipItem(item.GetComponent<ItemWeapon>().WeaponType, slotIndex);
            }
        }
        public void ServerUnEquipItem(ArmorType armorType, int? slotIndex = null)
        {
            if (serverArmorEquipSlots.TryGetValue(armorType, out ItemEquipable _itemEquipable))
            {
                serverArmorEquipSlots.Remove(armorType);

                if (slotIndex == null)
                {
                    slotIndex = FindFirstOpenItemSlot();       
                }

                if (slotIndex == null)
                {
                    // TODO: send message that we cant unequip due to inventory space
                }
                else
                {
                    serverInventory[slotIndex.Value] = _itemEquipable.InternalName;
                }

                Destroy(_itemEquipable.gameObject);
            }
        }
        public void ServerUnEquipItem(WeaponType weaponType, int? slotIndex = null)
        {
            if (serverWeaponEquipSlots.TryGetValue(weaponType, out ItemEquipable _itemEquipable))
            {
                serverWeaponEquipSlots.Remove(weaponType);

                if (slotIndex == null)
                {
                    slotIndex = FindFirstOpenItemSlot();
                }

                if (slotIndex == null)
                {
                    // TODO: send message that we cant unequip due to inventory space
                }
                else
                {
                    serverInventory[slotIndex.Value] = _itemEquipable.InternalName;
                }

                Destroy(_itemEquipable.gameObject);
            }
        }
        #endregion

        // Client functions
        #region
        public int? FindFirstOpenItemSlot()
        {
            for (int i = 0; i < serverInventory.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(serverInventory[i]))
                {
                    return i;
                }
            }
            return null;
        }

        public void AddPrefabItemObjectToPlayerInventory(GameObject itemPrefab)
        {
            // Instantiates Item's gameobject and sets the parent as the InventoryHolder
            GameObject _tempObject = Instantiate(itemPrefab, EquipmentHolder);

            // Grab reference to the item script
            Item _tempItem = _tempObject.GetComponent<Item>();

            _tempItem.OnItemPickup();

            InventoryItems.Add(_tempItem);
        }

        public void EquipItemToCharacter(ItemEquipable item)
        {

            bool successfullEquip = false;

            if (typeof(ItemArmor).IsAssignableFrom(item.GetType()))
            {
                successfullEquip = EquipArmor(item.GetComponent<ItemArmor>());
            }
            else if (typeof(ItemWeapon).IsAssignableFrom(item.GetType()))
            {
                successfullEquip = EquipWeapon(item.GetComponent<ItemWeapon>());
            }

            if (successfullEquip)
            {
                item.OnEquip();
            }
        }
        bool EquipWeapon(ItemWeapon itemWeapon)
        {
            ItemWeapon _tempWeapon = GetEquippedWeapon(itemWeapon.WeaponType);

            if (_tempWeapon)
            {
                UnEquipWeapon(itemWeapon.WeaponType);
            }

            switch (itemWeapon.WeaponType)
            {
                case WeaponType.MainHand:
                    itemWeapon.gameObject.transform.SetParent(MainHandWeaponTransform, false);
                    EquippedMainHandWeapon = itemWeapon;
                    //GetComponent<ServerPlayerBrain>().SetWeaponHitBox(itemWeapon.GetComponent<Weapon>().WeaponHitBox);
                    return true;
                case WeaponType.OffHand:
                    itemWeapon.gameObject.transform.SetParent(OffHandWeaponTransform, false);
                    EquippedOffHandWeapon = itemWeapon;
                    return true;
                default:
                    break;
            }

            return false;
        }
        void UnEquipWeapon(WeaponType weaponType)
        {
            ItemWeapon _tempSel = default;

            switch (weaponType)
            {
                case WeaponType.MainHand:
                    _tempSel = EquippedMainHandWeapon;
                    EquippedMainHandWeapon = null;
                    break;
                case WeaponType.OffHand:
                    _tempSel = EquippedOffHandWeapon;
                    EquippedOffHandWeapon = null;
                    break;
                default:
                    break;
            }

            _tempSel.OnUnEquip();
            _tempSel.gameObject.transform.SetParent(EquipmentHolder, false);
        }
        bool EquipArmor(ItemArmor itemArmor)
        {
            ItemArmor _tempArmor = GetEquippedArmor(itemArmor.ArmorType);

            if (_tempArmor)
            {
                UnEquipArmor(itemArmor.ArmorType);
            }

            switch (itemArmor.ArmorType)
            {
                case ArmorType.HeadPiece:
                    itemArmor.gameObject.transform.SetParent(HeadTransform, false);
                    EquippedHelmetPiece = itemArmor;
                    return true;
                case ArmorType.ChestPiece:
                    itemArmor.gameObject.transform.SetParent(ChestPieceTransform, false);
                    EquippedChestPiece = itemArmor;
                    return true;
                case ArmorType.GlovePiece:
                    itemArmor.gameObject.transform.SetParent(GlovePieceTransform, false);
                    EquippedGlovePiece = itemArmor;
                    return true;
                case ArmorType.LegPiece:
                    itemArmor.gameObject.transform.SetParent(LegPieceTransform, false);
                    EquippedLegPiece = itemArmor;
                    return true;
                case ArmorType.RingR:
                    itemArmor.gameObject.transform.SetParent(Ring1PieceTransform, false);
                    EquippedRing1Piece = itemArmor;
                    return true;
                case ArmorType.RingL:
                    itemArmor.gameObject.transform.SetParent(Ring2PieceTransform, false);
                    EquippedRing1Piece = itemArmor;
                    return true;
                case ArmorType.Necklace:
                    itemArmor.gameObject.transform.SetParent(NecklacePieceTransform, false);
                    EquippedRing2Piece = itemArmor;
                    return true;
                default:
                    break;
            }

            return false;
        }
        void UnEquipArmor(ArmorType armorType, ItemSlot itemSlot = null)
        {
            ItemArmor _tempSel = EquippedChestPiece;

            switch (armorType)
            {
                case ArmorType.HeadPiece:
                    _tempSel = EquippedHelmetPiece;
                    EquippedHelmetPiece = null;
                    break;
                case ArmorType.ChestPiece:
                    _tempSel = EquippedChestPiece;
                    EquippedChestPiece = null;
                    break;
                case ArmorType.GlovePiece:
                    _tempSel = EquippedGlovePiece;
                    EquippedGlovePiece = null;
                    break;
                case ArmorType.LegPiece:
                    _tempSel = EquippedLegPiece;
                    EquippedLegPiece = null;
                    break;
                case ArmorType.RingR:
                    _tempSel = EquippedRing1Piece;
                    EquippedRing1Piece = null;
                    break;
                case ArmorType.RingL:
                    _tempSel = EquippedRing2Piece;
                    EquippedRing1Piece = null;
                    break;
                case ArmorType.Necklace:
                    _tempSel = EquippedNecklacePiece;
                    EquippedNecklacePiece = null;
                    break;
                default:
                    break;
            }

            _tempSel.OnUnEquip();
            if (itemSlot)
            {
                _tempSel.gameObject.transform.SetParent(EquipmentHolder, false);
                itemSlot.SetSlottedItem(_tempSel);
            }
            else
            {
                _tempSel.gameObject.transform.SetParent(null, false);
            }
        }
        ItemArmor GetEquippedArmor(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.HeadPiece:
                    return EquippedHelmetPiece;
                case ArmorType.ChestPiece:
                    return EquippedChestPiece;
                case ArmorType.GlovePiece:
                    return EquippedGlovePiece;
                case ArmorType.LegPiece:
                    return EquippedLegPiece;
                case ArmorType.RingR:
                    return EquippedRing1Piece;
                case ArmorType.RingL:
                    return EquippedRing2Piece;
                case ArmorType.Necklace:
                    return EquippedNecklacePiece;
                default:
                    break;
            }
            return null;
        }
        ItemWeapon GetEquippedWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.MainHand:
                    return EquippedMainHandWeapon;
                case WeaponType.OffHand:
                    return EquippedOffHandWeapon;
                default:
                    break;
            }
            return null;
        }
        #endregion
    }
}


