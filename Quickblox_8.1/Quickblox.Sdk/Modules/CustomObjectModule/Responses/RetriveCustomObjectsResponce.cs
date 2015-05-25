using Quickblox.Sdk.Modules.CustomObjectModule.Models;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Responses
{
    public class RetriveCustomObjectsResponce<T> where T : BaseCustomObject
    {
        public CustomObjectItems<T> CustomObjectItems { get; set; }
    }
}
