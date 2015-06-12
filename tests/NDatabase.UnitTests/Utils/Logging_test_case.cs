using NDatabase.Api;
using NDatabase.Tool;
using NUnit.Framework;

namespace NDatabase.UnitTests.Utils
{
    internal class FakeLogger : ILogger
    {
        private string _debugMessage;
        private string _errorMessage;
        private string _infoMessage;
        private string _warningMessage;

        public void Warning(string message)
        {
            _warningMessage = message;
        }

        public void Debug(string message)
        {
            _debugMessage = message;
        }

        public void Info(string message)
        {
            _infoMessage = message;
        }

        public void Error(string message)
        {
            _errorMessage = message;
        }

        public string GetWarningMessage()
        {
            return _warningMessage;
        }

        public string GetInfoMessage()
        {
            return _infoMessage;
        }

        public string GetDebugMessage()
        {
            return _debugMessage;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }
    }

    public class Logging_test_case
    {
        [Test]
        public void Register_logger_and_check_logged_messages()
        {
            var logger = new FakeLogger();
            OdbConfiguration.RegisterLogger(logger);

            DLogger.Info("info");
            DLogger.Debug("debug");
            DLogger.Warning("warning");
            DLogger.Error("error");

            Assert.That(logger.GetInfoMessage(), Is.EqualTo("info"));
            Assert.That(logger.GetDebugMessage(), Is.EqualTo("debug"));
            Assert.That(logger.GetWarningMessage(), Is.EqualTo("warning"));
            Assert.That(logger.GetErrorMessage(), Is.EqualTo("error"));
        }
    }
}