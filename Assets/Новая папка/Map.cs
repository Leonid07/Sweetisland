using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    [Header("Индекс уровня")]
    public int indexLevel = 0;

    Button thisButton;
    public Image imageBlockAndNumber;
    public Sprite spriteBlockGround;
    public Sprite spriteUnBlockGround;
    public Sprite imageNumber;
    public Sprite imageBlock;

    public int isLoad = 0; // 0 не пройдено 1 пройдено
    public string idLevel;

    public Map mapNextLevel;

    public Map thisLevel;
    public Image thisImage;

    private void Awake()
    {
        thisLevel = GetComponent<Map>();
        thisImage = GetComponent<Image>();
        idLevel = gameObject.name;
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnPointerClick);
        CheckLevel();
    }
    public void OnPointerClick()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        if (isLoad == 0)
            return;

        PanelManager.InstancePanel.SetDisActivePanel();
        GameManager.InstanceGame.player.RestartMovement();
        SoundManager.InstanceSound.musicLevel.Play();
        SoundManager.InstanceSound.musicFon.Stop();
        DataManager.InstanceData.mapNextLevel = thisLevel;
        GameManager.InstanceGame.player.DisActiveDead();
        GameManager.InstanceGame.StartGame(indexLevel);
        GameManager.InstanceGame.RestartEnemy();
        PanelManager.InstancePanel.SetActiveButtonStart();
    }
    public void OpenLevel()
    {
        mapNextLevel.isLoad = 1;
        mapNextLevel.CheckLevel();
        DataManager.InstanceData.SaveLevel();
    }

    public void CheckLevel()
    {
        if (isLoad == 0)
        {
            imageBlockAndNumber.sprite = imageBlock;
            thisImage.sprite = spriteBlockGround;
        }
        if (isLoad == 1)
        {
            imageBlockAndNumber.sprite = imageNumber;
            thisImage.sprite = spriteUnBlockGround;
        }
    }
}