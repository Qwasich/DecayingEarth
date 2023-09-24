using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    public class UIMenuHandler : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainMenuObject;

        private void Start()
        {
            Singleton_ControlSettings.Instance.PauseMenuButtonPressed += SwitchMainMenu;
        }

        private void OnDestroy()
        {
            Singleton_ControlSettings.Instance.PauseMenuButtonPressed -= SwitchMainMenu;
        }

        public void SwitchMainMenu()
        {
            if (m_MainMenuObject.activeSelf == true)
            {
                m_MainMenuObject.SetActive(false);
                Singleton_SessionData.Instance.UpdateMainMenu(false);
            }

            if (m_MainMenuObject.activeSelf == false)
            {
                m_MainMenuObject.SetActive(true);
                Singleton_SessionData.Instance.UpdateMainMenu(true);
                Singleton_MouseItemHolder.Instance.DropItem();
                Singleton_CraftingEntryPoint.Instance.DisableCraftingUI();
                Singleton_MouseItemHolder.Instance.HideTooltip();
                Singleton_GlobalChestController.Instance.CloseInventory();
            }
        }

        public void DisableMenu()
        {
            m_MainMenuObject.SetActive(false);
            Singleton_SessionData.Instance.UpdateMainMenu(false);
        }
    }
}
