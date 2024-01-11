using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public abstract class Screen : MonoBehaviour
    {
        private Dictionary<Type, IView> _views;

        public void Awake()
        {
            _views = GetComponentsInChildren<IView>()
                .ToDictionary(item => item.GetType(), item => item);
        }

        public T Bind<T>(IViewModel viewModel) where T : IView
        {
            var type = typeof(T);
            _views.TryGetValue(type, out var view);
            if (view != null) view.Bind(viewModel);
            return (T)view;
        }
    }
    
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<Type, Screen> _screens = new();

        protected override void Init()
        {
            _screens = GetComponentsInChildren<Screen>()
                .ToDictionary(item => item.GetType(), item => item);
        }

        public T GetScreen<T>() where T : Screen
        {
            var type = typeof(T);
            
            if (_screens.TryGetValue(type, out var screen)) return (T)screen;
            return null;
        }
    }
}