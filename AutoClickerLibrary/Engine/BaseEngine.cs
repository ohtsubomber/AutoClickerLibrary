using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoClicker.Engine
{
    public class BaseEngine : IEngine
    {
        private string ImageDir = "";
        public async Task ExecuteScriptAsync(string script)
        {
            var lines = script.Split(new string[] {"\r\n","\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var args = line.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (args[0].StartsWith("#")) continue;
                switch (args[0])
                {
                    case "setmousepos":
                        await SetMousePos(int.Parse(args[1]),int.Parse(args[2]));
                        break;
                    case "movetoimage":
                        await MoveToImage(args[1],args[2]);
                        break;
                    case "click":
                        await Click();
                        break;
                    case "doubleclick":
                        await DoubleClick();
                        break;
                    case "rightclick":
                        await RightClick();
                        break;
                    case "wait":
                        await Wait(int.Parse(args[1]));
                        break;
                    case "setimagedir":
                        SetImageDir(args[1]);
                        break;
                }
            }
        }
        private async Task SetMousePos(int x, int y)
        {
            await Actions.Mouse.SetPosAsync(x, y);
        } 

        private async Task Click()
        {
            await Actions.Mouse.ClickAsync();
        }

        private async Task DoubleClick()
        {
            await Actions.Mouse.DoubleClickAsync();
        }

        private async Task RightClick()
        {
            await Actions.Mouse.RightClickAsync();
        }

        private async Task Wait(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }

        private async Task MoveToImage(string imagePath, string position)
        {
            var imageFullPath = $"{ImageDir}/{imagePath}";
            int? xOffset = null;
            int? yOffset = null;
            var regex = new Regex("\\((?<xOffset>[-0-9]+),(?<yOffset>[-0-9]+)\\)");
            var match = regex.Match(position);
            if (match.Success)
            {
                xOffset = int.Parse(match.Groups["xOffset"].Value);
                yOffset = int.Parse(match.Groups["yOffset"].Value);
            }

            await Actions.Mouse.MoveToImage(imageFullPath, xOffset, yOffset);
        }
        public void SetImageDir(string imageDir)
        {
            ImageDir = imageDir;
        }
    }
}
