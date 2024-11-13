using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordStomp : MonoBehaviour, ISkill
{

    [SerializeField] public float pushDuration = 0.15f;
    [SerializeField] public float maxDistance = 3.65f;

    private Collider2D stompAreaCollider;
    private bool isReady = true;


    private void Start()
    {
        Transform stompAreaTransform = PlayerController.Instance.transform.Find("Stomp Area");
        stompAreaCollider = stompAreaTransform.GetComponent<Collider2D>();
    }

    public void ExecuteSkill(float cooldown)
    {
        if (stompAreaCollider == null)
        {
            stompAreaCollider = PlayerController.Instance.transform.Find("Stomp Area").GetComponent<Collider2D>();
        }

        if (isReady)
        {
            // Find all enemies in collider on layer mask "Enemy"
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(LayerMask.GetMask("Enemy"));

            Collider2D[] enemiesInArea = new Collider2D[10];
            Debug.Log(stompAreaCollider.name);
            int enemyCount = stompAreaCollider.OverlapCollider(contactFilter, enemiesInArea);

            Shake.Instance.ShakeScreen();

            if (enemyCount > 0)
            {
                foreach (Collider2D enemy in enemiesInArea)
                {

                    if (enemy != null)
                    {
                        Vector2 directionToPlayer = (enemy.transform.position - PlayerController.Instance.transform.position).normalized;

                        float distanceToCenter = Vector2.Distance(enemy.transform.position, stompAreaCollider.bounds.center);

                        // Moves enemies as far away from the player as they are from the center of Collider
                        float maxColliderRadius = stompAreaCollider.bounds.extents.magnitude;
                        float relativeDistance = Mathf.InverseLerp(0f, maxColliderRadius, distanceToCenter);
                        float pushDistance = Mathf.Lerp(maxDistance, 0f, relativeDistance / 1.45f);

                        StartCoroutine(SmoothPushEnemy(enemy, directionToPlayer, pushDistance));
                    }
                }
            }
        }

        StartCoroutine(SetCooldown(cooldown));
    }

    private IEnumerator SmoothPushEnemy(Collider2D enemy, Vector2 direction, float distance)
    {
        float journeyLength = distance;
        float startTime = Time.time;
        Vector3 startPosition = enemy.transform.position;
        Vector3 targetPosition = startPosition + (Vector3)(direction * distance);

        Vector3 lastValidPosition = startPosition;

        // continuous checking to see if the enemy has gone out of bounds
        while (Time.time - startTime < pushDuration)
        {
            // progress in time
            float distanceCovered = (Time.time - startTime) * (distance / pushDuration);
            float fractionOfJourney = distanceCovered / journeyLength;

            //Smoothly shifting the enemy's position
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            RaycastHit2D hit = Physics2D.Raycast(currentPosition, Vector2.zero, 0f, LayerMask.GetMask("Environment"));

            if (hit.collider == null) 
            {
                enemy.transform.position = currentPosition;
                lastValidPosition = currentPosition;
            }
            else
            {
                enemy.transform.position = lastValidPosition;
                break;
            }

            yield return null;
        }

        if (Physics2D.Raycast(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Environment")).collider == null)
        {
            enemy.transform.position = targetPosition;
        }

    }

    private IEnumerator SetCooldown(float cooldown)
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }




}
