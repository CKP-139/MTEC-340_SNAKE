using UnityEngine;

public class BGM : MonoBehaviour
{
    
    [SerializeField] private AudioClip _BGM;
    [SerializeField] private AudioClip _gameOver;

    private AudioSource _source;
    private bool _gameOverPlayed = false;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.resource = _BGM;
        _source.Play();
    }

    void Update()
    {
        if (Manager.Instance.State == Utilities.GameState.GameOver && !_gameOverPlayed)
        {
            _gameOverPlayed = true;
            _source.Stop();
            _source.resource = _gameOver;
            _source.Play();
        }
    }
}