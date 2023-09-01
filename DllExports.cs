using System.Runtime.InteropServices;
using AndrealImageGenerator.Beans;
using AndrealImageGenerator.Beans.Json;
using AndrealImageGenerator.Graphics;
using AndrealImageGenerator.Graphics.Generators;
using Newtonsoft.Json;

namespace AndrealImageGenerator
{
    [ComVisible(true)]
    public class ImageGeneratorDll
    {
        /// <summary>拜托，计算器诶，超酷的好不好</summary>
        /// <param name="a">int a</param>
        /// <param name="b">int b</param>
        /// <returns>a + b</returns>
        public int Add(int a, int b)
        {
            return a + b;
        }

        private static MemoryStream FileStreamResult(BackGround backGround)
        {
            var ms = new MemoryStream();
            backGround.SaveAsPng(ms);
            ms.Position = 0;
            backGround.Dispose();
            return ms;
        }

        /// <summary>生成用户的最近分数样式的查分图</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI 的对应 API 的返回值，直接作为请求体<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/info.md
        /// </param>
        /// <param name="imgVersion">图片类型，可选值为 [1, 2, 3]</param>
        /// <returns>很酷的 info 图，PNG 格式</returns>
        public byte[] GetUserInfo(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserInfoContent>>(jsonStr);
            var backGround = RecordGenerator.Generate(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }

        /// <summary>生成用户的最高分数样式的查分图</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI 的对应 API 的返回值，直接作为请求体<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/best.md
        /// </param>
        /// <param name="imgVersion">图片类型，可选值为 [1, 2, 3]</param>
        /// <returns>很酷的 best 图，PNG 格式</returns>
        public byte[] GetUserBest(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserBestContent>>(jsonStr);
            var backGround = RecordGenerator.Generate(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }

        private static Task<BackGround> GetBestsGeneratorImage(UserBestsContent content, ImgVersion imgVersion)
        {
            switch (imgVersion)
            {
                case ImgVersion.ImgV2:
                    return new Best40Generator(content).Generate();
                default:
                    return new Best30Generator(content).Generate();
            }
        }

        /// <summary>生成用户的最近 30 首谱面的查分图</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI 的对应 API 的返回值，直接作为请求体<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/bests/result.md
        /// </param>
        /// <param name="imgVersion">图片类型，可选值为 [1, 2]</param>
        /// <returns>很酷的 B30 图，PNG 格式</returns>
        public byte[] GetUserBest30(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserBestsContent>>(jsonStr);
            var backGround = GetBestsGeneratorImage(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }
    }
}