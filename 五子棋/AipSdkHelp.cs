using Baidu.Aip.Speech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 五子棋
{
    public class AipSdkHelp
    {
        // 设置APPID/AK/SK
        static string APP_ID = "10379396";
        static string API_KEY = "T5ErkLF9MyZ432AKBtoj6jNL";
        static string SECRET_KEY = "8280d528e7ba061fa4e543d620d9652f";
        static Tts client = new Tts(API_KEY, SECRET_KEY);

        public static string Speech(string text, int spd, int vol, int per)
        {
            // 可选参数
            var option = new Dictionary<string, object>()
            {
                {"spd", spd}, // 语速 5
                {"vol", vol}, // 音量 7
                {"per", per}  // 发音人，4：情感度丫丫童声
            };
            try
            {
                var result = client.Synthesis(text, option);
                if (result.ErrorCode == 0)  // 或 result.Success
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".mp3";
                    File.WriteAllBytes(fileName, result.Data);
                }
            }
            catch (Exception e)
            {
                //return null;
                throw e;
            }

            return null;
        }
    }
}
