using Game.NPC;
using Game.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Managers
{
    public class UIManager : StaticInstance<UIManager>
    {
        [Header("Canvases")]
        [SerializeField] Canvas mainMenuCanvas;
        [SerializeField] Canvas gamePanelCanvas;
        [SerializeField] Canvas gameOverCanvas;

        [Header("Components")]
        [SerializeField] Slider staminaBar;
        [SerializeField] TextMeshProUGUI ingameScoreText;
        [SerializeField] TextMeshProUGUI endgameScoreText;
        [SerializeField] NPCController npc;

        float maxStamina;

        #region UNITY EVENTS
        private void Start() => maxStamina = npc.GetCurrentStamina();

        private void Update()
        {
            if (!npc.IsPlaying) return;
            UpdateStaminaBar();
        }

        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
        }
        #endregion

        #region PUBLIC METHODS
        public void UpdateScoreText(int score)
        {
            ingameScoreText.text = score.ToString("0000");
            endgameScoreText.text = score.ToString("0000");
        }
        #endregion

        #region PRIVATE METHODS
        private void OnGameStateChanged(GameState state)
        {
            mainMenuCanvas.gameObject.SetActive(state == GameState.Menu);
            gamePanelCanvas.gameObject.SetActive(state == GameState.Playing);
            gameOverCanvas.gameObject.SetActive(state == GameState.End || state == GameState.Tired);
            staminaBar.gameObject.SetActive(state == GameState.Playing);
        }

        private void UpdateStaminaBar()
        {
            staminaBar.value = npc.GetCurrentStamina() / maxStamina;
        }
        #endregion
    }
}
