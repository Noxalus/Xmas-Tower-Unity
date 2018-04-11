using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public bool isGameScreen;
    public AudioClip MenuMusic;
    public AudioClip GameIntroMusic;
    public AudioClip GameLoopMusic;

    private AudioSource audioSource;
    private bool isPlayingIntro;

    void Start () {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        if (isGameScreen)
            StartCoroutine(playGameMusic());
        else
        {
            audioSource.clip = MenuMusic;
            audioSource.Play();
        }
	}

    private IEnumerator playGameMusic()
    {
        audioSource.clip = GameIntroMusic;
        audioSource.Play();
        isPlayingIntro = true;

        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.clip = GameLoopMusic;
        audioSource.Play();
        isPlayingIntro = false;
    }

}
