using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UIFramework;

namespace UIFramework.Examples
{
    /// <summary>
    /// 示例背包页面
    /// </summary>
    public class ExampleInventoryPage : Page
    {
        [Header("Inventory Page Elements")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text itemCountText;
        [SerializeField] private Button backButton;
        [SerializeField] private Button addItemButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private GameObject itemPrefab;

        private List<string> items = new List<string>();

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            // 绑定按钮事件
            if (backButton != null)
                backButton.onClick.AddListener(() => UIManager.Instance.ShowPage<ExampleMainPage>());

            if (addItemButton != null)
                addItemButton.onClick.AddListener(AddRandomItem);

            if (clearButton != null)
                clearButton.onClick.AddListener(ClearItems);

            // 初始化一些物品
            items.AddRange(new[] { "剑", "盾牌", "药水", "金币" });
        }

        protected override void OnAfterShow(params object[] data)
        {
            base.OnAfterShow(data);

            // 更新标题
            if (titleText != null)
            {
                titleText.text = "背包页面";
            }

            // 处理传入的物品数据
            if (data != null && data.Length > 0)
            {
                foreach (var item in data)
                {
                    if (item is string itemName)
                    {
                        items.Add(itemName);
                    }
                }
            }

            RefreshItemDisplay();

            Debug.Log($"InventoryPage shown with {data?.Length ?? 0} parameters");
        }

        protected override void OnRefresh(params object[] data)
        {
            base.OnRefresh(data);

            // 处理刷新数据
            if (data != null && data.Length > 0)
            {
                foreach (var item in data)
                {
                    if (item is string itemName)
                    {
                        items.Add(itemName);
                    }
                }
            }

            RefreshItemDisplay();

            Debug.Log($"InventoryPage refreshed with {data?.Length ?? 0} parameters");
        }

        private void RefreshItemDisplay()
        {
            // 更新物品数量显示
            if (itemCountText != null)
            {
                itemCountText.text = $"物品数量: {items.Count}";
            }

            // 清空现有显示
            if (itemContainer != null)
            {
                foreach (Transform child in itemContainer)
                {
                    Destroy(child.gameObject);
                }

                // 创建物品显示
                foreach (var item in items)
                {
                    CreateItemDisplay(item);
                }
            }
        }

        private void CreateItemDisplay(string itemName)
        {
            if (itemPrefab != null && itemContainer != null)
            {
                GameObject itemGO = Instantiate(itemPrefab, itemContainer);
                Text itemText = itemGO.GetComponentInChildren<Text>();
                if (itemText != null)
                {
                    itemText.text = itemName;
                }
            }
        }

        private void AddRandomItem()
        {
            string[] randomItems = { "新剑", "新盾", "新药水", "宝石", "卷轴", "钥匙" };
            string randomItem = randomItems[Random.Range(0, randomItems.Length)];
            items.Add(randomItem);
            RefreshItemDisplay();
            Debug.Log($"Added item: {randomItem}");
        }

        private void ClearItems()
        {
            items.Clear();
            RefreshItemDisplay();
            Debug.Log("Cleared all items");
        }
    }
}
