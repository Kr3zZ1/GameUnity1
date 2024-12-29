using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ImageRevealMultiple : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Настройки изображений")]
    public Image[] targetImages;  // Массив изображений
    public float fadeSpeed = 2f;  // Скорость проявления

    private Coroutine fadeCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeImages(1f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeImages(0f));
    }

    private IEnumerator FadeImages(float targetAlpha)
    {
        bool fading = true;
        while (fading)
        {
            fading = false;
            foreach (Image img in targetImages)
            {
                if (img != null && !Mathf.Approximately(img.color.a, targetAlpha))
                {
                    Color color = img.color;
                    color.a = Mathf.MoveTowards(img.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
                    img.color = color;
                    fading = true;
                }
            }
            yield return null;
        }
    }
}