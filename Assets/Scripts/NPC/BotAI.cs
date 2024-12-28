using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BotAI : MonoBehaviour
{
    // Перечисление состояний бота
    public enum State { Patrol, Chase, Interact, Idle, Attack } // Состояния патрулирования, погони, взаимодействия и ожидания
    public State currentState; // Текущее состояние

    public NavMeshAgent agent; // Компонент NavMeshAgent для перемещения бота
    Transform player; // Ссылка на объект игрока для отслеживания его позиции
    public float patrolRadius = 10f; // Радиус патрулирования
    public float chaseRadius = 5f; // Радиус преследования игрока
    public float interactRadius = 3f; // Радиус для начала взаимодействия
    public float idleTime = 5f; // Время ожидания перед переходом в состояние Idle

    private Vector3 walkPoint; // Точка для патрулирования
    private bool walkPointSet; // Проверка, установлена ли точка патрулирования
    private float idleTimer; // Таймер для отслеживания времени ожидания
    private bool isInteracting; // Флаг для блокировки других состояний во время взаимодействия


    public float attackRadius = 1.5f; // Радиус атаки
    public int attackDamage = 10;        // Урон, который бот наносит
    public float timeBetweenAttacks = 1f; // Время между атаками
    public float attackCooldown = 2f; // Задержка между атаками
    private float attackTimer;

    private bool isPlayerAlive = true;

    private NPCStats stats;
    private Vector3 lastPlayerPosition;
    public Animator animator; // Ссылка на Animator для анимаций бота
    public LayerMask whatIsGround; // Слой, указывающий, что является землей для патрулирования

    public string NPCId { get; private set; }

    private void Awake()
    {
        NPCId = name + GetComponent<NPCStats>().type;

        if (GameManager.instance.EnemiesEliminated.Contains(NPCId))
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath += Death;
        GameEventsManager.instance.playerEvents.onPlayerDeath += HandlePlayerDeath;
        stats = GetComponent<NPCStats>();
        player = GameObject.Find("Player").transform;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath -= Death;
        GameEventsManager.instance.playerEvents.onPlayerDeath -= HandlePlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Interact:
                Interact();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Idle:
                Idle();
                break;
        }

        if (!isPlayerAlive)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Сначала проверяем радиус атаки
        if (distanceToPlayer <= attackRadius)
        {
            if (currentState != State.Attack)
            {
                currentState = State.Attack;
                attackTimer = attackCooldown; // Сброс таймера для немедленной атаки
            }
        }
        // Затем проверяем радиус взаимодействия
        else if (distanceToPlayer <= interactRadius)
        {
            if (currentState != State.Interact)
            {
                currentState = State.Interact;
            }
        }
    }

    void Death(NPCTypes type)
    {
        if (stats.GetHealth() > 0) return;

        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return null; // Подождите до следующего кадра

        agent.enabled = false;
        animator.SetTrigger("Death");

        var healthBarService = GetComponentInChildren<HealthBarService>();
        healthBarService.DestroyService();

        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false; // Выключите скрипт
    }

    void Patrol()
    {
        animator.Play("walking");
        animator.SetBool("isWalking", true);

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        // Проверка, достиг ли бот точки патрулирования
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;

        // Если бот перестал двигаться, запускаем таймер для перехода в Idle
        if (agent.velocity.magnitude < 0.1f)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                currentState = State.Idle;
                idleTimer = 0f;
            }
        }
        else
        {
            idleTimer = 0f;
        }

        // Проверка на расстояние до игрока, чтобы перейти в состояние погони       
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseRadius)
        {
            currentState = State.Chase;
        }
    }

    private IEnumerator WaitAndPatrol()
    {
        yield return new WaitForSeconds(5f);
        isInteracting = false;
        currentState = State.Patrol;
    }


    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-patrolRadius, patrolRadius);
        float randomX = Random.Range(-patrolRadius, patrolRadius);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void Chase()
    {
        // Включаем анимацию ходьбы
        animator.SetBool("isWalking", true);

        // Устанавливаем цель на игрока
        agent.SetDestination(player.position);


        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Переход в состояние взаимодействия, если игрок рядом
        if (distanceToPlayer <= interactRadius)
        {
            currentState = State.Interact;
        }
        // Возврат в патрулирование, если игрок далеко
        else if (distanceToPlayer > chaseRadius)
        {
            currentState = State.Patrol;
        }
    }

    void Interact()
    {
        // Проверка на активное взаимодействие
        if (isInteracting) return;

        // Устанавливаем флаг для блокировки других состояний
        isInteracting = true;

        // Останавливаем бота и запускаем анимацию взаимодействия
        agent.SetDestination(transform.position);

        animator.SetBool("isWalking", false);
        animator.SetTrigger("Interact");

        // Бот поворачивается к игроку
        transform.LookAt(player);
        Debug.Log("Бот взаимодействует с игроком!");

        // Корутина для завершения взаимодействия и возвращения к патрулированию
        StartCoroutine(WaitAndPatrol());
    }

    void Idle()
    {
        // Останавливаем бота и активируем анимацию Idle
        agent.SetDestination(transform.position);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);

        // Таймер ожидания
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            currentState = State.Patrol; // Возвращаемся к патрулированию
            idleTimer = 0f;
            animator.SetBool("isIdle", false);
        }
    }

    void Attack()
    {
        agent.SetDestination(transform.position); // Останавливаем бота
        animator.SetBool("isWalking", false);
        transform.LookAt(player);

        // Проверяем таймер атаки
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            animator.SetTrigger("Attack"); // Запускаем анимацию атаки
            Debug.Log("Бот атакует игрока!");
            attackTimer = 0f;

            player.GetComponent<PlayerStats>().TakeDamage(attackDamage);
        }

        // Возвращаемся к состоянию погони, если игрок далеко
        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            currentState = State.Chase;
        }
    }

    void HandlePlayerDeath()
    {
        // Устанавливаем флаг, что игрок мертв
        isPlayerAlive = false;

        // Останавливаем бота
        agent.SetDestination(transform.position);

        // Переводим бота в состояние Idle
        currentState = State.Idle;

        // Отключаем анимации атаки и ходьбы
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);

        // Логирование для отладки
        Debug.Log("Игрок мертв. Бот переходит в состояние ожидания.");
    }

}
