using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }
    public void PlayScene()
    {
        _source.Play();
        Debug.Log("start button clicked");
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        _source.Play();
        Debug.Log("Quitting");
        Application.Quit();
    }
}