using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;

    private static object m_Lock = new object();
    public static GameManager Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (GameManager)FindObjectOfType(typeof(GameManager));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";

                        // Make instance persistent.
                        //DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            };
        }
    }

    public PlayerBrain clientPlayer;

    public List<GameObject> PossibleItems = new List<GameObject>();

    static Dictionary<string, GameObject> ItemDirectory = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (GameObject item in PossibleItems)
        {
            ItemDirectory.Add(item.GetComponent<Item>().InternalName, item);
        }
    }

    public GameObject SearchItems(string itemName)
    {
        GameObject returnItem;
        
        ItemDirectory.TryGetValue(itemName, out returnItem);

        return returnItem;
    }

}
