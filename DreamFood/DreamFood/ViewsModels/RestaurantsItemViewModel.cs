using System;
using System.Collections.Generic;
using System.Text;

namespace DreamFood.ViewsModels
{
    using System.Linq;
    using DreamFood.Services;
    using DreamFood.Helpers;
    using DreamFood.Views;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using DreamFood.Common.Models;

    public class RestaurantsItemViewModel: Restaurant
    {
        #region Atributes
        private ApiServices apiService;
        #endregion
       
        #region Constructors
        public RestaurantsItemViewModel()
        {
            this.apiService = new ApiServices();
        }
        #endregion

        #region Commands

        public ICommand RecommendationsCommand
        {
            get
            {
                return new RelayCommand(GoToRecommendations);
            }
        }

        private async void GoToRecommendations()
        {
            MainViewModel.GetInstance().Recommendations = new RecommendationViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new RecommendationsPage());
        }

        #endregion
    }
}

