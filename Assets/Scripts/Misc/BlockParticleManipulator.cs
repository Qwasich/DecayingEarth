using UnityEngine;

namespace DecayingEarth
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BlockParticleManipulator : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_ParticleSystem;

        public void ChangeParticles(Color color, Sprite sprite, int particleCount)
        {
            Color darkColor = new (color.r / 2, color.g / 2, color.b / 2);

            ParticleSystem.MainModule MainParticleParameters = m_ParticleSystem.main;
            ParticleSystem.ShapeModule ShapeModule = m_ParticleSystem.shape;
            ParticleSystem.MinMaxGradient Gradient = new (darkColor, color);
            ParticleSystem.EmissionModule Emission = m_ParticleSystem.emission;
            ParticleSystem.Burst count = new(0,particleCount);

            MainParticleParameters.startColor = Gradient;
            ShapeModule.sprite = sprite;
            Emission.SetBurst(0,count);

        }


    }
}
