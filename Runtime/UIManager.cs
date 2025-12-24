using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using CommonTools;
using System;
using System.Security.Cryptography.X509Certificates;


namespace UIFramework
{

    [Serializable]
    public class CollectionAnimationData
    {
        public List<GameObject> target;
        public Vector3 startPosition;
        public Transform targetTransform;
        public bool isMultiple;
        //是否指定endPosition
        public bool  isSpecifyEndPosition;
        public Vector3 endPosition;
        public string amountStr;

        public CollectionAnimationData(List<GameObject> target, Vector3 startPosition, bool isMultiple, string amountStr)
        {
            this.target = target;
            this.startPosition = startPosition;
            this.isMultiple = isMultiple;
            this.isSpecifyEndPosition = false;
            this.endPosition = Vector3.zero;
            this.amountStr = amountStr;
        }
    }
    /// <summary>
    /// UI管理器，用于统一管理所有页面和弹窗
    /// </summary>
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [SerializeField] private Notice notice;
        [SerializeField] private GameObject tipsPrefab;
        [SerializeField] private GameObject rewardsTipPrefab;
        [SerializeField] private GameObject addTextAnimationPrefab;
        [SerializeField] private GameObject collectionAnimationPrefab;
        // 页面列表
        [SerializeField] private List<Page> pages = new List<Page>();
        // 弹窗列表
        [SerializeField] private List<Popup> popups = new List<Popup>();
        // 页面栈
        private Stack<Page> pageStack = new Stack<Page>();

        // 弹窗队列系统
        private Popup currentVisiblePopup = null;

        private List<CollectionAnimation> collectionAnimations = new List<CollectionAnimation>();
        private List<AddTextAnimation> addTextAnimations = new List<AddTextAnimation>();
        private List<Tips> tips = new List<Tips>();
        private List<RewardsTip> rewardsTips = new List<RewardsTip>();

        void Start()
        {

            // CloseAllPages();
            // CloseAllPopups();
        }

        void Update()
        {
        }

        public void ShowNotice(string text)
        {
            if (notice != null)
            {
                notice.gameObject.SetActive(true);
                notice.ShowNotice(text);
            }
        }

        public void ShowCollectionEffect(CollectionAnimationData data)
        {
            for (int i = 0; i < collectionAnimations.Count; i++)
            {
                if (!collectionAnimations[i].gameObject.activeInHierarchy)
                {
                    collectionAnimations[i].ShowEffect(data);
                    
                    return;
                }
            }
            GameObject collectionAnimation = Instantiate(collectionAnimationPrefab, transform);
            collectionAnimation.GetComponent<CollectionAnimation>().ShowEffect(data);
            collectionAnimations.Add(collectionAnimation.GetComponent<CollectionAnimation>());
        }

        public void ShowAddTextEffect(Vector3 targetPos, string text)
        {
            for (int i = 0; i < addTextAnimations.Count; i++)
            {
                if (!addTextAnimations[i].gameObject.activeInHierarchy)
                {
                    addTextAnimations[i].ShowAddCountText(targetPos, text);
                    return;
                }
            }

            GameObject addTextAnimation = Instantiate(addTextAnimationPrefab, transform);
            addTextAnimation.GetComponent<AddTextAnimation>().ShowAddCountText(targetPos, text);
            addTextAnimations.Add(addTextAnimation.GetComponent<AddTextAnimation>());
        }

        

        public void ShowTips(string tipStr)
        {
            for (int i = 0; i < tips.Count; i++)
            {
                if (!tips[i].gameObject.activeInHierarchy)
                {
                    tips[i].ShowTips(tipStr);
                    return;
                }
            }
            GameObject tipsObj = Instantiate(tipsPrefab, transform);
            tipsObj.GetComponent<Tips>().ShowTips(tipStr);
            tips.Add(tipsObj.GetComponent<Tips>());
        }

        public void ShowRewardsTip(GameObject target,string amountStr,float delay = 2)
        {
            for (int i = 0; i < rewardsTips.Count; i++)
            {
                if (!rewardsTips[i].gameObject.activeInHierarchy)
                {
                    rewardsTips[i].Initialize(target, amountStr, delay);
                    return;
                }
            }
            GameObject rewardsTipObj = Instantiate(rewardsTipPrefab, transform);
            rewardsTipObj.GetComponent<RewardsTip>().Initialize(target, amountStr, delay);
            rewardsTips.Add(rewardsTipObj.GetComponent<RewardsTip>());
        }

        /// <summary>
        /// 显示页面
        /// </summary>
        /// <param name="args">可变参数</param>
        public void ShowPage<T>(params object[] args) where T : Page
        {
            System.Type type = typeof(T);
            Page page = FindPage(type);
            if (page == null)
            {
                Debug.LogError($"页面 {type.Name} 未注册");
                return;
            }

            // // 隐藏当前页面
            // if (pageStack.Count > 0)
            // {
            //     pageStack.Peek().Hide(args);
            // }

            // 显示新页面
            page.Show(args);
            // pageStack.Push(page);
        }

        /// <summary>
        /// 显示弹窗（支持队列等待）
        /// </summary>
        /// <param name="needQueue">是否需要排队等候，默认为true</param>
        /// <param name="args">可变参数</param>
        public void ShowPopup<T>(params object[] args) where T : Popup
        {
            Utils.Log($"ShowPopup<T> : {args}", Utils.LogType.Info);
            System.Type type = typeof(T);
            Popup popup = FindPopup(type);
            if (popup == null)
            {
                Debug.LogError($"弹窗 {type.Name} 未注册");
                return;
            }

            // 直接显示弹窗
            ShowPopupInternal(popup, args);
        }

        // /// <summary>
        // /// 显示弹窗（兼容旧版本调用方式）
        // /// </summary>
        // /// <param name="args">可变参数</param>
        // public void ShowPopup<T>(params object[] args) where T : Popup
        // {
        //     ShowPopup<T>(args);
        // }

        /// <summary>
        /// 立即显示弹窗（不排队）
        /// </summary>
        /// <param name="args">可变参数</param>
        public void ShowPopupImmediately<T>(params object[] args) where T : Popup
        {
            ShowPopup<T>(args);
        }

        /// <summary>
        /// 内部显示弹窗方法
        /// </summary>
        private void ShowPopupInternal(Popup popup, object[] args)
        {
            currentVisiblePopup = popup;
            popup.Show(args);
            Utils.Log($"弹窗 {popup.GetType().Name} 已显示", Utils.LogType.Info);
        }

        /// <summary>
        /// 检查是否有可见的弹窗
        /// </summary>
        private bool HasVisiblePopup()
        {
            if (currentVisiblePopup != null && currentVisiblePopup.IsVisible())
            {
                return true;
            }

            // 如果当前记录的弹窗已经不可见，清除记录
            if (currentVisiblePopup != null && !currentVisiblePopup.IsVisible())
            {
                currentVisiblePopup = null;
            }

            return false;
        }

        /// <summary>
        /// 检查是否为MessagePopup类型
        /// MessagePopup类型的弹窗一定不需要排队，会立即显示
        /// </summary>
        private bool IsMessagePopup(System.Type type)
        {
            return type.Name.Contains("MessagePopup") ||
                   type.Name.Contains("MessagerPopup") ||
                   type.Name.Equals("MessagerPopup");
        }



        /// <summary>
        /// 隐藏页面
        /// </summary>
        /// <param name="args">可变参数</param>
        public void HidePage<T>(params object[] args) where T : Page
        {
            System.Type type = typeof(T);
            Page page = FindPage(type);
            if (page == null)
            {
                Debug.LogError($"页面 {type.Name} 未注册");
                return;
            }

            page.Hide(args);
            // if (pageStack.Count > 0 && pageStack.Peek() == page)
            // {
            //     pageStack.Pop();
            // }
        }

        /// <summary>
        /// 隐藏弹窗
        /// </summary>
        /// <param name="args">可变参数</param>
        public void HidePopup<T>(params object[] args) where T : Popup
        {
            System.Type type = typeof(T);
            Popup popup = FindPopup(type);
            if (popup == null)
            {
                Debug.LogError($"弹窗 {type.Name} 未注册");
                return;
            }

            popup.Hide(args);

            // 如果隐藏的是当前可见弹窗，清除记录并处理队列
            if (currentVisiblePopup == popup)
            {
                currentVisiblePopup = null;
            }
        }

        /// <summary>
        /// 返回上一页
        /// </summary>
        /// <param name="args">可变参数</param>
        public void GoBack(params object[] args)
        {
            if (pageStack.Count <= 1)
            {
                Debug.LogWarning("没有上一页");
                return;
            }

            // 隐藏当前页面
            pageStack.Pop().Hide(args);
            // 显示上一页
            pageStack.Peek().Show(args);
        }

        /// <summary>
        /// 检查页面是否显示
        /// </summary>
        public bool IsPageVisible<T>() where T : Page
        {
            System.Type type = typeof(T);
            Page page = FindPage(type);
            if (page == null)
            {
                Debug.LogError($"页面 {type.Name} 未注册");
                return false;
            }

            return page.IsVisible();
        }

        /// <summary>
        /// 检查弹窗是否显示
        /// </summary>
        public bool IsPopupVisible<T>() where T : Popup
        {
            System.Type type = typeof(T);
            Popup popup = FindPopup(type);
            if (popup == null)
            {
                Debug.LogError($"弹窗 {type.Name} 未注册");
                return false;
            }

            return popup.IsVisible();
        }

        /// <summary>
        /// 获取页面
        /// </summary>
        public T GetPage<T>() where T : Page
        {
            System.Type type = typeof(T);
            Page page = FindPage(type);
            if (page == null)
            {
                Debug.LogError($"页面 {type.Name} 未注册");
                return null;
            }

            return page as T;
        }

        /// <summary>
        /// 获取弹窗
        /// </summary>
        public T GetPopup<T>() where T : Popup
        {
            System.Type type = typeof(T);
            Popup popup = FindPopup(type);
            if (popup == null)
            {
                Debug.LogError($"弹窗 {type.Name} 未注册");
                return null;
            }

            return popup as T;
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        public void RefreshPage<T>() where T : Page
        {
            System.Type type = typeof(T);
            Page page = FindPage(type);
            if (page == null)
            {
                Debug.LogError($"页面 {type.Name} 未注册");
                return;
            }

            page.Refresh();
        }

        /// <summary>
        /// 刷新弹窗
        /// </summary>
        public void RefreshPopup<T>() where T : Popup
        {
            System.Type type = typeof(T);
            Popup popup = FindPopup(type);
            if (popup == null)
            {
                Debug.LogError($"弹窗 {type.Name} 未注册");
                return;
            }

            popup.Refresh();
        }

        /// <summary>
        /// 查找页面
        /// </summary>
        private Page FindPage(System.Type type)
        {
            return pages.Find(page => page.GetType() == type);
        }

        /// <summary>
        /// 查找弹窗
        /// </summary>
        private Popup FindPopup(System.Type type)
        {
            return popups.Find(popup => popup.GetType() == type);
        }

        /// <summary>
        /// 关闭所有页面
        /// </summary>
        /// <param name="args">可变参数</param>
        public void CloseAllPages(params object[] args)
        {
            // // 清空页面栈
            // while (pageStack.Count > 0)
            // {
            //     pageStack.Pop().Hide(args);
            // }

            // 隐藏所有页面
            foreach (var page in pages)
            {
                if (page.IsVisible())
                {
                    page.Hide(args);
                }
            }


        }

        public void CloseAllPopups(params object[] args)
        {

            // 隐藏所有弹窗
            foreach (var popup in popups)
            {
                if (popup.IsVisible())
                {
                    popup.Hide(args);
                }
            }

            // 清除当前可见弹窗记录
            currentVisiblePopup = null;

            Utils.Log("所有弹窗已关闭，队列已清空", Utils.LogType.Info);
        }

        /// <summary>
        /// 通知弹窗已关闭（供弹窗自身调用）
        /// </summary>
        public void OnPopupClosed(Popup popup)
        {

            if (currentVisiblePopup == popup)
            {
                currentVisiblePopup = null;
            }
        }

    }
}
