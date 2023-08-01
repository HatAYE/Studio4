using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DraggingObjects : MonoBehaviour
{
    bool isDragging = false;
    Vector3 offset;
    BoxCollider2D restrains;
    private void Start()
    {
        restrains = transform.parent.transform.parent.GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, 0f, LayerMask.GetMask("Items"));

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                offset = transform.position - mousePosition;
                isDragging = true;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(mousePosition.x + offset.x, restrains.bounds.min.x, restrains.bounds.max.x),
                Mathf.Clamp(mousePosition.y + offset.y, restrains.bounds.min.y, restrains.bounds.max.y),
                transform.position.z);

            transform.position = clampedPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
