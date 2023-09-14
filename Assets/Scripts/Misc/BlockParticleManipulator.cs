using UnityEngine;

namespace DecayingEarth
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BlockParticleManipulator : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_ParticleSystem;

        public void ChangeParticleColorsAndSprite(Color color, Sprite sprite)
        {
            Color darkColor = new Color(color.r / 2, color.g / 2, color.b / 2);

            ParticleSystem.MainModule MainParticleParameters = m_ParticleSystem.main;
            ParticleSystem.ShapeModule ShapeModule = m_ParticleSystem.shape;
            ParticleSystem.MinMaxGradient Gradient = new ParticleSystem.MinMaxGradient(darkColor, color);

            MainParticleParameters.startColor = Gradient;
            ShapeModule.sprite = sprite;

        }


    }
}
