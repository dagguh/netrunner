﻿using model.cards.types;

namespace model.cards.corp
{
    public class VanityProject : Card
    {
        public VanityProject(Game game) : base(game) { }
        override public string FaceupArt => "vanity-project";
        override public string Name => "Vanity Project";
        override public Faction Faction => Factions.SHADOW;
        override public int InfluenceCost => 1;
        override public ICost PlayCost { get { throw new System.Exception("Agendas don't have play costs"); } }
        override public IEffect Activation { get { throw new System.Exception("Agendas don't have activations"); } }
        override public IType Type => new Agenda(printedAgendaPoints: 4, game);
    }
}
