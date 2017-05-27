﻿// **********************************************************************
// 
//   LoginViewModel.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Le rond-point
// 
// ***********************************************************************

using System.Collections.Generic;
using FormsSkiaBikeTracker.Models;
using FormsSkiaBikeTracker.ViewModels;
using LRPLib.Mvx.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Realms;
using SimpleCrypto;

namespace FormsSkiaBikeTracker.Shared.ViewModels
{
    public class LoginViewModel : LrpViewModel
    {
        [MvxInject]
        public ICryptoService Crypto { get; set; }

        private IEnumerable<Athlete> _athletes;
        public IEnumerable<Athlete> Athletes
        {
            get { return _athletes; }
            set
            {
                if (Athletes != value)
                {
                    _athletes = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Athlete _selectedAthlete;
        public Athlete SelectedAthlete
        {
            get { return _selectedAthlete; }
            set
            {
                if (SelectedAthlete != value)
                {
                    _selectedAthlete = value;
                    RaisePropertyChanged();
                }
            }
        }

        private IMvxCommand _loginAthleteCommand;
        public IMvxCommand LoginAthleteCommand
        {
            get
            {
                if (_loginAthleteCommand == null)
                {
                    _loginAthleteCommand = new MvxCommand<string>(LoginAthlete);
                }

                return _loginAthleteCommand;
            }
        }

        private IMvxCommand _goToSignupCommand;
        public IMvxCommand GoToSignupCommand
        {
            get
            {
                if (_goToSignupCommand == null)
                {
                    _goToSignupCommand = new MvxCommand(GoToSignup);
                }

                return _goToSignupCommand;
            }
        }

        public LoginViewModel()
        {
        }

        public override void Start()
        {
            base.Start();

            Athletes = Realm.GetInstance().All<Athlete>();
        }
        
        private void LoginAthlete(string password)
        {
            string hashedPassword = Crypto.Compute(password, SelectedAthlete.PasswordSalt);
            bool isPasswordValid = Crypto.Compare(SelectedAthlete.PasswordHash, hashedPassword);

            if (isPasswordValid)
            {
                ShowViewModel<MainViewModel>();
            }
            else
            {
//                UserDialogs
            }
        }

        private void GoToSignup()
        {
            ShowViewModel<SignUpViewModel>();
        }
    }
}

