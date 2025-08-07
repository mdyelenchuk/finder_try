using UnityEngine;
using System.Collections;

public class GenerateEnemy : MonoBehaviour
{
    public Transform[] points;
    public GameObject factory;

    // Додаємо поле для шару Ground
    public LayerMask groundLayer;

    private void Start()
    {
        StartCoroutine(SpawnFactory());
    }

    IEnumerator SpawnFactory()
    {
        for (int i = 0; i < points.Length; i++)
        {
            yield return new WaitForSeconds(10f);

            // Спавнимо об'єкт, але ще не ставимо позицію
            GameObject spawn = Instantiate(factory);
            Destroy(spawn.GetComponent<PlaceObjects>());

            // Позиція точки спавну
            Vector3 spawnPoint = points[i].position + Vector3.up * 10f;

            // Raycast вниз для знаходження поверхні з шаром Ground
            if (Physics.Raycast(spawnPoint, Vector3.down, out RaycastHit hit, 100f, groundLayer))
            {
                spawn.transform.position = hit.point;

                float randomYRotation = Random.Range(0f, 360f);
                spawn.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);

                spawn.GetComponent<AutoCarCreate>().enabled = true;
                spawn.GetComponent<AutoCarCreate>().IsEnemy = true;
            }
            else
            {
                Debug.LogWarning("Не знайдено землю під точкою спавну: " + points[i].name);
                Destroy(spawn);
            }
        }
    }
}
