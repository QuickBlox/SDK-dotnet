using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Modules.UsersModule.Models;

namespace Quickblox.Sdk.Modules.UsersModule.Responses
{
    public class RetrieveUsersResponse : PagedResponse<UserResponse> { }

    public class RetrieveUsersResponse<T> : PagedResponse<UserResponse<T>> where T : User { }
}
