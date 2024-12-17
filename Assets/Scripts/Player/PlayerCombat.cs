using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator _animator;
    private PlayerStats _stats;

    [Header("GameObjects")]
    public Transform attackPoint;      // Точка, откуда будет исходить атака
    public LayerMask enemyLayers;      // Слой врагов
    [Header("Costs")]
    public int pounchCost = 10;

    [Header("Parameters")]
    private float attackCoolDown = 1;
    public float attackRange = 5f;   // Радиус атаки
    public int attackDamage = 20;      // Урон от атаки
    


    public bool canAttack { get; private set; }

    private void Start()
    {
        try
        {
            GameEventsManager.instance.inputEvents.onAttackPressed += Attack;

            _animator = GetComponent<Animator>();
            _stats = GetComponent<PlayerStats>();

            canAttack = true;

            if (_animator == null)
                throw new MissingComponentException(nameof(Animator), gameObject.name, GetType().Name, "Make sure you have assigned AnimatorController component to player");

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
        GameEventsManager.instance.inputEvents.onAttackPressed -= Attack;
    }


    void Attack()
    {
        if (PauseMenu.instance.isPaused || InventoryUI.Instance.isActive || DialogueManager.instance.dialogueIsPlaying)
            return;

        if (canAttack && _stats.TakeStamina(pounchCost))
        {
            canAttack = false;
            _animator.SetTrigger("Attack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                NPCStats foe = enemy.GetComponent<NPCStats>();

                if (!foe.isDead)
                {
                    foe.TakeDamage(attackDamage);
                }
            }

            StartCoroutine(ResetAttackWithDelay(attackCoolDown));
        }
    }

    IEnumerator ResetAttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

}
