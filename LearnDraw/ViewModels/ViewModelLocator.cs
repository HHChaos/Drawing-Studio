using System;
using CommonServiceLocator;
using LearnDraw.Services;
using LearnDraw.Views;
using Unity.ServiceLocation;
using Unity;
using Unity.Lifetime;
using HHChaosToolkit.UWP.Services;
using HHChaosToolkit.UWP.Mvvm;
using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.ViewModels.PickerViewModels;
using LearnDraw.Views.Pickers;

namespace LearnDraw.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                InitViewModelLocator();
            }
        }
        private IUnityContainer _container;
        public void InitViewModelLocator()
        {
            _container = new UnityContainer();
            var _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);

            _container.RegisterType<ObjectPickerService>(new ContainerControlledLifetimeManager())
                .RegisterType<SubWindowsService>(new ContainerControlledLifetimeManager())
                .RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());

            RegisterNavigationService<MainViewModel, MainPage>(ShellViewModel.ContentNavigationServiceKey);
            RegisterNavigationService<AnimDrawingViewModel, AnimDrawingPage>(ShellViewModel.ContentNavigationServiceKey);

            RegisterObjectPicker<bool, FirstRunViewModel, FirstRunPage>();
            RegisterObjectPicker<bool, SettingsViewModel, SettingsPage>();
        }

        public ObjectPickerService ObjectPickerService => ServiceLocator.Current.GetInstance<ObjectPickerService>();
        public SubWindowsService SubWindowsService => ServiceLocator.Current.GetInstance<SubWindowsService>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public FirstRunViewModel FirstRunViewModel => ServiceLocator.Current.GetInstance<FirstRunViewModel>();

        public void RegisterNavigationService<VM, V>(string nsKey)
            where VM : ViewModelBase
        {
            _container.RegisterType<VM>(new ContainerControlledLifetimeManager());
            if (!NavigationServiceList.Instance.IsRegistered(nsKey))
                NavigationServiceList.Instance.Register(nsKey, new NavigationService());
            var contentService = NavigationServiceList.Instance[nsKey];
            contentService.Configure(typeof(VM).FullName, typeof(V));
        }
        public void RegisterObjectPicker<T, VM, V>()
            where VM : ObjectPickerBase<T>
        {
            _container.RegisterType<VM>(new ContainerControlledLifetimeManager());
            ObjectPickerService.Configure(typeof(T).FullName, typeof(VM).FullName, typeof(V));
        }
        public void RegisterSubWindow<VM, V>()
            where VM : SubWindowBase
        {
            _container.RegisterType<VM>();
            SubWindowsService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
