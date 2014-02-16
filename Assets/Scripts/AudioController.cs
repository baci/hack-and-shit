using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	AudioSource musicSource;
	float basePitch;
	AudioSource sfxSource;

	private static AudioController _instance;
	public static AudioController instance{
		get{
			if(_instance == null){
				GameObject o = new GameObject("Audio Controller", typeof(AudioController));
				DontDestroyOnLoad(o);
				_instance = o.GetComponent<AudioController>();
			}

			return _instance;
		}
	}

	void Awake () {
		musicSource = gameObject.AddComponent<AudioSource>();
		basePitch = musicSource.pitch;
		sfxSource = gameObject.AddComponent<AudioSource>();
	}

	public void PlaySfx(AudioClip clip, float pitch = 1f){
		if(clip == null) return;
		sfxSource.pitch = pitch;
		sfxSource.volume = 0.7f;
		sfxSource.PlayOneShot(clip);
	}

	public void PlayMusic(AudioClip music){
		musicSource.clip = music;
		musicSource.loop = true;
		musicSource.pitch = basePitch;
		musicSource.Play();
	}
}