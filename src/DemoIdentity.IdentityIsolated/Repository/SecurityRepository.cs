using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoIdentity.IdentityIsolated.ContextConfiguration;
using DemoIdentity.IdentityIsolated.DTO;
using DemoIdentity.IdentityIsolated.Entities;
using DemoIdentity.IdentityIsolated.Repository.Interfaces;
using DemoIdentity.IdentityIsolated.ViewModels;

namespace DemoIdentity.IdentityIsolated.Repository
{
    public class SecurityRepository : ISecurityRepository
    {
        private ApplicationDbContext Db;
        protected DbSet<ClaimGroup> DbSetClaimGroup;

        private void SetDbContext(ApplicationDbContext value)
        {
            Db = value;
        }

        public SecurityRepository(ApplicationDbContext context)
        {
            SetDbContext(context);
            DbSetClaimGroup = Db.Set<ClaimGroup>();
        }


        public Paged<ClaimGroup> GetAllClaimGroup(string name, int pageSize, int pageNumber)
        {
            var cn = Db.Database.GetDbConnection();

            var sql = @"SELECT ClaimGroupId AS CLAIMGROUPID, NAME, DESCRIPTION, CanEdit AS CANEDIT FROM SecurityClaimGroup " +
                      " WHERE CanEdit = 1 AND (@Name IS NULL OR LOWER(NAME) LIKE @Name || '%') " +
                      " AND ROWNUM <= " + pageSize + " AND ROWNUM >= " + pageSize * (pageNumber - 1) +
                      " ORDER BY NAME ";

            var multi = cn.QueryMultiple(sql, new { Name = name != null ? name.ToLower() : null });
            var clientes = multi.Read<ClaimGroup>();
            //var total = multi.Read<int>().FirstOrDefault();

            var pagedList = new Paged<ClaimGroup>()
            {
                List = clientes,
                TotalCount = 0
            };

            return pagedList;
        }

        public Paged<AppClaim> GetAllClaims(string name, int pageSize, int pageNumber)
        {
            var cn = Db.Database.GetDbConnection();

            var sql = @"SELECT AppClaimId AS CLAIMID, NAME, DESCRIPTION, CanEdit AS CANEDIT FROM SecurityAppClaim " +
                      " WHERE CanEdit = 1 AND (@Name IS NULL OR LOWER(NAME) LIKE @Name || '%') " +
                      " AND ROWNUM <= " + pageSize + " AND ROWNUM >= " + pageSize * (pageNumber - 1) +
                      " ORDER BY NAME ";

            var multi = cn.QueryMultiple(sql, new { Name = name != null ? name.ToLower() : null });
            var clientes = multi.Read<AppClaim>();
            //var total = multi.Read<int>().FirstOrDefault();

            var pagedList = new Paged<AppClaim>()
            {
                List = clientes,
                TotalCount = 0
            };

            return pagedList;
        }

        public Paged<ApplicationUser> GetAllUsers(string name, int pageSize, int pageNumber)
        {
            var cn = Db.Database.GetDbConnection();

            var sql = @"SELECT 
                        UserId AS USERID, 
                        FIRST_NAME AS FIRSTNAME,
                        LAST_NAME AS LASTNAME, 
                        EMAIL, 
                        EMAIL_CONFIRMED AS EMAILCONFIRMED,
                        PASSWORD_HASH AS PASSWORDHASH,
                        SECURITY_STAMP AS SECURITYSTAMP,
                        PHONE_NUMBER AS PHONENUMBER,
                        PHONE_NUMBER_CONFIRMED AS PHONENUMBERCONFIRMED,
                        TWO_FACTOR_ENABLED AS TWOFACTORENABLED, 
                        LOCK_OUT_END_DATE_UTC AS LOCKOUTENDDATEUTC, 
                        LOCK_OUT_ENABLED AS LOCKOUTENABLED, 
                        ACCESS_FAILED_COUNT AS ACCESSFAILEDCOUNT, 
                        USER_NAME AS USERNAME 
                        FROM SecurityUser " +
                      " WHERE (@Name IS NULL OR LOWER(FIRST_NAME) LIKE @Name || '%') " +
                      " AND ROWNUM <= " + pageSize + " AND ROWNUM >= " + pageSize * (pageNumber - 1) +
                      " ORDER BY FIRST_NAME ";

            var multi = cn.QueryMultiple(sql, new { Name = name != null ? name.ToLower() : null });
            var clientes = multi.Read<ApplicationUser>();
            //var total = multi.Read<int>().FirstOrDefault();

            var pagedList = new Paged<ApplicationUser>()
            {
                List = clientes,
                TotalCount = 0
            };

            return pagedList;
        }

        public ClaimGroup GetClaimGroupById(long id)
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT ClaimGroupId, NAME, DESCRIPTION, CanEdit FROM SecurityClaimGroup C WHERE C.ClaimGroupId = @id";

            var claimGroup = cn.Query<ClaimGroup>(sql, new { id });
            return claimGroup.FirstOrDefault();
        }

        public List<AppClaim> GetClaimByUserId(long userId)
        {
            var claimsDenied = GetClaimDeniedByUserId(userId).Select(x => x.AppClaimId).ToList();
            if (claimsDenied.Count == 0)
                claimsDenied.Add(0);

            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT c.Code FROM 
                        SecurityUser u
                        INNER JOIN SecurityUserRole ur ON u.UserId = ur.UserId
                        INNER JOIN SecurityRole r ON ur.RoleId = r.RoleId
                        INNER JOIN SecurityClaimInRole cr ON r.RoleId = cr.RoleId
                        INNER JOIN SecurityAppClaim c ON cr.AppClaimId = c.AppClaimId
                        WHERE u.UserId = @userId and cr.Status = @status and cr.AppClaimId not in @claimsdenied";

            var claims = cn.Query<AppClaim>(sql, new { userId, status = (int)ClaimStatusEnum.Yes, claimsDenied }).ToList();

            return claims;
        }

        public List<AppClaim> GetClaimDeniedByUserId(long userId)
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT CR.AppClaimId as ClaimId FROM 
                        SecurityUser U
                        INNER JOIN SecurityUserRole UR ON U.UserId = UR.UserId
                        INNER JOIN SecurityRole R ON UR.RoleId = R.RoleId
                        INNER JOIN SecurityClaimInRole CR ON R.RoleId = CR.RoleId
                        INNER JOIN SecurityAppClaim C ON CR.AppClaimId = C.AppClaimId
                        WHERE U.UserId = @userId AND CR.STATUS = @status";

            var claims = cn.Query<AppClaim>(sql, new { userId, status = (int)ClaimStatusEnum.Denied }).ToList();
            return claims;
        }

        public async Task<IEnumerable<System.Security.Claims.Claim>> GetSecurityClaimByUserIdAsync(long userId)
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT C.CODE as Type, '_' as Value FROM 
                        SecurityUser U
                        INNER JOIN SecurityUserRole UR ON U.UserId = UR.UserId
                        INNER JOIN SecurityRole R ON UR.RoleId = R.RoleId
                        INNER JOIN SecurityClaimInRole CR ON R.RoleId = CR.RoleId
                        INNER JOIN SecurityAppClaim C ON CR.AppClaimId = C.AppClaimId
                        WHERE U.UserId = @sid";

            return await cn.QueryAsync<System.Security.Claims.Claim>(sql, new { sid = userId });
        }

        public AppClaim GetClaimById(long id)
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT AppClaimId, NAME, DESCRIPTION, CanEdit FROM SecurityAppClaim C WHERE C.AppClaimId = @id";

            var claim = cn.Query<AppClaim>(sql, new { id });
            return claim.FirstOrDefault();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<ApplicationRole> GetAllRoles()
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT RoleId AS ID, NAME FROM SecurityRole";

            var roles = cn.Query<ApplicationRole>(sql);
            return roles;
        }

        public ApplicationRole GetRoleById(long id)
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT 
                        @id AS Id,
                        (SELECT Name FROM SecurityRole WHERE RoleId = @id) AS Name,                                               
                        PARENTCG.ClaimGroupId AS ParentClaimGroupId,
                        PARENTCG.Name,
                        PARENTCG.InPosition,
                        CG.ClaimGroupId,                        
                        CG.Name,
                        CG.InPosition,
                        CG.Description,                       
                        C.AppClaimId,
                        C.NAME,
                        C.CODE,
                        ISNULL(CR.Status,1) AS Status,
                        ISNULL(CR.ClaimInRoleId,0) AS ClaimInRoleId, 
                        CASE WHEN R.RoleId IS NULL THEN 0 ELSE 1 END AS ISCHECKED  
                        FROM SecurityAppClaim C
                        LEFT JOIN SecurityClaimInRole CR ON C.AppClaimId = CR.AppClaimId AND CR.RoleId = @id
                        LEFT JOIN SecurityRole R ON R.RoleId = CR.RoleId  AND R.RoleId = @id
                        LEFT JOIN SecurityClaimInClaimGroup CCG ON C.AppClaimId = CCG.AppClaimId
                        LEFT JOIN SecurityClaimGroup CG ON CCG.ClaimGroupId = CG.ClaimGroupId 
                        LEFT JOIN SecurityClaimGroup PARENTCG ON CG.ParentClaimGroupId = PARENTCG.ClaimGroupId 
                        ORDER BY R.NAME,PARENTCG.InPosition,PARENTCG.ClaimGroupId,CG.InPosition,CG.NAME,C.NAME";


            var applicationRole = new ApplicationRole();

            cn.Query<ApplicationRole, ClaimGroup, ClaimGroup, AppClaim, ApplicationRole>(sql,
                (ap, pcg, cg, c) =>
                {
                    if (applicationRole.Id == 0)
                        applicationRole = ap;

                    if (pcg != null && pcg.ParentClaimGroupId > 0)
                    {
                        var parentClaimGroup = applicationRole.ClaimGroups.FirstOrDefault(x => x.ParentClaimGroupId == pcg.ParentClaimGroupId);
                        if (parentClaimGroup != null)
                        {
                            var childClaimGroup = parentClaimGroup.ChildrenClaimGroup.FirstOrDefault(x => x.ClaimGroupId == cg.ClaimGroupId);
                            if (childClaimGroup != null)
                            {
                                childClaimGroup.AppClaims.Add(c);
                            }
                            else
                            {
                                cg.AppClaims.Add(c);
                                parentClaimGroup.ChildrenClaimGroup.Add(cg);
                            }

                        }
                        else
                        {
                            cg.AppClaims.Add(c);
                            pcg.ChildrenClaimGroup.Add(cg);
                            applicationRole.ClaimGroups.Add(pcg);
                        }
                    }
                    else
                    {
                        var claimGroup = applicationRole.ClaimGroups.FirstOrDefault(x => x.ClaimGroupId == cg.ClaimGroupId);
                        if (claimGroup != null)
                        {
                            claimGroup.AppClaims.Add(c);
                        }
                        else
                        {
                            cg.AppClaims.Add(c);
                            applicationRole.ClaimGroups.Add(cg);
                        }
                    }

                    return applicationRole;
                },
                new { id },
                    splitOn: "ID,ParentClaimGroupId,ClaimGroupId,AppClaimId");

            return applicationRole;

        }

        public void UpdateRole(ApplicationRole applicationRole)
        {
            var entry = Db.Entry(applicationRole);
            Db.Roles.Attach(applicationRole);
            entry.State = EntityState.Modified;
        }

        public void InsertRole(ApplicationRole applicationRole)
        {
            Db.Roles.Add(applicationRole);
        }

        public void UpdateRoleClaims(long id, long userId, List<ClaimGroup> claimGroups)
        {
            var claimInRole = new ClaimInRole();

            foreach (var claimGroup in claimGroups)
            {
                foreach (var claim in claimGroup.AppClaims)
                {
                    claimInRole = new ClaimInRole();
                    claimInRole.ClaimInRoleId = claim.ClaimInRoleId;
                    claimInRole.RoleId = id;
                    claimInRole.AppClaimId = claim.AppClaimId;
                    claimInRole.Status = claim.Status;
                    Db.Entry(claimInRole).State = EntityState.Modified;
                }
            }
        }
        public void InsertRoleClaims(long id, long userId, List<ClaimGroup> claimGroups)
        {
            var claimInRole = new ClaimInRole();
            foreach (var claimGroup in claimGroups)
            {
                foreach (var claim in claimGroup.AppClaims)
                {
                    claimInRole = new ClaimInRole();
                    claimInRole.AppClaimId = claim.AppClaimId;
                    claimInRole.Status = claim.Status;
                    claimInRole.RoleId = id;
                    Db.ClaimsInRole.Add(claimInRole);
                }
            }
        }

        private bool UpdateRoleScope(long id, long userId, List<long> claimIds)
        {
            string sql = "UPDATE SecurityClaimInRole SET EndedOn = null, EndedBy = null,ModifiedOn = GETDATE(), modified_by = @userId WHERE RoleId = @id and AppClaimId in @claimIds";
            var count = Db.Database.GetDbConnection().Execute(sql, new { id, userId, claimIds });
            return count > 0;
        }

        private bool InsertRoleScope(long id, long userId, List<long> claimIds)
        {
            string sql = @"insert into clain_in_role
                          (AppClaimId, RoleId, created_on, created_by)
                           select AppClaimId, @id ,GETDATE(),@userId from security_claim
                            where AppClaimId in @claimIds and AppClaimId not in (select AppClaimId from SecurityClaimInRole where RoleId = @id)";
            var count = Db.Database.GetDbConnection().Execute(sql, new { id, modifiedBy = userId, claimIds, userId });
            return count > 0;
        }

        private bool RemoveRoleScope(long id, long userId, List<long> claimIds)
        {
            string sql = "UPDATE SecurityClaimInRole SET EndedOn = GETDATE(),EndedBy = @endedby WHERE RoleId = @id";
            if (claimIds != null && claimIds.Count > 0)
                sql += " and AppClaimId not in @claimIds";
            var count = Db.Database.GetDbConnection().Execute(sql, new { id, endedby = userId, claimIds });
            return count > 0;
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }

        public IEnumerable<RoleViewModel> GetRoles()
        {
            var cn = Db.Database.GetDbConnection();
            var sql = @"SELECT 
                        RoleId,
                        Name
                        FROM SecurityRole WHERE EndedOn IS NULL";

            var roles = cn.Query<RoleViewModel>(sql);
            return roles;
        }


        public void SetUserRole(long id, long roleId, long userId)
        {
            Db.UserRoles.Add(new ApplicationUserRole() { RoleId = roleId, UserId = id, CreatedOn = DateTime.Now, CreatedBy = userId });
        }
    }
}