using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementView : MonoBehaviour
{
    private const string ProgressText = "Прогресс ";
    private const string LevelText = "Уровень ";
    private const string MoneyText = " монет";

    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _progress;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _reward;
    [SerializeField] private Button _rewardButton;

    public void Fill(Achievement achievement)
    {
        _header.text = achievement.HeaderName;
        _description.text = achievement.Description;
        _progress.text = ProgressText + achievement.CurrentFullness + "/" + achievement.MaxFullness;
        _level.text = LevelText + achievement.Level;
        _reward.text = achievement.Reward + MoneyText;
        _rewardButton.onClick.AddListener(() => Money.AddMoney(achievement.TakeReward()));
        _rewardButton.onClick.AddListener(() => _reward.text = achievement.Reward + MoneyText);
    }

    private void OnDisable()
    {
        _rewardButton.onClick.RemoveAllListeners();
    }
}
