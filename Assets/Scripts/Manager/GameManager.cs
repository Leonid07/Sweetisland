using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Текста с золотом")]
    public Text[] textGold;
    public int gold;
    public string idGold = "gold";

    [Header("Противники")]
    public Enemy[] enemies;
    public SpriteRenderer[] spriteRendererEnemy;
    public SpriteRenderer[] spriteRendererEnemyCutting;
    public Sprite[] spriteEnemy;

    [Header("Игрок")]
    public Player player;

    public static GameManager InstanceGame { get; private set; }

    private void Awake()
    {
        if (InstanceGame != null && InstanceGame != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceGame = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RestartEnemy()
    {
        for (int i =0; i < spriteRendererEnemy.Length; i++)
        {
            Color currentColor = spriteRendererEnemy[i].color;
            Color currentColorCutting = spriteRendererEnemyCutting[i].color;

            float newAlpha = 1f;
            float newAlphaCutting = 1f;

            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            Color newColorCutting = new Color(currentColorCutting.r, currentColorCutting.g, currentColorCutting.b, newAlphaCutting);

            spriteRendererEnemy[i].color = newColor;
            spriteRendererEnemyCutting[i].color = newColorCutting;

            spriteRendererEnemyCutting[i].gameObject.SetActive(false);
        }
    }

    // применение золота ко всем текстам
    public void ApplyGold()
    {
        foreach (Text text in textGold)
        {
            text.text = gold.ToString();
        }
    }
    public void StartGame(int levelValue)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].damage = enemies[i].standartDamage;
            int rand = Random.Range(0, spriteEnemy.Length);
            enemies[i].spriteEnemy.sprite = spriteEnemy[rand];
            enemies[i].damage = (i * 10)+(enemies[i].damage * levelValue);
        }
    }
}