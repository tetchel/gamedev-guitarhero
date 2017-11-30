using UnityEngine;
using UnityEngine.SceneManagement;

public class Pauser : MonoBehaviour {

    public GameObject pauseMenu;

    private AudioSource musicSource;

    // Use this for initialization
    void Start() {
        if (pauseMenu != null) {
            pauseMenu.gameObject.SetActive(false);
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(pauseMenu.gameObject.activeSelf) {
                unPause();
            }
            else {
                pause();
            }
        }
    }

    public void pause() {
        Time.timeScale = 0;
        musicSource.Pause();

        pauseMenu.gameObject.SetActive(true);
    }

    public void unPause() {
        Time.timeScale = 1;
        musicSource.UnPause();

        if (pauseMenu != null) {
            pauseMenu.gameObject.SetActive(false);
        }
    }

    public void onPlayAgain() {
        //gameOver = false;
        musicSource.Stop();
        unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onMainMenu() {
        //gameOver = false;
        musicSource.Stop();
        unPause();
        SceneManager.LoadScene("mainmenu");
    }

    public void setMusicSource(AudioSource src) {
        musicSource = src;
    }
}
