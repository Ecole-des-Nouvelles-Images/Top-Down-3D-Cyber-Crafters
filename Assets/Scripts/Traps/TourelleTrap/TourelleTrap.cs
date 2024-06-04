using System.Collections;
using System.Collections.Generic;
using Player;
using Train.Wagon;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TourelleTrap : MonoBehaviour
{
    public Animator animator;
    public float turnSpeed = 10f;
    private PlayerInput _playerInput;
    bool _isActivated = false;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 100f;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    public Camera mainCamera;
    public List<MeshRenderer> childsMeshRenderers = new List<MeshRenderer>();
    private Color _baseColor;

    [Header("SFX")] public AudioClip shotClip;
    public AudioClip aimingClip;
    public AudioClip cooldownClip;
    public AudioSource audioSource;

    public GameObject Tourelle;

    private Quaternion _initialRotation;
    private bool isAiming;
    
    
    public ParticleSystem overheatParticle;

    public int maxShots = 5;
    private int shotCount = 0;
    private bool isCooldown = false;

    private void Awake()
    {
        _initialRotation = Tourelle.transform.rotation;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _baseColor = Color.gray;
    }

    private void Update()
    {
        if (_isActivated)
        {
            //Rotation de le tourelle
            Vector2 input = _playerInput.currentActionMap.FindAction("Aim").ReadValue<Vector2>();
            // Qu est ce ???
            // foreach (MeshRenderer childMr in childsMeshRenderers)
            // {
            //     childMr.material.color = _playerInput.GetComponent<PlayerController>().playerMesh
            //         .GetComponent<SkinnedMeshRenderer>().material.color;
            // }

            //Calculs par rapport à la camera
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            Vector3 direction = transform.position + cameraRight * input.x + cameraForward * input.y;

            // Vérifiez si l'entrée du joystick est égale à zéro
            if (input.magnitude > 0)
            {
                //SFX
                if (!isAiming)
                {
                    audioSource.clip = aimingClip;
                    audioSource.loop = true;
                    audioSource.Play();
                    isAiming = true;
                }

                //Animation Vanne
                animator.SetBool("isAiming", true);
                // Calculez l'angle cible en utilisant Mathf.Atan2 pour obtenir l'angle en radians, puis convertissez-le en degrés
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                // Mappez l'angle cible entre 0 et 360 degrés
                targetAngle = Mathf.Repeat(targetAngle, 360);

                // Créez une rotation cible en utilisant Quaternion.Euler
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                // Utilisez Quaternion.RotateTowards pour faire pivoter la tourelle vers la rotation cible à une vitesse constante
                Tourelle.transform.rotation = Quaternion.RotateTowards(Tourelle.transform.rotation, targetRotation,
                    turnSpeed * Time.deltaTime);
                //Changez le sens de rotation de la vanne en fonction de la visée du joueur
                animator.SetFloat("rotation", input.x);
            }
            else
            {
                animator.SetBool("isAiming", false);
                isAiming = false;
                audioSource.clip = null;
            }

            //Gérer le tir ( shoot in the actionMap )
            if (_playerInput.actions["Shoot"].WasPressedThisFrame() && Time.time > nextFireTime && !isCooldown)
            {
                shotCount++;
                //SFX
                audioSource.PlayOneShot(shotClip);
                //Instancie la balle
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                // Pas besoin de définir la vélocité du Rigidbody car la balle se déplace maintenant par elle-même
                nextFireTime = Time.time + 1f / fireRate;
                if (shotCount >= maxShots) StartCoroutine(Cooldown());
            }

            // Gérer la sortie
            if (_playerInput.actions["Exit"].WasPressedThisFrame())
            {
             Exit();
                // foreach (MeshRenderer childMr in childsMeshRenderers)
                // {
                //     childMr.material.color = _baseColor;
                // }
            }
        }
    }

    // Gere le changement de mode de controle 
    // Ajouter animation pour que le joueur soit bien devant la station de controle
    public void OnControlStation(PlayerController playerController)
    {
        if (!_isActivated)
        {
            _playerInput = playerController.GetComponent<PlayerInput>();
            _playerInput.currentActionMap =
                playerController.GetComponent<PlayerInput>().actions.FindActionMap("Turret");
            _playerInput.SwitchCurrentActionMap("Turret");
            _isActivated = true;
            SteamPipeManager steamPipeManager = FindObjectOfType<SteamPipeManager>();
            steamPipeManager.OnAllSteamPipesDestroyed += Exit;

        }
    }

    private IEnumerator Cooldown()
    {
        audioSource.PlayOneShot(cooldownClip);
        overheatParticle.Play();
        isCooldown = true; // Mettez la tourelle en phase de refroidissement
        //Activate particles or shader hot ? 
        yield return new WaitForSeconds(5); // Attendez 5 secondes
        isCooldown = false; // Sortez la tourelle de la phase de refroidissement
        shotCount = 0; // Réinitialisez le compteur de tirs
        audioSource.clip = null;
        overheatParticle.Stop();    
    }

    private void Exit()
    {
        _isActivated = false;
        animator.SetBool("isAiming", false);
        audioSource.clip = null;
        _playerInput.currentActionMap = _playerInput.actions.FindActionMap("Gameplay");
        SteamPipeManager steamPipeManager = FindObjectOfType<SteamPipeManager>();
        steamPipeManager.OnAllSteamPipesDestroyed -= Exit;

    }
    // when on the triggerZone press A to activate the tourelle
    // lock player movement
    // Change input map 
    // switch to tourelle control mode
}