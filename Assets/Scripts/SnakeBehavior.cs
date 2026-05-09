using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SnakeBehavior : MonoBehaviour
{
    [Range(0.1f,10.0f)]
    [SerializeField] public float movementInterval; // in seconds
    [SerializeField] private float _movementIntervalShortner;
    [SerializeField] private float _powerupDuration;
    public bool powerupActive = false;
    private float _timer;

    [SerializeField] private KeyCode _upDirection;
    [SerializeField] private KeyCode _downDirection;
    [SerializeField] private KeyCode _leftDirection;
    [SerializeField] private KeyCode _rightDirection;

    // audio
    [SerializeField] private AudioClip _appleEat;
    [SerializeField] private AudioClip _greetEat;
    [SerializeField] private AudioClip _greetOff;
    private AudioSource _source;

    private Vector2 _headPosition;
    private Vector2 _movementDirection;
    private float spriteDirectionAngle;
    [SerializeField] public GameObject bodyPartPrefab;
    private int _snakeBodyLength = 1;

    private List<Vector2> _partPositionList =  new List<Vector2>();
    private List<Transform> _partTransformList =  new List<Transform>();

    //private List<GameObject> bodyPartList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _headPosition = Vector2.zero;
        _timer = 0;
        _movementDirection = Vector2.right;
        transform.position = new Vector3(_headPosition.x,_headPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.Instance.State == Utilities.GameState.Play)
        {
            // movement key checks
            if (Input.GetKeyDown(_upDirection) && _movementDirection != Vector2Int.down)
            {
                _movementDirection = Vector2.up;
                spriteDirectionAngle = 90.0f;
            }
            if (Input.GetKeyDown(_downDirection) && _movementDirection != Vector2Int.up)
            {
                _movementDirection = Vector2.down;
                spriteDirectionAngle = -90.0f; 
            }
            if (Input.GetKeyDown(_leftDirection) && _movementDirection != Vector2Int.right)
            {
                _movementDirection = Vector2.left;
                spriteDirectionAngle = 180.0f; 
            }
            if (Input.GetKeyDown(_rightDirection) && _movementDirection != Vector2Int.left)
            {
                _movementDirection = Vector2.right;
                spriteDirectionAngle = 0.0f;
            }

            // update timer and movement
            _timer += Time.deltaTime;
            
            //shorten movement interval if powerup is active
            float movement = powerupActive ? 
            movementInterval - _movementIntervalShortner : 
            movementInterval;

            if (_timer >= movement)
            {
                //Move();
                _partPositionList.Insert(0, _headPosition);

                _headPosition += _movementDirection;
                transform.position = new Vector3(_headPosition.x,_headPosition.y);
                transform.eulerAngles = new Vector3(0,0,spriteDirectionAngle);

                // remove last position item in list
                if (_partPositionList.Count >= _snakeBodyLength + 1) {_partPositionList.RemoveAt(_partPositionList.Count-1);}

                // move all subsequent body parts
                for (int i = 0; i < _partTransformList.Count; i++)
                {
                    Vector3 partPosition = new Vector3(_partPositionList[i].x,_partPositionList[i].y);
                    _partTransformList[i].position = partPosition;
                }
                _timer -= movement;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision registered");
        if (other.gameObject.CompareTag("Apple"))
        {
            _source.resource = _appleEat;
            _source.Play();

            Manager.Instance.IncreaseScore(100);
            Grow();
            Destroy(other.gameObject);
            Manager.Instance.SpawnItem();
        } else if (other.gameObject.CompareTag("Greet"))
        {
            _source.resource = _greetEat;
            _source.Play();

            Manager.Instance.IncreaseScore(250);
            Grow();
            StartCoroutine(PowerupSpeed(_powerupDuration));
            Destroy(other.gameObject);
            Manager.Instance.SpawnItem();
        } else
        {
            Manager.Instance.GameOver();
        }
    }

    private void Grow()
    {
        _snakeBodyLength++;
        Vector3 partPosition = new Vector3(_partPositionList[_partPositionList.Count - 1].x,_partPositionList[_partPositionList.Count - 1].y);
        GameObject bodyPart = Instantiate(bodyPartPrefab, partPosition, new Quaternion(0,0,0,1));
        _partTransformList.Add(bodyPart.transform);
    }

    IEnumerator PowerupSpeed(float duration)
    {
        float elapsedTime = 0.0f;

        powerupActive = true;

        while (elapsedTime < duration)
        {
            if (Manager.Instance.State == Utilities.GameState.Play)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            } else if (Manager.Instance.State == Utilities.GameState.GameOver)
            {
                break;
            }
        }
        _source.resource = _greetOff;
        powerupActive = false;

        _source.Play();
    }

    // private void Move()
    // {
    
    //     _partPositionList.Insert(0, _headPosition);

    //     _headPosition += _movementDirection;
    //     transform.position = new Vector3(_headPosition.x,_headPosition.y);
    //     transform.eulerAngles = new Vector3(0,0,spriteDirectionAngle);

    //     // remove last position item in list
    //     if (_partPositionList.Count >= _snakeBodyLength + 1) {_partPositionList.RemoveAt(_partPositionList.Count - 1);}

    //     // move all subsequent body parts
    //     for (int i = 0; i < _partPositionList.Count; i++)
    //     {
    //         Vector3 partPosition = new Vector3(_partPositionList[i].x,_partPositionList[i].y);
    //         _partTransformList[i].position = partPosition;
    //     }
    // }
}

