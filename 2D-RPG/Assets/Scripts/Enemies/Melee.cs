using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour, IEnemy
{

    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    public void Attack()
    {
        enemyPathfinding.FollowPoint(PlayerController.Instance.transform.position);
    }
}
