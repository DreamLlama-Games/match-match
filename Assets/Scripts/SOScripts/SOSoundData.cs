using UnityEngine;

namespace SOScripts
{
    [CreateAssetMenu(fileName = "SoundsData", menuName = "Scriptable Objects/Sounds Data", order = 0)]
    public class SOSoundData : ScriptableObject
    {
        public AudioClip onCardSelected;
        public AudioClip onMatchFound;
        public AudioClip onMisMatch;
        public AudioClip onGameOver;
    }
}