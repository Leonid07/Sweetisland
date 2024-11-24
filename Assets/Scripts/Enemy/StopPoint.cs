using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPoint : MonoBehaviour
{
    public bool requiresPause = true; // Определяет, нужно ли останавливать персонажа на этой точке
    public bool lastEnemy = false;
    public Enemy enemy;

    public float fadeDuration = 1.0f;

    [Header("Параметры игрока")]
    public Player player;
    public GameObject spawnPoint_1;
    public GameObject buttonStart;

    // Метод для обработки достижения точки (можно добавить другие параметры и действия)
    public void OnReached()
    {
        if (requiresPause)
        {
            if (enemy != null)
            {
                Debug.Log($"{enemy.damage}                   {PanelManager.InstancePanel.powerPlayer}");
                if (enemy.damage >= PanelManager.InstancePanel.powerPlayer)
                {
                    GameManager.InstanceGame.player.Die();
                    SoundManager.InstanceSound.musicFon.Play();
                    SoundManager.InstanceSound.musicLevel.Stop();
                    PanelManager.InstancePanel._UIPanelFade.FadeIn();
                }
                else
                {
                    enemy.cutting.gameObject.SetActive(true);
                    StartCoroutine(FadeAndPlaySound());
                    GameManager.InstanceGame.player.Attack();
                    if (lastEnemy == true)
                    {
                        if (DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad == 0)
                        {
                            PanelManager.InstancePanel.SetActivePanel(true);
                            DataManager.InstanceData.mapNextLevel.OpenLevel();
                        }
                        else
                        {
                            Debug.Log("прохождение одного и тогоже уровня");
                            PanelManager.InstancePanel.SetActivePanel(false);
                        }
                        SoundManager.InstanceSound.musicFon.Play();
                        SoundManager.InstanceSound.musicLevel.Stop();
                        GameManager.InstanceGame.gold += DataManager.InstanceData.mapNextLevel.indexLevel * 50;
                        DataManager.InstanceData.SaveGold();
                        Debug.Log("End Game");
                        buttonStart.SetActive(false);
                    }
                }
            }
        }
    }
    private IEnumerator FadeAndPlaySound()
    {

        if (SoundManager.InstanceSound.soundDamage != null)
        {
            SoundManager.InstanceSound.soundDamage.Play();
        }

        float elapsedTime = 0f;
        Color originalColor = enemy.spriteEnemy.color;
        Color originalColorcutting = enemy.cutting.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = originalColor;
            Color newColorcutting = originalColorcutting;

            newColor.a = Mathf.Lerp(originalColor.a, 0, t);
            newColorcutting.a = Mathf.Lerp(originalColorcutting.a, 0, t);

            enemy.spriteEnemy.color = newColor;
            enemy.cutting.color = newColorcutting;

            yield return null;
        }

        Color finalColor = enemy.spriteEnemy.color;
        Color finalColorcutting = enemy.cutting.color;

        finalColor.a = 0;
        finalColorcutting.a = 0;

        enemy.spriteEnemy.color = finalColor;
        enemy.cutting.color = finalColorcutting;

    }
}