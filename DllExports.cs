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
        /// <summary>���У���������������ĺò���</summary>
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

        /// <summary>�����û������������ʽ�Ĳ��ͼ</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI �Ķ�Ӧ API �ķ���ֵ��ֱ����Ϊ������<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/info.md
        /// </param>
        /// <param name="imgVersion">ͼƬ���ͣ���ѡֵΪ [1, 2, 3]</param>
        /// <returns>�ܿ�� info ͼ��PNG ��ʽ</returns>
        public byte[] GetUserInfo(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserInfoContent>>(jsonStr);
            var backGround = RecordGenerator.Generate(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }

        /// <summary>�����û�����߷�����ʽ�Ĳ��ͼ</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI �Ķ�Ӧ API �ķ���ֵ��ֱ����Ϊ������<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/best.md
        /// </param>
        /// <param name="imgVersion">ͼƬ���ͣ���ѡֵΪ [1, 2, 3]</param>
        /// <returns>�ܿ�� best ͼ��PNG ��ʽ</returns>
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

        /// <summary>�����û������ 30 ������Ĳ��ͼ</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI �Ķ�Ӧ API �ķ���ֵ��ֱ����Ϊ������<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/bests/result.md
        /// </param>
        /// <param name="imgVersion">ͼƬ���ͣ���ѡֵΪ [1, 2]</param>
        /// <returns>�ܿ�� B30 ͼ��PNG ��ʽ</returns>
        public byte[] GetUserBest30(string jsonStr, int imgVersion)
        {
            var json = JsonConvert.DeserializeObject<ResponseRoot<UserBestsContent>>(jsonStr);
            var backGround = GetBestsGeneratorImage(json.Content, (ImgVersion)imgVersion).Result;

            return FileStreamResult(backGround).ToArray();
        }
    }
}