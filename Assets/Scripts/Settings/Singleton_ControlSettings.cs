using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utility;

namespace DecayingEarth
{
    public class Singleton_ControlSettings : MonoSingleton<Singleton_ControlSettings>
    {
        public event UnityAction OnLeftMouseButtonPressed;
        public event UnityAction OnLeftMouseButtonHold;
        public event UnityAction OnRightMouseButtonPressed;
        public event UnityAction OnRightMouseButtonHold;
        public event UnityAction InvButtonPressed;
        public event UnityAction PauseMenuButtonPressed;
        public event UnityAction<int> HotbarButtonPressedByIndex;
        public event UnityAction<TileBlockICraftingStation> CraftingStationUsed;

        private bool m_IBPressed = false;
        private bool m_PMBPressed = false;
        private bool m_HBPressed = false;

        private bool MouseLeft => Input.GetMouseButtonDown(0);
        private bool MouseLeftHold => Input.GetMouseButton(0);
        private bool MouseRight => Input.GetMouseButtonDown(1);
        private bool MouseRightHold => Input.GetMouseButton(1);
        private float Inventory => Input.GetAxis("Inventory Open");
        private float Cancel => Input.GetAxis("Cancel");

        private float HB0 => Input.GetAxis("Inventory Hotkey 1");
        private float HB1 => Input.GetAxis("Inventory Hotkey 2");
        private float HB2 => Input.GetAxis("Inventory Hotkey 3");
        private float HB3 => Input.GetAxis("Inventory Hotkey 4");
        private float HB4 => Input.GetAxis("Inventory Hotkey 5");
        private float HB5 => Input.GetAxis("Inventory Hotkey 6");
        private float HB6 => Input.GetAxis("Inventory Hotkey 7");
        private float HB7 => Input.GetAxis("Inventory Hotkey 8");
        private float HB8 => Input.GetAxis("Inventory Hotkey 9");


        private void Update()
        {
            if (MouseLeft)
            {
                if(Singleton_SessionData.Instance != null) Singleton_SessionData.Instance.UpdateLastClick(IsClickOnCanvas());
                OnLeftMouseButtonPressed?.Invoke();
            }
            if (MouseLeftHold)
            {
                OnLeftMouseButtonHold?.Invoke();
            }
            if (MouseRight)
            {

                if (Singleton_SessionData.Instance != null) Singleton_SessionData.Instance.UpdateLastClick(IsClickOnCanvas());
                var tile = IsClickOnTile();
                if (!Singleton_SessionData.Instance.IsLastClickWasOnCanvas && tile != null)
                {
                    if (tile is TileBlockICraftingStation)
                    {
                        if (Singleton_SessionData.Instance.IsInventoryHidden)
                        {
                            m_IBPressed = true;
                            InvButtonPressed?.Invoke();
                        }
                        CraftingStationUsed?.Invoke((TileBlockICraftingStation)tile);
                        Debug.Log(tile.name);
                    }

                }
                else OnRightMouseButtonPressed?.Invoke();
            }
            if (MouseRightHold)
            {
                OnRightMouseButtonHold?.Invoke();
            }
            if (Inventory >= 0.1 && !m_IBPressed)
            {
                m_IBPressed = true;
                InvButtonPressed?.Invoke();
            }
            else if (Inventory <= 0.1 && m_IBPressed) m_IBPressed = false;

            if (Cancel != 0 && !m_PMBPressed)
            {
                m_PMBPressed = true;
                PauseMenuButtonPressed?.Invoke();
            }
            else if (Cancel == 0 && m_PMBPressed) m_PMBPressed = false;

            UpdateHotbarButtons();
        }

        private bool IsClickOnCanvas()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            List<RaycastResult> rr = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, rr);

            foreach(RaycastResult rhr in rr)
            {
                CanvasRenderer cr;
                cr = rhr.gameObject.GetComponent<CanvasRenderer>();
                if (cr != null) return true;
            }
            return false;

        }

        private TileBlockBase IsClickOnTile()
        {
            Tilemap m_WallsTilemap = Singleton_GridLibrary.Instance.WallsTilemap;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            var tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);

            if (tile != null) return tile;
            else return null;
        }
        
        /// <summary>
        /// Проверяет все кнопки хотбара.
        /// </summary>
        private void UpdateHotbarButtons()
        {
            if (HB0 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(0);
            }
            if (HB1 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(1);
            }
            if (HB2 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(2);
            }
            if (HB3 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(3);
            }
            if (HB4 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(4);
            }
            if (HB5 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(5);
            }
            if (HB6 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(6);
            }
            if (HB7 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(7);
            }
            if (HB8 != 0 && !m_HBPressed)
            {
                m_HBPressed = true;
                HotbarButtonPressedByIndex?.Invoke(8);
            }
            if (HB0 == 0 && HB1 == 0 && HB2 == 0 && HB3 == 0 && HB4 == 0 && HB5 == 0 && HB6 == 0 && HB7 == 0 && HB8 == 0) m_HBPressed = false;
        }
    }
}
