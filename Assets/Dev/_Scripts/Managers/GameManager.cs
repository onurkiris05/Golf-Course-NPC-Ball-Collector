using System;
using Game.Utilities;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;
        public static event Action OnTired;
        public GameState State { get; private set; }


        #region UNITY EVENTS

        protected override void Awake()
        {
            base.Awake();
            State = GameState.Menu;
        }

        #endregion

        #region PUBLIC METHODS

        public void ChangeState(GameState newState)
        {
            if (newState == State) return;

            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState)
            {
                case GameState.Menu:
                    break;
                case GameState.Playing:
                    break;
                case GameState.Tired:
                    break;
            }

            OnAfterStateChanged?.Invoke(newState);
            Debug.Log($"New state: {newState}");
        }

        // Invoke from TAP TO START button
        public void InvokeOnStartButton()
        {
            ChangeState(GameState.Playing);
        }

        // Invoke from EXIT button
        public void InvokeOnExitButton()
        {
            Application.Quit();
        }

        // Invoke from RETURN TO MAIN MENU button
        public void InvokeOnReturnToMainMenuButton()
        {
            SceneManager.Instance.ReloadScene();
        }

        #endregion
    }

    public enum GameState
    {
        Menu,
        Playing,
        Tired,
        End
    }
}