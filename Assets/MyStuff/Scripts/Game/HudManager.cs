using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{

    private static HudManager m_Instance;

    private static object m_Lock = new object();
    public static HudManager Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (HudManager)FindObjectOfType(typeof(HudManager));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<HudManager>();
                        singletonObject.name = typeof(HudManager).ToString() + " (Singleton)";

                        // Make instance persistent.
                        //DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            };
        }
    }

    internal void OpenStorage(StorageObject storageObject)
    {
        // TODO: load contents network message and display results

    }

    //Canvases
    public Canvas charInvCanvas;
    public Canvas abilityBarCanvas;
    public Canvas chestCanvas;

    //Health And Mana Sliders
    public Slider healthSlider;
    public Slider manaSlider;

    //Health And Mana Text
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    //AbilityIcons
    public Image ability1Icon;
    public Image ability2Icon;
    public Image ability3Icon;
    public Image ability4Icon;

    //Ability Icon Shades
    public Image ability1Shade;
    public Image ability2Shade;
    public Image ability3Shade;
    public Image ability4Shade;

    //Item Dragging
    public Transform ItemDragSlot;

    //
    public ItemSlot EquippedHeadPieceSlot;
    public ItemSlot EquippedChestPieceSlot;
    public ItemSlot EquippedGlovesPieceSlot;
    public ItemSlot EquippedLegsPieceSlot;
    public ItemSlot EquippedRing1PieceSlot;
    public ItemSlot EquippedRing2PieceSlot;
    public ItemSlot EquippedMainHandWeaponSlot;
    public ItemSlot EquippedOffHandWeaponSlot;
    public ItemSlot EquippedNecklacePieceSlot;
    public List<ItemSlot> InventoryItemSlots = new List<ItemSlot>();
    public GameObject ItemSlotPrefab;
    public Transform ItemSlotHolder;

    public void EquippedItem(ItemEquipable itemEquippable) 
    {
        if (typeof(ItemArmor).IsAssignableFrom(itemEquippable.GetType()))
        {
            switch (itemEquippable.GetComponent<ItemArmor>().ArmorType)
            {
                case ArmorType.HeadPiece:
                    EquippedHeadPieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.ChestPiece:
                    EquippedChestPieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.GlovePiece:
                    EquippedGlovesPieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.LegPiece:
                    EquippedLegsPieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.RingR:
                    EquippedRing1PieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.RingL:
                    EquippedRing2PieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.Necklace:
                    EquippedNecklacePieceSlot.SetSlottedItem(itemEquippable);
                    break;
                default:
                    break;
            }
        }
        else if (typeof(ItemWeapon).IsAssignableFrom(itemEquippable.GetType()))
        {
            switch (itemEquippable.GetComponent<ItemWeapon>().WeaponType)
            {
                case WeaponType.MainHand:
                    EquippedMainHandWeaponSlot.SetSlottedItem(itemEquippable);
                    break;
                case WeaponType.OffHand:
                    EquippedOffHandWeaponSlot.SetSlottedItem(itemEquippable);
                    break;
                default:
                    break;
            }
        }
    }

    public void UnEquippedItem(ItemEquipable itemEquippable) 
    {
        if (typeof(ItemArmor).IsAssignableFrom(itemEquippable.GetType()))
        {
            switch (itemEquippable.GetComponent<ItemArmor>().ArmorType)
            {
                case ArmorType.HeadPiece:
                    EquippedHeadPieceSlot.ClearSlot();
                    break;
                case ArmorType.ChestPiece:
                    EquippedChestPieceSlot.ClearSlot();
                    break;
                case ArmorType.GlovePiece:
                    EquippedGlovesPieceSlot.ClearSlot();
                    break;
                case ArmorType.LegPiece:
                    EquippedLegsPieceSlot.ClearSlot();
                    break;
                case ArmorType.RingR:
                    EquippedRing1PieceSlot.ClearSlot();
                    break;
                case ArmorType.RingL:
                    EquippedRing2PieceSlot.ClearSlot();
                    break;
                case ArmorType.Necklace:
                    EquippedNecklacePieceSlot.ClearSlot();
                    break;
                default:
                    break;
            }
        }
        else if (typeof(ItemWeapon).IsAssignableFrom(itemEquippable.GetType()))
        {
            switch (itemEquippable.GetComponent<ItemWeapon>().WeaponType)
            {
                case WeaponType.MainHand:
                    EquippedMainHandWeaponSlot.ClearSlot();
                    break;
                case WeaponType.OffHand:
                    EquippedOffHandWeaponSlot.ClearSlot();
                    break;
                default:
                    break;
            }
        }
    }

    private void Awake()
    {
        // instantiate itemslots for the inventory
        for (int i = 0; i < 25; i++)
        {
            GameObject _tempObject = Instantiate(ItemSlotPrefab, ItemSlotHolder);
            ItemSlot _slot = _tempObject.GetComponent<ItemSlot>();
            _slot.Setup(i);
            InventoryItemSlots.Add(_slot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ClientPlayer)
        {
            //Health And Mana
            healthText.text = GameManager.Instance.ClientPlayer.currentHealth.ToString();
            healthSlider.value = GameManager.Instance.ClientPlayer.currentHealth;
            manaText.text = GameManager.Instance.ClientPlayer.currentMana.ToString();
            manaSlider.value = GameManager.Instance.ClientPlayer.currentMana;
            //Update CD Shades
            foreach(var ability in GameManager.Instance.ClientPlayer.abilities)
			{
                if (ability != null)
                {
                    ability1Shade.color = new Color32(0, 69, 185, (byte)ShadeCalculator(ability.GetCDPercentage()));
                }
                else
                {
                    ability1Shade.color = Color.clear;
                }
            }
        }
    }

    public void OpenChest()
    {

    }

    public void CloseChest()
    {

    }

    public bool ToggleInventory()
    {
        charInvCanvas.enabled = !charInvCanvas.enabled;
        return charInvCanvas.enabled;
    }
    public void ToggleInventory(bool _value)
    {
        charInvCanvas.enabled = _value;
    }

    public void RedrawInventory() 
    {
        
    }

    //calcs alpha value for ability cooldown UI
    public int ShadeCalculator(float cdPercentage)
    {
        if (cdPercentage != 0)
            return 60 + (int)(cdPercentage * 140);
        else
            return 0;
    }
}
