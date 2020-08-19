using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Simon.Models
{
        public class MessageThreadList
        {
            public List<messageThread> messagesThread { get; set; }
        }
        public class messageThread
        {
            public string id { get; set; }
            public string msgRecipient { get; set; }
            public string msgText { get; set; }
            public DateTime msgDateTime { get; set; }
        }
}

