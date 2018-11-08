using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ChatItem
    {
        public string chatItem;
        public int currTimer;

        public ChatItem(string ChatItem)
        {
            chatItem = ChatItem;
        }

        public void Update()
        {
            currTimer++;
        }
    }
}
