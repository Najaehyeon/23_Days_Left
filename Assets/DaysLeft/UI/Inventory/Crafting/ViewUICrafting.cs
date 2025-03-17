using System.Collections.Generic;
using DaysLeft.Menu;

namespace DaysLeft.Inventory
{    
    public class ViewUICrafting : UIScreen
    {
        protected override void Start()
        {
            base.Start();

            List<Recipe> recipes = Global.Instance.RecipeManager.GetRecipes();
             
            foreach(var plugin in Plugins)
                if(plugin is ScreenPluginCraftingItemSlot craftingPlugin)
                    craftingPlugin.Set( recipes.GetAndRemoveAt(0) );
        }
    }
}