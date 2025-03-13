using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecoverable 
{
    float RecoverAmount { get; set; }
    void RecoverPerSecond(); 


}
