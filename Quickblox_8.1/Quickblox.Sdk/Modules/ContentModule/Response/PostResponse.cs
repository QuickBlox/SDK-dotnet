using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.ContentModule.Response
{
    /// <summary>
    /// Parse AWS response after uploading file
    /// </summary>
    [KnownType(typeof(PostResponse))]
    [DataContract(Name = "PostResponse")]
    public class PostResponse
    {
        //<Location>http://qbprod.s3.amazonaws.com/353aa4f39b914620b979b58fdbf3e25c00</Location>
        //<Bucket>qbprod</Bucket>
        //<Key>353aa4f39b914620b979b58fdbf3e25c00</Key>
        //<ETag>"22871588b487e49c29efcfc31c0d97ea"</ETag>

        [DataMember]
        public String Location { get; set; }

        [DataMember]
        public String Bucket { get; set; }

        [DataMember]
        public String Key { get; set; }

        [DataMember]
        public String ETag { get; set; }
    }
}
