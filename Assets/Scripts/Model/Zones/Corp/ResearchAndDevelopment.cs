﻿using model.cards;

namespace model.zones.corp
{
    public class ResearchAndDevelopment
    {
        private Deck deck;

        public ResearchAndDevelopment(Deck deck)
        {
            this.deck = deck;
        }

        public void Shuffle()
        {
            deck.Shuffle();
        }

        public bool HasCards() => deck.Size() > 0;

        internal void Draw(int cards, Headquarters hq)
        {
            for (int i = 0; i < cards; i++)
            {
                if (HasCards())
                {
                    hq.Add(RemoveTop());
                }
            }
        }

        public ICard RemoveTop()
        {
            return deck.RemoveTop();
        }
    }
}