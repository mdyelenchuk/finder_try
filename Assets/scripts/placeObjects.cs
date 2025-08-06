using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    public LayerMask layer;
    public float rotateSpeed = 60.0f;


    private void Start()
    {
        PositionObject();
    }

    private void PositionObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            Vector3 currentPos = transform.position;
            Vector3 hitPos = hit.point;
        }
    }

    private void Update()
     {
        PositionObject();
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, layer))

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
            }



        {
            Vector3 currentPos = transform.position;
            Vector3 hitPos = hit.point;

            // Меняем только X и Z, Y оставляем как был
            transform.position = new Vector3(hitPos.x, 3, hitPos.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<AutoCarCreate>().enabled = true;
            Destroy(this); // Удаляет этот скрипт
        }
     }
}
