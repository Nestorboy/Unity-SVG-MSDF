using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Nessie.MSDF
{
    public static class MsdfUtils
    {
        [PublicAPI]
        public static Texture2D SvgToSdf(string svgLocalPath, int width, int height) => ConvertSvg(svgLocalPath, width, height, GeneratorMode.SDF);

        [PublicAPI]
        public static Texture2D SvgToMsdf(string svgLocalPath, int width, int height) => ConvertSvg(svgLocalPath, width, height, GeneratorMode.MSDF);

        [PublicAPI]
        public static Texture2D ConvertSvg(string svgLocalPath, int width, int height, GeneratorMode mode)
        {
            var tempDirectory = GetTemporaryDirectory();
            EnsurePathExists(tempDirectory);

            var svgAbsolutePath = Path.GetFullPath(svgLocalPath);
            var tempPngPath = GetTemporaryTexturePath();

            RunConversionProcess(svgAbsolutePath, tempPngPath, width, height, mode);

            var sdfTexture = new Texture2D(width, height, GetFormat(mode), TextureCreationFlags.None);
            sdfTexture.LoadImage(File.ReadAllBytes(tempPngPath), false);

            Directory.Delete(tempDirectory, true);

            return sdfTexture;
        }

        private static void RunConversionProcess(string svgAbsolutePath, string pngAbsolutePath, int width, int height, GeneratorMode mode)
        {
            var msdfProcess = new MsdfGenProcess();
            var args = msdfProcess.Arguments;
            args.InputPath = svgAbsolutePath;
            args.OutputPath = pngAbsolutePath;
            args.Mode = mode;
            args.Width = width;
            args.Height = height;

            msdfProcess.StartAndWaitForExit();
        }

        private static string GetTemporaryDirectory() => Path.Combine(Path.GetTempPath(), "NessieSvgMsdf");

        private static string GetTemporaryTexturePath() => Path.Combine(GetTemporaryDirectory(), "result.png");

        private static GraphicsFormat GetFormat(GeneratorMode mode)
        {
            return mode switch
            {
                GeneratorMode.SDF or GeneratorMode.PSDF => GraphicsFormat.R8_UNorm,
                GeneratorMode.MSDF or GeneratorMode.MTSDF => GraphicsFormat.R8G8B8A8_UNorm,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
        }

        private static void EnsurePathExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}