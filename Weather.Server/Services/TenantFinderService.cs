using Weather.Server.Data;
using Weather.Server.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Weather.Server.Services
{
    public class TenantFinderService: ITenantFinderInterface
    {
         public async Task <Guid> GetTenantId (string userEmail, ApplicationDbContext context)
        {
            Guid tenantId = Guid.Empty;

            if (string.IsNullOrEmpty (userEmail)) 
            {
                return tenantId;
            }
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

            if (user == null || user.TenantId == default)
            {
                return tenantId;
            }
            return user.TenantId;
        }

    }
        






    }
    

