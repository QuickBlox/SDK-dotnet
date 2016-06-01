namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public class AttachmentExtraParamValue
    {
        public string Id { get; set; }

        public Types Type { get; set; }
    }

    public enum Types
    {
        image
    }
}
