using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace DecayingEarth
{
    public class Singleton_CraftingEntryPoint : MonoSingleton<Singleton_CraftingEntryPoint>
    {
        [SerializeField] private GameObject m_CraftingWindow;
        public bool IsCraftingWindowOpen => m_CraftingWindow.activeSelf;
        [SerializeField] private GameObject m_ContentWindow;
        [SerializeField] private InvEntryPoint m_Inventory;
        [SerializeField] private List<CraftingRecipeBase> m_HandRecipes;

        [SerializeField] private Text m_DefaultCraftingText;
        [SerializeField] private string m_DefaultCraftingTitle = "Crafting";

        private TileBlockICraftingStation m_LastStation = null;

        public TileBlockICraftingStation LastStation => m_LastStation;

        private void Start()
        {
            Singleton_ControlSettings.Instance.CraftingStationUsed += InitiateCrafting;
        }

        private void OnDestroy()
        {
            Singleton_ControlSettings.Instance.CraftingStationUsed -= InitiateCrafting;
        }

        /// <summary>
        /// Очистить окно крафта и выключить его
        /// </summary>
        public void DisableCraftingUI()
        {
            if (m_CraftingWindow.activeSelf == false) return;
            if (m_ContentWindow.transform.childCount > 0) while (m_ContentWindow.transform.childCount > 0) DestroyImmediate(m_ContentWindow.transform.GetChild(0).gameObject);
            m_CraftingWindow.SetActive(false);
            m_LastStation = null;
        }

        public void UpdateCrafting() => InitiateCrafting();

        /// <summary>
        /// Открывает окно крафта, в зависимости от передаваемых рецептов.
        /// </summary>
        /// <param name="craftingStation">Тайл с передаваемыми рецептами</param>
        public void InitiateCrafting(TileBlockICraftingStation craftingStation = null)
        {
            List<CraftingRecipeBase> CraftableRecipes = new List<CraftingRecipeBase>();
            List<CraftingRecipeBase> UnavailableRecipes = new List<CraftingRecipeBase>();
            m_CraftingWindow.SetActive(true);
            Singleton_GlobalChestController.Instance.CloseInventory();

            if (m_ContentWindow.transform.childCount > 0) while (m_ContentWindow.transform.childCount > 0) DestroyImmediate(m_ContentWindow.transform.GetChild(0).gameObject);

            bool result;

            if (craftingStation != null && craftingStation.WindowName != "") m_DefaultCraftingText.text = craftingStation.WindowName;
            else if (m_LastStation != null && m_LastStation.WindowName != "") m_DefaultCraftingText.text = m_LastStation.WindowName;
            else m_DefaultCraftingText.text = m_DefaultCraftingTitle;



            if (m_HandRecipes.Count > 0)
            {
                foreach (var recipe in m_HandRecipes)
                {
                    result = ScanInventoryForRecipe(recipe);
                    if (result) CraftableRecipes.Add(recipe);
                    else UnavailableRecipes.Add(recipe);
                }
            }

            if (craftingStation != null && craftingStation.Recepies.Count > 0)
            {
                m_LastStation = craftingStation;
                foreach (var recipe in craftingStation.Recepies)
                {
                    result = ScanInventoryForRecipe(recipe);
                    if (result) CraftableRecipes.Add(recipe);
                    else UnavailableRecipes.Add(recipe);
                }
            }
            else if (craftingStation == null && m_LastStation != null)
            {
                foreach (var recipe in m_LastStation.Recepies)
                {
                    result = ScanInventoryForRecipe(recipe);
                    if (result) CraftableRecipes.Add(recipe);
                    else UnavailableRecipes.Add(recipe);
                }
            }

            foreach (var recipe in CraftableRecipes)
            {
                var UIRecipe = Instantiate(Singleton_PrefabLibrary.Instance.BigRecipeHolder, m_ContentWindow.transform);
                UIRecipeCreator creator = UIRecipe.GetComponent<UIRecipeCreator>();
                creator.Initialize(recipe, true);
            }

            foreach (var recipe in UnavailableRecipes)
            {
                var UIRecipe = Instantiate(Singleton_PrefabLibrary.Instance.BigRecipeHolder, m_ContentWindow.transform);
                UIRecipeCreator creator = UIRecipe.GetComponent<UIRecipeCreator>();
                creator.Initialize(recipe, false);
            }
        }

        /// <summary>
        /// Подтвердить крафт рецепта.
        /// </summary>
        /// <param name="recipe">Рецепт</param>
        public void ConfirmRecipeCrafting(CraftingRecipeBase recipe)
        {
            if (Singleton_MouseItemHolder.Instance.HandItem.Item != null && Singleton_MouseItemHolder.Instance.HandItem.Item != recipe.Result.Item) return;
            if (Singleton_MouseItemHolder.Instance.HandItem.Item != null && Singleton_MouseItemHolder.Instance.HandItem.StackCount + recipe.Result.Amount > Singleton_MouseItemHolder.Instance.HandItem.Item.MaxStackCount) return;

            ScanInventoryForRecipe(recipe, true);
            if (Singleton_MouseItemHolder.Instance.HandItem.Item == null) Singleton_MouseItemHolder.Instance.GrabItem(new InvItem(recipe.Result.Item, recipe.Result.Amount));
            else Singleton_MouseItemHolder.Instance.IncreaseHandItemByNumber(recipe.Result.Amount);
            Singleton_MouseItemHolder.Instance.HideTooltip();

            InitiateCrafting(m_LastStation);
        }

        /// <summary>
        /// Сканирует инвентарь на необходимые предметы
        /// </summary>
        /// <param name="recipe">Проверяемый рецепт</param>
        /// <param name="mode">Если True - будет так же попутно удалять предметы</param>
        /// <returns></returns>
        private bool ScanInventoryForRecipe(CraftingRecipeBase recipe, bool mode = false)
        {
            int length = recipe.Components.Count;
            

            for (int i = 0; i < length; i++)
            {
                int itemCount = recipe.Components[i].Amount;
                ItemBase item = recipe.Components[i].Item;

                for (int j = 0; j < m_Inventory.Inventory.Items.Count; j++)
                {
                    if (m_Inventory.Inventory.Items[j].Item != item) continue;

                    int sub = m_Inventory.Inventory.Items[j].StackCount;

                    if (mode)
                    {
                        if (itemCount - sub < 0) sub -=  sub - itemCount;
                        m_Inventory.Inventory.DecreaseItemCount(j,sub);
                        m_Inventory.UpdateButton(j);
                       
                    }
                    itemCount -= sub;


                    if (itemCount <= 0) break;
                    continue;
                }

                if (itemCount > 0) return false;

            }

            return true;
        }


    }
}
