using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CanvasGroup _activatedCheckpointGroup;
    private bool _isActivated = false;

    private void Start()
    {
        SetAlpha(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isActivated)
            ActivateCheckpoint();
    }

    private void ActivateCheckpoint()
    {
        _isActivated = true;
        Debug.Log("Checkpoint Activated at position: " + transform.position);
        GameManager.Instance.SetCheckpoint(transform.position);
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        yield return StartCoroutine(Fade(0, 1, 1f));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Fade(1, 0, 1f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(endAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (_activatedCheckpointGroup != null)
        {
            _activatedCheckpointGroup.alpha = alpha;
            _activatedCheckpointGroup.interactable = alpha > 0.5f;
            _activatedCheckpointGroup.blocksRaycasts = alpha > 0.5f;
        }
    }
}