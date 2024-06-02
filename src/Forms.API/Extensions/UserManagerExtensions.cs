using Forms.API.Models;
using Forms.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forms.API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByKeyAsync(this UserManager<ApplicationUser> userManager, string key)
        {
            return await userManager?.Users?.FirstOrDefaultAsync(x => x.Key == key);
        }

        public static async Task<IEnumerable<ApplicationUser>> GetUsersByIds(this UserManager<ApplicationUser> userManager, IEnumerable<string> ids)
        {
            return await userManager?.Users?.Where(x => ids.Contains(x.Id)).ToListAsync();   
        }

        public static async Task<IEnumerable<ApplicationUser>> GetAllUsersPaginated(this UserManager<ApplicationUser> userManager, int page, int pageSize)
        {
            return await userManager?.Users?.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

    }
}
