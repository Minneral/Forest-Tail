using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private Transform _camera;
    private Transform _groundCheck;

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

    void Start()
    {
        try
        {
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _camera = Camera.main?.transform;
            _groundCheck = transform.Find("GroundChecker");

            if (_controller == null)
                throw new MissingComponentException(nameof(CharacterController), gameObject.name, GetType().Name);

            if (_animator == null)
                throw new MissingComponentException(nameof(Animator), gameObject.name, GetType().Name);

            if (_camera == null)
                throw new MissingComponentException(nameof(Camera), gameObject.name, GetType().Name);

            if (_groundCheck == null)
                throw new MissingComponentException(nameof(Transform), gameObject.name, GetType().Name, "You need assign an Empty object called 'GroundChecker' as a parent object");
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }
    }

    private void Update()
{
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    // Определяем направление движения
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    // Проверка нажатия клавиш для ходьбы и бега
    bool isMoving = direction.magnitude >= 0.1f;
    bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift); // Бег только при зажатом Shift и W

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

    // Прыжок
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        _animator.SetTrigger("Jumping");
    }
}

}