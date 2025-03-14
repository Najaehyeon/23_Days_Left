using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : Singleton<Global>
{
    [Header("Monobehaviour")]
    public AudioManager     AudioManager;
    public DataLoadManager  DataLoadManager;
}
