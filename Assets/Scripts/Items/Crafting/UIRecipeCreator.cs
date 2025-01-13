using UnityEngine;
using UnityEngine.UI;

namespace DecayingEarth
{
    public class UIRecipeCreator : MonoBehaviour
    {
        [SerializeField] private CraftingComponentButton m_ResultButton;

        [SerializeField] private GameObject m_Container;

        [SerializeField] private Color m_DimColor;

        private CraftingRecipeBase m_StoredRecipe;

        /// <summary>
        /// Инициализировать целый рецепт
        /// </summary>
        /// <param name="recipe">Рецепт</param>
        /// <param name="isCraftable">Если True - можно взаимодействовать с кнопками, False - нет</param>
        public void Initialize(CraftingRecipeBase recipe, bool isCraftable)
        {
            m_StoredRecipe = recipe;

            if (!isCraftable) DimMenu();

            m_ResultButton.Initiate(recipe.Result,this);
            m_ResultButton.SetButtonInteractability(isCraftable);
            m_ResultButton.UpdateButtonGraphics();
            if (!isCraftable) m_ResultButton.DimButton();

            foreach(var component in recipe.Components)
            {
                var button = Instantiate(Singleton_PrefabLibrary.Instance.RecipeButton, m_Container.transform);
                CraftingComponentButton ccb = button.GetComponent<CraftingComponentButton>();
                ccb.Initiate(component,this);
                ccb.SetButtonInteractability(false);
                ccb.UpdateButtonGraphics();
                if (!isCraftable) ccb.DimButton();

            }
        }

        public void StartCrafting()
        {
            Debug.Log("UirecipeReached");
            Singleton_CraftingEntryPoint.Instance.ConfirmRecipeCrafting(m_StoredRecipe);
        }

        private void DimMenu()
        {
            GetComponent<Image>().color = m_DimColor;
        }
    }
}
