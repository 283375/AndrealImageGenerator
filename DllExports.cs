using System.Runtime.InteropServices;

namespace AndrealImageGenerator
{
    [ComVisible(true)]
    public static class ImageGeneratorDll
    {
        /// <summary>拜托，计算器诶，超酷的好不好</summary>
        /// <param name="a">int a</param>
        /// <param name="b">int b</param>
        /// <returns>a + b</returns>
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static byte[] GetUserInfo(string jsonStr, int imgVersion)
        {
            return Api.Api.GetUserInfo(jsonStr, imgVersion);
        }

        public static byte[] GetUserBest(string jsonStr, int imgVersion)
        {
            return Api.Api.GetUserBest(jsonStr, imgVersion);
        }

        public static byte[] GetUserBest30(string jsonStr, int imgVersion)
        {
            return Api.Api.GetUserBest30(jsonStr, imgVersion);
        }
    }
}