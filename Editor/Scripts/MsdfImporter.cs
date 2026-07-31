using System;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Nessie.MSDF
{
    [ScriptedImporter(1, "msdf")]
    public class MsdfImporter : ScriptedImporter
    {
        [SerializeField] private int width = 32, height = 32;

        [SerializeField] private TextureType textureType = TextureType.Texture2D;
        [SerializeField] private GeneratorMode generatorMode = GeneratorMode.MSDF;
        [SerializeField] private TextureWrapMode wrapMode = TextureWrapMode.Repeat;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);
            switch (textureType)
            {
                case TextureType.Texture2D:
                    GenerateTextureMsdfAsset(ctx, assetName);
                    break;
                case TextureType.Sprite:
                    GenerateSpriteMsdfAsset(ctx, assetName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GenerateTextureMsdfAsset(AssetImportContext ctx, string assetName)
        {
            var tex = BuildMsdf(ctx, assetName);

            ctx.AddObjectToAsset("tex", tex);
            ctx.SetMainObject(tex);
        }

        private void GenerateSpriteMsdfAsset(AssetImportContext ctx, string assetName)
        {
            var tex = BuildMsdf(ctx, assetName);
            var sprite = TextureToSprite(tex);

            ctx.AddObjectToAsset("sprite", sprite);
            ctx.AddObjectToAsset("tex", tex);
            ctx.SetMainObject(sprite);
        }

        private Texture2D BuildMsdf(AssetImportContext ctx, string assetName)
        {
            var tex = MsdfUtils.ConvertSvg(ctx.assetPath, width, height, generatorMode);
            tex.hideFlags = HideFlags.None;
            tex.name = assetName;
            tex.wrapMode = wrapMode;

            return tex;
        }

        private static Sprite TextureToSprite(Texture2D tex)
        {
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            sprite.hideFlags = HideFlags.None;
            sprite.name = tex.name;

            return sprite;
        }
    }
}