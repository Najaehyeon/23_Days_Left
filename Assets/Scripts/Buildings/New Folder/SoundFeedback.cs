using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Click,
    Place,
    wrongPlacement
}


public class SoundFeedBack : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickSound, placeSound, wrongPlacementSound;

    [SerializeField]
    private AudioSource audioSource;

    public void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Click:
                audioSource.PlayOneShot(clickSound);
                break;
            case SoundType.Place:
                audioSource.PlayOneShot(placeSound);
                break;
            case SoundType.wrongPlacement:
                audioSource.PlayOneShot(wrongPlacementSound);
                break;
            default:
                break;
        }
    }
}
