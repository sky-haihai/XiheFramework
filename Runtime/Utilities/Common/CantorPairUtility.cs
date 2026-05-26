using System;

namespace XiheFramework.Runtime.Utility {
    public static class CantorPairUtility {
        public static int CantorPair(int x, int y) {
            return (x + y) * (x + y + 1) / 2 + y;
        }

        public static void ReverseCantorPair(int cantor, out int x, out int y) {
            var t = (int)Math.Floor((-1 + Math.Sqrt(1 + 8 * cantor)) / 2);
            x = t * (t + 3) / 2 - cantor;
            y = cantor - t * (t + 1) / 2;
        }

        public static int SignedCantorPair(int x, int y) {
            x = x >= 0 ? 2 * x : x * -2 + 1;
            y = y >= 0 ? 2 * y : y * -2 + 1;

            return (x + y) * (x + y + 1) / 2 + y;
        }

        public static void SignedReverseCantorPair(int cantor, out int x, out int y) {
            var t = (int)Math.Floor((-1 + Math.Sqrt(1 + 8 * cantor)) / 2);
            x = t * (t + 3) / 2 - cantor;
            y = cantor - t * (t + 1) / 2;

            x = x % 2 == 0 ? x / 2 : (1 - x) / 2;
            y = y % 2 == 0 ? y / 2 : (1 - y) / 2;
        }
    }
}