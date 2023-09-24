using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    public class GenerateOnStart : MonoBehaviour
    {

        private void Start()
        {
            if (Singleton_CaveGenerator.Instance != null) Singleton_CaveGenerator.Instance.GenerateMap();
        }
    }
}
