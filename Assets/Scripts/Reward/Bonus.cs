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
    public Text hourlyBonusText; // ����� ��� �������� ������

    [Space(10)]
    public Text hourlyText; // ����� ��� �������� ������
    public Text weeklyText; // ����� ��� �������� ������
    public Text dailyText; // ����� ��� �������� ������
    [Space(10)]
    public Button dailyBonusButton;
    public Button weeklyBonusButton;
    public Button hourlyBonusButton; // ������ ��� �������� ������

    public GameObject dailyFG;
    public GameObject weeklyFG;
    public GameObject hourlyFG; // ����������� ������ ��� �������� ������

    private const string DailyBonusTimeKey = "daily_bonus_time";
    private const string WeeklyBonusTimeKey = "weekly_bonus_time";
    private const string HourlyBonusTimeKey = "hourly_bonus_time"; // ���� ��� ���������� ������� �������� ������

    public int HourlyBonusCooldownInSeconds = 3600; // 1 ���
    public int DailyBonusCooldownInSeconds = 86400; // 24 ����
    public int WeeklyBonusCooldownInSeconds = 604800; // 7 ����

    public int countHourly = 1; // ���������� ������� �� ������� �����
    public int countDaily = 5;
    public int countWeekly = 50;

    private void Start()
    {
        dailyBonusButton.onClick.AddListener(ClaimDailyBonus);
        weeklyBonusButton.onClick.AddListener(ClaimWeeklyBonus);
        hourlyBonusButton.onClick.AddListener(ClaimHourlyBonus); // ���������� ��������� ��� �������� ������
        StartCoroutine(UpdateBonusTextsRoutine());
    }

    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f); // ���������� ������ ������ 0.5 �������
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");
        string hourlyBonusTimeStr = PlayerPrefs.GetString(HourlyBonusTimeKey, "0"); // ��������� ������� �������� ������

        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);
        long hourlyBonusTime = long.Parse(hourlyBonusTimeStr); // �������������� ������� �������� ������

        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;
        long hourlyCooldown = hourlyBonusTime + HourlyBonusCooldownInSeconds - currentTimestamp; // ���������� ����������� ������� ��� �������� ������

        dailyBonusText.text = FormatTimeDaily(dailyCooldown);
        weeklyBonusText.text = FormatTimeWeekly(weeklyCooldown);
        hourlyBonusText.text = FormatTimeHourly(hourlyCooldown); // ���������� ������ �������� ������

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
        hourlyBonusButton.interactable = hourlyCooldown <= 0; // ���������� ������ �������� ������
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
    private string FormatTimeHourly(long seconds) // �������������� ������ ��� �������� ������
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

    private void ClaimHourlyBonus() // ����� ��� ��������� �������� ������
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
