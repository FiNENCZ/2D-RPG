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
        // Provedeme Raycast na hr��e
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, directionToPlayer, attackRange, playerLayer);

        if (hitPlayer.collider != null && hitPlayer.collider.transform == PlayerController.Instance.transform)
        {
            // Pokud jsme zas�hli hr��e, zkontrolujeme p�ek�ky na vrstv� "Environment"

            // Provedeme Raycast na vrstv� Environment a zkontrolujeme, zda n�jak� p�ek�ka je mezi nep��telem a hr��em
            RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, directionToPlayer, attackRange, LayerMask.GetMask("Environment"));

            // Pokud je n�jak� p�ek�ka a je bl�e k nep��teli ne� hr��, vr�t�me false
            if (hitObstacle.collider != null && hitObstacle.distance < hitPlayer.distance)
            {
                Debug.Log("Obstacle detected between enemy and player.");
                return false;
            }

            // Pokud nen� ��dn� p�ek�ka mezi nep��telem a hr��em, vr�t�me true
            return true;
        }

        // Pokud hr�� nen� v dosahu, vr�t�me false
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
