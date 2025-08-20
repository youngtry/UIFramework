using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace CommonTools
{
    public class Utils : MonoBehaviour
    {

        public static string Country = "US";

        public static bool ShowLog = true;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        ///resize layout
        public static void ResizeLayout(GameObject container)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());

        }

        /// <summary>
        /// 删除指定游戏对象的所有子节点
        /// </summary>
        /// <param name="parent">需要清空子节点的父对象</param>
        public static void DeleteAllChildren(Transform parent)
        {
            if (parent == null)
                return;

            // 从后向前删除所有子对象，避免索引变化问题
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 删除指定游戏对象的所有子节点（立即删除版本）
        /// </summary>
        /// <param name="parent">需要清空子节点的父对象</param>
        public static void DeleteAllChildrenImmediate(Transform parent)
        {
            if (parent == null)
                return;

            // 从后向前删除所有子对象，避免索引变化问题
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 将对象转换为字符串表示
        /// </summary>
        private static string ObjectToString(object obj, int depth = 0)
        {
            if (obj == null) return "null";

            // 防止过深递归
            if (depth > 3) return "...";

            // 处理列表类型
            if (obj is System.Collections.IList list)
            {
                if (list.Count == 0) return "[]";

                var items = new List<string>();
                foreach (var item in list)
                {
                    items.Add(ObjectToString(item, depth + 1));
                }
                return "[" + string.Join(", ", items) + "]";
            }

            // 处理字典类型
            if (obj is System.Collections.IDictionary dict)
            {
                if (dict.Count == 0) return "{}";

                var items = new List<string>();
                foreach (System.Collections.DictionaryEntry entry in dict)
                {
                    items.Add($"{ObjectToString(entry.Key, depth + 1)}: {ObjectToString(entry.Value, depth + 1)}");
                }
                return "{" + string.Join(", ", items) + "}";
            }

            // 处理其他类型
            return obj.ToString();
        }

        /// <summary>
        /// 打印日志信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="type">日志类型（Debug/Info/Warning/Error）</param>
        /// <param name="args">格式化参数</param>
        public static void Log(string message, LogType type = LogType.Info, params object[] args)
        {
            if (!ShowLog)
                return;
            // 处理参数中的复杂类型
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = ObjectToString(args[i]);
            }

            string formattedMessage = args.Length > 0 ? string.Format(message, args) : message;

            if (Application.isEditor)
            {
                switch (type)
                {
                    case LogType.Debug:
                        Debug.Log($"[Debug] {formattedMessage}");
                        break;
                    case LogType.Info:
                        Debug.Log($"<color=#00FF00>[Info] {formattedMessage}</color>");
                        break;
                    case LogType.Warning:
                        Debug.Log($"<color=#FFFF00>[Warning] {formattedMessage}</color>");
                        break;
                    case LogType.Error:
                        Debug.LogError($"<color=#FF0000>[Error] {formattedMessage}</color>");
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case LogType.Debug:
                        Debug.Log($"[Debug] {formattedMessage}");
                        break;
                    case LogType.Info:
                        Debug.Log($"[Info] {formattedMessage}");
                        break;
                    case LogType.Warning:
                        Debug.Log($"[Warning] {formattedMessage}");
                        break;
                    case LogType.Error:
                        Debug.LogError($"[Error] {formattedMessage}");
                        break;
                }
            }
        }

        /// <summary>
        /// 日志类型枚举
        /// </summary>
        public enum LogType
        {
            Debug,
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// 将秒数转换为时:分:秒格式，当小时数为0时只显示分:秒
        /// </summary>
        /// <param name="totalSeconds">总秒数</param>
        /// <returns>格式化的时间字符串 (HH:mm:ss 或 mm:ss)</returns>
        public static string FormatTime(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            if (hours > 0)
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
            }
            else
            {
                return string.Format("{0:D2}:{1:D2}", minutes, seconds);
            }
        }

        /// <summary>
        /// 获取当前UTC时间的时间戳（秒）
        /// </summary>
        /// <returns>UTC时间戳（秒）</returns>
        public static long GetUtcTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取当前UTC时间的时间戳（毫秒）
        /// </summary>
        /// <returns>UTC时间戳（毫秒）</returns>
        public static long GetUtcTimestampMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }


        public static string Format(double value)
        {
            if (value < 1000)
                return value.ToString(); // 千位以下直接显示整数

            int unitIndex = 0;
            double divisor = 1;

            while (value / divisor >= 1000 && unitIndex + 1 < Units.Count)
            {
                divisor *= 1000;
                unitIndex++;
            }

            if (unitIndex < Units.Count)
            {
                double scaled = value * 100; // 保留两位小数
                double intResult = scaled / divisor;

                decimal display = (decimal)intResult / 100;
                return display.ToString("0.##", CultureInfo.InvariantCulture) + Units[unitIndex];
            }
            else
            {
                // 超出支持单位，使用科学计数法（保留两位小数）
                double sci = double.Parse(value.ToString(), CultureInfo.InvariantCulture);
                return sci.ToString("0.##e+0", CultureInfo.InvariantCulture);
            }
        }

        private static readonly List<string> Units = new List<string>();




        /// <summary>
        /// 格式化double数字，最多保留2位小数（使用四舍五入）
        /// </summary>
        /// <param name="number">要格式化的数字</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatDouble(double number)
        {
            if (number >= 1000)
            {
                string formatted = Format(number);
                return formatted;
            }

            // 将数字转换为字符串，使用截断方式避免四舍五入
            // double truncated = Math.Truncate(number * 100) / 100; // 截断到2位小数
            string str = number.ToString("0.00", CultureInfo.InvariantCulture);

            // 如果包含小数点
            if (str.Contains("."))
            {
                // 移除末尾的0
                str = str.TrimEnd('0');
                // 如果小数点后没有数字了，移除小数点
                if (str.EndsWith("."))
                {
                    str = str.TrimEnd('.');
                }
            }

            return str;
        }


        /// <summary>
        /// 将数字转换为英语序数词格式（如：1st, 2nd, 3rd, 4th等）
        /// </summary>
        /// <param name="number">要转换的数字</param>
        /// <returns>序数词字符串</returns>
        public static string ToOrdinal(int number)
        {
            if (number <= 0)
                return number.ToString();

            // 获取个位数和十位数
            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            // 特殊情况：11th, 12th, 13th
            if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            {
                return number + "th";
            }

            // 根据个位数确定后缀
            switch (lastDigit)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        /// <summary>
        /// 从Sprite Atlas中获取Sprite
        /// </summary>
        /// <param name="spriteName">Sprite名称</param>
        /// <returns>找到的Sprite，如果没找到返回null</returns>
        public static Sprite GetSpriteFromAtlas(string spriteName, string atlasName)
        {
            if (string.IsNullOrEmpty(atlasName))
            {
                var atlas = Resources.Load<UnityEngine.U2D.SpriteAtlas>(atlasName);
                if (atlas != null)
                {
                    Sprite sprite = atlas.GetSprite(spriteName);
                    if (sprite != null)
                    {
                        return sprite;
                    }
                }
            }
            else
            {
                // 遍历所有Atlas查找
                var atlases = Resources.FindObjectsOfTypeAll<UnityEngine.U2D.SpriteAtlas>();
                foreach (var atlasItem in atlases)
                {
                    Sprite sprite = atlasItem.GetSprite(spriteName);
                    if (sprite != null)
                    {
                        return sprite;
                    }
                }
            }
            return null;
        }



        /// <summary>
        /// 获取指定国家/地区的货币符号
        /// </summary>
        /// <param name="countryCode">国家代码（ISO 3166-1 alpha-2）或货币代码（ISO 4217）</param>
        /// <returns>货币符号，如果未找到则返回默认符号</returns>
        public static string GetCurrencySymbol()
        {
            string countryCode = Country;
            // Utils.Log("countryCode : " + countryCode);
            if (string.IsNullOrEmpty(countryCode))
                return "$"; // 默认返回美元符号

            // 将输入转换为大写以便匹配
            countryCode = countryCode.ToUpper();

            // 货币符号映射表
            var currencyMap = new Dictionary<string, string>
        {
            // 主要国家/地区货币符号
            {"US", "$"},     // 美国 - 美元
            {"USD", "$"},    // 美元
            {"CN", "¥"},     // 中国 - 人民币
            {"CNY", "¥"},    // 人民币
            {"JP", "¥"},     // 日本 - 日元
            {"JPY", "¥"},    // 日元
            {"KR", "₩"},     // 韩国 - 韩元
            {"KRW", "₩"},    // 韩元
            {"GB", "£"},     // 英国 - 英镑
            {"GBP", "£"},    // 英镑
            {"EU", "€"},     // 欧盟 - 欧元
            {"EUR", "€"},    // 欧元
            {"DE", "€"},     // 德国 - 欧元
            {"FR", "€"},     // 法国 - 欧元
            {"IT", "€"},     // 意大利 - 欧元
            {"ES", "€"},     // 西班牙 - 欧元
            {"NL", "€"},     // 荷兰 - 欧元
            {"BE", "€"},     // 比利时 - 欧元
            {"AT", "€"},     // 奥地利 - 欧元
            {"PT", "€"},     // 葡萄牙 - 欧元
            {"FI", "€"},     // 芬兰 - 欧元
            {"IE", "€"},     // 爱尔兰 - 欧元
            {"GR", "€"},     // 希腊 - 欧元
            {"CA", "C$"},    // 加拿大 - 加元
            {"CAD", "C$"},   // 加元
            {"AU", "A$"},    // 澳大利亚 - 澳元
            {"AUD", "A$"},   // 澳元
            {"NZ", "NZ$"},   // 新西兰 - 新西兰元
            {"NZD", "NZ$"},  // 新西兰元
            {"CH", "CHF"},   // 瑞士 - 瑞士法郎
            {"CHF", "CHF"},  // 瑞士法郎
            {"SE", "kr"},    // 瑞典 - 瑞典克朗
            {"SEK", "kr"},   // 瑞典克朗
            {"NO", "kr"},    // 挪威 - 挪威克朗
            {"NOK", "kr"},   // 挪威克朗
            {"DK", "kr"},    // 丹麦 - 丹麦克朗
            {"DKK", "kr"},   // 丹麦克朗
            {"RU", "₽"},     // 俄罗斯 - 卢布
            {"RUB", "₽"},    // 卢布
            {"IN", "₹"},     // 印度 - 印度卢比
            {"INR", "₹"},    // 印度卢比
            {"BR", "R$"},    // 巴西 - 巴西雷亚尔
            {"BRL", "R$"},   // 巴西雷亚尔
            {"MX", "$"},     // 墨西哥 - 墨西哥比索
            {"MXN", "$"},    // 墨西哥比索
            {"AR", "$"},     // 阿根廷 - 阿根廷比索
            {"ARS", "$"},    // 阿根廷比索
            {"CL", "$"},     // 智利 - 智利比索
            {"CLP", "$"},    // 智利比索
            {"CO", "$"},     // 哥伦比亚 - 哥伦比亚比索
            {"COP", "$"},    // 哥伦比亚比索
            {"PE", "S/"},    // 秘鲁 - 秘鲁索尔
            {"PEN", "S/"},   // 秘鲁索尔
            {"ZA", "R"},     // 南非 - 南非兰特
            {"ZAR", "R"},    // 南非兰特
            {"TH", "฿"},     // 泰国 - 泰铢
            {"THB", "฿"},    // 泰铢
            {"SG", "S$"},    // 新加坡 - 新加坡元
            {"SGD", "S$"},   // 新加坡元
            {"MY", "RM"},    // 马来西亚 - 马来西亚林吉特
            {"MYR", "RM"},   // 马来西亚林吉特
            {"ID", "Rp"},    // 印度尼西亚 - 印尼盾
            {"IDR", "Rp"},   // 印尼盾
            {"PH", "₱"},     // 菲律宾 - 菲律宾比索
            {"PHP", "₱"},    // 菲律宾比索
            {"VN", "₫"},     // 越南 - 越南盾
            {"VND", "₫"},    // 越南盾
            {"HK", "HK$"},   // 香港 - 港币
            {"HKD", "HK$"},  // 港币
            {"TW", "NT$"},   // 台湾 - 新台币
            {"TWD", "NT$"},  // 新台币
            {"TR", "₺"},     // 土耳其 - 土耳其里拉
            {"TRY", "₺"},    // 土耳其里拉
            {"IL", "₪"},     // 以色列 - 以色列新谢克尔
            {"ILS", "₪"},    // 以色列新谢克尔
            {"SA", "﷼"},     // 沙特阿拉伯 - 沙特里亚尔
            {"SAR", "﷼"},    // 沙特里亚尔
            {"AE", "د.إ"},   // 阿联酋 - 阿联酋迪拉姆
            {"AED", "د.إ"},  // 阿联酋迪拉姆
            {"EG", "£"},     // 埃及 - 埃及镑
            {"EGP", "£"},    // 埃及镑
            {"NG", "₦"},     // 尼日利亚 - 奈拉
            {"NGN", "₦"},    // 奈拉
            {"KE", "KSh"},   // 肯尼亚 - 肯尼亚先令
            {"KES", "KSh"},  // 肯尼亚先令
            {"GH", "₵"},     // 加纳 - 加纳塞地
            {"GHS", "₵"},    // 加纳塞地
            {"PL", "zł"},    // 波兰 - 波兰兹罗提
            {"PLN", "zł"},   // 波兰兹罗提
            {"CZ", "Kč"},    // 捷克 - 捷克克朗
            {"CZK", "Kč"},   // 捷克克朗
            {"HU", "Ft"},    // 匈牙利 - 匈牙利福林
            {"HUF", "Ft"},   // 匈牙利福林
            {"RO", "lei"},   // 罗马尼亚 - 罗马尼亚列伊
            {"RON", "lei"},  // 罗马尼亚列伊
            {"BG", "лв"},    // 保加利亚 - 保加利亚列弗
            {"BGN", "лв"},   // 保加利亚列弗
            {"HR", "kn"},    // 克罗地亚 - 克罗地亚库纳
            {"HRK", "kn"},   // 克罗地亚库纳
            {"RS", "дин"},   // 塞尔维亚 - 塞尔维亚第纳尔
            {"RSD", "дин"},  // 塞尔维亚第纳尔
            {"UA", "₴"},     // 乌克兰 - 乌克兰格里夫纳
            {"UAH", "₴"},    // 乌克兰格里夫纳
            {"BY", "Br"},    // 白俄罗斯 - 白俄罗斯卢布
            {"BYN", "Br"},   // 白俄罗斯卢布
            {"KZ", "₸"},     // 哈萨克斯坦 - 哈萨克斯坦坚戈
            {"KZT", "₸"},    // 哈萨克斯坦坚戈
            {"UZ", "лв"},    // 乌兹别克斯坦 - 乌兹别克斯坦索姆
            {"UZS", "лв"},   // 乌兹别克斯坦索姆
            {"BD", "৳"},     // 孟加拉国 - 孟加拉塔卡
            {"BDT", "৳"},    // 孟加拉塔卡
            {"PK", "₨"},     // 巴基斯坦 - 巴基斯坦卢比
            {"PKR", "₨"},    // 巴基斯坦卢比
            {"LK", "₨"},     // 斯里兰卡 - 斯里兰卡卢比
            {"LKR", "₨"},    // 斯里兰卡卢比
            {"NP", "₨"},     // 尼泊尔 - 尼泊尔卢比
            {"NPR", "₨"},    // 尼泊尔卢比
            {"MM", "K"},     // 缅甸 - 缅甸元
            {"MMK", "K"},    // 缅甸元
            {"KH", "៛"},     // 柬埔寨 - 柬埔寨瑞尔
            {"KHR", "៛"},    // 柬埔寨瑞尔
            {"LA", "₭"},     // 老挝 - 老挝基普
            {"LAK", "₭"},    // 老挝基普
            {"MN", "₮"},     // 蒙古 - 蒙古图格里克
            {"MNT", "₮"},    // 蒙古图格里克
            {"AM", "֏"},     // 亚美尼亚 - 亚美尼亚德拉姆
            {"AMD", "֏"},    // 亚美尼亚德拉姆
            {"GE", "₾"},     // 格鲁吉亚 - 格鲁吉亚拉里
            {"GEL", "₾"},    // 格鲁吉亚拉里
            {"AZ", "₼"},     // 阿塞拜疆 - 阿塞拜疆马纳特
            {"AZN", "₼"},    // 阿塞拜疆马纳特
            {"IR", "﷼"},     // 伊朗 - 伊朗里亚尔
            {"IRR", "﷼"},    // 伊朗里亚尔
            {"IQ", "ع.د"},   // 伊拉克 - 伊拉克第纳尔
            {"IQD", "ع.د"},  // 伊拉克第纳尔
            {"SY", "£"},     // 叙利亚 - 叙利亚镑
            {"SYP", "£"},    // 叙利亚镑
            {"LB", "£"},     // 黎巴嫩 - 黎巴嫩镑
            {"LBP", "£"},    // 黎巴嫩镑
            {"JO", "د.ا"},   // 约旦 - 约旦第纳尔
            {"JOD", "د.ا"},  // 约旦第纳尔
            {"KW", "د.ك"},   // 科威特 - 科威特第纳尔
            {"KWD", "د.ك"},  // 科威特第纳尔
            {"QA", "﷼"},     // 卡塔尔 - 卡塔尔里亚尔
            {"QAR", "﷼"},    // 卡塔尔里亚尔
            {"BH", "د.ب"},   // 巴林 - 巴林第纳尔
            {"BHD", "د.ب"},  // 巴林第纳尔
            {"OM", "﷼"},     // 阿曼 - 阿曼里亚尔
            {"OMR", "﷼"},    // 阿曼里亚尔
            {"YE", "﷼"},     // 也门 - 也门里亚尔
            {"YER", "﷼"},    // 也门里亚尔
            {"AF", "؋"},     // 阿富汗 - 阿富汗尼
            {"AFN", "؋"},    // 阿富汗尼
            {"ET", "Br"},    // 埃塞俄比亚 - 埃塞俄比亚比尔
            {"ETB", "Br"},   // 埃塞俄比亚比尔
            {"TZ", "TSh"},   // 坦桑尼亚 - 坦桑尼亚先令
            {"TZS", "TSh"},  // 坦桑尼亚先令
            {"UG", "USh"},   // 乌干达 - 乌干达先令
            {"UGX", "USh"},  // 乌干达先令
            {"RW", "₣"},     // 卢旺达 - 卢旺达法郎
            {"RWF", "₣"},    // 卢旺达法郎
            {"BI", "₣"},     // 布隆迪 - 布隆迪法郎
            {"BIF", "₣"},    // 布隆迪法郎
            {"DJ", "₣"},     // 吉布提 - 吉布提法郎
            {"DJF", "₣"},    // 吉布提法郎
            {"SO", "S"},     // 索马里 - 索马里先令
            {"SOS", "S"},    // 索马里先令
            {"MG", "Ar"},    // 马达加斯加 - 马达加斯加阿里亚里
            {"MGA", "Ar"},   // 马达加斯加阿里亚里
            {"MU", "₨"},     // 毛里求斯 - 毛里求斯卢比
            {"MUR", "₨"},    // 毛里求斯卢比
            {"SC", "₨"},     // 塞舌尔 - 塞舌尔卢比
            {"SCR", "₨"},    // 塞舌尔卢比
            {"MV", "Rf"},    // 马尔代夫 - 马尔代夫拉菲亚
            {"MVR", "Rf"},   // 马尔代夫拉菲亚
        };

            // 查找货币符号
            if (currencyMap.TryGetValue(countryCode, out string symbol))
            {
                return symbol;
            }

            // 如果没有找到，尝试使用.NET的CultureInfo获取
            try
            {
                // 尝试作为货币代码处理
                var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                foreach (var culture in cultures)
                {
                    try
                    {
                        var regionInfo = new RegionInfo(culture.Name);
                        if (regionInfo.ISOCurrencySymbol.Equals(countryCode, StringComparison.OrdinalIgnoreCase) ||
                            regionInfo.TwoLetterISORegionName.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                        {
                            return regionInfo.CurrencySymbol;
                        }
                    }
                    catch
                    {
                        // 忽略无效的区域信息
                        continue;
                    }
                }
            }
            catch
            {
                // 如果.NET方法失败，继续使用默认值
            }

            // 如果都没找到，返回默认的美元符号
            return "$";
        }

        /// <summary>
        /// 获取所有支持的货币符号列表
        /// </summary>
        /// <returns>包含国家代码和货币符号的字典</returns>
        public static Dictionary<string, string> GetAllCurrencySymbols()
        {
            return new Dictionary<string, string>
        {
            {"US", "$"},     // 美国 - 美元
            {"CN", "¥"},     // 中国 - 人民币
            {"JP", "¥"},     // 日本 - 日元
            {"KR", "₩"},     // 韩国 - 韩元
            {"GB", "£"},     // 英国 - 英镑
            {"EU", "€"},     // 欧盟 - 欧元
            {"CA", "C$"},    // 加拿大 - 加元
            {"AU", "A$"},    // 澳大利亚 - 澳元
            {"NZ", "NZ$"},   // 新西兰 - 新西兰元
            {"CH", "CHF"},   // 瑞士 - 瑞士法郎
            {"SE", "kr"},    // 瑞典 - 瑞典克朗
            {"NO", "kr"},    // 挪威 - 挪威克朗
            {"DK", "kr"},    // 丹麦 - 丹麦克朗
            {"RU", "₽"},     // 俄罗斯 - 卢布
            {"IN", "₹"},     // 印度 - 印度卢比
            {"BR", "R$"},    // 巴西 - 巴西雷亚尔
            {"MX", "$"},     // 墨西哥 - 墨西哥比索
            {"TH", "฿"},     // 泰国 - 泰铢
            {"SG", "S$"},    // 新加坡 - 新加坡元
            {"MY", "RM"},    // 马来西亚 - 马来西亚林吉特
            {"ID", "Rp"},    // 印度尼西亚 - 印尼盾
            {"PH", "₱"},     // 菲律宾 - 菲律宾比索
            {"VN", "₫"},     // 越南 - 越南盾
            {"HK", "HK$"},   // 香港 - 港币
            {"TW", "NT$"},   // 台湾 - 新台币
            {"TR", "₺"},     // 土耳其 - 土耳其里拉
            {"IL", "₪"},     // 以色列 - 以色列新谢克尔
            {"SA", "﷼"},     // 沙特阿拉伯 - 沙特里亚尔
            {"AE", "د.إ"},   // 阿联酋 - 阿联酋迪拉姆
            {"EG", "£"},     // 埃及 - 埃及镑
            {"ZA", "R"},     // 南非 - 南非兰特
            {"NG", "₦"},     // 尼日利亚 - 奈拉
            {"PL", "zł"},    // 波兰 - 波兰兹罗提
            {"CZ", "Kč"},    // 捷克 - 捷克克朗
            {"HU", "Ft"},    // 匈牙利 - 匈牙利福林
            {"UA", "₴"},     // 乌克兰 - 乌克兰格里夫纳
            {"BD", "৳"},     // 孟加拉国 - 孟加拉塔卡
            {"PK", "₨"},     // 巴基斯坦 - 巴基斯坦卢比
            {"IR", "﷼"},     // 伊朗 - 伊朗里亚尔
            {"KW", "د.ك"},   // 科威特 - 科威特第纳尔
            {"QA", "﷼"},     // 卡塔尔 - 卡塔尔里亚尔
            {"AF", "؋"},     // 阿富汗 - 阿富汗尼
            {"ET", "Br"},    // 埃塞俄比亚 - 埃塞俄比亚比尔
            {"KE", "KSh"},   // 肯尼亚 - 肯尼亚先令
            {"GH", "₵"},     // 加纳 - 加纳塞地
            {"MN", "₮"},     // 蒙古 - 蒙古图格里克
            {"AM", "֏"},     // 亚美尼亚 - 亚美尼亚德拉姆
            {"GE", "₾"},     // 格鲁吉亚 - 格鲁吉亚拉里
            {"AZ", "₼"},     // 阿塞拜疆 - 阿塞拜疆马纳特
        };
        }


    }
}


