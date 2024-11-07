using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private LayerMask playerLayer;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange && IsPlayerInSight())
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private bool IsPlayerInSight()
    {
        // If playerLayer is not filled in
        if (playerLayer == 0)
        {
            return true;
        }

        Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
        // Provedeme Raycast na hráèe
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, directionToPlayer, attackRange, playerLayer);

        if (hitPlayer.collider != null && hitPlayer.collider.transform == PlayerController.Instance.transform)
        {
            // Pokud jsme zasáhli hráèe, zkontrolujeme pøekážky na vrstvì "Environment"

            // Provedeme Raycast na vrstvì Environment a zkontrolujeme, zda nìjaká pøekážka je mezi nepøítelem a hráèem
            RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, directionToPlayer, attackRange, LayerMask.GetMask("Environment"));

            // Pokud je nìjaká pøekážka a je blíže k nepøíteli než hráè, vrátíme false
            if (hitObstacle.collider != null && hitObstacle.distance < hitPlayer.distance)
            {
                Debug.Log("Obstacle detected between enemy and player.");
                return false;
            }

            // Pokud není žádná pøekážka mezi nepøítelem a hráèem, vrátíme true
            return true;
        }

        // Pokud hráè není v dosahu, vrátíme false
        return false;
    }


    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {

            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }


    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }


}
