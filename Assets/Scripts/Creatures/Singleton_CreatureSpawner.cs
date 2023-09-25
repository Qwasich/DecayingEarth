using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    [Serializable]
    public struct CreatureSpawnParameters
    {
        public GameObject SpawnedCreature;
        public string FloorDetsTag;
    }

    public class Singleton_CreatureSpawner : MonoBehaviour
    {
        [SerializeField] private List<CreatureSpawnParameters> m_ListOfCreatures;

        [SerializeField] private int m_MaxAmountOfCreatures = 10;
        [SerializeField] private float m_MaxDistanceToSpawn;
        [SerializeField] private float m_MinDistanceToSpawn;

        [SerializeField] private float m_CheckTimer;

        private float m_Timer;

        private List<GameObject> m_ListOfSpawnedCreatures;

        private void Start()
        {
            m_Timer = m_CheckTimer;
            m_ListOfSpawnedCreatures = new();
        }

        private void Update()
        {
            if (m_Timer > 0) m_Timer -= Time.deltaTime;

            if (m_Timer <= 0)
            {
                SpawnCreatures();
                CheckForDespawn();
            }
        }

        private void SpawnCreatures()
        {
            if (m_ListOfCreatures.Count == 0 || m_ListOfSpawnedCreatures.Count == m_MaxAmountOfCreatures) return;

            int c = 10000;

            for (int i = 0; i < m_MaxAmountOfCreatures - m_ListOfSpawnedCreatures.Count; i++)
            {
                c--;
                if (c <= 0) break;

                Vector2 pos = (Vector2)Singleton_PlayerInfo.Instance.Player.transform.position + (Random.insideUnitCircle * m_MaxDistanceToSpawn);

                float dist = Vector2.Distance(Singleton_PlayerInfo.Instance.Player.transform.position, pos);
                if (dist < m_MinDistanceToSpawn)
                {
                    i--;
                    continue;
                }
                if (Singleton_GridLibrary.Instance.WallsTilemap.GetTile(Singleton_GridLibrary.Instance.WallsTilemap.WorldToCell(pos)) != null)
                {
                    i--;
                    continue;
                }
                CreatureSpawnParameters csp = m_ListOfCreatures[Random.Range(0, m_ListOfCreatures.Count)];
                TileBlockBase tile = (TileBlockBase)Singleton_GridLibrary.Instance.FloorDetsTilemap.GetTile(Singleton_GridLibrary.Instance.FloorDetsTilemap.WorldToCell(pos));

                if (csp.FloorDetsTag != "" && tile != null && tile.Tag != csp.FloorDetsTag)
                {
                    i--;
                    continue;
                }

                if (csp.FloorDetsTag != "" && tile == null)
                {
                    i--;
                    continue;
                }

                GameObject go = Instantiate(csp.SpawnedCreature, pos, Quaternion.identity);
                m_ListOfSpawnedCreatures.Add(go);
                
            }


        }

        private void CheckForDespawn()
        {
            if (m_ListOfSpawnedCreatures.Count == 0) return;

            for (int i = 0; i < m_ListOfSpawnedCreatures.Count; i++)
            {
                if (m_ListOfSpawnedCreatures[i] == null)
                {
                    m_ListOfSpawnedCreatures.RemoveAt(i);
                    i--;
                    continue;
                }

                float dist = Vector2.Distance(Singleton_PlayerInfo.Instance.Player.transform.position, m_ListOfSpawnedCreatures[i].transform.position);

                if (dist > m_MaxDistanceToSpawn)
                {
                    Destroy(m_ListOfSpawnedCreatures[i]);
                    m_ListOfSpawnedCreatures.RemoveAt(i);
                    i--;
                }
            }

            m_Timer = m_CheckTimer;
        }
    }
}
