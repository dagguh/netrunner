﻿using model.cards;

namespace model.effects.runner
{
    public class Install : IEffect
    {
        private ICard card;

        public Install(ICard card)
        {
            this.card = card;
        }

        void IEffect.Resolve(Game game)
        {
            game.runner.rig.Install(card);
        }
    }
}