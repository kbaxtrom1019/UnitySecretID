using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
	public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static SoundManager GetInstance()
    {
        return instance;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
	{
		GameObject AudioObj = new GameObject("SFX:" + clip.name);
		AudioSource Source = AudioObj.AddComponent<AudioSource>();
		Source.clip = clip;
		Source.Play();
		Destroy(AudioObj, clip.length);
	}
}