using SOScripts;
using UnityEngine;

namespace GameLogicScripts
{
    public class SoundPlayer : MonoBehaviour
    {
        //Main audio source
        [SerializeField] private AudioSource audioSource;
        //Audio data
        [SerializeField] private SOSoundData soundData;
        
        private void OnMatchFound(Card twin1, Card twin2)
        {
            audioSource.PlayOneShot(soundData.onMatchFound);
        }

        private void OnCardSelected(Card card)
        {
            audioSource.PlayOneShot(soundData.onCardSelected);
        }

        private void OnCardMismatch()
        {
            audioSource.PlayOneShot(soundData.onMisMatch);
        }

        private void OnGameOver()
        {
            audioSource.PlayOneShot(soundData.onGameOver);
        }

        public void Subscribe(GameEvents gameEvents)
        {
            gameEvents.CardSelected += OnCardSelected;
            gameEvents.MatchingCardSelected += OnMatchFound;
            gameEvents.CardMisMatch += OnCardMismatch;
            gameEvents.GameOver += OnGameOver;
        }

        public void Unsubscribe(GameEvents gameEvents)
        {
            gameEvents.CardSelected -= OnCardSelected;
            gameEvents.MatchingCardSelected -= OnMatchFound;
            gameEvents.CardMisMatch -= OnCardMismatch;
            gameEvents.GameOver -= OnGameOver;
        }
    }
}