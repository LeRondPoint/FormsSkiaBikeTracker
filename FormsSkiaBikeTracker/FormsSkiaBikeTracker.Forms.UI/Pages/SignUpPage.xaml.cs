﻿// **********************************************************************
// 
//   SignUpPage.xaml.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Le rond-point
// 
// ***********************************************************************

using System;
using FormsSkiaBikeTracker.Forms.UI.Effects;
using FormsSkiaBikeTracker.Services.Validation;
using MvvmCross.Platform.WeakSubscription;

namespace FormsSkiaBikeTracker.Forms.UI.Pages
{
    public partial class SignUpPagePage
    {
        public IValidationResult CurrentErrors { get; } = new ValidationResult();

        private MvxWeakEventSubscription<IValidationResult> _errorsChangedSubscription;

        public SignUpPagePage()
        {
            InitializeComponent();

            _errorsChangedSubscription = CurrentErrors.WeakSubscribe(nameof(CurrentErrors.ErrorsChanged), ValidationResultErrorsChanged);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        private void ValidationResultErrorsChanged(object sender, EventArgs e)
        {
            if (CurrentErrors.GetError(nameof(ViewModel.Name)) != null)
            {
                if (NameEntry.Behaviors.Count == 0)
                {
                    NameEntry.Behaviors.Add(new InvalidEntryBehavior());
                }
            }
            else if (NameEntry.Behaviors.Count > 0)
            {
                NameEntry.Behaviors.RemoveAt(0);
            }

            if (CurrentErrors.GetError(nameof(ViewModel.Password)) != null)
            {
                if (PasswordEntry.Behaviors.Count == 0)
                {
                    PasswordEntry.Behaviors.Add(new InvalidEntryBehavior());
                }
            }
            else if (PasswordEntry.Behaviors.Count > 0)
            {
                PasswordEntry.Behaviors.RemoveAt(0);
            }

            if (CurrentErrors.GetError(nameof(ViewModel.PasswordConfirmation)) != null)
            {
                if (ConfirmationEntry.Behaviors.Count == 0)
                {
                    ConfirmationEntry.Behaviors.Add(new InvalidEntryBehavior());
                }
            }
            else if (ConfirmationEntry.Behaviors.Count > 0)
            {
                ConfirmationEntry.Behaviors.RemoveAt(0);
            }
        }
    }
}