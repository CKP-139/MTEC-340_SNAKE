using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    private Utilities.GameState _state;
    public Utilities.GameState State
    {
        get => _state;
        set
        {
            _state = value;
        }
    }

    // UI elements
    [SerializeField] private TMP_Text _messagesUI;
    [SerializeField] private TMP_Text _scoreUI;
    [SerializeField] private GameObject _quitButton;
    private int _score = 0;
    
    
    // variables for item spawning + gamefield size
    private Vector2 itemPosition;
    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;
    [SerializeField] private float _greetChance = 0.5f;

    [SerializeField] public GameObject greetPrefab;
    [SerializeField] public GameObject applePrefab; 
    [SerializeField] private GameObject _snakeHead;
    private SnakeBehavior _snakeBehavior;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("New manager instance intialized");
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
            Debug.Log("Deleted duplicate manager instance");
        }
    }

    void Start()
    {
        _snakeBehavior = _snakeHead.GetComponent<SnakeBehavior>();
        ResetGame();
        State = Utilities.GameState.Play;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (State == Utilities.GameState.Play)
            {
                State = Utilities.GameState.Pause;

                _messagesUI.text = "PAUSED";
                _messagesUI.gameObject.SetActive(true);
                _quitButton.SetActive(true);
            } else if (State == Utilities.GameState.Pause)
            {
                State = Utilities.GameState.Play;

                _messagesUI.gameObject.SetActive(false);
            }
        }
    }
    public void GameOver()
    {
        _messagesUI.text = "GAME OVER"; 
        _messagesUI.gameObject.SetActive(true);

        State = Utilities.GameState.GameOver;

        _quitButton.SetActive(true);
    }
    public void SpawnItem()
    {
        bool isTouchingSnake;

        // get a random position, then see if it's touching the snake; if it is, reroll
        do
        {
            itemPosition = new Vector3(Random.Range(-gridWidth,gridWidth)/2,Random.Range(-gridHeight,gridHeight)/2);
            isTouchingSnake = Physics2D.BoxCast(itemPosition,Vector2.one,0.0f,Vector2.zero);
        } while (isTouchingSnake == true);

        if (Utilities.GetNonZeroRandomFloat() <= _greetChance && !_snakeBehavior.powerupActive)
        {
            // spawn greet
            GameObject greet = Instantiate(greetPrefab, itemPosition, new Quaternion(0,0,0,1));
        } else
        {
            // spawn apple
            GameObject apple = Instantiate(applePrefab, itemPosition, new Quaternion(0,0,0,1));
        }
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _scoreUI.text = $"SCORE: {_score}";
    }

    public void ResetGame()
    {
        _score = 0;
        SpawnItem();
    }
}
