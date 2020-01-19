﻿using model.cards;
using model.choices;
using model.costs;
using model.effects.corp;
using model.zones;
using model.zones.corp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.play.corp
{
    public class ActionCard : IResolutionObserver, IZoneAdditionObserver, IRemoteObserver
    {
        private zones.corp.Zones zones;
        public readonly Ability credit;
        public readonly Ability draw;
        private TaskCompletionSource<bool> actionTaking;
        private ActionPermission permission = new ActionPermission();
        private List<Ability> potentialActions = new List<Ability>();
        private HashSet<IActionPotentialObserver> actionPotentialObservers = new HashSet<IActionPotentialObserver>();

        public ActionCard(zones.corp.Zones zones)
        {
            this.zones = zones;
            zones.hq.Zone.ObserveAdditions(this);
            zones.ObserveRemotes(this);
            credit = new Ability(new Conjunction(new CorpClickCost(1), permission), new Gain(1));
            credit.ObserveResolution(this);
            MarkPotential(credit);
            draw = new Ability(new Conjunction(new CorpClickCost(1), permission), new Draw(1));
            draw.ObserveResolution(this);
            MarkPotential(draw);
        }

        public Ability Play(Card card)
        {
            Ability play = new Ability(
                new Conjunction(
                    new CorpClickCost(1),
                    card.PlayCost,
                    permission,
                    new InZone(card, zones.hq.Zone)
                ),
                new Play(card)
            );
            play.ObserveResolution(this);
            return play;
        }

        public Ability InstallInRemote(Card card, IRemoteInstallationChoice remoteChoice)
        {
            Ability install = new Ability(
                new Conjunction(
                    new CorpClickCost(1),
                    permission,
                    new InZone(card, zones.hq.Zone)
                ),
                new InstallInRemote(card, remoteChoice)
            );
            install.ObserveResolution(this);
            return install;
        }

        async public Task TakeAction()
        {
            permission.Grant();
            actionTaking = new TaskCompletionSource<bool>();
            await actionTaking.Task;
        }

        void IResolutionObserver.NotifyResolved()
        {
            actionTaking.SetResult(true);
        }

        public void ObservePotentialActions(IActionPotentialObserver observer)
        {
            actionPotentialObservers.Add(observer);
            foreach (var action in potentialActions)
            {
                observer.NotifyPotentialAction(action);
            }
        }

        private void MarkPotential(Ability action)
        {
            potentialActions.Add(action);
            foreach (var observer in actionPotentialObservers)
            {
                observer.NotifyPotentialAction(action);
            }
        }

        void IZoneAdditionObserver.NotifyCardAdded(Card card)
        {
            foreach (var action in InferActions(card))
            {
                MarkPotential(action);
            }
        }

        private List<Ability> InferActions(Card card)
        {
            if (card.Type.Playable)
            {
                return new List<Ability> { Play(card) };
            }
            if (card.Type.Installable)
            {
                var existingRemoteChoices = zones.remotes.Select(remote => new ExistingRemoteInstallationChoice(remote));
                var newRemoteChoices = new List<IRemoteInstallationChoice> { new NewRemoteInstallationChoice(zones) };
                return existingRemoteChoices
                    .Concat(newRemoteChoices)
                    .Select(remoteChoice => InstallInRemote(card, remoteChoice))
                    .ToList();
            }
            return new List<Ability>();
        }

        void IRemoteObserver.NotifyRemoteExists(Remote remote)
        {
            zones
                  .hq
                  .Zone
                  .Cards
                  .Where(card => card.Type.Installable)
                  .Select(card => InstallInRemote(card, new ExistingRemoteInstallationChoice(remote)))
                  .ToList()
                  .ForEach(action => MarkPotential(action));
        }
    }

    public interface IActionPotentialObserver
    {
        void NotifyPotentialAction(Ability action);
    }
}