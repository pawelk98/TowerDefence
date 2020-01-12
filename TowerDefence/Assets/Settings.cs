using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject.transform);
    }
    public void SetVolume(float volume)
    {
        //audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        audioMixer.SetFloat("volume", volume);
    }
   
}
