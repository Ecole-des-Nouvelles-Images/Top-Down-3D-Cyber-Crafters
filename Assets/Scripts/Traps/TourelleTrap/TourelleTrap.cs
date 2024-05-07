using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class TourelleTrap : MonoBehaviour
{
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

    public GameObject Tourelle;

    private Quaternion _initialRotation;


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
            foreach (MeshRenderer childMr in childsMeshRenderers)
            {
                childMr.material.color = _playerInput.GetComponent<PlayerController>().playerMesh
                    .GetComponent<SkinnedMeshRenderer>().material.color;
            }

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
                // Calculez l'angle cible en utilisant Mathf.Atan2 pour obtenir l'angle en radians, puis convertissez-le en degrés
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                // Mappez l'angle cible entre 0 et 360 degrés
                targetAngle = Mathf.Repeat(targetAngle, 360);

                // Créez une rotation cible en utilisant Quaternion.Euler
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                // Utilisez Quaternion.RotateTowards pour faire pivoter la tourelle vers la rotation cible à une vitesse constante
                Tourelle.transform.rotation = Quaternion.RotateTowards(Tourelle.transform.rotation, targetRotation,
                    turnSpeed * Time.deltaTime);
            }

            //Gérer le tir ( shoot in the actionMap )
            if (_playerInput.actions["Shoot"].WasPressedThisFrame() && Time.time > nextFireTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                // Utilisez la rotation de la tourelle pour définir la direction initiale de la balle
                Vector3 bulletDirection = Tourelle.transform.forward;
                bulletDirection.y = 0; // Réinitialisez la composante y pour que la balle reste à plat sur le sol
                bulletDirection =
                    bulletDirection.normalized; // Normalisez la direction pour obtenir une vitesse constante
                rb.velocity = bulletDirection * bulletSpeed;
                nextFireTime = Time.time + 1f / fireRate;
            }

            // Gérer la sortie
            if (_playerInput.actions["Exit"].WasPressedThisFrame())
            {
                _isActivated = false;
                _playerInput.currentActionMap = _playerInput.actions.FindActionMap("Gameplay");
                foreach (MeshRenderer childMr in childsMeshRenderers)
                {
                    childMr.material.color = _baseColor;
                }
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
        }
    }
    // when on the triggerZone press A to activate the tourelle
    // lock player movement
    // Change input map 
    // switch to tourelle control mode
}