using System;

namespace DemoIdentity.IdentityIsolated.Entities
{

    public class ClaimInRole //: BaseEntity
    {
        public long ClaimInRoleId { get; set; }
        public long AppClaimId { get; set; }
        public long RoleId { get; set; }
        public ClaimStatusEnum Status { get; set; }

        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }
        public virtual AppClaim AppClaim { get; set; }
    }
}