using UnityEngine;

namespace Racist
{
    public class SettingsLoader : MonoBehaviour
    {
        [SerializeField] private Setting[] m_AllSettings;

        private void Awake()
        {
            for (int i = 0; i< m_AllSettings.Length; i++)
            {
                m_AllSettings[i].Load();
                m_AllSettings[i].Apply();
            }
        }
    }
}
