using UnityEngine;
using System.Collections;

public class CarAttack : MonoBehaviour
{
    public int _health = 100;
    public float radius = 70f;
    public GameObject bullet;

    private Transform currentTarget;
    private Coroutine attackCoroutine;

    private void Update()
    {
        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            FindNewTarget();
        }
    }

    private void FindNewTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var col in hitColliders)
        {
            if ((CompareTag("Player") && col.CompareTag("Enemy")) ||
                (CompareTag("Enemy") && col.CompareTag("Player")))
            {
                currentTarget = col.transform;

                if (CompareTag("Enemy"))
                    GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(currentTarget.position);

                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);

                attackCoroutine = StartCoroutine(AttackTarget(currentTarget));
                break; // атакуємо тільки одного
            }
        }
    }

    private bool IsTargetInRange(Transform target)
    {
        if (target == null || !target.gameObject.activeInHierarchy)
            return false;

        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= radius;
    }

    IEnumerator AttackTarget(Transform target)
    {
        while (target != null && IsTargetInRange(target))
        {
            Vector3 spawnPos = transform.GetChild(1).position;

            GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity);

            BulletController bulletCtrl = obj.GetComponent<BulletController>();
            if (bulletCtrl != null)
            {
                bulletCtrl.position = target.position; // або bulletCtrl.SetTarget(target);
            }

            yield return new WaitForSeconds(1f);
        }

        // якщо атака завершилась — очищаємо ціль і корутину
        currentTarget = null;
        attackCoroutine = null;
    }
}
