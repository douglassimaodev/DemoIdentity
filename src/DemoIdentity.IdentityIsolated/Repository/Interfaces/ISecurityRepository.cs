using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoIdentity.IdentityIsolated.DTO;
using DemoIdentity.IdentityIsolated.Entities;
using DemoIdentity.IdentityIsolated.ViewModels;

namespace DemoIdentity.IdentityIsolated.Repository.Interfaces
{
    public interface ISecurityRepository
    {
        Paged<ClaimGroup> GetAllClaimGroup(string name, int pageSize, int pageNumber);

        Paged<AppClaim> GetAllClaims(string name, int pageSize, int pageNumber);

        Paged<ApplicationUser> GetAllUsers(string name, int pageSize, int pageNumber);

        ClaimGroup GetClaimGroupById(long id);

        AppClaim GetClaimById(long id);

        List<AppClaim> GetClaimByUserId(long userId);
        Task<IEnumerable<Claim>> GetSecurityClaimByUserIdAsync(long userId);

        IEnumerable<ApplicationRole> GetAllRoles();
        ApplicationRole GetRoleById(long id);
        void InsertRoleClaims(long id, long userId, List<ClaimGroup> claimGroups);
        void UpdateRoleClaims(long id, long userId, List<ClaimGroup> claimGroups);

        void UpdateRole(ApplicationRole applicationRole);
        void InsertRole(ApplicationRole applicationRole);

        void SaveChanges();

        //void SendEmail(string msgTo, string msgFromEmail,
        //    string msgFromName, string msgSubject, string msgText, string msgType);

        IEnumerable<RoleViewModel> GetRoles();
        void SetUserRole(long newUserId, long roleId, long userLoggedId);
    }
}
