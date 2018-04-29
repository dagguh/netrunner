﻿using System.Collections.Generic;

namespace model.play
{
    public class Ability : IPayabilityObserver
    {
        public readonly ICost cost;
        public readonly IEffect effect;
        private HashSet<IUsabilityObserver> observers = new HashSet<IUsabilityObserver>();

        public Ability(ICost cost, IEffect effect)
        {
            this.cost = cost;
            this.effect = effect;
        }

        public void Trigger(Game game)
        {
            if (cost.CanPay(game))
            {
                cost.Pay(game);
                effect.Resolve(game);
            }
        }

        public void Observe(IUsabilityObserver observer, Game game)
        {
            observers.Add(observer);
            cost.Observe(this, game);
        }

        void IPayabilityObserver.NotifyPayable(bool payable)
        {
            foreach (IUsabilityObserver observer in observers)
            {
                observer.NotifyUsable(payable);
            }
        }
    }

    public interface IUsabilityObserver
    {
        void NotifyUsable(bool usable);
    }
}