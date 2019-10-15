namespace DreamFood.ViewsModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using DreamFood.Common.Models;
    using DreamFood.Helpers;
    using DreamFood.ViewModels;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;

    public class MainViewModel
    {

        #region Properties

        public RestaurantsViewModel Restaurants { get; set; }

        public RecommendationViewModel Recommendations { get; set; }

        public LoginViewModel Login { get; set; }

        public AddRecommendationViewModel AddRecommendation { get; set; }

        public AddRestaurantViewModel AddRestaurant { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
       
        public RegisterViewModel Register { get; set; }

        public MyUserASP UserASP { get; set; }

        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }

        public string UserImageFullPath
        {
            get
            {
                foreach (var claim in this.UserASP.Claims)
                {
                    if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri")
                    {
                        if (claim.ClaimValue.StartsWith("~"))
                        {
                            return $"https://apidreamfood.azurewebsites.net{claim.ClaimValue.Substring(1)}";
                        }

                        return claim.ClaimValue;
                    }
                }

                return null;
            }
        }

        public ICommand AddRestaurantCommand
        {
            get
            {
                return new RelayCommand(GoToAddRestaurant);
            }
        }

        public ICommand AddRecommendationCommand
        {
            get
            {
                return new RelayCommand(GoToAddRecommendation);
            }
        }

        public ICommand GotoMapCommand
        {
            get
            {
                return new RelayCommand(GoToMap);
            }
        }

        public ICommand GoToRecommendationCommand
        {
            get
            {
                return new RelayCommand(GoToRecommendation);
            }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
           
        }
        #endregion

        #region Methods

        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            //this.Menu.Add(new MenuItemViewModel
            //{
            //    //Icon = "ic_phonelink_setup",
            //    PageName = "SetupPage",
            //    Title = Languages.Setup,
            //});

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });
        }


        #endregion


        #region Commands

        private async void GoToAddRestaurant()
        {
            this.AddRestaurant = new AddRestaurantViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddRestaurantPage());
        }

        private async void GoToAddRecommendation()
        {
            this.AddRecommendation = new AddRecommendationViewModel(Recommendations.restaurant);
            await Application.Current.MainPage.Navigation.PushAsync(new AddRecommendationPage());
        }

        private async void GoToMap()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MapPage());
        }

        private async void GoToRecommendation()
        {
            this.Recommendations = new RecommendationViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RecommendationsPage());
        }


        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }



}
