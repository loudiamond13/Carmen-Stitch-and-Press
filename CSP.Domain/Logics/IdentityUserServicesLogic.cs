using Azure.Core;
using CSP.Domain.Logics.Interfaces;
using CSP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics
{
    public class IdentityUserServicesLogic : IIdentityUserServices
    {
        private readonly UserManager<CarmenStitchAndPressUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IdentityUserServicesLogic(UserManager<CarmenStitchAndPressUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #region get single user
        public async Task<CarmenStitchAndPressUserModel> GetUserByEmail(string email)
        {
            var user = _userManager.Users.Where(x => x.Email == email)
                                            .Select(u => new CarmenStitchAndPressUserModel
                                            {
                                                Id = u.Id,
                                                Email = u.Email,
                                                FirstName = u.FirstName,
                                                LastName = u.LastName,
                                                LockoutEnd = u.LockoutEnd,
                                                PasswordHash = "",
                                                SecurityStamp = "",
                                                ConcurrencyStamp = "",
                                                UserName = u.UserName
                                            }).FirstOrDefaultAsync();

            if (user != null)
            {
                return await user;
            }

            return null;
        }
        #endregion
        #region Get All users
        public async Task<List<CarmenStitchAndPressUserModel>> GetAllUsers()
        {
            List<CarmenStitchAndPressUserModel> userList = new();

            userList = await _userManager.Users
                                        .Select(u => new CarmenStitchAndPressUserModel
                                        {
                                            Id = u.Id,
                                            Email = u.Email,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            LockoutEnd = u.LockoutEnd,
                                            PasswordHash = "",
                                            SecurityStamp = "",
                                            ConcurrencyStamp = "",
                                            UserName = u.UserName
                                        })
                                        .ToListAsync();
            return userList;
        }
        #endregion

        #region create user
        public async Task<bool> CreateUser(string email, string password, string role)
        {
            CarmenStitchAndPressUserModel newUser = new CarmenStitchAndPressUserModel
            {
                Email = email,
                UserName = email,
            };

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(newUser, role);

                return true;
            }

            return false;
        }
        #endregion

        #region Assign Role To User
        public async Task<bool> AssignRoleToUser(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.AddToRoleAsync(user, role);

                return result.Succeeded;
            }

            return false;
        }
        #endregion

        #region
        public async Task<bool> RemoveUserRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            var res = await _userManager.RemoveFromRoleAsync(user, role);

            return res.Succeeded;
        }
        #endregion

        #region get users by role
        public async Task<List<CarmenStitchAndPressUserModel>> GetUsersByRole(string role) 
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            var result = users.Select(u => new CarmenStitchAndPressUserModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                LockoutEnd = u.LockoutEnd,
                PasswordHash = "",
                SecurityStamp = "",
                ConcurrencyStamp = "",
                UserName = u.UserName
            }).ToList();


            return result;
        }
        #endregion
        #region get user by id 
        public async Task<CarmenStitchAndPressUserModel> GetUserByIdAsync(string userId) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return user;
            }
            else 
            {
                return null;
            }
        }
        #endregion
        #region unlock or lock user
        public async Task UnlockLockUserByIdAsync( string userId) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            // if user is currently lock
            if (user?.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // user is currently lock
                user.LockoutEnd = DateTime.Now;
            }
            //else if user is not lock, 
            //lock the user
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(50);
            }

             await _userManager.UpdateAsync( user);
            
        }
        #endregion


    }
}
