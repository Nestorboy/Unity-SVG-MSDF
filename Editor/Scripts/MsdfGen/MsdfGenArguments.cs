using System.Text;

namespace Nessie.MSDF
{
    /// <summary>
    /// Wrapper for msdfgen CLI arguments.
    /// </summary>
    public class MsdfGenArguments
    {
        /// <summary>
        /// Absolute input of the input SVG.
        /// </summary>
        public string InputPath;

        /// <summary>
        /// Absolute path of the output PNG.
        /// </summary>
        public string OutputPath;

        public GeneratorMode Mode = GeneratorMode.MSDF;
        public int Width = 512;
        public int Height = 512;
        public float Range = 4;

        /// <summary>
        /// Combines arguments into formatted command.
        /// </summary>
        /// <returns>Formatted CLI command.</returns>
        public string ToCommandString()
        {
            StringBuilder args = new();

            string genMode = Mode.ToString().ToLowerInvariant();
            args.Append($"\"{genMode}\" ");
            args.Append($"-svg \"{InputPath}\" ");
            args.Append($"-o \"{OutputPath}\" ");
            args.Append($"-dimensions {Width} {Height} ");
            args.Append($"-range {Range} ");
            args.Append("-format png ");
            args.Append("-autoframe ");

            return args.ToString();
        }
    }
}