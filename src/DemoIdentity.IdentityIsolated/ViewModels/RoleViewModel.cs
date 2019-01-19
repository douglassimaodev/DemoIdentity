using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DemoIdentity.IdentityIsolated.Entities;
using resx = DemoIdentity.IdentityIsolated.Resources.IdentityResx;

namespace DemoIdentity.IdentityIsolated.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            ClaimGroups = new HashSet<ClaimGroup>();
        }

        public long RoleId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(resx))]
        [MaxLength(256)]
        [Display(ResourceType = typeof(resx), Name = "Name")]
        public string Name { get; set; }

        public ICollection<ClaimGroup> ClaimGroups { get; set; }
    }
}