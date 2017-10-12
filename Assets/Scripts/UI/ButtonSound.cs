using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public Sprite spriteOff;
    public Sprite spriteOn;
    // Use this for initialization

    private void Start()
    {
        SoundSettings.Volume = PlayerPrefs.GetInt("SoundVolume", 1);
        if (SoundSettings.Volume == 0)
        {
            SoundSettings.MuteSounds(true);
        }
        SetSoundImage();
    }
    
    public void Click()
    {
        SoundSettings.SwitchSounds();
        SetSoundImage();
    }

    private void SetSoundImage()
    {
        if (SoundSettings.Volume == 1) GetComponent<Image>().sprite = spriteOn;
        else GetComponent<Image>().sprite = spriteOff;
    }
}