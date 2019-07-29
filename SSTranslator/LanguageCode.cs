using System.Collections.Generic;
using System.Linq;

namespace SSTranslator
{
    public class LanguageCode
    {
        public string Name { get; }
        public string TranslateCode { get; } // https://cloud.google.com/translate/docs/languages
        public string SpeechCode { get; } // https://cloud.google.com/text-to-speech/docs/voices
        public string SpeechName { get; }

        private static readonly Dictionary<string, LanguageCode> Languages = new Dictionary<string, LanguageCode>();

        public static LanguageCode English = new LanguageCode("English", "en", "en-US", "en-US-Wavenet-C");
        public static LanguageCode Japanese = new LanguageCode("Japanese", "ja", "ja-JP", "ja-JP-Wavenet-A");
        public static LanguageCode Korean = new LanguageCode("Korean", "ko", "ko-Kr", "ko-KR-Wavenet-A");

        static LanguageCode()
        {
            Languages.Add(English.Name, English);
            Languages.Add(Japanese.Name, Japanese);
            Languages.Add(Korean.Name, Korean);
        }

        public LanguageCode(string name, string translateCode, string speechCode, string speechName)
        {
            Name = name;
            TranslateCode = translateCode;
            SpeechCode = speechCode;
            SpeechName = speechName;
        }

        public static List<string> GetLanguageList()
        {
            return Languages.Keys.ToList();
        }

        public static LanguageCode GetLanguageCode(string language)
        {
            if (Languages.ContainsKey(language))
            {
                return Languages[language];
            }

            return English;
        }
    }
}