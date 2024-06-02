using Forms.Core.Messages;
using Forms.Core.Messages.CommonMessages.DomainEvents;
using System;
using System.Collections.Generic;

namespace Forms.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<DomainEvent> _notificacoes;
        public IReadOnlyCollection<DomainEvent> Notificacoes => _notificacoes?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(DomainEvent evento)
        {
            _notificacoes = _notificacoes ?? new List<DomainEvent>();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(DomainEvent eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
