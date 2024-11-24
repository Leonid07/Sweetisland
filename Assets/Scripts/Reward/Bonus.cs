using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public Text dailyBonusText;
    public Text weeklyBonusText;
    public Text hourlyBonusText; // Текст для часового бонуса

    [Space(10)]
    public Text hourlyText; // Текст для часового бонуса
    public Text weeklyText; // Текст для часового бонуса
    public Text dailyText; // Текст для часового бонуса
    [Space(10)]
    public Button dailyBonusButton;
    public Button weeklyBonusButton;
    public Button hourlyBonusButton; // Кнопка для часового бонуса

    public GameObject dailyFG;
    public GameObject weeklyFG;
    public GameObject hourlyFG; // Графический объект для часового бонуса

    private const string DailyBonusTimeKey = "daily_bonus_time";
    private const string WeeklyBonusTimeKey = "weekly_bonus_time";
    private const string HourlyBonusTimeKey = "hourly_bonus_time"; // Ключ для сохранения времени часового бонуса

    public int HourlyBonusCooldownInSeconds = 3600; // 1 час
    public int DailyBonusCooldownInSeconds = 86400; // 24 часа
    public int WeeklyBonusCooldownInSeconds = 604800; // 7 дней

    public int countHourly = 1; // Количество награды за часовой бонус
    public int countDaily = 5;
    public int countWeekly = 50;

    private void Start()
    {
        dailyBonusButton.onClick.AddListener(ClaimDailyBonus);
        weeklyBonusButton.onClick.AddListener(ClaimWeeklyBonus);
        hourlyBonusButton.onClick.AddListener(ClaimHourlyBonus); // Добавление слушателя для часового бонуса
        StartCoroutine(UpdateBonusTextsRoutine());
    }

    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f); // Обновление текста каждые 0.5 секунды
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");
        string hourlyBonusTimeStr = PlayerPrefs.GetString(HourlyBonusTimeKey, "0"); // Получение времени часового бонуса

        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);
        long hourlyBonusTime = long.Parse(hourlyBonusTimeStr); // Преобразование времени часового бонуса

        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;
        long hourlyCooldown = hourlyBonusTime + HourlyBonusCooldownInSeconds - currentTimestamp; // Вычисление оставшегося времени для часового бонуса

        dailyBonusText.text = FormatTimeDaily(dailyCooldown);
        weeklyBonusText.text = FormatTimeWeekly(weeklyCooldown);
        hourlyBonusText.text = FormatTimeHourly(hourlyCooldown); // Обновление текста часового бонуса

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
        hourlyBonusButton.interactable = hourlyCooldown <= 0; // Активность кнопки часового бонуса
    }

    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            dailyFG.SetActive(false);
            dailyText.gameObject.SetActive(true);
            return "Ready";
        }
        dailyFG.SetActive(true);
        dailyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
    private string FormatTimeWeekly(long seconds)
    {
        if (seconds <= 0)
        {
            weeklyFG.SetActive(false);
            weeklyText.gameObject.SetActive(true);
            return "Ready";
        }
        weeklyFG.SetActive(true);
        weeklyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        int totalHours = (int)timeSpan.TotalHours;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", totalHours, timeSpan.Minutes, timeSpan.Seconds);
    }
    private string FormatTimeHourly(long seconds) // Форматирование текста для часового бонуса
    {
        if (seconds <= 0)
        {
            hourlyFG.SetActive(false);
            hourlyText.gameObject.SetActive(true);
            return "Ready";
        }
        hourlyFG.SetActive(true);
        hourlyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countDaily;
        DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    private void ClaimWeeklyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countWeekly;
        DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(WeeklyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Weekly Bonus Claimed!");
        Debug.Log($"New Weekly Bonus Time: {currentTimestamp}");
    }

    private void ClaimHourlyBonus() // Метод для получения часового бонуса
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countHourly;
        DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(HourlyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Hourly Bonus Claimed!");
        Debug.Log($"New Hourly Bonus Time: {currentTimestamp}");
    }
}
