using System.Text;

using AdvancedStringBuilder;

using WebMarkupMin.Core.Resources;

namespace TestWebMarkupMinLogging
{
    public class MinimizerLogger : WebMarkupMin.Core.Loggers.LoggerBase
    {
        private readonly ILogger _logger;


        public MinimizerLogger(ILogger<MinimizerLogger> logger)
        {
            _logger = logger;
        }


        private static string GenerateDetails(string category, string message, string filePath = "",
            int lineNumber = 0, int columnNumber = 0, string sourceFragment = "")
        {
            var stringBuilderPool = StringBuilderPool.Shared;
            StringBuilder detailsBuilder = stringBuilderPool.Rent();
            detailsBuilder.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_Category, category);
            detailsBuilder.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_Message, message);

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                detailsBuilder.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_File, filePath);
            }

            if (lineNumber > 0)
            {
                detailsBuilder.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_LineNumber,
                    lineNumber.ToString());
            }

            if (columnNumber > 0)
            {
                detailsBuilder.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_ColumnNumber,
                    columnNumber.ToString());
            }

            if (!string.IsNullOrWhiteSpace(sourceFragment))
            {
                detailsBuilder.AppendFormatLine("{1}:{0}{0}{2}", Environment.NewLine,
                    Strings.ErrorDetails_SourceFragment, sourceFragment);
            }

            string details = detailsBuilder.ToString();
            stringBuilderPool.Return(detailsBuilder);

            return details;
        }

        public override void Error(string category, string message, string filePath = "", int lineNumber = 0,
            int columnNumber = 0, string sourceFragment = "")
        {
            _logger.LogError(GenerateDetails(category, message, filePath, lineNumber, columnNumber, sourceFragment));
        }

        public override void Warn(string category, string message, string filePath = "", int lineNumber = 0,
            int columnNumber = 0, string sourceFragment = "")
        {
            _logger.LogWarning(GenerateDetails(category, message, filePath, lineNumber, columnNumber, sourceFragment));
        }

        public override void Debug(string category, string message, string filePath = "")
        {
            _logger.LogDebug(GenerateDetails(category, message, filePath, 0, 0, string.Empty));
        }
    }
}