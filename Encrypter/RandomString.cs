using System;
using System.Text;
using System.Threading.Tasks;

namespace Encrypter {
    [Flags]
    public enum StringType {
        UpperAlpha = 0b0000_0001,
        LowerAlpha = 0b0000_0010,
        Number = 0b0000_0100
    };

    public class RandomString {
        private static char[] _upperLetters;
        private static char[] _lowerLetters;
        private static char[] _numbers;

        static RandomString() {
            _upperLetters = new char[26] {
                'A','B','C','D','E','F','G',
                'H','I','J','K','L','M','N',
                'O','P','Q','R','S','T','U',
                'V','W','X','Y','Z'
            };
            _lowerLetters = new char[26] {
                'a','b','c','d','e','f','g',
                'h','i','j','k','l','m','n',
                'o','p','q','r','s','t','u',
                'v','w','x','y','z'
            };
            _numbers = new char[10] {
                '0','1','2','3','4','5','6','7','8','9'
            };
        }

        public static string GetString(int length, StringType stringType) {
            StringBuilder sb = new StringBuilder(length);
            Random rnd = new Random();

            while (sb.Length < length) {
                switch (rnd.Next(0, 3)) {
                    case 0:
                        if ((stringType & StringType.UpperAlpha) != StringType.UpperAlpha) {
                            continue;
                        }
                        sb.Append(_upperLetters[rnd.Next(0, 26)]);
                        break;
                    case 1:
                        if ((stringType & StringType.LowerAlpha) != StringType.LowerAlpha) {
                            continue;
                        }
                        sb.Append(_lowerLetters[rnd.Next(0, 26)]);
                        break;
                    case 2:
                        if ((stringType & StringType.Number) != StringType.Number) {
                            continue;
                        }
                        sb.Append(_numbers[rnd.Next(0, 10)]);
                        break;
                }
            }

            return sb.ToString();
        }

        public async static Task<string> GetStringAsync(int length, StringType stringType) {
            return await Task.Run(() => {
                return GetString(length, stringType);
            });
        }
    }
}
