using UnityEngine;
using UnityEngine.UI;
using UIFramework;

namespace UIFramework.Examples
{
    /// <summary>
    /// 示例设置页面
    /// </summary>
    public class ExampleSettingsPage : Page
    {
        [Header("Settings Page Elements")]
        [SerializeField] private Text titleText;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            // 绑定按钮事件
            if (backButton != null)
                backButton.onClick.AddListener(() => UIManager.Instance.ShowPage<ExampleMainPage>());

            if (saveButton != null)
                saveButton.onClick.AddListener(SaveSettings);

            // 绑定UI事件
            if (volumeSlider != null)
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

            if (soundToggle != null)
                soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }

        protected override void OnAfterShow(params object[] data)
        {
            base.OnAfterShow(data);

            // 更新标题
            if (titleText != null)
            {
                titleText.text = "设置页面";
            }

            // 加载设置
            LoadSettings();

            Debug.Log($"SettingsPage shown with {data?.Length ?? 0} parameters");
        }

        private void LoadSettings()
        {
            // 加载音量设置
            if (volumeSlider != null)
            {
                volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);
            }

            // 加载声音开关设置
            if (soundToggle != null)
            {
                soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
            }
        }

        private void SaveSettings()
        {
            // 保存音量设置
            if (volumeSlider != null)
            {
                PlayerPrefs.SetFloat("Volume", volumeSlider.value);
            }

            // 保存声音开关设置
            if (soundToggle != null)
            {
                PlayerPrefs.SetInt("SoundEnabled", soundToggle.isOn ? 1 : 0);
            }

            PlayerPrefs.Save();
            Debug.Log("Settings saved!");
        }

        private void OnVolumeChanged(float value)
        {
            // 实时调整音量
            AudioListener.volume = value;
        }

        private void OnSoundToggleChanged(bool enabled)
        {
            // 实时调整声音开关
            AudioListener.pause = !enabled;
        }
    }
}
