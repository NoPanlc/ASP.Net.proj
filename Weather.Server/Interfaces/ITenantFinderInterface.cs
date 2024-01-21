using Microsoft.AspNetCore.Identity;
using Weather.Server.Data;

namespace Weather.Server.Interfaces
{
    public interface ITenantFinderInterface
    {
       Task<Guid> GetTenantId (string userEmail, ApplicationDbContext context);
    }
}
