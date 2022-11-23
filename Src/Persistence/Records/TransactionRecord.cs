using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Persistence.Common;

namespace Persistence.Records
{
    public class TransactionRecord : TrackedEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public virtual AccountRecord Account { get; set; }
    }
}
