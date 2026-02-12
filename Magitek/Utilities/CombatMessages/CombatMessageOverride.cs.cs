using System;

namespace Magitek.Utilities.CombatMessages
{
    internal static class CombatMessageOverride
    {
        private static string _message;
        private static string _image;
        private static long _untilMs;

        public static bool Active => Environment.TickCount64 < _untilMs;

        public static string Message => _message;
        public static string ImageSource => _image;

        public static void Set(string message, string imageSource, int durationMs = 900)
        {
            _message = message;
            _image = imageSource;
            _untilMs = Environment.TickCount64 + durationMs;
        }

        public static void Clear()
        {
            _message = null;
            _image = null;
            _untilMs = 0;
        }
    }
}