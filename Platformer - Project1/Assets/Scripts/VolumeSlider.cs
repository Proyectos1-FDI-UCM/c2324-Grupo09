using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,

        MUSIC,

        SFX
    }

    [Header("Type")]

    [SerializeField] VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }
    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.Instance.mastervolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.Instance.Musicvolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.Instance.SFXvolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported" + volumeType);
                break;
        }
    }
    public void OnSliderValueChanged()
    {
        try
        {
            switch (volumeType)
            {
                case VolumeType.MASTER:
                    AudioManager.Instance.mastervolume = volumeSlider.value;
                    GameManager.Instance.SaveMasterVolume(volumeSlider.value);
                    break;
                case VolumeType.MUSIC:
                    AudioManager.Instance.Musicvolume = volumeSlider.value;
                    GameManager.Instance.SaveMusicVolume(volumeSlider.value);
                    break;
                case VolumeType.SFX:
                    AudioManager.Instance.SFXvolume = volumeSlider.value;
                    GameManager.Instance.SaveSFXVolume(volumeSlider.value);
                    break;
                default:
                    Debug.LogWarning("Volume Type not supported" + volumeType);
                    break;
            }
        }
        catch
        {
            Debug.Log("Peta onsliderValueChanged");
        }

      
    }
}
