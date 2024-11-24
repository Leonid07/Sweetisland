using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Настройки масштабирования")]
    public float inhaleSpeed = 1f; // Скорость вдоха
    public float exhaleSpeed = 1f; // Скорость выдоха

    [Header("Пределы масштабирования")]
    public float maxScaleY = 1.2f; // Максимальный масштаб по оси Y
    public float minScaleY = 0.8f; // Минимальный масштаб по оси Y

    private bool isInhaling = true;

    [Header("Статистика монстра")]
    public int damage = 175;
    public int standartDamage = 175;
    public SpriteRenderer spriteEnemy;
    public SpriteRenderer cutting;

    private void Start()
    {
        spriteEnemy = GetComponent<SpriteRenderer>();
        cutting.gameObject.SetActive(false);
        StartCoroutine(BreathingCoroutine());
    }

    private IEnumerator BreathingCoroutine()
    {
        while (true)
        {
            Vector3 scale = transform.localScale;
            Vector3 position = transform.localPosition;

            if (isInhaling)
            {
                while (scale.y < maxScaleY)
                {
                    scale.y += inhaleSpeed * Time.deltaTime;
                    position.y += exhaleSpeed * Time.deltaTime;
                    transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    transform.position = new Vector3(position.x, position.y, position.z);
                    yield return null;
                }
                isInhaling = false;
            }
            else
            {
                while (scale.y > minScaleY)
                {
                    scale.y -= exhaleSpeed * Time.deltaTime;
                    position.y -= exhaleSpeed * Time.deltaTime;
                    transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    transform.position = new Vector3 (position.x, position.y, position.z);
                    yield return null;
                }
                isInhaling = true;
            }

            yield return null;
        }
    }
}