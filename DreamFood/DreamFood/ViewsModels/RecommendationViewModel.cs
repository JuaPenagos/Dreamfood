using DreamFood.Common.Models;
using DreamFood.Helpers;
using DreamFood.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace DreamFood.ViewsModels
{
    public class RecommendationViewModel: BaseViewModel
    {
        #region Attributes
        private ApiServices apiService;

        private ObservableCollection<RecommendationItemViewModel> recommendations;

        private bool isRefreshing;

        private string filter;

        public Restaurant restaurant;

        public Restaurant Myrestaurant;
        #endregion

        #region Properties
        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<Restaurant> MyRestaurants { get; set; }

        public List<Recommendation> MyRecommendations { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<RecommendationItemViewModel> Recommendations
        {
            get { return this.recommendations; }
            set { this.SetValue(ref this.recommendations, value); }
        }

    #endregion

        #region Constructors
        public RecommendationViewModel()
        {
            instance = this;
            this.apiService = new ApiServices();
            this.LoadRestaurant();
            int milliseconds = 1000;
            Thread.Sleep(milliseconds);
            this.LoadRecommendations();
            
        }
        public RecommendationViewModel(Restaurant restaurant)
        {
            instance = this;
            this.apiService = new ApiServices();
            this.LoadRestaurant();
            this.restaurant = restaurant;
            int milliseconds = 1000;
            Thread.Sleep(milliseconds);
            this.LoadRecommendationsByIdRestaurant();
        }
        #endregion

        #region Singleton
        private static RecommendationViewModel instance;

            public static RecommendationViewModel GetInstance()
            {
                if (instance == null)
                {
                    return new RecommendationViewModel();
                }

                return instance;
            }
        #endregion

        #region Methods
        private async void LoadRecommendationsByIdRestaurant()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlRecommendationsController"].ToString();
            var response = await this.apiService.GetList<Recommendation>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            };

            this.MyRecommendations = (List<Recommendation>)response.Result;
            this.MyRecommendations = MyRecommendations.Where(r => r.IdRestaurant == this.restaurant.IdRestaurant).ToList();
            this.RefreshList();
            this.IsRefreshing = false;
        }
        private async void LoadRecommendations()
        {
            
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlRecommendationsController"].ToString();
            var response = await this.apiService.GetList<Recommendation>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            };
            this.IsRefreshing = false;
            this.MyRecommendations = (List<Recommendation>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;

        }
        private async void LoadRestaurant()
        {

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlRestaurantsController"].ToString();
            var response = await this.apiService.GetList<Restaurant>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            };

            this.MyRestaurants = (List<Restaurant>)response.Result;
           
        }
        
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                if (this.restaurant == null)
                {
                    return new RelayCommand(LoadRecommendations);
                }
                else {
                    return new RelayCommand(LoadRecommendationsByIdRestaurant);
                }
            }
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListRecommendationsItemViewModel = MyRecommendations.Select(r => new RecommendationItemViewModel
                {
                    RecommendationUser = r.RecommendationUser,
                    ImagePathRecomm = r.ImagePathRecomm,
                    Score = r.Score,
                    DateRecomm=r.DateRecomm,
                    IdRestaurant =  r.IdRestaurant
                });
                

                this.Recommendations = new ObservableCollection<RecommendationItemViewModel>(
                    myListRecommendationsItemViewModel.OrderByDescending(r => r.DateRecomm));

                if (this.MyRestaurants == null)
                {
                }
                else
                { 
                    foreach (Recommendation recommendation in this.Recommendations)
                    {

                        recommendation.RecommendationUser = recommendation.RecommendationUser;
                        recommendation.ImagePathRecomm = recommendation.ImagePathRecomm;
                        recommendation.Score = recommendation.Score;
                        recommendation.DateRecomm = recommendation.DateRecomm;
                        recommendation.IdRestaurant = recommendation.IdRestaurant;
                        recommendation.Restaurant = this.MyRestaurants.Where(r => r.IdRestaurant == recommendation.IdRestaurant).FirstOrDefault();
                    }
                }

            }
            else
            {
                var myListRecommendationsItemViewModel = MyRecommendations.Select(r => new RecommendationItemViewModel
                {
                    RecommendationUser = r.RecommendationUser,
                    ImagePathRecomm = r.ImagePathRecomm,
                    Score = r.Score,
                    DateRecomm = r.DateRecomm,
                    IdRestaurant = r.IdRestaurant

                }).Where(r => r.RecommendationUser.ToLower().Contains(this.Filter.ToLower())).ToList(); ;

                this.Recommendations = new ObservableCollection<RecommendationItemViewModel>(
                    myListRecommendationsItemViewModel.OrderByDescending(r => r.DateRecomm));

                if (this.MyRestaurants == null)
                {
                }
                else
                {
                    foreach (Recommendation recommendation in this.Recommendations)
                    {

                        recommendation.RecommendationUser = recommendation.RecommendationUser;
                        recommendation.ImagePathRecomm = recommendation.ImagePathRecomm;
                        recommendation.Score = recommendation.Score;
                        recommendation.DateRecomm = recommendation.DateRecomm;
                        recommendation.IdRestaurant = recommendation.IdRestaurant;
                        recommendation.Restaurant = this.MyRestaurants.Where(r => r.IdRestaurant == recommendation.IdRestaurant).FirstOrDefault();
                    }
                }

            }

        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }
        

        

        #endregion
        }


}

