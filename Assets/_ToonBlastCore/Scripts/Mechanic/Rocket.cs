using System;
using System.Collections;
using System.Collections.Generic;
using _ToonBlastCore.Scripts.Managers;
using Level;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public bool isVertical;
    public bool isActive;
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rb;
    public Rigidbody2D otherRb;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        SetDirection();
    }

    private void SetDirection()
    {
        isVertical = UnityEngine.Random.Range(0, 1000) % 2 == 0;

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

        SetRocketProperties();
        InstantiateOtherRocket();
        LaunchRockets();
        StartCoroutine(DestroyRockets());
    }

    private void SetRocketProperties()
    {
        rb.isKinematic = true;
        boxCollider2D.size /= 2;
        isActive = true;
        rb.constraints = RigidbodyConstraints2D.None;
        boxCollider2D.isTrigger = true;
    }

    private void InstantiateOtherRocket()
    {
        var otherRocket = Instantiate(gameObject, transform.position,  transform.localRotation * Quaternion.Euler(0, 0, isVertical? -90 : -180));
        otherRb = otherRocket.GetComponent<Rigidbody2D>();
    }

    private void LaunchRockets()
    {
        var direction = isVertical ? new Vector3(0, 1, 0) : new Vector3(-1, 0, 0);
        rb.velocity = direction * 5f;
        otherRb.velocity = direction * -1 * 5f;
    }

    IEnumerator DestroyRockets()
    {
        yield return new WaitForSeconds(0.7f);
        isActive = false;
        yield return new WaitForSeconds(1.3f);
        Destroy(otherRb.gameObject);
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
