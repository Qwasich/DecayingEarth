using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace DecayingEarth
{
    [Serializable]
    public struct ChestLocation
    {
        public Vector3Int ChestPosition;
        public Inventory AttachedInventory;

        public ChestLocation(Vector3Int pos, Inventory inv)
        {
            ChestPosition = pos;
            AttachedInventory = inv;
        }
    }

    public class Singleton_GlobalChestController : MonoSingleton<Singleton_GlobalChestController>
    {

        [SerializeField] private InvEntryPoint m_ChestPoint;
        [SerializeField] private GameObject m_ChestUI;
        [SerializeField] private GameObject m_ChestObject;

        private List<ChestLocation> m_Inventories;
        /// <summary>
        /// Лист ВСЕХ инвентарей (за исключением игрока) на этой сцене.
        /// </summary>
        public List<ChestLocation> Inventories => m_Inventories;

        private void Start()
        {
            m_Inventories = new List<ChestLocation>();
            Singleton_ControlSettings.Instance.ChestStorageUsed += OpenInventory;
        }

        private void OnDestroy()
        {
            Singleton_ControlSettings.Instance.ChestStorageUsed -= OpenInventory;
        }

        public void AddInventory(Vector3Int pos)
        {
            GameObject chest = Instantiate(m_ChestObject, transform);
            Inventory inv = chest.GetComponent<Inventory>();
            m_Inventories.Add(new ChestLocation(pos,inv));
        }

        public void OpenInventory(Vector3Int pos)
        {
            for (int i = 0; i < m_Inventories.Count; i++)
            {
                if (m_Inventories[i].ChestPosition == pos)
                {
                    m_ChestUI.SetActive(true);
                    Singleton_CraftingEntryPoint.Instance.DisableCraftingUI();
                    m_ChestPoint.SetInventory(m_Inventories[i].AttachedInventory);
                    m_ChestPoint.InitiateInvPoint();
                }
            }
        }

        public void CloseInventory()
        {
            m_ChestUI.SetActive(false);
        }

        public void RemoveInventory(Vector3Int pos)
        {
            for (int i = 0; i < m_Inventories.Count; i++)
            {
                if (m_Inventories[i].ChestPosition == pos)
                {
                    GameObject chest = m_Inventories[i].AttachedInventory.gameObject;
                    m_Inventories.Remove(m_Inventories[i]);
                    DestroyImmediate(chest);
                }
            }
        }

        public bool CheckInventoryEmpriness(Vector3Int pos)
        {
            for (int i = 0; i < m_Inventories.Count; i++)
            {
                if (m_Inventories[i].ChestPosition == pos)
                {
                    Inventory check = m_Inventories[i].AttachedInventory;

                    if (m_ChestUI.activeSelf && m_ChestPoint.Inventory == check) return false;
                    for (int j = 0; j < check.Items.Count; j ++)
                    {
                        if (check.Items[j].Item != null) return false;

                    }
                }
            }
            return true;

        }


        
    }
}
