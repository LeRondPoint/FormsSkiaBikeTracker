﻿// **********************************************************************
// 
//   LrpFormsPagePresenter.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Le rond-point
// 
// ***********************************************************************
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenter.Core;
using Xamarin.Forms;

namespace FormsSkiaBikeTracker.Forms.UI
{
    public abstract class LrpFormsPagePresenter : MvxFormsPagePresenter
    {
        protected LrpFormsPagePresenter()
        {
        }

        protected LrpFormsPagePresenter(Application mvxFormsApp) : base(mvxFormsApp)
        {
        }

        public override void Show(MvxViewModelRequest request)
        {
            bool callBase = true;
            bool replaceMain = false;

            if (request.PresentationValues != null)
            {
                if (request.PresentationValues.ContainsKey(PresenterConstants.ReplaceMainPagePresentation))
                {
                    bool.TryParse(request.PresentationValues[PresenterConstants.ReplaceMainPagePresentation], out replaceMain);
                }
            }

            if (replaceMain)
            {
                Page newMain = CreateAndSetupPage(request);

                if (newMain != null)
                {
                    SetMainPage(newMain);
                    callBase = false;
                }
            }
            
            if (callBase)
            {
                base.Show(request);
            }
        }

        private void SetMainPage(Page newMain)
        {
            MvxFormsApp.MainPage = newMain;
            InitRootViewController(newMain);
        }

        private Page CreateAndSetupPage(MvxViewModelRequest request)
        {
            Page page = MvxPresenterHelpers.CreatePage(request);

            if (page != null)
            {
                IMvxViewModel viewModel = MvxPresenterHelpers.LoadViewModel(request);

                SetupForBinding(page, viewModel, request);
            }

            return page;
        }

        private void SetupForBinding(Page page, IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            IMvxContentPage mvxContentPage = page as IMvxContentPage;

            if (mvxContentPage != null)
            {
                mvxContentPage.Request = request;
                mvxContentPage.ViewModel = viewModel;
            }
            else
            {
                page.BindingContext = viewModel;
            }
        }

        protected override void CustomPlatformInitialization(NavigationPage mainPage)
        {
            InitRootViewController(mainPage);
        }

        protected abstract void InitRootViewController(Page rootPage);
    }
}
