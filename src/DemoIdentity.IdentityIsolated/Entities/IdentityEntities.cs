using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoIdentity.IdentityIsolated.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public long? EndedBy { get; set; }
        public DateTime? EndedOn { get; set; }
    }

    public class ApplicationRole : IdentityRole<long>
    {
        public ApplicationRole()
        {
            ClaimGroups = new List<ClaimGroup>();
        }

        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public long? EndedBy { get; set; }
        public DateTime? EndedOn { get; set; }

        public virtual ICollection<ClaimInRole> ClaimsInRole { get; set; }

        /// <summary>
        /// /Used in Dappert to get the information easy without navigation
        /// </summary> 
        [NotMapped]
        public virtual List<ClaimGroup> ClaimGroups { get; set; }

    }

    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public long? EndedBy { get; set; }
        public DateTime? EndedOn { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<long>
    {
        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public long? EndedBy { get; set; }
        public DateTime? EndedOn { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<long>
    {

    }

    public class ApplicationRoleClaim : IdentityRoleClaim<long>
    {
        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public long? EndedBy { get; set; }
        public DateTime? EndedOn { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<long>
    {

    }

}