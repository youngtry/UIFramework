using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    /// <summary>
    /// UI基础抽象类
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class BasePage : MonoBehaviour
    {
        /// <summary>
        /// 初始化UI
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// 显示UI
        /// </summary>
        /// <param name="args">可变参数</param>
        public abstract void Show(params object[] args);

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="args">可变参数</param>
        public abstract void Hide(params object[] args);

        /// <summary>
        /// 刷新UI数据
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// 获取UI是否显示
        /// </summary>
        public abstract bool IsVisible();
    }
}
