using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private Transform _camera;
    private Transform _groundCheck;
    private PlayerStats _stats;
    private InventoryUI _inventoryUI;


    Vector3 direction;
    private float speed;
    public float turnSmoothTime = 0.1f;  // Время для плавного поворота
    float turnSmoothVelocity;  // Переменная для хранения скорости 

    public float walkSpeed = 1f; // Скорость ходьбы 
    public float runSpeed = 3f; // Скорость бега

    public float groundDistance = 0.01f; // Радиус сферы для проверки касания с землей
    public LayerMask groundMask; // Маска для слоев земли
    private bool isGrounded; // Переменная для хранения информации о том, на земле ли персонаж

    public float gravity = -9.81f; // Гравитация 
    public float jumpHeight = 1f; // Высота прыжка 
    private Vector3 velocity; // Вектор для хранения скорости движения

    public int staminaCostPerSecond = 5; // Расход стамины за секунду бега
    public int jumpStaminaCost = 20; // Расход стамины за прыжок

    private float staminaUsage; // Счетчик для учета потраченной стамины

    void Start()
    {
        try
        {
            GameEventsManager.instance.inputEvents.onMovePressed += MovePressed;
            GameEventsManager.instance.inputEvents.onJumpPressed += Jump;

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
        // GameEventsManager.instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
        // GameEventsManager.instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
    }

    private void Update()
    {
        if ((_inventoryUI && _inventoryUI.isActive) || DialogueManager.instance.dialoguePanel.activeSelf)
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
        // Прыжок
        if (isGrounded)
        {
            if (_stats.GetStamina() >= jumpStaminaCost) // Проверяем, хватает ли стамины
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _animator.SetTrigger("Jumping");
                _stats.TakeStamina(jumpStaminaCost); // Снимаем стамину за прыжок
            }
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Определяем направление движения
        // Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Проверка нажатия клавиш для ходьбы и бега
        bool isMoving = direction.magnitude >= 0.1f;
        bool canRun = _stats.GetStamina() > 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift) && canRun; // Бег только при зажатом Shift, W и наличии стамины

        // Определяем скорость в зависимости от состояния
        speed = isRunning ? runSpeed : (isMoving ? walkSpeed : 0);

        // Обновляем состояние анимации
        _animator.SetBool("isWalking", isMoving && !isRunning);
        _animator.SetBool("isRunning", isRunning);

        // Если есть движение, поворачиваем персонажа и двигаем его
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Проверка земли
        isGrounded = Physics.CheckSphere(_groundCheck.position, groundDistance, groundMask);

        // Сбрасываем скорость падения, если на земле
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Применяем гравитацию
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);

        // Расход стамины при беге
        if (isRunning)
        {
            staminaUsage += staminaCostPerSecond * Time.deltaTime; // Увеличиваем счетчик на основании времени кадра
            int staminaToConsume = Mathf.CeilToInt(staminaUsage); // Округляем в большую сторону
            if (staminaToConsume > 0)
            {
                _stats.TakeStamina(staminaToConsume); // Снимаем стамину
                staminaUsage -= staminaToConsume; // Уменьшаем счетчик на израсходованное
            }
        }
        else
        {
            staminaUsage = 0f; // Сбрасываем счетчик, если не бежим
        }
    }

    private void StopMovement()
    {
        speed = 0;
        velocity = new Vector3(0, velocity.y, 0); //  Vector3.zero;

        _animator.SetBool("isWalking", false);
        _animator.SetBool("isRunning", false);

        _controller.Move(Vector3.zero);
    }

}
