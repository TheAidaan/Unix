using UnityEngine.UI;
using UnityEngine;

public class SoundSlider : MonoBehaviour
{
    void Start()
    {
        GetComponent<Slider>().value = AudioManager.SoundVolume;
    }
}
