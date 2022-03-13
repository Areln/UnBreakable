using UnityEngine;

public class ParticleDestroyCallback : MonoBehaviour
{
    public GameObject BaseDestroy;

    public void OnParticleSystemStopped() 
    {
        if (BaseDestroy)
        {
            Destroy(BaseDestroy);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
