using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScreen : BaseMenu
{
    public Slider musicSlider;
    public Slider sndEffectSlider;
    private OnButtonClick onResumeClicked;
    private OnButtonClick onQuitClicked;
    private OnSliderValueChanged onMusicValueChanged;
    private OnSliderValueChanged onSndEffectValueChanged;

    public void SetResumeButtonListener(OnButtonClick onClick)
    {
        onResumeClicked = onClick;
    }

    public void SetQuitButtonListener(OnButtonClick onClick)
    {
        onQuitClicked = onClick;
    }

    public void SetMusicValueListener(OnSliderValueChanged onValueChanged)
    {
        onMusicValueChanged = onValueChanged;
    }

    public void SetMusicLevelValue(float value)
    {
        value = Mathf.Clamp01(value);
        musicSlider.value = value;
    }

    public void SetSndEffectLevelValue(float value)
    {
        value = Mathf.Clamp01(value);
        sndEffectSlider.value = value;
    }


    public void SetSndEffectValueListener(OnSliderValueChanged onValueChanged)
    {
        onSndEffectValueChanged = onValueChanged;
    }

    public void OnResumeButtonClicked()
    {
        onResumeClicked();
    }

    public void OnQuitButtonClicked()
    {
        onQuitClicked();
    }

    public void OnSndEffectSliderChanged()
    {
        onSndEffectValueChanged(sndEffectSlider.value);
    }

    public void OnMusicSliderChanged()
    {
        onMusicValueChanged(musicSlider.value);
    }
}
