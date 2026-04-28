using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestSystemUI : MonoBehaviour
    {
        [SerializeField] private GameObject questItemPrefab;
        [SerializeField] private Transform contentParent;
        [SerializeField] public GameObject questPanel;
        [SerializeField] private GameObject notificationImage;

        private List<QuestItemUI> questItems = new List<QuestItemUI>();
        public bool ShowPanelDefault;
        private void OnEnable()
        {
            Invoke("RefreshUI",1f);
        }
        private void Start()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.onQuestUpdated += OnQuestUpdated;
                RefreshUI();
            }
            if (ShowPanelDefault)
            {
                ToggleQuestPanel();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            // Initially hide notification
            if (notificationImage != null)
                notificationImage.SetActive(false);
        }

        private void OnDestroy()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.onQuestUpdated -= OnQuestUpdated;
            }
        }
        public void CompleteQuest(string id)
        {
            QuestManager.Instance.CompleteQuest(id);
        }
        private void OnQuestUpdated()
        {
            QuestManager qm = QuestManager.Instance;
            if(qm.activeQuests[0].isCompleted&& qm.activeQuests[1].isCompleted/*&& qm.activeQuests[2].isCompleted*/&& Application.loadedLevelName=="Exploration")
            {
                GameManager.Instance.ShowWinScreen();
            }
            RefreshUI();

            // Show notification if the panel is not currently open
            if (notificationImage != null && (questPanel == null || !questPanel.activeSelf))
            {
                notificationImage.SetActive(true);
                GetComponent<AudioSource>().Play();
            }
        }
        
        public void ToggleQuestPanel()
        {
            if (questPanel == null) return;
            Debug.LogError("OPEN");
            bool isOpen = !questPanel.activeSelf;
            questPanel.SetActive(isOpen);
            if (Application.loadedLevelName != "MCQs")
            {
                if (questPanel.activeInHierarchy)
                {
                    GameManager.Instance.Player.isEditing = true;

                }
                else
                {
                    GameManager.Instance.Player.isEditing = false;
                    GameManager.Instance.Player.LockCursor();

                }
            }
            if(Application.loadedLevelName == "PlanetQuiz")
            {
                if (QuizManager.instance.MainPanel.activeInHierarchy|| QuizManager.instance.quizPanel.activeInHierarchy||questPanel.activeInHierarchy)
                {
                    GameManager.Instance.Player.isEditing = true;
                    GameManager.Instance.Player.UnlockCursor();
                    Debug.LogError("active");

                }
                else
                {
                    GameManager.Instance.Player.isEditing = false;
                    GameManager.Instance.Player.LockCursor();
                    Debug.LogError("notactive");
                    

                }
            }
            
            // Hide notification when opening the panel
            if (isOpen && notificationImage != null)
            {
                notificationImage.SetActive(false);
            }
        }

        public void RefreshUI()
        {
            // Clear existing items
            foreach (var item in questItems)
            {
                Destroy(item.gameObject);
            }
            questItems.Clear();

            // Create new items
            List<Quest> quests = QuestManager.Instance.GetAllQuests();
            foreach (var quest in quests)
            {
                GameObject go = Instantiate(questItemPrefab, contentParent);
                QuestItemUI ui = go.GetComponent<QuestItemUI>();
                if (ui != null)
                {
                    ui.UpdateUI(quest);
                    questItems.Add(ui);
                }
            }
        }
    }
}
