using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public static ShipController instance;
    [SerializeField]
    protected MovementControlsBase _movementControls;

    [SerializeField]
    List<ShipEngine> _engines;

    //[SerializeField]
    //private AnimateCockpitControls _cockpitAnimationControls;

    Rigidbody _rigidBody;
    [Range(-1f, 1f)]
    float _pitchAmount, _rollAmount, _yawAmount = 0f;

    protected DamageHandler _damageHandler;

    IMovementControls MovementInput => _movementControls;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _damageHandler = GetComponent<DamageHandler>();
        instance = this;
        _currentSpeed = _initialSpeed;   // Set initial speed
        _startPosition = transform.position;  // Store the initial position
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Rock")
        {
            other.gameObject.SetActive(false);
        }
    }
    public virtual void OnEnable()
    {
        if (_damageHandler == null) return;
        _damageHandler.Init(_shipData.MaxHealth);
    }

    public virtual void Update()
    {
        
    }

    private float beforeTurn;
    void FixedUpdate()
    {
        
        if (isGameStarted)
        {
            _rollAmount = MovementInput.RollAmount;
            _yawAmount = MovementInput.YawAmount;
            _pitchAmount = MovementInput.PitchAmount;
            if (!Mathf.Approximately(0f, _pitchAmount))
            {
                _rigidBody.AddTorque(transform.right * (_shipData.PitchForce * _pitchAmount * Time.fixedDeltaTime * 0.5f));
            }

            if (!Mathf.Approximately(0f, _rollAmount))
            {
                _rigidBody.AddTorque(transform.forward * (_shipData.RollForce * _rollAmount * Time.fixedDeltaTime * 0.5f));
            }

            if (!Mathf.Approximately(0f, _yawAmount))
            {
                _rigidBody.AddTorque(transform.up * (_yawAmount * _shipData.YawForce * Time.fixedDeltaTime * 0.5f));
            }
            // Increase speed over time
            _currentSpeed += _speedIncreaseRate * Time.fixedDeltaTime;

            // Apply continuous forward movement with increasing speed
            _rigidBody.AddForce(transform.forward * _currentSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            // Calculate the distance traveled
            _distanceTraveled = Vector3.Distance(_startPosition, transform.position);
            _totalDistanceTraveled += _distanceTraveled;
            _startPosition = transform.position;
            // Update the TMP text with the distance traveled
            //_distanceText.text = $"Distance: {_totalDistanceTraveled:F0} meters";

            _distanceText.text = (_totalDistanceTraveled / 1000) .ToString("F4") + "X"; 

            moveTimer += Time.deltaTime;

            Vector3 pos = transform.position;
            //HandleBounds();
            //transform.position = pos;

            if (!hasExploded && (int)_totalDistanceTraveled >= FInalvalue)
            {
                _rigidBody.isKinematic = true;
                _distanceText.text = (FInalvalue/1000) .ToString() + "X";
                Debug.Log("hello");
                DestroyShip();
                hasExploded = true;  // Set the flag to true so the explosion doesn't repeat
                isGameStarted = false;
            }
        }
    }



    public bool HandleBounds()
    {
        Vector3 pos = transform.position;

        bool shouldTurnBackward = false;

        if (pos.x >= (bounds.x / 2 - beforeTurn) || pos.x <= (-bounds.x / 2 + beforeTurn))
        {
            shouldTurnBackward = true;
        }

        if (pos.y >= (bounds.y / 2 - beforeTurn) || pos.y <= (-bounds.y / 2 + beforeTurn))
        {
            shouldTurnBackward = true;
        }

        if (pos.z >= (bounds.z / 2 - beforeTurn) || pos.z <= (-bounds.z / 2 + beforeTurn))
        {
            shouldTurnBackward = true;
        }

        //if (shouldTurnBackward)
        //{
        //    // Turn backward
        //    //transform.Rotate(Vector3.up * 180f); // Rotate 180 degrees
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed * Time.deltaTime);
        //}

        return shouldTurnBackward;
        //pos.x = Mathf.Clamp(pos.x, (-bounds.x / 2 - beforeTurn), (bounds.x / 2 - beforeTurn));
        //pos.y = Mathf.Clamp(pos.y, (-bounds.y / 2 - beforeTurn), (bounds.y / 2 - beforeTurn));
        //pos.z = Mathf.Clamp(pos.z, (-bounds.z / 2 - beforeTurn), (bounds.z / 2 - beforeTurn));
        //transform.position = pos;
    }





























    private int FInalvalue;


    [SerializeField]
    ShipDataSo _shipData;


    [SerializeField]
    private TMP_Text _distanceText;  // Reference to the TMP text component

    [SerializeField]
    private TMP_Text FinavalueToDEstroy;
    private float _initialSpeed = 500f;   // Initial speed of forward movement
    private float _speedIncreaseRate = 1000f;   // Rate at which speed increases over time
    private float _currentSpeed;
    private Vector3 _startPosition;  // To store the initial position
    private float _distanceTraveled;  // To store the distance traveled
    private bool isGameStarted = false;
    private bool hasExploded = false;  // Flag to check if the explosion has already occurred

    public GameObject bigExplosionPrefab;


    public void disableText()
    {
        _distanceText.gameObject.SetActive(false);
        FinavalueToDEstroy.gameObject.SetActive(false);
    }
    public void StartShipMovement()
    {
        IsActivated = true;
        _distanceText.gameObject.SetActive(true);
        FinavalueToDEstroy.gameObject.SetActive(true);
        bigExplosionPrefab.SetActive(false);
        FInalvalue = Random.Range(200,20000);
        FinavalueToDEstroy.text = (FInalvalue/1000).ToString();
        foreach (ShipEngine engine in _engines)
        {
            engine.Init(MovementInput, _rigidBody, _shipData.ThrustForce / _engines.Count);
        }

        //if (_cockpitAnimationControls != null)
        //{
        //    isGameStarted = true;
        //    SetNextMoveTime();
        //    _cockpitAnimationControls.Init(MovementInput);
        //    beforeTurn = Random.Range(100, 200);
        //}
    }

    public bool IsActivated
    {
        get { return isGameStarted; }  // Getter
        set { isGameStarted = value; } // Setter
    }

    public float turnSpeed = 0.02f;
    public Vector3 bounds = new Vector3(2000f, 1000f, 4000f);

    private Vector3 targetDirection;
    public float minMoveTime = 4f; // Minimum time to move straight before turning
    public float maxMoveTime = 7f; // Maximum time to move straight before turning

    private float moveTime; // Time to move straight before the next turn
    private float moveTimer; // Tracks the current movement time
    private float _totalDistanceTraveled = 0f;
    //void FixedUpdate()
    //{
    //    if (isGameStarted)
    //    {
    //        // Increase speed over time
    //        _currentSpeed += _speedIncreaseRate * Time.fixedDeltaTime;

    //        // Apply continuous forward movement with increasing speed
    //        _rigidBody.AddForce(transform.forward * _currentSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

    //        // Calculate the distance traveled
    //        _distanceTraveled = Vector3.Distance(_startPosition, transform.position);
    //        _totalDistanceTraveled += _distanceTraveled;
    //        _startPosition = transform.position;
    //        // Update the TMP text with the distance traveled
    //        _distanceText.text = $"Distance: {_totalDistanceTraveled:F0} meters";

    //        moveTimer += Time.deltaTime;

    //        // Check if it's time to turn
    //        if (moveTimer >= moveTime)
    //        {
    //            // Generate a random point within the bounds
    //            Vector3 randomPoint = new Vector3(
    //                Random.Range(-bounds.x / 2, bounds.x / 2),
    //                Random.Range(-bounds.y / 2, bounds.y / 2),
    //                Random.Range(-bounds.z / 2, bounds.z / 2)
    //            );

    //            // Calculate the new direction to the random point
    //            targetDirection = (randomPoint - transform.position).normalized;

    //            // Reset the move timer and set the next move time
    //            moveTimer = 0f;
    //            SetNextMoveTime();
    //        }

    //        // Smoothly rotate towards the target direction
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed * Time.deltaTime);

    //        // Keep the GameObject within bounds
    //        Vector3 pos = transform.position;
    //        pos.x = Mathf.Clamp(pos.x, -bounds.x / 2, bounds.x / 2);
    //        pos.y = Mathf.Clamp(pos.y, -bounds.y / 2, bounds.y / 2);
    //        pos.z = Mathf.Clamp(pos.z, -bounds.z / 2, bounds.z / 2);
    //        transform.position = pos;

    //        if (!hasExploded && (int)_totalDistanceTraveled >= FInalvalue)
    //        {
    //            _distanceText.text = FInalvalue.ToString();
    //            Debug.Log("hello");
    //            DestroyShip();
    //            hasExploded = true;  // Set the flag to true so the explosion doesn't repeat
    //            isGameStarted = false;
    //        }
    //    }
    //}

    bool isExploded = false;
    public float explosionForce = 500f;  // Force of the explosion
    public float explosionRadius = 5f;   // Radius of the explosion
    public float upwardsModifier = 1f;   // Additional upward force

    void SetNextMoveTime()
    {
        moveTime = Random.Range(minMoveTime, maxMoveTime);
    }
    void DestroyShip()
    {

        CameraFollow.instance.OnDestruction();
        TriggerExplosion();
    }

    public void TriggerExplosion()
    {
        if (isExploded) return;  // Prevent multiple explosions

        isExploded = true;  // Set the flag to true

        if (bigExplosionPrefab != null)
        {
            bigExplosionPrefab.SetActive(true);
        }
        DismantlePlane.instance.TriggerExplosion();
        // Optionally, destroy the parent object or deactivate it
        Destroy(gameObject, 2f);  // Destroy the parent object after 2 seconds
    }
    private void OnDisable()
    {
    }
}
