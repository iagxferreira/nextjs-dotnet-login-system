using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.SharedContext.Entities
{
    public abstract class Entity : IEquatable<Guid>
    {
        public Guid Id { get; }
        protected Entity() => Id = Guid.NewGuid();

        bool IEquatable<Guid>.Equals(Guid id) => Id == id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
