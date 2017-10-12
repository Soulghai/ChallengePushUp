using UnityEngine;

namespace UnityStandardAssets.Effects
{
    public class ExtinguishableParticleSystem : MonoBehaviour
    {
        private ParticleSystem[] m_Systems;
        public float multiplier = 1;


        private void Start()
        {
            m_Systems = GetComponentsInChildren<ParticleSystem>();
        }


        public void Extinguish()
        {
            foreach (var system in m_Systems)
            {
                var emission = system.emission;
                emission.enabled = false;
            }
        }
    }
}