using System;

namespace GameLogicScripts
{
    public class GameEvents
    {
        //Creating game events
        public Action GameStarted;
        public Action<Card> CardSelected;
        public Action<Card,Card> MatchingCardSelected;
        public Action CardMisMatch;
        public Action GameOver;
    }
}