﻿using GalaSoft.MvvmLight.Command;
using DreamFood.Helpers;
using DreamFood.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using DreamFood.ViewsModels;

namespace DreamFood.ViewModels
{
    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(Goto);
            }
        }

        private void Goto()
        {
            if (this.PageName == "LoginPage")
            {
                Settings.AccessToken = string.Empty;
                Settings.TokenType = string.Empty;
                Settings.IsRemembered = false;
                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            //else if (this.PageName == "AboutPage")
            //{
            //    App.Master.IsPresented = false;
            //    await App.Navigator.PushAsync(new MapPage());
            //}
        }
        #endregion
    }
}