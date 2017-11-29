using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GHMusicPlayer : MonoBehaviour {

    private AudioSource audioSource;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
        StartCoroutine(countdownAudioEnd());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator countdownAudioEnd() {
        float len = audioSource.clip.length / audioSource.pitch * Time.timeScale;

        yield return new WaitForSeconds(len + 2);
        // Send a message somewhere that the song is over
        SceneManager.LoadScene("postsong");
    }
}
