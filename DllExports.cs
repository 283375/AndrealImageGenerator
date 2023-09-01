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

        public byte[] GetUserInfo(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserInfoContent>>(jsonStr);
            var backGround = RecordGenerator.Generate(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }

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

        public byte[] GetUserBest30(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserBestsContent>>(jsonStr);
            var backGround = GetBestsGeneratorImage(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }
    }
}