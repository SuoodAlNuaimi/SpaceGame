using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    [System.Serializable]
    public class Quest
    {
        public string id;
        public string description;
        public bool isCompleted;
        public int rewardAmount;
        public bool isRewardClaimed;
        public UnityEvent onComplete = new UnityEvent();

        public Quest(string id, string description, int rewardAmount = 0)
        {
            this.id = id;
            this.description = description;
            this.rewardAmount = rewardAmount;
            this.isCompleted = false;
            this.isRewardClaimed = false;
        }
    }
}
