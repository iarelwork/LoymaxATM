using Persistence.Common.Interfaces;

namespace Persistence.Common
{
    public abstract class TrackedEntity : IDateCreatedAndUpdated
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
