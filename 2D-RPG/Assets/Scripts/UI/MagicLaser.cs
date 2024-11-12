using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;

    private bool isGrowing = true;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Indestructible>() && !other.isTrigger)
        {
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }


    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            // sprite 
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            // collider
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearT)) / 2, capsuleCollider2D.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    public void LaserSlash(float laserRange, float slashDeegre)
    {
        this.laserRange = laserRange;
        StartCoroutine(LaserSlashRoutine(slashDeegre));
    }

    private IEnumerator LaserSlashRoutine(float slashDeegre)
    {
        float timePassed = 0f;

        // Laser is on final range immediately
        spriteRenderer.size = new Vector2(laserRange, 1f);
        capsuleCollider2D.size = new Vector2(laserRange, capsuleCollider2D.size.y);
        capsuleCollider2D.offset = new Vector2(laserRange / 2, capsuleCollider2D.offset.y);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Mouse course is center of slash rotation
        Vector2 directionToMouse = mousePosition - transform.position;
        float startRotation = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        float middleRotation = startRotation - slashDeegre/2;
        float targetRotation = middleRotation + slashDeegre;

        // Duration of rotation
        while (timePassed < 0.5f)
        {
            timePassed += Time.deltaTime;
            float currentRotation = Mathf.Lerp(middleRotation, targetRotation, timePassed / 0.5f);
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }


    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }
}
