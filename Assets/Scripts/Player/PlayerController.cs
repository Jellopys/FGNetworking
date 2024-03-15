using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour, IPlayerActions
{
    private PlayerInput _playerInput;
    private Vector2 _moveInput = new();
    private Vector2 _cursorLocation;
    private bool _isSpeedBurstOnCooldown = false;
    private float _speedBurstMultiplier = 1f;

    private SpriteRenderer _spriteRenderer;
    private Transform _shipTransform;
    private Rigidbody2D _rb;

    private Transform turretPivotTransform;

    public UnityAction<bool> onFireEvent;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float shipRotationSpeed = 200f;
    [SerializeField] private float turretRotationSpeed = 4f;
    [SerializeField] private float speedBurstCooldown = 3;
    [SerializeField] private float speedBurstDuration = 0.5f;
    

    [SerializeField] private Sprite moveSprite;
    [SerializeField] private Sprite idleSprite;

    public NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isSpeedBursting = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        isMoving.OnValueChanged += ChangeSpriteOnMovement;
        isSpeedBursting.OnValueChanged += ToggleSpeedBurst;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (!IsOwner) return;

        if (_playerInput == null)
        {
            _playerInput = new();
            _playerInput.Player.SetCallbacks(this);
        }
        _playerInput.Player.Enable();

        _rb = GetComponent<Rigidbody2D>();
        _shipTransform = transform;
        turretPivotTransform = transform.Find("PivotTurret");
        if (turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);

    }

    private void ChangeSpriteOnMovement(bool oldValue, bool newValue)
    {
        if (_spriteRenderer == null) return;
        _spriteRenderer.sprite = newValue ? moveSprite : idleSprite;
    }

    private void ToggleSpeedBurst(bool oldValue, bool newValue)
    {
        _speedBurstMultiplier = newValue ? 2 : 1;
    }


    public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFireEvent.Invoke(true);
        }
        else if (context.canceled)
        {
            onFireEvent.Invoke(false);
        }
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        if (_moveInput.magnitude != 0)
        {
            isMoving.Value = true;
        }
        else
        {
            isMoving.Value = false;
        }
    }

    IEnumerator StartSpeedBurstCooldown()
    {
        _isSpeedBurstOnCooldown = true;
        isSpeedBursting.Value = true;
        yield return new WaitForSeconds(speedBurstDuration);
        isSpeedBursting.Value = false;
        yield return new WaitForSeconds(speedBurstCooldown);
        _isSpeedBurstOnCooldown = false;

    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;
        _rb.velocity = transform.up * _moveInput.y * movementSpeed * _speedBurstMultiplier;
        _rb.MoveRotation(_rb.rotation + _moveInput.x * -shipRotationSpeed * _speedBurstMultiplier * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        if(!IsOwner) return;
        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
        Vector2 targetDirection = new Vector2(screenToWorldPosition.x - turretPivotTransform.position.x, screenToWorldPosition.y - turretPivotTransform.position.y).normalized;
        Vector2 currentDirection = Vector2.Lerp(turretPivotTransform.up, targetDirection, Time.deltaTime * turretRotationSpeed);
        turretPivotTransform.up = currentDirection;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _cursorLocation = context.ReadValue<Vector2>();
    }

    public void OnSpeedBurst(InputAction.CallbackContext context)
    {
        if (context.performed && _isSpeedBurstOnCooldown == false)
        {
            StartCoroutine(StartSpeedBurstCooldown());
        }
    }
}
