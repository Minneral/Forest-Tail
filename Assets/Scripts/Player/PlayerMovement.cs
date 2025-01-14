using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioClip footStepClip;
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip dodgClip;
    private CharacterController _controller;
    private Animator _animator;
    private Transform _camera;
    private Transform _groundCheck;
    private PlayerStats _stats;
    private InventoryUI _inventoryUI;

    public float stepInterval = 0.5f;
    private float stepTimer = 0;
    Vector3 direction;
    private float speed;
    public float turnSmoothTime = 0.1f;  // Время для плавного поворота
    float turnSmoothVelocity;  // Переменная для хранения скорости 

    public float walkSpeed = 1f; // Скорость ходьбы 
    public float runSpeed = 3f; // Скорость бега
    public float dodgeDistance = 5f; // Дистанция отскока
    public float dodgeCoolDown = 1f;
    private bool canDodge = true;
    public float dodgeDuration = 0.5f;

    public float groundDistance = 0.01f; // Радиус сферы для проверки касания с землей
    public LayerMask groundMask; // Маска для слоев земли

    public float gravity = -9.81f; // Гравитация 
    public float jumpHeight = 1f; // Высота прыжка 
    private Vector3 velocity; // Вектор для хранения скорости движения

    public int staminaCostPerSecond = 5; // Расход стамины за секунду бега
    public int jumpStaminaCost = 20; // Расход стамины за прыжок
    public int dodgeStaminaCost = 15; // Расход стамины за отскок

    private float staminaUsage; // Счетчик для учета потраченной стамины

    void Start()
    {
        try
        {
            GameEventsManager.instance.inputEvents.onMovePressed += MovePressed;
            GameEventsManager.instance.inputEvents.onJumpPressed += Jump;
            GameEventsManager.instance.inputEvents.onDodgePressed += Dodge;
            GameEventsManager.instance.playerEvents.onPlayerDeath += HandleDeath;

            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _camera = Camera.main?.transform;
            _groundCheck = transform.Find("GroundChecker");
            _stats = GetComponent<PlayerStats>();
            _inventoryUI = GameObject.FindObjectOfType<InventoryUI>();

            if (_controller == null)
                throw new MissingComponentException(nameof(CharacterController), gameObject.name, GetType().Name);

            if (_animator == null)
                throw new MissingComponentException(nameof(Animator), gameObject.name, GetType().Name, "Make sure you have assigned AnimatorController component to player");

            if (_camera == null)
                throw new MissingComponentException(nameof(Camera), gameObject.name, GetType().Name, "Make sure you have assigned tag 'MainCamera' to an existing Camera");

            if (_groundCheck == null)
                throw new MissingComponentException(nameof(Transform), gameObject.name, GetType().Name, "You need to assign an Empty object called 'GroundChecker' as a parent object");

            if (_stats == null)
                throw new MissingComponentException(nameof(PlayerStats), gameObject.name, GetType().Name, "You need to append 'PlayerStats' script");
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.inputEvents.onMovePressed -= MovePressed;
        GameEventsManager.instance.inputEvents.onJumpPressed -= Jump;
        GameEventsManager.instance.inputEvents.onDodgePressed -= Dodge;
        GameEventsManager.instance.playerEvents.onPlayerDeath -= HandleDeath;
        // GameEventsManager.instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
        // GameEventsManager.instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
    }

    private void Update()
    {
        if (GameEventsManager.instance.IsAnyUIVisible(typeof(QuestPanelUI)))
        {
            StopMovement();
            return;
        }

        HandleMovement();
    }

    void MovePressed(Vector2 movDir)
    {
        direction = new Vector3(movDir.x, 0, movDir.y).normalized;
    }

    void Jump()
    {
        if (IsGrounded())
        {
            if (_stats.GetStamina() >= jumpStaminaCost) // Проверяем, хватает ли стамины
            {
                MasterVolume.instance.audioSource.PlayOneShot(jumpClip);

                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _animator.SetTrigger("Jumping");
                _stats.TakeStamina(jumpStaminaCost); // Снимаем стамину за прыжок
            }
        }
    }

    void Dodge()
    {
        if (IsGrounded() && canDodge)
        {
            MasterVolume.instance.audioSource.PlayOneShot(dodgClip);
            canDodge = false;
            _animator.SetTrigger("Dodge");
            _stats.TakeStamina(dodgeStaminaCost); // Снимаем стамину за отскок

            // Расчет отскока назад
            Vector3 dodgeDirection = -transform.forward * dodgeDistance;
            StartCoroutine(PerformDodge(dodgeDirection));
        }
    }

    private IEnumerator PerformDodge(Vector3 dodgeDirection)
    {
        float elapsedTime = 0;

        while (elapsedTime < dodgeDuration)
        {
            _controller.Move(dodgeDirection * (Time.deltaTime / dodgeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dodgeCoolDown);
        canDodge = true;
    }

    void HandleMovement()
    {
        bool isMoving = direction.magnitude >= 0.1f;
        bool canRun = _stats.GetStamina() > 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift) && !GetComponent<Inventory>().IsOverLoaded && canRun;

        speed = isRunning ? runSpeed : (isMoving ? walkSpeed : 0);

        _animator.SetBool("isWalking", isMoving && !isRunning);
        _animator.SetBool("isRunning", isRunning);

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                if (!MasterVolume.instance.audioSource.isPlaying) // Проверяем, не воспроизводится ли уже звук
                {
                    MasterVolume.instance.audioSource.PlayOneShot(footStepClip);
                }
                stepTimer = 0;
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);

        if (isRunning)
        {
            staminaUsage += staminaCostPerSecond * Time.deltaTime;
            int staminaToConsume = Mathf.CeilToInt(staminaUsage);
            if (staminaToConsume > 0)
            {
                if (!MasterVolume.instance.audioSource.isPlaying) // Проверяем, не воспроизводится ли уже звук
                {
                    MasterVolume.instance.audioSource.PlayOneShot(runClip);
                }
                _stats.TakeStamina(staminaToConsume);
                staminaUsage -= staminaToConsume;
            }
        }
        else
        {
            staminaUsage = 0f;
        }
    }



    // Добавьте проверку слоя при помощи LayerMask
    bool IsGrounded()
    {
        Ray ray = new Ray(_groundCheck.position, Vector3.down);
        float rayLength = groundDistance + 0.1f; // Небольшой запас на случай погрешности
        return Physics.Raycast(ray, rayLength);
    }


    public void StopMovement()
    {
        speed = 0;
        velocity = new Vector3(0, velocity.y, 0); //  Vector3.zero;

        _animator.SetBool("isWalking", false);
        _animator.SetBool("isRunning", false);

        _controller.Move(Vector3.zero);
    }
    void HandleDeath()
    {
        _animator.SetTrigger("Death");
    }
}
