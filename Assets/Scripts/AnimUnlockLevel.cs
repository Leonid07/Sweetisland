using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimUnlockLevel : MonoBehaviour
{
    [Header("Свечение")]
    public RectTransform lightPanel;
    public float rotationSpeed = 100f; // скорость вращения

    [Header("Замок")]
    public RectTransform panel; // UI панель, которую мы будем анимировать
    public float rotationDuration = 1f; // время анимации в секундах
    public Image panelImage;
    public Sprite newSprite; // новый спрайт для панели

    [Header("Свечение")]
    public RectTransform panelImageLight;    // Панель, которую нужно анимировать
    public Vector2 targetSize;     // Целевой размер панели
    public float duration = 1f;    // Длительность анимации
    public float fadeDuration = 1f; // Длительность затухания

    public void StartAnimUnlockLevel()
    {
        panelImageLight.sizeDelta = Vector2.zero;
        StartCoroutine(RotateCoroutine());
        StartCoroutine(RotatePanel());
        SoundManager.InstanceSound.soundLevelUnlock.Play();
    }

    IEnumerator RotateCoroutine()
    {
        while (true) // бесконечный цикл
        {
            lightPanel.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime); // вращаем объект по оси Z
            yield return null; // ждем до следующего кадра
        }
    }

    IEnumerator RotatePanel()
    {
        // Вращаем на 40 градусов вправо
        yield return RotateToAngle(40f, rotationDuration);

        // Вращаем на 40 градусов влево
        yield return RotateToAngle(-40f, rotationDuration);

        // Возвращаемся к 0 градусам
        yield return RotateToAngle(0f, rotationDuration);

        // Выводим сообщение в консоль
        yield return StartCoroutine(AnimateResize(panelImageLight, targetSize, duration));

        // Меняем спрайт панели
        ChangePanelSprite();

        // Запускаем затухание и отключение Raycast Target
        yield return StartCoroutine(FadeOutAndDisableRaycast());

        // Деактивируем панель после всех анимаций
        gameObject.SetActive(false);
    }

    IEnumerator AnimateResize(RectTransform rectTransform, Vector2 endSize, float time)
    {
        Vector2 startSize = rectTransform.sizeDelta;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }

        // Убедитесь, что размер точно равен целевому размеру в конце
        rectTransform.sizeDelta = endSize;
    }

    IEnumerator RotateToAngle(float targetAngle, float duration)
    {
        float startAngle = panel.eulerAngles.z;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float zRotation = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);
            panel.eulerAngles = new Vector3(0f, 0f, zRotation);
            yield return null;
        }

        panel.eulerAngles = new Vector3(0f, 0f, targetAngle);
    }

    void ChangePanelSprite()
    {
        if (panelImage != null && newSprite != null)
        {
            panelImage.sprite = newSprite;
        }
    }

    IEnumerator FadeOutAndDisableRaycast()
    {
        float elapsedTime = 0f;

        // Затухание для lightPanel
        CanvasGroup lightPanelGroup = lightPanel.GetComponent<CanvasGroup>();
        if (lightPanelGroup == null)
        {
            lightPanelGroup = lightPanel.gameObject.AddComponent<CanvasGroup>();
        }

        // Затухание для panel
        CanvasGroup panelGroup = panel.GetComponent<CanvasGroup>();
        if (panelGroup == null)
        {
            panelGroup = panel.gameObject.AddComponent<CanvasGroup>();
        }

        // Затухание для panelImageLight
        CanvasGroup panelImageLightGroup = panelImageLight.GetComponent<CanvasGroup>();
        if (panelImageLightGroup == null)
        {
            panelImageLightGroup = panelImageLight.gameObject.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (lightPanelGroup)
                lightPanelGroup.alpha = 1 - t;
            if (panelGroup)
                panelGroup.alpha = 1 - t;
            if (panelImageLightGroup)
                panelImageLightGroup.alpha = 1 - t;
            yield return null;
        }

        if (lightPanelGroup)
            lightPanelGroup.alpha = 0;
        if (panelGroup)
            panelGroup.alpha = 0;
        if (panelImageLightGroup)
            panelImageLightGroup.alpha = 0;

        // Отключаем Raycast Target
        if (lightPanelGroup)
            lightPanelGroup.blocksRaycasts = false;
        if (panelGroup)
            panelGroup.blocksRaycasts = false;
        if (panelImageLightGroup)
            panelImageLightGroup.blocksRaycasts = false;
    }
}
