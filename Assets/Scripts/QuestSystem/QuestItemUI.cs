using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image checkmarkImage;
        [SerializeField] private Button rewardButton;
        [SerializeField] private TextMeshProUGUI rewardButtonText;

        private string currentQuestId;

        private void Start()
        {
            if (rewardButton != null)
                rewardButton.onClick.AddListener(OnRewardClicked);
        }

        public void UpdateUI(Quest quest)
        {
            currentQuestId = quest.id;

            if (descriptionText != null)
                descriptionText.text = quest.description;

            if (checkmarkImage != null)
                checkmarkImage.gameObject.SetActive(quest.isCompleted);

            if (rewardButton != null)
            {
                // Only interactable if completed AND not yet claimed
                rewardButton.interactable = quest.isCompleted && !quest.isRewardClaimed;
                
                if (rewardButtonText != null)
                {
                    rewardButtonText.text = quest.isRewardClaimed ? "Claimed" : $"Reward: {quest.rewardAmount}";
                }
            }
        }

        private void OnRewardClicked()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.ClaimReward(currentQuestId);
                QuestManager.Instance.GetComponent<AudioSource>().Play();
            }
        }
    }
}
