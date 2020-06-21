using System.Collections.Generic;

namespace YourBudget.API.Utils
{
    public class Localizer
    {
        public IEnumerable<string> Languages()
        {
            return new string[]
            {
                "Русский (Russian)"
            };
        }

        public string CurrentLanguage { get; set; }
        public string EmptyFieldMsg { get { return this["Поле не может быть пустым"]; } }
        public string UniqFieldMsg { get { return this["Значение должно быть уникальным"]; } }
        public string ReqestDenied { get { return this["Запрос отклонен"]; } }

        public string this[string text]
        {
            get
            {
                return GetLocalString(text, CurrentLanguage);
            }
        }

        private string GetLocalString(string text, string currentLanguage)
        {
            return text;
        }
    }
}
