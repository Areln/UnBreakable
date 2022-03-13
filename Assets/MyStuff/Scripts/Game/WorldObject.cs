using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    public string objectName;

    public abstract void Activate(PlayerBrain pb);
}
