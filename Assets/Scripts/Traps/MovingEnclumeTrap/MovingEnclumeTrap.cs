using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;


//Line renderer sur l'enclume pour montrer ou tombe l'enclume

public class MovingEnclumeTrap : MonoBehaviour
{
    public Enclume enclumePrefab;
    public Enclume enclume;
    public Transform enclumeSpawnPoint;
    public Animator animator;
    public float moveSpeed = 5f; // Vitesse de déplacement de l'enclume
    //public float moveRange = 10f; // Distance maximale que l'enclume peut parcourir
    public BoxCollider moveArea;  // Zone de déplacement de l'enclume
    
    private Vector3 moveDirection;
    private Vector3 initialPosition;

    private float coolDown;
    private bool isDropped;

    private PlayerInput _playerInput;
    private bool _isActivated = false;

    public GameObject enclumeHolder;
    
    [Header("SFX")] 
    //public AudioClip activateClip;
    
    public AudioClip moveClip;
    public AudioSource _audioSource;
    

    private void Start()
    {
        initialPosition = enclumeSpawnPoint.position;
    }

    private void Update()
    {
        if (_isActivated)
        {
            //Déplacement de l'enclume
            Vector2 input = _playerInput.currentActionMap.FindAction("MoveEnclume").ReadValue<Vector2>();
            moveDirection = new Vector3(0, 0, input.x);
            Vector3 newPosition = enclumeHolder.transform.position + moveDirection * (moveSpeed * Time.deltaTime);
            if (moveArea.bounds.Contains(newPosition))
            {
                enclumeHolder.transform.position = newPosition;
                if (!_audioSource.isPlaying)
                {
                    _audioSource.clip = moveClip;
                    _audioSource.Play();
                }
            }

            // Stop audio if player is not moving the enclume
            if (input == Vector2.zero && _audioSource.clip == moveClip)
            {
                _audioSource.Stop();
            }

            if (_playerInput.actions["Drop"].WasPressedThisFrame() && !isDropped)
            {
                //_audioSource.PlayOneShot(activateClip); ( moved on enclume directly )
                isDropped = true;
                enclume.DropEnclume();
                Destroy(enclume.gameObject, 2);
                StartCoroutine(InstantiateEnclumeAfterDelay(5f)); // change delay with cooldowntime
            }

            if (_playerInput.actions["Exit"].WasPressedThisFrame())
            {
                
                _playerInput.currentActionMap = _playerInput.actions.FindActionMap("Gameplay");
                _isActivated = false;
            }
        }
    }

    public void OnControlStation(PlayerController playerController)
    {
        if (!_isActivated)
        {
            _playerInput = playerController.GetComponent<PlayerInput>();
            _playerInput.currentActionMap =
                _playerInput.actions.FindActionMap("MovingEnclume");
            _playerInput.SwitchCurrentActionMap("MovingEnclume");
            _isActivated = true;
        }
    }
    
    private IEnumerator InstantiateEnclumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enclume = Instantiate(enclumePrefab, enclumeHolder.transform, false);
        isDropped = false;
    }
}
