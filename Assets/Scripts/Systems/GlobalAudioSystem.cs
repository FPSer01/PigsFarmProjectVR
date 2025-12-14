using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ласс системы дл€ глобальных звуков, их проигрывани€ и контрол€:
/// <br>Ќапример дл€ проигрывани€ звуков UI</br>
/// </summary>

public class GlobalAudioSystem : MonoBehaviour
{
    public static GlobalAudioSystem instance;

    [SerializeField] private AudioSource sfxSource; // »сточник звуков дл€ глобальных SFX

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayGlobalSFX(AudioClip clip, float volumeScale)
    {
        sfxSource.PlayOneShot(clip, volumeScale);
    }
}
