using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoIdentity.IdentityIsolated.Entities
{
    public class ClaimGroup //: BaseEntity
    {
        public ClaimGroup()
        {
            ClaimInClaimGroups = new HashSet<ClaimInClaimGroup>();
            AppClaims = new List<AppClaim>();
            ChildrenClaimGroup = new List<ClaimGroup>();
        }

        public long ClaimGroupId { get; set; }
        public long? ParentClaimGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanEdit { get; set; }
        public int InPosition { get; set; }
       

        public ClaimGroup ParentClaimGroup { get; set; }

        public List<ClaimGroup> ChildrenClaimGroup { get; set; }

        public virtual ICollection<ClaimInClaimGroup> ClaimInClaimGroups { get; set; }


        /// <summary>
        /// /Used in Dappert to get the information easy without navigation
        /// </summary> 
        [NotMapped]
        public virtual List<AppClaim> AppClaims { get; set; }
    }
}