using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [Header("Кнопки в главном меню")]
    public Button buttonReward;
    public Button buttonPersonal;
    public Button buttonOptions;

    [Header("Пенели из главного меню")]
    public GameObject panelReward;
    public GameObject panelPersonal;
    public GameObject panelOptions;

    [Header("кнопки закрытия окон")]
    public Button buttonRewardClose;
    public Button buttonPersonalClose;
    public Button buttonOptionsClose;

    [Space(20)]
    [Header("Кнопки улучшения персонажа")]
    public Button buttonPlayerUpdate;
    public Button buttonPlayerPanelClose;
    public Button buttonUpdate;

    [Header("Текстовые панели в улучшении")]
    public Text textBeforeUpdate;
    public Text textAfterUpdate;
    public Text textPriceOnButton;
    public Text textPlayerDamage;

    public int damage = 350;
    public int updateCost = 150;
    public string idUpdateCost = "_costUpdate";

    public int powerPlayer;
    public string idPowerPlayer = "power";

    public int levelPLayer = 1;
    public string idLevelPLayer = "level_";

    public int countFirstUpdate;
    public double growthFactor = 1.5;

    [Header("Панель улучшения персонажа")]
    public GameObject panelUpdate;

    [Header("Персонаж")]
    public GameObject buttonStart;

    public GameObject[] panelIsActive;

    public AnimationPAnel animCandy;

    [Header("Анимация открытия уровня")]
    public AnimUnlockLevel animUnlockLevel;

    [Header("Банель проигрыша")]
    public GameObject panelGameover;
    public UIPanelFade _UIPanelFade;
    public Button buttonBackToMainMenu;

    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        buttonReward.onClick.AddListener(() => ActivePanel(panelReward));
        buttonPersonal.onClick.AddListener(() => ActivePanel(panelPersonal));
        buttonOptions.onClick.AddListener(() => ActivePanel(panelOptions));

        buttonRewardClose.onClick.AddListener(() => ClosePanel(panelReward));
        buttonPersonalClose.onClick.AddListener(() => ClosePanel(panelPersonal));
        buttonOptionsClose.onClick.AddListener(() => ClosePanel(panelOptions));

        buttonPlayerUpdate.onClick.AddListener(() => { PanelUpdateActive(panelUpdate); SetValueForUpdate(); });
        buttonPlayerPanelClose.onClick.AddListener(() => PanelUpdateDisActive(panelUpdate));

        buttonUpdate.onClick.AddListener(() => { UpdatePlayer(); });

        buttonBackToMainMenu.onClick.AddListener(() => { SetActivePanel(); SetDisActiveButtonStart(); });
    }

    public void PanelUpdateActive(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void PanelUpdateDisActive(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void ActivePanel(GameObject panel)
    {
        animCandy.StartAnimation(panel, true);
    }
    public void ClosePanel(GameObject panel)
    {
        animCandy.StartAnimation(panel, false);
    }

    public void SetActivePanel(bool lose = false)
    {
        if (lose == false)
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {

                StartCoroutine(animCandy.MovePanelOutAndBack(panelIsActive[0], true, panelIsActive));
            }
        }
        else
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {

                animCandy.StartAnimationUnLockLevel(panelIsActive[i]);
            }
        }
    }
    public void SetDisActivePanel()
    {
        StartCoroutine(animCandy.MovePanelOutAndBack(panelIsActive[0], false, panelIsActive));
    }
    public void SetActiveButtonStart()
    {
        animCandy.StartAnimation(buttonStart, true);
    }
    public void SetDisActiveButtonStart()
    {
        _UIPanelFade.FadeOut();
        animCandy.StartAnimation(buttonStart, false);
        buttonStart.SetActive(false);
        SetActivePanel(false);
    }
    public void SetValueForUpdate()
    {

        textBeforeUpdate.text = powerPlayer.ToString();

        levelPLayer++;
        int calculatedDamage = Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer - 1));
        levelPLayer--;
        int calculatedPrice = Convert.ToInt32(updateCost * Math.Pow(growthFactor, levelPLayer - 1));
        textAfterUpdate.text = $"{calculatedDamage}";
        textPriceOnButton.text = $"{calculatedPrice}";
    }

    public void UpdatePlayer()
    {
        if (countFirstUpdate <= GameManager.InstanceGame.gold)
        {
            countFirstUpdate = Convert.ToInt32(updateCost * Math.Pow(growthFactor, levelPLayer - 1));

            GameManager.InstanceGame.gold -= countFirstUpdate;
            powerPlayer = Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer));
            textAfterUpdate.text = $"{Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer - 1))}";
            Debug.Log($"powerPlayer  {powerPlayer}");
            textPlayerDamage.text = powerPlayer.ToString();

            levelPLayer++;
            SetValueForUpdate();
            DataManager.InstanceData.SaveLevelPlayer();
            DataManager.InstanceData.SaveGold();
            DataManager.InstanceData.SavePowerPlayer();
        }
    }
}