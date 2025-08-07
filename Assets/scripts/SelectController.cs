 using UnityEngine;
using System.Collections.Generic;

public class SelectController : MonoBehaviour
{
    [Header("Prefab, который будет появляться при клике")]
    public GameObject Cube;

    [Header("Слой, по которому можно кликать (например, Ground)")]
    public LayerMask layer,LayerMask;

    private Camera _cam;
    private GameObject _cubeSelection;
    private Vector3 _startPoint;
    public List <GameObject> players;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && players.Count > 0)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit agentTarget, 1000f, layer))
                foreach (var el in players)
                    el.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(agentTarget.point);
        }
        if (_cam == null) return;

        // Клик мышью — создаём куб
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var el in players)
                if(el != null)
                el.transform.GetChild(0).gameObject.SetActive(false);
                
            players.Clear();
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layer))
            {
                // Удаляем старый, если был
                if (_cubeSelection != null)
                {
                    Destroy(_cubeSelection);
                }

                _startPoint = hit.point;

                Vector3 spawnPosition = new Vector3(_startPoint.x, 1f, _startPoint.z);
                _cubeSelection = Instantiate(Cube, spawnPosition, Quaternion.identity);

                // Начальный масштаб 0.1 чтобы видеть куб сразу
                _cubeSelection.transform.localScale = new Vector3(0.1f, 1f, 0.1f);
            }
        }

        
        if (Input.GetMouseButton(0) && _cubeSelection != null)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitDrag, 1000f, layer))
            {
                float scaleX = Mathf.Abs((hitDrag.point.x - _startPoint.x) * -1);
                float scaleZ = Mathf.Abs(hitDrag.point.z - _startPoint.z);

               
                scaleX = Mathf.Max(0.1f, scaleX);
                scaleZ = Mathf.Max(0.1f, scaleZ);

                _cubeSelection.transform.localScale = new Vector3(scaleX, 1f, scaleZ);

                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
             RaycastHit[] hits = Physics.BoxCastAll(
                _cubeSelection.transform.position,
                _cubeSelection.transform.localScale,
                Vector3.up,
                Quaternion.identity,
                0,
                LayerMask);

             foreach (var el in  hits)
             {
                 if(el.collider.CompareTag("Enemy")) continue;
                 players.Add(el.transform.gameObject);
                 el.transform.GetChild(0).gameObject.SetActive(true);
             }
            
            
            Destroy(_cubeSelection);
        }
        
    }
    
}