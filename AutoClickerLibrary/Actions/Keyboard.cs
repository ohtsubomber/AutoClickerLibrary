using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClicker.Actions
{
    public static class Keyboard
    {
        public static class Codes
        {
            public const string BackSpace = "{BACKSPACE}";
            public const string Enter = "{ENTER}";
        }

        /// <summary>
        /// 文字を送信します。メッセージ中に中括弧は使用できません。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendAsync(string message)
        {
            await Task.Run(() =>
            {
                SendKeys.SendWait(message);
            });
        }

        private static string Escape(string message)
        {
            return message.Replace("+", "{+}")
                .Replace("^", "{^}")
                .Replace("%", "{%}")
                .Replace("~", "{~}")
                .Replace("(", "{(}")
                .Replace(")", "{)}");
        }
    }
}
