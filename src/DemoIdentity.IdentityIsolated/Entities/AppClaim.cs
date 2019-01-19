using System.ComponentModel.DataAnnotations.Schema;

namespace DemoIdentity.IdentityIsolated.Entities
{
    public class AppClaim
    {
        public long AppClaimId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanEdit { get; set; }


        /// <summary>
        /// /Used in Dappert to get the information easy without navigation
        /// </summary>        
        [NotMapped]
        public ClaimStatusEnum Status { get; set; }

        /// <summary>
        /// /Used in Dappert to get the information easy without navigation
        /// </summary> 
        [NotMapped]
        public long ClaimInRoleId { get; set; }

        /// <summary>
        /// /Used in Dappert to get the information easy without navigation
        /// </summary> 
        [NotMapped]
        public bool IsSelected { get; set; }

    }
}