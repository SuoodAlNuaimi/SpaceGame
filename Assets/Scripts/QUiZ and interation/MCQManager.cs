using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

namespace QuizSystem
{
    [System.Serializable]
    public class MCQQuestion
    {
        [TextArea(3, 10)]
        public string questionText;
        public string[] options = new string[4];
        public int correctOptionIndex;
    }

    [System.Serializable]
    public class MCQRound
    {
        public string roundName;
        public List<MCQQuestion> questions;
        public float timeLimit = 60f;
        public int targetCorrectAnswers = 3;
    }

    public class MCQManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject quizPanel;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] private GameObject floatingTextPrefab;
        [SerializeField] private Transform floatingTextSpawnPoint;
        [SerializeField] private Button[] optionButtons;
        [SerializeField] private TextMeshProUGUI[] optionTexts;

        [Header("Settings")]
        [SerializeField] private List<MCQRound> rounds;
        [SerializeField] private float feedbackDelay = 1f;
        [SerializeField] private Color correctColor = Color.green;
        [SerializeField] private Color wrongColor = Color.red;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private int baseReward = 1;

        [Header("Events")]
        public UnityEngine.Events.UnityEvent onAllRoundsComplete;
        public UnityEngine.Events.UnityEvent onRoundFailed;

        private int currentRoundIndex = 0;
        private int currentQuestionIndex = 0;
        private int correctAnswersInRound = 0;
        private int currentStreak = 0;
        private float timeRemaining;
        private bool isRoundActive = false;
        private bool isAnswering = false;


        public AudioClip Correct;
        public AudioClip Wrong;
        public AudioClip CompleteAudio;
        public AudioSource source;
        private void Start()
        {
            //if (quizPanel != null) quizPanel.SetActive(false);
            SetupButtons();
            
            // For testing, you can call StartGame() here or from another script
            StartGame(); 
        }

        private void SetupButtons()
        {
            for (int i = 0; i < optionButtons.Length; i++)
            {
                int index = i;
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
        }

        /// <summary>
        /// Starts the MCQ game from the first round.
        /// </summary>
        public void StartGame()
        {
            currentRoundIndex = 0;
            StartRound(currentRoundIndex);
        }

        public void StartRound(int index)
        {
            if (rounds == null || rounds.Count == 0)
            {
                Debug.LogError("No rounds defined in MCQManager!");
                return;
            }

            if (index >= rounds.Count)
            {
                OnAllRoundsComplete();
                return;
            }

            currentRoundIndex = index;
            currentQuestionIndex = 0;
            correctAnswersInRound = 0;
            currentStreak = 0; // Reset streak at start of round? Or keep it? User didn't specify. I'll reset it.
            timeRemaining = rounds[currentRoundIndex].timeLimit;
            isRoundActive = true;
            isAnswering = false;
            
            if (quizPanel != null) quizPanel.SetActive(true);
            
            UpdateProgressUI();
            DisplayQuestion();
        }

        private void Update()
        {
            if (isRoundActive && !isAnswering)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();

                if (timeRemaining <= 0)
                {
                    OnTimeUp();
                }
            }
        }

        private void DisplayQuestion()
        {
            isAnswering = false;
            var round = rounds[currentRoundIndex];
            
            if (round.questions == null || round.questions.Count == 0)
            {
                Debug.LogError($"No questions in round {currentRoundIndex}!");
                return;
            }

            var question = round.questions[currentQuestionIndex];
            questionText.text = question.questionText;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < question.options.Length)
                {
                    optionTexts[i].text = question.options[i];
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].interactable = true;
                    SetButtonColor(optionButtons[i], defaultColor);
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnOptionSelected(int index)
        {
            if (isAnswering || !isRoundActive) return;
            isAnswering = true;

            var round = rounds[currentRoundIndex];
            var question = round.questions[currentQuestionIndex];

            // Disable interaction during feedback
            foreach (var btn in optionButtons) btn.interactable = false;

            if (index == question.correctOptionIndex)
            {
                correctAnswersInRound++;
                currentStreak++;
                int reward = baseReward + (currentStreak - 1);
                
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddScore(reward);
                }
                if (currentStreak == 2)
                {
                    QuestSystem.QuestManager.Instance.CompleteQuest("5");
                }
                source.PlayOneShot(Correct);
                SpawnFloatingStreak(currentStreak);
                SetButtonColor(optionButtons[index], correctColor);
            }
            else
            {
                currentStreak = 0; // Streak broken!
                source.PlayOneShot(Wrong);
                SetButtonColor(optionButtons[index], wrongColor);
                SetButtonColor(optionButtons[question.correctOptionIndex], correctColor);
            }

            UpdateProgressUI();
            UpdateStreakUI();
            StartCoroutine(HandleNextStep());
        }

        private IEnumerator HandleNextStep()
        {
            yield return new WaitForSeconds(feedbackDelay);

            var round = rounds[currentRoundIndex];
            if (correctAnswersInRound >= round.targetCorrectAnswers)
            {
                // Round complete! Move to next.
                StartRound(currentRoundIndex + 1);
            }
            else
            {
                // Move to next question in the pool
                currentQuestionIndex++;

                if (currentQuestionIndex >= round.questions.Count)
                {
                    // Ran out of questions before reaching target!
                    OnRoundFailed("Ran out of questions!");
                }
                else
                {
                    DisplayQuestion();
                }
            }
        }

        private void UpdateTimerUI()
        {
            if (timerText != null)
            {
                timerText.text = $"Time: {Mathf.Max(0, timeRemaining):F0}s";
            }
        }

        private void UpdateProgressUI()
        {
            if (progressText != null)
            {
                var round = rounds[currentRoundIndex];
                progressText.text = $"{round.roundName}\nCorrect: {correctAnswersInRound} / {round.targetCorrectAnswers}";
            }
        }

        private void UpdateStreakUI()
        {
            if (streakText != null)
            {
                streakText.text = currentStreak > 0 ? $"Streak: {currentStreak} 🔥" : "";
            }
        }

        private void SpawnFloatingStreak(int streak)
        {
            if (floatingTextPrefab == null || floatingTextSpawnPoint == null) return;

            GameObject go = Instantiate(floatingTextPrefab, floatingTextSpawnPoint.position, Quaternion.identity, floatingTextSpawnPoint);
            var ft = go.GetComponent<MCQFloatingText>();
            if (ft != null)
            {
                ft.Setup($"+{streak} Streak! 🔥", correctColor);
            }
        }

        private void OnTimeUp()
        {
            OnRoundFailed("Time's Up!");
        }

        private void OnRoundFailed(string reason)
        {
            isRoundActive = false;
            if (questionText != null) questionText.text = reason;
            onRoundFailed?.Invoke();
            Debug.Log($"Round Failed: {reason}");
        }

        private void OnAllRoundsComplete()
        {
            isRoundActive = false;
            if (questionText != null) questionText.text = "All Rounds Complete! Great Job!";
            onAllRoundsComplete?.Invoke();
                source.PlayOneShot(CompleteAudio);

            // Optionally hide panel after delay
            //StartCoroutine(HidePanelAfterDelay(3f));
        }

        private IEnumerator HidePanelAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (quizPanel != null) quizPanel.SetActive(false);
        }

        private void SetButtonColor(Button btn, Color color)
        {
            var cb = btn.colors;
            cb.normalColor = color;
            cb.selectedColor = color;
            cb.disabledColor = color; // Keep color even when disabled
            btn.colors = cb;
        }
    }
}
