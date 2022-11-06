using System;
using System.Collections;
using System.Collections.Generic;
using _ToonBlastCore.Scripts.Managers;
using Level;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public bool isVertical;
    public bool isOpposite;
    public bool isActive;
    public BoxCollider2D _boxCollider2D;
    public Rigidbody2D rb;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        isVertical = UnityEngine.Random.Range(0, 1000) % 2 == 0;

        // isVertical = true;
        if (isVertical)
        {
            transform.localRotation *= Quaternion.Euler(0, 0, -90);
        }
    }

    private void OnMouseDown()
    {
        CreateOppositeAndSetActive();
    }

    private void CreateOppositeAndSetActive()
    {
        EventManager.TriggerEvent("onHit", new Dictionary<string, object> { { "x", transform.position.x } });
        rb.isKinematic = true;
        _boxCollider2D.size /= 2;
        isActive = true;
        rb.constraints = RigidbodyConstraints2D.None;
        _boxCollider2D.isTrigger = true;
        var otherRocket = Instantiate(gameObject, transform.position,  transform.localRotation * Quaternion.Euler(0, 0, isVertical? -90 : -180));
        var direction = isVertical ? new Vector3(0, 1, 0) : new Vector3(-1, 0, 0);
        direction = isOpposite ? direction * -1 : direction;
        rb.velocity = direction * 5f;
        var otherRb = otherRocket.GetComponent<Rigidbody2D>();
        otherRb.isKinematic = true;
        otherRb.velocity = direction * -1 * 5f;

        StartCoroutine(DestroyRockets(otherRocket));
    }

    IEnumerator DestroyRockets(GameObject otherRocket)
    {
        yield return new WaitForSeconds(2f);
        Destroy(otherRocket);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isActive)
        {
            return;
        }
        if (col.TryGetComponent(out Tile tile))
        {
            tile.DestroyWithDelay();
        }
    }
}
