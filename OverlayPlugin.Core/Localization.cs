using System.Collections.Generic;

namespace RainbowMage.OverlayPlugin
{
    public enum TextItem
    {
        ErrorTitle,
        RequiredAssemblyFileNotFound,
        RequiredAssemblyFileCannotRead,
        RequiredAssemblyFileBlocked,
        RequiredAssemblyFileException,
        DoNotSort,
        SortStringAscending,
        SortStringDescending,
        SortNumberAscending,
        SortNumberDescending,
        ToggleVisible,
        ToggleClickthru,
        ToggleLock
    }

    public static class Localization
    {
        private static LocalizationDict dict;

        static Localization()
        {
            dict = new LocalizationDict();

            dict[TextItem.ErrorTitle, ""] = "Error";
            dict[TextItem.ErrorTitle, "ja"] = "エラー";
            dict[TextItem.ErrorTitle, "ko"] = "에러";
            dict[TextItem.RequiredAssemblyFileNotFound, ""] = "Required assembly file {0} was not found.";
            dict[TextItem.RequiredAssemblyFileNotFound, "ja"] = "アセンブリ {0} が存在しません。";
            dict[TextItem.RequiredAssemblyFileNotFound, "ko"] = "필요한 어셈블리 파일 {0}을(를) 찾을 수 없습니다.";
            dict[TextItem.RequiredAssemblyFileCannotRead, ""] = "Could not load required assembly file {0}.";
            dict[TextItem.RequiredAssemblyFileCannotRead, "ja"] = "アセンブリ {0} は存在しますが、読み込めません。";
            dict[TextItem.RequiredAssemblyFileCannotRead, "ko"] = "필요한 어셈블리 파일 {0}을(를) 불러올 수 없습니다.";
            dict[TextItem.RequiredAssemblyFileBlocked, ""] = "Could not load required assembly file {0} due to security reasons. It seems the file has blocked or placed on the untrusted zone (such as network drive).";
            dict[TextItem.RequiredAssemblyFileBlocked, "ja"] = "セキュリティ上の問題からアセンブリ {0} を読み込めません。アセンブリがネットワーク上にあるか、またはブロックされている可能性があります。";
            dict[TextItem.RequiredAssemblyFileBlocked, "ko"] = "보안 문제로 필요한 어셈블리 파일 {0}을(를) 불러올 수 없습니다. 파일이 차단되어 있거나, 신뢰할 수 없는 장소에 (네트워크 드라이브 등) 놓여져있을 수 있습니다.";
            dict[TextItem.RequiredAssemblyFileException, ""] = "Exception occured when loading required assembly file {0}:\n{1}";
            dict[TextItem.RequiredAssemblyFileException, "ja"] = "アセンブリ {0}の読み込み時に例外が発生しました:\n{1}";
            dict[TextItem.RequiredAssemblyFileException, "ko"] = "어셈블리 {0}을(를) 불러오는 도중 예외가 발생했습니다:\n{1}";

            dict[TextItem.DoNotSort, ""] = "Do not sort";
            dict[TextItem.DoNotSort, "ja"] = "ソートしない";
            dict[TextItem.DoNotSort, "ko"] = "정렬하지 않음";
            dict[TextItem.SortStringAscending, ""] = "String - Ascending";
            dict[TextItem.SortStringAscending, "ja"] = "文字列 - 昇順";
            dict[TextItem.SortStringAscending, "ko"] = "문자열 - 오름차순";
            dict[TextItem.SortStringDescending, ""] = "String - Descending";
            dict[TextItem.SortStringDescending, "ja"] = "文字列 - 降順";
            dict[TextItem.SortStringDescending, "ko"] = "문자열 - 내림차순";
            dict[TextItem.SortNumberAscending, ""] = "Number - Ascending";
            dict[TextItem.SortNumberAscending, "ja"] = "数値 - 昇順";
            dict[TextItem.SortNumberAscending, "ko"] = "숫자 - 오름차순";
            dict[TextItem.SortNumberDescending, ""] = "Number - Descending";
            dict[TextItem.SortNumberDescending, "ja"] = "数値 - 降順";
            dict[TextItem.SortNumberDescending, "ko"] = "숫자 - 내림차순";

            dict[TextItem.ToggleVisible, ""] = "Toggle visible/hide";
            dict[TextItem.ToggleVisible, "ja"] = "表示/非表示の切り替え";
            dict[TextItem.ToggleVisible, "ko"] = "표시 / 숨기기 토글";
            dict[TextItem.ToggleClickthru, ""] = "Toggle clickthru";
            dict[TextItem.ToggleClickthru, "ja"] = "クリック透過の切り替え";
            dict[TextItem.ToggleClickthru, "ko"] = "Toggle clickthru";
            dict[TextItem.ToggleLock, ""] = "Toggle lock";
            dict[TextItem.ToggleLock, "ja"] = "移動/リサイズ制限の切り替え";
            dict[TextItem.ToggleLock, "ko"] = "Toggle lock";
        }

        public static string GetText(TextItem item)
        {
            return dict[item, GetCurrentLocale()];
        }

        private static string GetCurrentLocale()
        {
            var culture = System.Globalization.CultureInfo.CurrentUICulture;
            return culture.TwoLetterISOLanguageName.ToLower();
        }
    }

    class LocalizationDict
    {
        IDictionary<TextItem, IDictionary<string, string>> dict;

        public LocalizationDict()
        {
            this.dict = new Dictionary<TextItem, IDictionary<string, string>>();
        }

        public string this[TextItem item, string locale]
        {
            get
            {
                if (dict.ContainsKey(item))
                {
                    if (dict[item].ContainsKey(locale))
                    {
                        return dict[item][locale];
                    }
                    else if (dict[item].ContainsKey(""))
                    {
                        return dict[item][""];
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, new Dictionary<string, string>());
                }
                if (!dict[item].ContainsKey(locale))
                {
                    dict[item].Add(locale, value);
                }
                else
                {
                    dict[item][locale] = value;
                }
            }
        }
    }
}