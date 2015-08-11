using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QMunicate.Core.DependencyInjection;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.UsersModule.Models;

namespace QMunicate.Helper
{
    public interface ICachingQuickbloxClient
    {
        Task<User> GetUserById(int userId);
    }

    public class CachingQuickbloxClient : ICachingQuickbloxClient
    {
        #region Fields

        private readonly IQuickbloxClient quickbloxClient;
        private readonly List<User> users = new List<User>();

        #endregion

        #region Ctor

        public CachingQuickbloxClient(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region ICachingQuickbloxClient Members

        public async Task<User> GetUserById(int userId)
        {
            var cachedUser = users.FirstOrDefault(u => u.Id == userId);
            if (cachedUser != null) return cachedUser;

            var response = await quickbloxClient.UsersClient.GetUserByIdAsync(userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                users.Add(response.Result.User);
                return response.Result.User;
            }

            return null;
        }

        #endregion

    }
}
