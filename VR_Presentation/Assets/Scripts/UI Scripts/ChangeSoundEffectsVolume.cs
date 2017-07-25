using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSoundEffectsVolume : MonoBehaviour {

    public Slider slider;
    public AudioSource soundEffectHighlight;
    public AudioSource soundEffectClose;
    public AudioSource soundEffectLoad;

    // Update is called once per frame
    void Update () {
        soundEffectHighlight.volume = slider.value;
        soundEffectClose.volume = slider.value;
        soundEffectLoad.volume = slider.value;
	}
}
