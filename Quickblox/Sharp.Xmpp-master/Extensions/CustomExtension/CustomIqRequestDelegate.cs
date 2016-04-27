using System;

namespace Sharp.Xmpp.Extensions
{
    /// <summary>
    /// Invoked when a CustomIqRequest is made.
    /// </summary>
    /// <param name="str">The serialised data stream</param>
    /// <returns>The serialised anwser string</returns>
    public delegate string CustomIqRequestDelegate(Jid jid, string str);
}
