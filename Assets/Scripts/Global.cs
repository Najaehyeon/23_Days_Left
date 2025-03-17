using UnityEngine;

/// <summary>
/// Manage other managers, do not destroyed on scene load.
/// </summary>
public class Global : Singleton<Global>
{
    [Header("Monobehaviour")]
    public AudioManager     AudioManager;
    public DataLoadManager  DataLoadManager;
    public RecipeManager    RecipeManager;

    [HideInInspector]
    public Player           Player;
}
