using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public interface IExtraParameter
    {
        XElement Build();
    }
}
