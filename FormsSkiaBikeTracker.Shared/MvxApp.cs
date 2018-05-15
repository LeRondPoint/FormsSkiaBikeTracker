﻿// **********************************************************************
// 
//   App.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Le rond-point
// 
// ***********************************************************************

using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Acr.Settings;
using Acr.UserDialogs;
using FormsSkiaBikeTracker.Services;
using FormsSkiaBikeTracker.Services.Interface;
using MvvmCross.Localization;
using FormsSkiaBikeTracker.ViewModels;
using LRPFramework.Mvx.Services.Localization;
using LRPFramework.Services;
using LRPFramework.Services.Resources;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.JsonLocalization;
using MvvmCross.ViewModels;
using SimpleCrypto;

namespace FormsSkiaBikeTracker
{
    public class MvxApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            InitializeServices();

            RegisterAppStart<LoadingViewModel>();

            base.Initialize();
        }

        public static IResourceLocator InitializeResources()
        {
            Assembly currentAssembly = Assembly.Load(new AssemblyName("FormsSkiaBikeTracker.Shared"));
            ResourceLocator resourceLocator = new ResourceLocator(Constants.GeneralNamespace, currentAssembly);

            resourceLocator.RegisterPath(ResourceKeys.RootKey, Constants.RootResourcesFolder);
            resourceLocator.RegisterPath(ResourceKeys.ImagesKey, Constants.RootImagesFolder);
            resourceLocator.RegisterPath(ResourceKeys.TextKey, Constants.RootTextFolder);
            resourceLocator.RegisterPath("Fonts", Constants.RootResourcesFolder + "/Fonts");

            return resourceLocator;
        }

        private void InitializeText()
        {
            LRPTextProviderBuilder builder = new LRPTextProviderBuilder(Constants.RootTextFolder, Constants.TextTypeKey);

            Mvx.RegisterSingleton<IMvxTextProviderBuilder>(builder);
            Mvx.RegisterSingleton<IMvxTextProvider>(builder.TextProvider);

            // Set language
#if DEBUG
            builder.LoadResources("fr");
#else
            builder.LoadResources(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
#endif
        }

        private void InitializeServices()
        {
            Mvx.LazyConstructAndRegisterSingleton<ILRPBootstrapper, LRPBootstrapper>();
            Mvx.RegisterSingleton<ICryptoService>(() => new PBKDF2());
            Mvx.RegisterSingleton<ILocationTracker>(InitializeLocationTracker);
            Mvx.RegisterSingleton<IResourceLocator>(InitializeResources);
            Mvx.RegisterSingleton<IUserDialogs>(UserDialogs.Instance);
            Mvx.RegisterSingleton<ISettings>(CrossSettings.Current);

            Mvx.CallbackWhenRegistered<MainThread>(InitializeText);
            Mvx.CallbackWhenRegistered<MainThread>(InitializeBootstrap);
        }

        private ILocationTracker InitializeLocationTracker()
        {
            ILocationTracker tracker = Mvx.IocConstruct<LocationTracker>();

            tracker.Start(8, 3, false);

            return tracker;
        }

        private void InitializeBootstrap()
        {
            ILRPBootstrapper bootstrapper = Mvx.Resolve<ILRPBootstrapper>();

            bootstrapper.AddAsyncStep(new LRPAsyncActionBootstrapStep(a => Task.Delay(1500)));
            bootstrapper.QueueStep(new LRPActionBootstrapStep(() => { }) { StepActionText = "Booting..." });

            bootstrapper.Boot();
        }
    }
}