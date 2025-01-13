using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    public class SpawnItem : MonoBehaviour
    {
        [SerializeField] private ItemBase m_SpawnItem;
        [SerializeField] private int m_SpawnCount = 1;
#if UNITY_EDITOR
        [ContextMenu(nameof(SpawnItemMenu))]
        private void SpawnItemMenu()
        {
            if (!UnityEditor.EditorApplication.isPlaying) return;
            Vector2 pos = Camera.main.transform.position;
            GameObject item = Instantiate(Singleton_PrefabLibrary.Instance.DummyItemPrefab, pos, Quaternion.identity);
            PhysicalItem pi = item.GetComponent<PhysicalItem>();
            pi.InitiateItem(m_SpawnItem, m_SpawnCount, false);

        }
#endif
    }
}
