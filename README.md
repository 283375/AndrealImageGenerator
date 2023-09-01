# Andreal ImageGenerator

为 Bot 开发者提供的 Andreal 查分图生成器

## 感谢

> 本项目的图片 UI 设计来源于 GNAQ、linwenxuan04、雨笙Fracture (按首字母排序)。

## 用户须知

> 您应知悉，使用本项目将违反 [Arcaea 使用条款](https://arcaea.lowiro.com/zh/terms_of_service) 中的 3.2-4 和 3.2-6，以及 [Arcaea 二次创作管理条例](https://arcaea.lowiro.com/zh/derivative_policy) 。
>
> 因使用本项目而造成的损失，Andreal 开发组不予承担任何责任。

## Forked Version

本项目 fork 自 [Awbugl/AndrealImageGenerator](https://github.com/Awbugl/AndrealImageGenerator)，将本地 API 服务转为 DLL 导出。

## 对外 API

> 可在 [DllExports.cs](./DllExports.cs) 下查看详情。

```cs
namespace AndrealImageGenerator
{
    [ComVisible(true)]
    public class ImageGeneratorDll
    {
        /// <summary>拜托，计算器诶，超酷的好不好</summary>
        /// <param name="a">int a</param>
        /// <param name="b">int b</param>
        /// <returns>a + b</returns>
        public int Add(int a, int b) {}

        /// <summary>生成用户的最近分数样式的查分图</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI 的对应 API 的返回值，直接作为请求体<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/info.md
        /// </param>
        /// <param name="imgVersion">图片类型，可选值为 [1, 2, 3]</param>
        /// <returns>很酷的 info 图，PNG 格式</returns>
        public byte[] GetUserInfo(string jsonStr, int imgVersion) {}

        /// <summary>生成用户的最高分数样式的查分图</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI 的对应 API 的返回值，直接作为请求体<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/best.md
        /// </param>
        /// <param name="imgVersion">图片类型，可选值为 [1, 2, 3]</param>
        /// <returns>很酷的 best 图，PNG 格式</returns>
        public byte[] GetUserBest(string jsonStr, int imgVersion) {}

        /// <summary>生成用户的最近 30 首谱面的查分图</summary>
        /// <param name="jsonStr">
        ///   UnofficialArcaeaAPI 的对应 API 的返回值，直接作为请求体<br></br>
        ///   https://github.com/Arcaea-Infinity/UnofficialArcaeaAPI.Docs/blob/main/user/bests/result.md
        /// </param>
        /// <param name="imgVersion">图片类型，可选值为 [1, 2]</param>
        /// <returns>很酷的 B30 图，PNG 格式</returns>
        public byte[] GetUserBest30(string jsonStr, int imgVersion) {}
    }
}
```

## 本地部署

### 准备工作

[前往原仓库，下载 release](https://github.com/Awbugl/AndrealImageGenerator/releases/)，或者直接在[本仓库](https://github.com/283375/AndrealImageGenerator/releases/)获取，如果有的话。

下载后，提取 `Andreal` 文件夹（内含 `Config`、`Fonts`、`Sources` 文件夹）备用

### 神秘资源

在 `Config` 文件夹下放置 `arcsong.json`，在 `Andreal` 文件夹下创建 `Arcaea` 文件夹，并创建 `Char`、`Icon` 和 `Song` 三个子文件夹。也就是：

- `/Andreal/Config/arcsong.json`
- `/Andreal/Arcaea/Char/`
- `/Andreal/Arcaea/Icon/`
- `/Andreal/Arcaea/Song/`

新建了这些文件。很明显，`Arcaea` 文件夹需要填充内容，恕本文不做讲解。感兴趣的可以看看 [283375/zip-extract-rename-util](https://github.com/283375/zip-extract-rename-util)，也许能节约你宝贵的时间。

### 编译 & 调用

使用 VS 打开本项目文件夹，在 `生成` > `发布` 内指定发布文件夹，调整 `目标运行时` 为你所需要部署的平台，点击 `发布` 即可获取 DLL。

调用方法不做讲解，确保 `Andreal` 文件夹置于正确位置即可。

## 画饼

也许会写一个可执行程序，最终实现类似

```sh
$ andreal-image-generator --type user-info --json ... --imgVersion 2
data:image/png;base64,iVBORw0KGgoAAAA...
```

的效果。
