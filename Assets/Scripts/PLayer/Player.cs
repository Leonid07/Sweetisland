using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Скорость перемещения
    public float smoothTime = 0.3f; // Время сглаживания
    public float rotationSpeed = 5f; // Скорость вращения
    public Button continueButton; // UI кнопка для продолжения движения

    public List<Transform> points = new List<Transform>(); // Список точек
    private int currentPointIndex = 0; // Индекс текущей точки
    private bool isWaitingForButtonPress = false; // Флаг для проверки, ожидается ли нажатие кнопки
    private Coroutine moveCoroutine;

    public GameObject player;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Запускаем корутину для перемещения
        if (points.Count > 0)
        {
            moveCoroutine = StartCoroutine(MoveToPoints());
        }

        continueButton.gameObject.SetActive(false);
        // Привязываем метод к событию нажатия кнопки
        continueButton.onClick.AddListener(OnContinueButtonPressed);
    }

    private IEnumerator MoveToPoints()
    {
        while (true)
        {
            if (points.Count == 0)
            {
                continueButton.gameObject.SetActive(false);
                yield break;
            }

            // Переходим к следующей точке
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = points[currentPointIndex].position;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation((targetPosition - startPosition).normalized);

            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float startTime = Time.time;

            animator.SetBool("isRun", true); // Проигрываем анимацию бега

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;

                transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fractionOfJourney * rotationSpeed);

                yield return null;
            }

            // Останавливаем анимацию бега и запускаем анимацию idle
            animator.SetBool("isRun", false);
            animator.SetBool("isIdle", true);

            // Обеспечиваем, чтобы финальная позиция и поворот точно соответствовали целевой
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            // Проверка на наличие компонента StopPoint и вызов OnReached
            StopPoint stopPoint = points[currentPointIndex].GetComponent<StopPoint>();
            if (stopPoint != null)
            {
                stopPoint.OnReached();
                if (stopPoint.requiresPause)
                {
                    // Проверка на последнюю точку перед активацией кнопки
                    if (currentPointIndex >= points.Count - 1)
                    {
                        continueButton.gameObject.SetActive(false);
                        yield break; // Завершить корутину, если достигли последней точки
                    }

                    // Активируем кнопку и устанавливаем флаг ожидания
                    continueButton.gameObject.SetActive(true);
                    isWaitingForButtonPress = true;

                    // Ожидание нажатия кнопки для продолжения
                    yield return new WaitUntil(() => !isWaitingForButtonPress);
                }
            }

            // Если достигли последней точки, завершить корутину
            if (currentPointIndex >= points.Count - 1)
            {
                Debug.Log("Достигнута последняя точка");
                continueButton.gameObject.SetActive(false);
                yield break; // Завершить корутину, если достигли последней точки
            }

            currentPointIndex++;
        }
    }

    private void OnContinueButtonPressed()
    {
        // Скрываем кнопку и сбрасываем флаг ожидания
        continueButton.gameObject.SetActive(false);
        isWaitingForButtonPress = false;

        // Проверяем, находится ли персонаж на последней точке
        if (currentPointIndex >= points.Count - 1)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void RestartMovement()
    {
        // Останавливаем текущую корутину, если она запущена
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Сбрасываем индекс текущей точки и перезапускаем корутину
        currentPointIndex = 0;
        transform.position = points[0].position;
        transform.Rotate(0, 0, 0);
        moveCoroutine = StartCoroutine(MoveToPoints());
    }

    // Метод для проигрывания анимации атаки
    public void Attack()
    {
        animator.SetBool("isAttack", true);
        //   animator.SetBool("isAttack", false);
    }
    public void DisActiveAttack()
    {
        animator.SetBool("isAttack", false);
    }

    public void DisActiveDead()
    {
        animator.SetBool("isDead", false);
    }

    // Метод для проигрывания анимации смерти
    public void Die()
    {
        animator.SetBool("isDead", true);
    }
}
