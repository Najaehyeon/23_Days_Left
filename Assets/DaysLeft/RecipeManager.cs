using System.Collections.Generic;
using UnityEngine;

public struct Recipe
{
    public ItemData target;
    public Dictionary<ItemData, int> ingredients;
}

public class RecipeManager : MonoBehaviour
{
    [Header("Recipe List"), SerializeField]
    private List<string>    _recipeList;
    private DataLoadManager _dataLoadManager;

    public void Start()
    {
        _dataLoadManager = Global.Instance.DataLoadManager;
    }

    public List<Recipe> GetRecipes()
    {
        List<Recipe> result = new();

        foreach (var comb in _recipeList)
        {
            result.Add(StringToRecipe(comb));
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="combination">ex) "3 11 22" = Can make item(id = 3) using item(id = 1) * 1 , item(id = 2) * 2</param>
    /// <returns></returns>
    public Recipe StringToRecipe(string combination)
    {
        string[] strings = combination.Split(' ');

        ItemData _target = _dataLoadManager.Query(int.Parse(strings[0]));
        Dictionary<ItemData, int> _ingredients = new();

        for (int i = 1; i < strings.Length; i += 2)
        {
            int itemId = int.Parse(strings[i]);
            int quantity = int.Parse(strings[i + 1]);

            _ingredients.Add(_dataLoadManager.Query(itemId), quantity);
        }

        return new Recipe() { target = _target, ingredients = _ingredients};
    }
}
