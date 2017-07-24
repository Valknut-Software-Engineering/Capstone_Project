using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMasterVolume : MonoBehaviour {

    public Slider sliderMaster;
    public Slider sliderMusic;
    public Slider sliderSoundEffects;
    public float previous = 100;

    // Update is called once per frame
    void Update()
    {
        if (sliderMaster.value != previous)
        {
            sliderMusic.value = sliderMaster.value;
            sliderSoundEffects.value = sliderMaster.value;
            previous = sliderMaster.value;
        } 
    }
}
