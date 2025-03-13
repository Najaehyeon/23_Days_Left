using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : Singleton<Global>
{
    [Header("Monobehaviour")]
    public AudioManager     audioManager;

    public DataLoadManager  dataLoadManager = new();

    private void Start()
    {
        
    }
}
