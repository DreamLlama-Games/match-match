using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameLogicScripts
{
    public class ScoreHandler
    {
        private int _activeScore;
        private readonly TMP_Text _activeScoreText;

        //Three level scoring
        private const int MinScore = 2;
        private const int MaxScore = 10;
        private const int AverageScore = 5;
        
        //Seen 
        private const int MaxSeenCount = 2;
        private const int AverageSeenCount = 4;
        
        private readonly Dictionary<string, int> _seenCount = new(); 
        
        public ScoreHandler(TMP_Text scoreText, GameEvents gameEvents)
        {
            _activeScoreText = scoreText;
            Subscribe(gameEvents);
        }

        private void Subscribe(GameEvents gameEvents)
        {
            gameEvents.GameStarted += Reset;
            gameEvents.GameOver += OnGameOver;
            gameEvents.CardSelected += OnCardSelected;
            gameEvents.MatchingCardSelected += OnMatchFound;
        }

        public void Unsubscribe(GameEvents gameEvents)
        {
            gameEvents.GameStarted -= Reset;
            gameEvents.GameOver -= OnGameOver;
            gameEvents.CardSelected -= OnCardSelected;
            gameEvents.MatchingCardSelected -= OnMatchFound;
        }

        private void SetScore(int score)
        {
            _activeScore = score;
            _activeScoreText.text = _activeScore.ToString();
        }

        private void Reset()
        {
            SetScore(0);
        }

        private void OnCardSelected(Card card)
        {
            if (!_seenCount.TryGetValue(card.CardName, out var count))
            {
                _seenCount.Add(card.CardName, 1);
                return;
            }
            _seenCount[card.CardName] =  count + 1;
        }

        private void OnMatchFound(Card twin1, Card twin2)
        {
            if (_seenCount.TryGetValue(twin1.CardName, out var count))
            {
                var allottedScore = count <= MaxSeenCount ? MaxScore : 
                    count <= AverageSeenCount ? AverageScore : MinScore;
                SetScore(_activeScore + allottedScore);
                return;
            }
            SetScore(_activeScore + MaxScore);
        }

        private void UpdateHighScore()
        {
            var highScore = PlayerPrefs.GetInt("HighScore");
            
            if (highScore <= _activeScore) return;
            PlayerPrefs.SetInt("HighScore", _activeScore);
            PlayerPrefs.Save();
        }

        private void OnGameOver()
        {
            UpdateHighScore();
        }
    }
}