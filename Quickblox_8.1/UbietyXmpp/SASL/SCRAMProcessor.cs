// SCRAMProcessor.cs
//
//Copyright © 2006 - 2012 Dieter Lunn
//Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using Gnu.Inet.Encoding;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using XMPP.common;
using XMPP.tags;

namespace XMPP.SASL
{
	public class SCRAMProcessor : SASLProcessor
	{
        public SCRAMProcessor(Manager manager) : base(manager) {}

		private readonly Encoding _utf = Encoding.UTF8;

		private string _nonce;
		private string _snonce;
		private byte[] _salt;
		private int _i;
		private string _clientFirst;
		private byte[] _serverFirst;
		private string _clientFinal;

		private byte[] _serverSignature;
		private string _clientProof;

		public override Tag Initialize()
		{
			base.Initialize();
#if DEBUG
			Manager.Events.LogMessage(this, LogType.Debug, "Initializing SCRAM Processor");
			Manager.Events.LogMessage(this, LogType.Debug, "Generating nonce");
#endif
			_nonce = NextInt64().ToString();
#if DEBUG
			Manager.Events.LogMessage(this, LogType.Debug, "Nonce: {0}", _nonce);
			Manager.Events.LogMessage(this, LogType.Debug, "Building Initial Message");
#endif
			var msg = new StringBuilder();
			msg.Append("n,,n=");
			msg.Append(Id.User);
			msg.Append(",r=");
			msg.Append(_nonce);

#if DEBUG
			Manager.Events.LogMessage(this, LogType.Debug, "Message: {0}", msg.ToString());
#endif

			_clientFirst = msg.ToString().Substring(3);

            tags.xmpp_sasl.auth tag = new tags.xmpp_sasl.auth();
            tag.mechanism = MechanismType.SCRAM;
			tag.Bytes = _utf.GetBytes(msg.ToString());
            return tag as tags.xmpp_sasl.auth;
		}

        public override Tag Step(Tag tag)
		{
            switch (tag.Name.LocalName)
			{
				case "challenge":
					{
						_serverFirst = tag.Bytes;
						var response = _utf.GetString(tag.Bytes, 0, tag.Bytes.Length );

#if DEBUG
						Manager.Events.LogMessage(this, LogType.Debug, "Challenge: {0}", response);
#endif

						// Split challenge into pieces
						var tokens = response.Split(',');

						_snonce = tokens[0].Substring(2);
						// Ensure that the first length of nonce is the same nonce we sent.
						var r = _snonce.Substring(0, _nonce.Length);
						if (0 != String.Compare(r, _nonce))
						{
#if DEBUG
							Manager.Events.LogMessage(this, LogType.Debug, "{0} does not match {1}", r, _nonce);
#endif
						}

#if DEBUG
						Manager.Events.LogMessage(this, LogType.Debug, "Getting Salt");
#endif
						var a = tokens[1].Substring(2);
						_salt = Convert.FromBase64String(a);
#if DEBUG
						Manager.Events.LogMessage(this, LogType.Debug, "Getting Iterations");
#endif
						var i = tokens[2].Substring(2);
						_i = int.Parse(i);
#if DEBUG
						Manager.Events.LogMessage(this, LogType.Debug, "Iterations: {0}", _i);
#endif

						var final = new StringBuilder();
						final.Append("c=biws,r=");
						final.Append(_snonce);

						_clientFinal = final.ToString();

						CalculateProofs();

						final.Append(",p=");
						final.Append(_clientProof);

#if DEBUG
						Manager.Events.LogMessage(this, LogType.Debug, "Final Message: {0}", final.ToString());
#endif

                        Tag resp = new tags.xmpp_sasl.response() as Tag;
						resp.Bytes = _utf.GetBytes(final.ToString());

						return resp;
					}

				case "success":
					{
						var response = _utf.GetString(tag.Bytes, 0, tag.Bytes.Length );
						var signature = Convert.FromBase64String(response.Substring(2));
						return _utf.GetString(signature, 0, signature.Length ) == _utf.GetString(_serverSignature, 0, _serverSignature.Length ) ? tag : null;
					}
				case "failure":
				    return tag;
			}

			return null;
		}

		private void CalculateProofs()
		{
            var hmac = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var hash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);

            var saltedPassword = Hi();
            var hmacKey = hmac.CreateKey(saltedPassword.AsBuffer());

            // Calculate Client Key
            var clientKey = CryptographicEngine.Sign(hmacKey, (_utf.GetBytes("Client Key")).AsBuffer()).ToArray();
            
            // Calculate Server Key
            var serverKey = CryptographicEngine.Sign(hmacKey, (_utf.GetBytes("Server Key")).AsBuffer()).ToArray();
 
            // Calculate Stored Key

            var storedKey = hash.HashData(clientKey.AsBuffer()).ToArray();

            var a = new StringBuilder();
            a.Append(_clientFirst);
            a.Append(",");
            a.Append(_utf.GetString(_serverFirst, 0, _serverFirst.Length));
            a.Append(",");
            a.Append(_clientFinal);

            var auth = _utf.GetBytes(a.ToString());

            // Calculate Client Signature
            var storedKeyhmacKey = hmac.CreateKey(storedKey.AsBuffer());
            var signature = CryptographicEngine.Sign(storedKeyhmacKey, auth.AsBuffer() ).ToArray();

            // Calculate Server Signature
            var serverKeyhmacKey = hmac.CreateKey(serverKey.AsBuffer());
            _serverSignature = CryptographicEngine.Sign(serverKeyhmacKey, auth.AsBuffer()).ToArray();

            // Calculate Client Proof
            var proof = new byte[20];
            for (var i = 0; i < signature.Length; ++i)
            {
                proof[i] = (byte)(clientKey[i] ^ signature[i]);
            }

            _clientProof = Convert.ToBase64String(proof);
		}

		private byte[] Hi()
		{
            var prev = new byte[20];

            // Add 1 to the end of salt with most significat octet first
            var key = new byte[_salt.Length + 4];

            Array.Copy(_salt, key, _salt.Length);
            byte[] g = { 0, 0, 0, 1 };
            Array.Copy(g, 0, key, _salt.Length, 4);

            // Compute initial hash
            var hmac = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");

            // Create Key 
            var passwordBuffer = CryptographicBuffer.ConvertStringToBinary(Stringprep.SASLPrep(Password), BinaryStringEncoding.Utf8);
            var hmacKey = hmac.CreateKey(passwordBuffer);

            var result = CryptographicEngine.Sign(hmacKey, key.AsBuffer()).ToArray();

            Array.Copy(result, prev, (int)result.Length);

            for (var i = 1; i < _i; ++i)
            {
                var temp = CryptographicEngine.Sign(hmacKey, prev.AsBuffer()).ToArray();
                for (var j = 0; j < temp.Length; ++j)
                {
                    result[j] ^= temp[j];
                }

                Array.Copy(temp, prev, temp.Length);
            }

            return result;
		}
	}
}
