using System.Collections;
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

    public PlayerBrain playerBrain;

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
    public ItemSlot EquippedKnecklacePieceSlot;
    public List<ItemSlot> InventoryItemSlots = new List<ItemSlot>();
    public GameObject ItemSlotPrefab;
    public Transform ItemSlotHolder;

    public void EquippedItem(ItemEquippable itemEquippable) 
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
                case ArmorType.Ring:
                    EquippedRing1PieceSlot.SetSlottedItem(itemEquippable);
                    break;
                case ArmorType.Knecklace:
                    EquippedKnecklacePieceSlot.SetSlottedItem(itemEquippable);
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

    public void UnEquippedItem(ItemEquippable itemEquippable) 
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
                case ArmorType.Ring:
                    EquippedRing1PieceSlot.ClearSlot();
                    EquippedRing2PieceSlot.ClearSlot();
                    break;
                case ArmorType.Knecklace:
                    EquippedKnecklacePieceSlot.ClearSlot();
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
            _slot.Setup();
            InventoryItemSlots.Add(_slot);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Health And Mana
        healthText.text = playerBrain.currentHealth.ToString();
        healthSlider.value = playerBrain.currentHealth;
        manaText.text = playerBrain.currentMana.ToString();
        manaSlider.value = playerBrain.currentMana;
        //Update CD Shades
        if (playerBrain.playerCombat.ability1 != null)
        {
            ability1Shade.color = new Color32(0, 69, 185, (byte)ShadeCalculator(playerBrain.playerCombat.ability1.GetCDPercentage()));
        }
        else
        {
            ability1Shade.color = Color.clear;
        }
        if (playerBrain.playerCombat.ability2 != null)
        {
            ability2Shade.color = new Color32(0, 69, 185, (byte)ShadeCalculator(playerBrain.playerCombat.ability2.GetCDPercentage()));
        }
        else
        {
            ability2Shade.color = Color.clear;
        }
        if (playerBrain.playerCombat.ability3 != null)
        {
            ability3Shade.color = new Color32(0, 69, 185, (byte)ShadeCalculator(playerBrain.playerCombat.ability3.GetCDPercentage()));
        }
        else
        {
            ability3Shade.color = Color.clear;
        }
        if (playerBrain.playerCombat.ability4 != null)
        {
            ability4Shade.color = new Color32(0, 69, 185, (byte)ShadeCalculator(playerBrain.playerCombat.ability4.GetCDPercentage()));
        }
        else
        {
            ability4Shade.color = Color.clear;
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
