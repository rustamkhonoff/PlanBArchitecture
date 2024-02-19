using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Services.UI.Core;
using Services.UI.Window;
using UnityEngine;

[assembly: InternalsVisibleTo("UI.Zenject")]

namespace Services.UI.StaticData
{
    [CreateAssetMenu(menuName = "Project/UI/Create UIStaticData", fileName = "UIStaticData", order = 0)]
    internal class UIStaticData : ScriptableObject
    {
        [SerializeField] private UIRoot _uiRootPrefab;
        [SerializeField] private List<WindowBase> _windows;

        public IEnumerable<WindowBase> Windows => _windows;

        public UIRoot RootPrefab => _uiRootPrefab;
    }
}