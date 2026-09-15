using System.IO;
using UnityEditor;

namespace Nessie.MSDF
{
    public static class MSDFContextMenu
    {
        private const string SVG_EXTENSION = ".svg";
        private const string MSDF_EXTENSION = ".msdf";
        private const string CONVERT_TO_MSDF_PATH = "Assets/MSDF/Convert to MSDF";

        [MenuItem(CONVERT_TO_MSDF_PATH, true)]
        private static bool ValidateConvertToMsdf() => Selection.activeObject != null && IsSvgAssetPath(GetSelectedAssetPath());

        [MenuItem(CONVERT_TO_MSDF_PATH)]
        private static void ConvertToMsdf()
        {
            string svgPath = GetSelectedAssetPath();

            if (string.IsNullOrEmpty(svgPath) || !IsSvgAssetPath(svgPath))
            {
                return;
            }

            string msdfPath = Path.ChangeExtension(svgPath, MSDF_EXTENSION);

            if (File.Exists(msdfPath))
            {
                if (!EditorUtility.DisplayDialog(
                    "MSDF File Already Exists",
                    $"A file already exists at:\n\n{msdfPath}\n\nOverwrite it?",
                    "Overwrite",
                    "Cancel"))
                {
                    return;
                }
            }

            File.Copy(svgPath, msdfPath, true);

            AssetDatabase.ImportAsset(msdfPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }

        private static string GetSelectedAssetPath() => AssetDatabase.GetAssetPath(Selection.activeObject);

        private static bool IsSvgAssetPath(string path) => path.EndsWith(SVG_EXTENSION, System.StringComparison.OrdinalIgnoreCase);
    }
}