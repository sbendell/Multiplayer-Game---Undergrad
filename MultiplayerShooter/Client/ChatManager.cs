using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    class ChatManager
    {

        public static List<ChatItem> chatList = new List<ChatItem>();
        public static List<string> chatStrings = new List<string>();

        public static List<Vector2> textPosList = new List<Vector2>();

        static int delay = 600;
        static string longestString;

        public static void Update()
        {
            if (chatStrings.Count > 0)
            {
                longestString = chatStrings.OrderByDescending(s => s.Length).First();

            }

            for (int i = 0; i < chatList.Count; i++)
            {
                if (chatList[i].currTimer >= delay)
                {
                    chatStrings.Remove(chatList[i].chatItem);
                    chatList.RemoveAt(i);
                }
                else
                {
                    chatList[i].Update();
                }
            }
        }

        public static void Draw(SpriteBatch sb, SpriteFont sf, Texture2D ChatBackGround)
        {
            if (chatList.Count > 0)
            {
                sb.Draw(ChatBackGround, new Rectangle(0, 0, (int)(sf.MeasureString(longestString).X) + 4, 10 + chatList.Count * 20), Color.White);
            }
            for (int i = 0; i < chatList.Count; i++)
            {
                sb.DrawString(sf, chatList[i].chatItem, new Vector2(2, i * 20), Color.DarkRed);
            }
        }
    }
}
