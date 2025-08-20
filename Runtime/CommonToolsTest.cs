using UnityEngine;

namespace CommonTools
{
    /// <summary>
    /// Simple test class to verify the package is working
    /// </summary>
    public static class CommonToolsTest
    {
        /// <summary>
        /// Test method to verify the package is loaded correctly
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void TestPackage()
        {
            if (Utils.ShowLog)
            {
                Utils.Log("CommonTools package loaded successfully!", Utils.LogType.Info);
            }
        }
        
        /// <summary>
        /// Quick test of basic functionality
        /// </summary>
        public static bool RunBasicTests()
        {
            try
            {
                // Test time formatting
                string time = Utils.FormatTime(3661);
                if (time != "01:01:01") return false;
                
                // Test number formatting
                string number = Utils.FormatDouble(123.456);
                if (string.IsNullOrEmpty(number)) return false;
                
                // Test ordinal conversion
                string ordinal = Utils.ToOrdinal(1);
                if (ordinal != "1st") return false;
                
                // Test currency symbol
                string currency = Utils.GetCurrencySymbol();
                if (string.IsNullOrEmpty(currency)) return false;
                
                Utils.Log("All basic tests passed!", Utils.LogType.Info);
                return true;
            }
            catch (System.Exception ex)
            {
                Utils.Log("Test failed: " + ex.Message, Utils.LogType.Error);
                return false;
            }
        }
    }
}
