using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racist
{
    public interface IScriptableObjectProperty
    {
        void ApplyProperty(ScriptableObject property);
    }

    public class SpawnObjectByPropertyList : MonoBehaviour
    {
        [SerializeField] private Transform m_Parent;
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private ScriptableObject[] m_Properties;

        [ContextMenu(nameof(SpawnInEditMode))]
        public void SpawnInEditMode()
        {
            if (Application.isPlaying == true) return;

            GameObject[] allObject = new GameObject[m_Parent.childCount];

            for (int i = 0; i < m_Parent.childCount; i++)
            {
                allObject[i] = m_Parent.GetChild(i).gameObject;
            }

            for (int i = 0; i < allObject.Length; i++)
            {
                DestroyImmediate(allObject[i]);
            }

            for (int i = 0; i < m_Properties.Length; i++)
            {
                GameObject go = Instantiate(m_Prefab, m_Parent);
                IScriptableObjectProperty property = go.GetComponent<IScriptableObjectProperty>();
                property.ApplyProperty(m_Properties[i]);
            }
        }


    }
}
