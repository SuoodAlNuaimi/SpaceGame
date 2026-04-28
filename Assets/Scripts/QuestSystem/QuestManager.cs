using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [SerializeField] public List<Quest> activeQuests = new List<Quest>();
        
        public delegate void OnQuestUpdated();
        public event OnQuestUpdated onQuestUpdated;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            // Optional:
             DontDestroyOnLoad(gameObject);
        }

        public void AddQuest(string id, string description, int rewardAmount = 0)
        {
            if (GetQuest(id) != null)
            {
                Debug.LogWarning($"Quest with ID {id} already exists.");
                return;
            }

            Quest newQuest = new Quest(id, description, rewardAmount);
            activeQuests.Add(newQuest);
            onQuestUpdated?.Invoke();
        }

        public void CompleteQuest(string id)
        {
            Quest quest = GetQuest(id);
            if (quest != null)
            {
                quest.isCompleted = true;
                quest.onComplete?.Invoke();
                onQuestUpdated?.Invoke();
            }
            else
            {
                Debug.LogWarning($"Quest with ID {id} not found.");
            }
        }

        public void ClaimReward(string id)
        {
            Quest quest = GetQuest(id);
            if (quest != null && quest.isCompleted && !quest.isRewardClaimed)
            {
                quest.isRewardClaimed = true;
                
                // Add to GameManager score
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddScore(quest.rewardAmount);
                    Debug.Log($"Claimed {quest.rewardAmount} points for quest {id}!");
                }

                onQuestUpdated?.Invoke();
            }
        }

        public Quest GetQuest(string id)
        {
            return activeQuests.Find(q => q.id == id);
        }

        public List<Quest> GetAllQuests()
        {
            return activeQuests;
        }
    }
}
