using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameObject instance;


    GameObject slider;

    private AudioSource audioSrc;

    void Start()
    {
        if(instance == null)
        {
            instance = this.gameObject;
            DontDestroyOnLoad(instance);
            audioSrc = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        slider = GameObject.FindGameObjectWithTag("BackgroundMusicSlider");
        if(slider != null)
        {
            audioSrc.volume = slider.GetComponent<Slider>().value;
        }

    }
}
