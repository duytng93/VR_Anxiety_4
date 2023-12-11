using UnityEngine;
using System.Collections;

public class SimpleWalk : MonoBehaviour
{
    public Transform targetPosition;
    public float speed = 1.0f;
    public float waitTime = 2.0f;

    private Animator animator;
    private Vector3 startPosition;
    private bool isReturning = false;
    private bool isWaiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isWaiting) return;

        float step = speed * Time.deltaTime;
        Vector3 target = isReturning ? startPosition : targetPosition.position;

        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = toRotation;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, step);

        float distanceToTarget = (transform.position - target).magnitude;

        if (distanceToTarget <= 0.5f)
        {
            Debug.Log(isReturning ? "Reached Original Position" : "Reached Target Position");
            transform.rotation = Quaternion.identity;
            isReturning = !isReturning;
            StartCoroutine(WaitCoroutine());
        }

        animator.SetBool("isWalking", !isWaiting);
    }

    IEnumerator WaitCoroutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
