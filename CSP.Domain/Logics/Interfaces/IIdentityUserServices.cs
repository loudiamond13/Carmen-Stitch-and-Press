using CSP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IIdentityUserServices
    {
        Task<CarmenStitchAndPressUserModel> GetUserByEmail(string email);
        Task<List<CarmenStitchAndPressUserModel>> GetAllUsers();
        Task<bool> CreateUser(string email, string password, string role);
        Task<bool> AssignRoleToUser(string id, string role);
        Task<bool> RemoveUserRole(string id, string role);

        Task<List<CarmenStitchAndPressUserModel>> GetUsersByRole(string role);

        Task<CarmenStitchAndPressUserModel> GetUserByIdAsync(string userId);

        Task UnlockLockUserByIdAsync(string userId);
    }
}
