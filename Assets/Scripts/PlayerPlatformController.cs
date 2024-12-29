using UnityEngine;
using System.Collections;

public class PlayerPlatformController : MonoBehaviour
{
    [SerializeField] private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerCollider;

    private void Start()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && currentOneWayPlatform != null)
            StartCoroutine(DisableCollision());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
            currentOneWayPlatform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
            currentOneWayPlatform = null;
    }

    private IEnumerator DisableCollision()
    {
        if (currentOneWayPlatform == null)
            yield break;

        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        if (platformCollider == null)
            yield break;

        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}