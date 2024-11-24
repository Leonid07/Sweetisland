using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPAnel : MonoBehaviour
{
    public Image parentImage;
    public RectTransform panel;
    public float duration = 1.0f;
    public float delay = 1.0f;
    public float rotationSpeed = 360f; // Скорость вращения в градусах в секунду

    private Vector2 initialPosition;
    private bool hasLoggedMessage = false;

    void Start()
    {
        initialPosition = panel.anchoredPosition;
    }

    public void StartAnimation(GameObject panel, bool isActive = false)
    {
        StartCoroutine(MovePanelOutAndBack(panel, isActive));
    }
    public void StartAnimationUnLockLevel(GameObject panel)
    {
        StartCoroutine(AnimatePanelUnlock(panel));
    }
    private IEnumerator AnimatePanelUnlock(GameObject panel)
    {
        //yield return StartCoroutine(MovePanelOutAndBack(panel));


        panel.SetActive(true);
        PanelManager.InstancePanel.animUnlockLevel.gameObject.SetActive(true);
        PanelManager.InstancePanel.animUnlockLevel.StartAnimUnlockLevel();

        yield return new WaitForSeconds(delay);

        //yield return StartCoroutine(MovePanelOutAndBack(panel));
    }

    public IEnumerator MovePanelOutAndBack(GameObject panel, bool isActive = false, GameObject[] panels = null)
    {

        parentImage.raycastTarget = true;
        float elapsedTime = 0f;

        Vector2 startPosition = new Vector2(-2487, this.panel.anchoredPosition.y); // Стартовая позиция x = -2487
        Vector2 endPosition = new Vector2(2487, startPosition.y); // Конечная позиция x = 2487

        // Двигаем панель вправо
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            this.panel.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            // Вращение по оси Z
            this.panel.localRotation = Quaternion.Euler(0, 0, elapsedTime * rotationSpeed);

            // Проверка на позицию x > 0
            if (!hasLoggedMessage && this.panel.anchoredPosition.x > 0)
            {
                if (isActive == false)
                {
                    panel.SetActive(false);
                    Debug.Log(panel.name);
                    Debug.Log(isActive);
                    if (panels != null)
                    {
                        for (int i = 0; i < panels.Length; i++)
                        {
                            panels[i].SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.Log(panel.name);
                    Debug.Log(isActive);
                    panel.SetActive(true);
                    if (panels != null)
                    {
                        for (int i = 0; i < panels.Length; i++)
                        {
                            panels[i].SetActive(true);
                        }
                    }
                }
                hasLoggedMessage = true; // Устанавливаем флаг, чтобы сообщение не выводилось снова
            }

            yield return null;
        }

        this.panel.anchoredPosition = endPosition;
        this.panel.localRotation = Quaternion.identity; // Сброс вращения после завершения

        // Мгновенно возвращаем панель на стартовую позицию
        this.panel.anchoredPosition = initialPosition;
        this.panel.localRotation = Quaternion.identity; // Сброс вращения после завершения

        parentImage.raycastTarget = false;

        hasLoggedMessage = false; // Сбрасываем флаг для следующей анимации
        yield return new WaitForSeconds(0);
    }
}