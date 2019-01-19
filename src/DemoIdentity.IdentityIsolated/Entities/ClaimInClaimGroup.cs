namespace DemoIdentity.IdentityIsolated.Entities
{
    public class ClaimInClaimGroup //: BaseEntity
    {
        public long ClaimInClaimGroupId { get; set; }
        public long ClaimGroupId { get; set; }
        public long AppClaimId { get; set; }       

        public virtual ClaimGroup ClaimGroup { get; set; }
        public virtual AppClaim AppClaim { get; set; }
    }
}
