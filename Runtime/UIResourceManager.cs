using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// UI资源管理器 - 允许用户自定义UI中使用的图片和资源
    /// </summary>
    [CreateAssetMenu(fileName = "UIResourceManager", menuName = "UI Framework/UI Resource Manager", order = 2)]
    public class UIResourceManager : ScriptableObject
    {
        [Header("Collection Animation Icons")]
        [Tooltip("金币图标")]
        public Sprite goldIcon;
        [Tooltip("钻石图标")]
        public Sprite diamondIcon;
        [Tooltip("金钱图标")]
        public Sprite moneyIcon;

        [Header("UI Background Images")]
        [Tooltip("默认背景图")]
        public Sprite defaultBackground;
        [Tooltip("弹窗背景图")]
        public Sprite popupBackground;
        [Tooltip("按钮背景图")]
        public Sprite buttonBackground;

        [Header("UI Icons")]
        [Tooltip("确认图标")]
        public Sprite confirmIcon;
        [Tooltip("取消图标")]
        public Sprite cancelIcon;
        [Tooltip("关闭图标")]
        public Sprite closeIcon;
        [Tooltip("警告图标")]
        public Sprite warningIcon;
        [Tooltip("错误图标")]
        public Sprite errorIcon;
        [Tooltip("成功图标")]
        public Sprite successIcon;
        [Tooltip("信息图标")]
        public Sprite infoIcon;

        [Header("Animation Prefabs")]
        [Tooltip("文本动画预制体")]
        public GameObject addTextAnimationPrefab;
        [Tooltip("提示框预制体")]
        public GameObject tipPrefab;
        [Tooltip("金币收集动画预制体")]
        public GameObject goldCollectionPrefab;
        [Tooltip("钻石收集动画预制体")]
        public GameObject diamondCollectionPrefab;
        [Tooltip("金钱收集动画预制体")]
        public GameObject moneyCollectionPrefab;

        [Header("Audio Clips")]
        [Tooltip("收集音效")]
        public AudioClip collectSound;
        [Tooltip("按钮点击音效")]
        public AudioClip buttonClickSound;
        [Tooltip("成功音效")]
        public AudioClip successSound;
        [Tooltip("错误音效")]
        public AudioClip errorSound;

        private static UIResourceManager _instance;

        /// <summary>
        /// 获取当前资源管理器实例
        /// </summary>
        public static UIResourceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<UIResourceManager>("UIResourceManager");
                    if (_instance == null)
                    {
                        Debug.LogWarning("UIResourceManager not found in Resources folder. Creating default instance.");
                        _instance = CreateDefaultInstance();
                    }
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        /// <summary>
        /// 创建默认实例
        /// </summary>
        private static UIResourceManager CreateDefaultInstance()
        {
            var instance = CreateInstance<UIResourceManager>();
            instance.name = "Default UIResourceManager";
            return instance;
        }

        /// <summary>
        /// 获取金币图标
        /// </summary>
        public Sprite GetGoldIcon()
        {
            return goldIcon;
        }

        /// <summary>
        /// 获取钻石图标
        /// </summary>
        public Sprite GetDiamondIcon()
        {
            return diamondIcon;
        }

        /// <summary>
        /// 获取金钱图标
        /// </summary>
        public Sprite GetMoneyIcon()
        {
            return moneyIcon;
        }

        /// <summary>
        /// 获取收集动画预制体
        /// </summary>
        public GameObject GetCollectionPrefab(CollectionType type)
        {
            switch (type)
            {
                case CollectionType.Gold:
                    return goldCollectionPrefab;
                case CollectionType.Diamond:
                    return diamondCollectionPrefab;
                case CollectionType.Money:
                    return moneyCollectionPrefab;
                default:
                    return goldCollectionPrefab;
            }
        }

        /// <summary>
        /// 获取图标
        /// </summary>
        public Sprite GetIcon(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.Confirm:
                    return confirmIcon;
                case IconType.Cancel:
                    return cancelIcon;
                case IconType.Close:
                    return closeIcon;
                case IconType.Warning:
                    return warningIcon;
                case IconType.Error:
                    return errorIcon;
                case IconType.Success:
                    return successIcon;
                case IconType.Info:
                    return infoIcon;
                default:
                    return infoIcon;
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySound(SoundType soundType, float volume = 1f)
        {
            AudioClip clip = null;
            switch (soundType)
            {
                case SoundType.Collect:
                    clip = collectSound;
                    break;
                case SoundType.ButtonClick:
                    clip = buttonClickSound;
                    break;
                case SoundType.Success:
                    clip = successSound;
                    break;
                case SoundType.Error:
                    clip = errorSound;
                    break;
            }

            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
            }
        }

        /// <summary>
        /// 设置自定义图标
        /// </summary>
        public void SetCustomIcon(IconType iconType, Sprite sprite)
        {
            switch (iconType)
            {
                case IconType.Confirm:
                    confirmIcon = sprite;
                    break;
                case IconType.Cancel:
                    cancelIcon = sprite;
                    break;
                case IconType.Close:
                    closeIcon = sprite;
                    break;
                case IconType.Warning:
                    warningIcon = sprite;
                    break;
                case IconType.Error:
                    errorIcon = sprite;
                    break;
                case IconType.Success:
                    successIcon = sprite;
                    break;
                case IconType.Info:
                    infoIcon = sprite;
                    break;
            }
        }

        /// <summary>
        /// 设置自定义收集图标
        /// </summary>
        public void SetCustomCollectionIcon(CollectionType type, Sprite sprite)
        {
            switch (type)
            {
                case CollectionType.Gold:
                    goldIcon = sprite;
                    break;
                case CollectionType.Diamond:
                    diamondIcon = sprite;
                    break;
                case CollectionType.Money:
                    moneyIcon = sprite;
                    break;
            }
        }
    }

    /// <summary>
    /// 收集类型枚举
    /// </summary>
    public enum CollectionType
    {
        Gold,
        Diamond,
        Money
    }

    /// <summary>
    /// 图标类型枚举
    /// </summary>
    public enum IconType
    {
        Confirm,
        Cancel,
        Close,
        Warning,
        Error,
        Success,
        Info
    }

    /// <summary>
    /// 音效类型枚举
    /// </summary>
    public enum SoundType
    {
        Collect,
        ButtonClick,
        Success,
        Error
    }
}
