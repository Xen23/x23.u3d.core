// File: STUITimer.cs
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

namespace SudokuToolkit
{
    public class GameBoardTimerUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private SudokuGameDataSO gameData;
        private Label timerLabel;
        private Coroutine timerCoroutine;

        private void Awake()
        {
            if (uiDocument?.rootVisualElement == null)
            {
                Debug.LogError("[GameBoardTimerUI] UIDocument or rootVisualElement is null.");
                return;
            }
            timerLabel = uiDocument.rootVisualElement.Q<Label>("timer");
            if (timerLabel == null)
                Debug.LogError("[GameBoardTimerUI] Timer label not found.");
        }

        public void StartTimer()
        {
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public void ResetTimer()
        {
            gameData.SetTimeElapsed(0);
            UpdateTimer();
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        }

        private IEnumerator TimerCoroutine()
        {
            while (true)
            {
                gameData.SetTimeElapsed(gameData.GetTimeElapsed() + 1);
                UpdateTimer();
                yield return new WaitForSeconds(1);
            }
        }

        private void UpdateTimer()
        {
            if (timerLabel != null)
            {
                int minutes = Mathf.FloorToInt(gameData.GetTimeElapsed() / 60);
                int seconds = Mathf.FloorToInt(gameData.GetTimeElapsed() % 60);
                timerLabel.text = $"Time: {minutes:00}:{seconds:00}";
            }
        }
    }
}