﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Quickblox.Sdk;
using Quickblox.Sdk.Cryptographic;
using System.Threading.Tasks;

namespace Quickblox.Android.Test
{
    [Activity(Label = "Quickblox.Android.Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {      
        public const string AccountKey = "Xh9wmkt6NeDVe4sLEETq";
        public const string ApiBaseEndPoint = "http://apidateifi.quickblox.com";
        public const int ApplicationId = 6;
        public const string AuthorizationKey = "n5bxC3eeKtqAXFu";
        public const string AuthorizationSecret = "zcqRqh3gCuFdMRd";
        public const string ChatEndpoint = "chatdateifi.quickblox.com";

        private QuickbloxClient client;

        #region chats
        private static string password1 = "cb33972b5b5de0afa5e0a37e8b416d8d03fce738";
		private static int id1 = 40606;
        
        private static QuickbloxClient client1;
        private static IPrivateChatManager chatManager1;
        //private static List<Presence> lastPresencesClient1;

        private static QuickbloxClient client2;
        private static IPrivateChatManager chatManager2;
        //private static Message lastMessageClient2;
        //private static List<Presence> lastPresencesClient2;

        private static string dialogId = "55a3a744535c1219ce001064";
        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += OnClicked;
        }

        private async void OnClicked(object o, EventArgs e)
        {
            Console.WriteLine("Start");
            client1 = new QuickbloxClient(ApplicationId, AuthorizationKey, AuthorizationSecret, ApiBaseEndPoint, ChatEndpoint);
#if DEBUG
            client1.ChatXmppClient.DebugClientName = "1";
#endif
            Console.WriteLine("Start connect. Id: " + id1 + 
                                " AppId: "+ ApplicationId + 
                                " Password: " + password1);
            //try
            //
                client1.ChatXmppClient.Connect(ChatEndpoint, id1, (int)ApplicationId, password1);
           // }
            //catch (Exception ex)
            //{

            //}

            Console.WriteLine("Client connected");

//            client1.MessagesClient.OnPresenceReceived += (sender, presence) => { if (lastPresencesClient1 != null) lastPresencesClient1.Add(presence); };

            //client2 = new QuickbloxClient(ApiBaseEndPoint, ChatEndpoint, new HmacSha1CryptographicProvider());
#if DEBUG
            //client2.MessagesClient.DebugClientName = "2";
#endif
            //client2.MessagesClient.Connect("chat.quickblox.com", id2, (int)ApplicationId, password2);
            //chatManager2 = client2.MessagesClient.GetPrivateChatManager(id1, dialogId);
            //            client2.MessagesClient.OnMessageReceived += (sender, message) =>
            //            {
            //                lastMessageClient2 = message;
            //            };
            //            client2.MessagesClient.OnPresenceReceived += (sender, presence) =>
            //            {
            //                if (lastPresencesClient2 != null) lastPresencesClient2.Add(presence);
            //            };

            //            lastMessageClient2 = null;
            //            string messageText = "Test message";

                        chatManager1.SendMessage("Hello from xamarin chat");
                        await Task.Delay(5000);

            //            Console.WriteLine(lastMessageClient2?.MessageText);
        }
    }
}

