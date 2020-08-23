using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    void Start()
    {
        float dur = GetComponent<ParticleSystem>().main.duration;
        Destroy(gameObject, dur);
    }
}
