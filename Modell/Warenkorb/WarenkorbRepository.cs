﻿using System;
using Infrastruktur.EventSourcing;

namespace Modell.Warenkorb
{

    public sealed class WarenkorbRepository : EventStoreBasedRepository
    {

        public WarenkorbRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            AffectedBy<WarenkorbWurdeEroeffnet>(e => e.Warenkorb);
            AffectedBy<ArtikelWurdeZuWarenkorbHinzugefuegt>(e => e.Warenkorb);
        }

        public Warenkorb Retrieve(Guid aggregateId)
        {
            return new Warenkorb(new WarenkorbProjektion(aggregateId, () => History(aggregateId)), e => Publish(aggregateId, e));
        }
    }
}
