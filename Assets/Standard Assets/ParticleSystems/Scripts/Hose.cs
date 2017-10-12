using UnityEngine;

namespace UnityStandardAssets.Effects
{
    public class Hose : MonoBehaviour
    {
        public float changeSpeed = 5;
        public ParticleSystem[] hoseWaterSystems;

        private float m_Power;
        public float maxPower = 20;
        public float minPower = 5;
        public Renderer systemRenderer;


        // Update is called once per frame
        private void Update()
        {
            m_Power = Mathf.Lerp(m_Power, Input.GetMouseButton(0) ? maxPower : minPower, Time.deltaTime * changeSpeed);

            if (Input.GetKeyDown(KeyCode.Alpha1))
                systemRenderer.enabled = !systemRenderer.enabled;

            foreach (var system in hoseWaterSystems)
            {
                var mainModule = system.main;
                mainModule.startSpeed = m_Power;
                var emission = system.emission;
                emission.enabled = m_Power > minPower * 1.1f;
            }
        }
    }
}