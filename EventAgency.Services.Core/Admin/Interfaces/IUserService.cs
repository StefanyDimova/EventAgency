using EventAgency.Web.ViewModels.Admin.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsByIdAsync(Guid userId);
        Task<IEnumerable<UserManagementIndexViewModel>> GetAllUsersAsync(string userId);
        Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel);

        Task<bool> RemoveUserRoleAsync(Guid userId, string roleName);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
