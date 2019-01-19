using DemoIdentity.IdentityIsolated.ContextConfiguration;
using DemoIdentity.IdentityIsolated.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace DemoIdentity.Presentation.Data
{
    public static class DbSeeder
    {

        public static void DbInit(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            #region Security

            context.Database.EnsureCreated();
            SeedClaimGroups(context);
            SeedClaims(context);
            SeedClaimInClaimGroup(context);
            SeedRoles(context, roleManager);
            SeedClaimInRole(context);
            SeedUser(context, userManager);
            SeedUserRole(context, userManager);

            #endregion            
        }

        #region Security

        private static void SeedClaimGroups(ApplicationDbContext context)
        {
            if (!context.ClaimGroups.Any())
            {
                context.ClaimGroups.Add(new ClaimGroup { Name = "Administração", Description = "Sessão de Administração", InPosition = 1 });
                context.ClaimGroups.Add(new ClaimGroup { Name = "Home Section", Description = "Sessão da Home", InPosition = 2 });
                context.SaveChanges();

                var adminSectionId = 1;
                var homeSectionId = 2;

                #region Administração

                context.ClaimGroups.Add(new ClaimGroup { Name = "Administração modulo 1", Description = "", InPosition = 1, ParentClaimGroupId = adminSectionId });
                context.ClaimGroups.Add(new ClaimGroup { Name = "Administração modulo 2", Description = "", InPosition = 2, ParentClaimGroupId = adminSectionId });

                #endregion

                #region Home

                context.ClaimGroups.Add(new ClaimGroup { Name = "Home modulo 1", Description = "", InPosition = 1, ParentClaimGroupId = homeSectionId });

                context.ClaimGroups.Add(new ClaimGroup { Name = "Home modulo 2", Description = "", InPosition = 2, ParentClaimGroupId = homeSectionId });

                #endregion
            }
        }

        private static void SeedClaims(ApplicationDbContext context)
        {
            if (!context.AppClaims.Any())
            {
                #region Admin

                context.AppClaims.Add(new AppClaim { Name = "Create", Description = "Pode Criar", Code = "Create" });
                context.AppClaims.Add(new AppClaim { Name = "Read", Description = "Pode Ler", Code = "Read" });
                context.AppClaims.Add(new AppClaim { Name = "Update", Description = "Pode Atualizar", Code = "Update" });
                context.AppClaims.Add(new AppClaim { Name = "Delete", Description = "Pode Deletar", Code = "Delete" });

                #endregion

                #region Home      

                context.AppClaims.Add(new AppClaim { Name = "PodeVerBotaoGlobal", Description = "Pode Ver Botao Global", Code = "PodeVerBotaoGlobal" });
                context.AppClaims.Add(new AppClaim { Name = "PodeVerAdminPage", Description = "Pode Ver Admin Pagen", Code = "PodeVerAdminPage" });

                #endregion            

                context.SaveChanges();
            }
        }

        private static void SeedClaimInClaimGroup(ApplicationDbContext context)
        {

            if (!context.ClaimsInClaimGroup.Any())
            {
                #region Admin

                //Legislation laws
                var id = context.ClaimGroups.Where(x => x.Name == "Administração modulo 1").Select(x => x.ClaimGroupId).Single();
                var clainsCode = new List<string>() { "Create", "Read" };

                var claims = context.AppClaims.Where(x => clainsCode.Contains(x.Code));

                foreach (var item in claims)
                {
                    context.ClaimsInClaimGroup.Add(new ClaimInClaimGroup { ClaimGroupId = id, AppClaimId = item.AppClaimId });
                }


                id = context.ClaimGroups.Where(x => x.Name == "Administração modulo 2").Select(x => x.ClaimGroupId).Single();
                clainsCode = new List<string>() { "Update", "Delete" };

                claims = context.AppClaims.Where(x => clainsCode.Contains(x.Code));

                foreach (var item in claims)
                {
                    context.ClaimsInClaimGroup.Add(new ClaimInClaimGroup { ClaimGroupId = id, AppClaimId = item.AppClaimId });
                }

                #endregion

                #region Home

                //Organization
                id = context.ClaimGroups.Where(x => x.Name == "Home modulo 1").Select(x => x.ClaimGroupId).Single();
                clainsCode = new List<string>() { "PodeVerBotaoGlobal" };

                claims = context.AppClaims.Where(x => clainsCode.Contains(x.Code));

                foreach (var item in claims)
                {
                    context.ClaimsInClaimGroup.Add(new ClaimInClaimGroup { ClaimGroupId = id, AppClaimId = item.AppClaimId });
                }

                //Organization Internal
                id = context.ClaimGroups.Where(x => x.Name == "Home modulo 2").Select(x => x.ClaimGroupId).Single();
                clainsCode = new List<string>() { "PodeVerAdminPage" };

                claims = context.AppClaims.Where(x => clainsCode.Contains(x.Code));

                foreach (var item in claims)
                {
                    context.ClaimsInClaimGroup.Add(new ClaimInClaimGroup { ClaimGroupId = id, AppClaimId = item.AppClaimId });
                }

                #endregion

                context.SaveChanges();
            }
        }

        private static void SeedRoles(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            if (!context.ApplicationRoles.Any())
            {
                var role = new ApplicationRole
                {
                    Name = "Super User",
                    NormalizedName = "SUPER USER"
                };

                roleManager.CreateAsync(role).Wait();

                role = new ApplicationRole
                {
                    Name = "Normal",
                    NormalizedName = "NORMAL"
                };

                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void SeedClaimInRole(ApplicationDbContext context)
        {
            if (!context.ClaimsInRole.Any())
            {
                var clainIds = context.AppClaims.Select(x => x.AppClaimId);

                foreach (var item in clainIds)
                {
                    context.ClaimsInRole.Add(new ClaimInRole { AppClaimId = item, RoleId = 1, Status = ClaimStatusEnum.Yes });
                }


                // Add todas as clains mas configura para dar acesso e nao dar acesso

                var clainsCode = new List<string>() { "Create", "Read", "Update", "Delete" };
                clainIds = context.AppClaims.Where(x => clainsCode.Contains(x.Code)).Select(x => x.AppClaimId);

                foreach (var item in clainIds)
                {
                    context.ClaimsInRole.Add(new ClaimInRole { AppClaimId = item, RoleId = 2, Status = ClaimStatusEnum.Yes });
                }

                clainsCode = new List<string>() { "PodeVerBotaoGlobal", "PodeVerAdminPage" };
                clainIds = context.AppClaims.Where(x => clainsCode.Contains(x.Code)).Select(x => x.AppClaimId);

                foreach (var item in clainIds)
                {
                    context.ClaimsInRole.Add(new ClaimInRole { AppClaimId = item, RoleId = 2, Status = ClaimStatusEnum.No });
                }

                context.SaveChanges();
            }
        }

        private static void SeedUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.ApplicationUsers.Any())
            {
                var user = new ApplicationUser
                {
                    Email = "supper_admin913746@demo.com",
                    UserName = "supper_admin913746@demo.com"
                };

                userManager.CreateAsync(user, "@FakeUserForSecurity79812734912@%").Wait();

                user = new ApplicationUser
                {
                    Email = "supper_admin4679138@demo.com",
                    UserName = "supper_admin4679138@demo.com"
                };

                userManager.CreateAsync(user, "@FakeUserForSecurity98756489465484@%").Wait();

                user = new ApplicationUser
                {
                    Email = "admin@demo.com",
                    UserName = "admin@demo.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(user, "@Adm123456").Wait();


                user = new ApplicationUser
                {
                    Email = "user@demo.com",
                    UserName = "user@demo.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(user, "@Use123456").Wait();
            }
        }

        private static void SeedUserRole(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.ApplicationUserRoles.Any())
            {
                // _userManager.FindByNameAsync(model.Email);
                var user = userManager.FindByNameAsync("admin@demo.com").Result;
                userManager.AddToRoleAsync(user, "Super User").Wait();

                user = userManager.FindByNameAsync("user@demo.com").Result;
                userManager.AddToRoleAsync(user, "Normal").Wait();
            }
        }

        #endregion       
    }
}