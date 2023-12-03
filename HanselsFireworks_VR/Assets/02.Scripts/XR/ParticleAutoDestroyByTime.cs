using System;
using UnityEngine;
namespace VR
{
    public class ParticleAutoDestroyByTime : MonoBehaviour
    {
        private ParticleSystem particle;

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (particle.isPlaying == false)
            {
                Destroy(gameObject);
            }
        }
    }

}
