namespace DreamFood.ViewsModels
{
    using System.Windows.Input;
    using Common.Models;
    using Helpers;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using Xamarin.Forms;
    using Plugin.Media.Abstractions;
    using Plugin.Media;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;

    public class AddRecommendationViewModel: BaseViewModel
    {
        #region Attributes

        private MediaFile file;

        private ImageSource imageSource;

        private ApiServices apiService;

        public bool isRunning;

        public bool isEnabled;

        private ObservableCollection<RestaurantsItemViewModel> restaurants;

        private Restaurant restaurant;
        #endregion

        #region Properties
        public List<Restaurant> Myrestaurants { get; set; }

        public string IdRestaurant { get; set; }

        public string UserId { get; set; }

        public string RecommendationUser { get; set; }

        public string Score { get; set; }

        public Restaurant Restaurant 
        {
            get { return this.restaurant; }
            set { this.SetValue(ref this.restaurant, value); } 
        }
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public ObservableCollection<RestaurantsItemViewModel> Restaurants
        {
            get { return this.restaurants; }
            set { this.SetValue(ref this.restaurants, value); }
        }
        #endregion

        #region Constructors

        public AddRecommendationViewModel(Restaurant restaurant)
        {
            this.apiService = new ApiServices();
            this.IsEnabled = true;
            this.ImageSource = "picture";
            if (restaurant == null)
            {
                LoadRestaurants();
            }
            else
            {
                LoadRestaurants(restaurant);
            }
            
        }
        #endregion

        #region Methods
        private async void LoadRestaurants(Restaurant restaurant)
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
               
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            };

            this.Myrestaurants = (List<Restaurant>)response.Result;
            this.Myrestaurants = Myrestaurants.Where(r => r.IdRestaurant == restaurant.IdRestaurant).ToList();

            var myListRestaurantsItemViewModel = Myrestaurants.Select(r => new RestaurantsItemViewModel
            {
                IdRestaurant = r.IdRestaurant,
                Name = r.Name,
                Type = r.Type,
                Remarks = r.Remarks,
                Phone = "Tel: " + r.Phone,
                Address = r.Address,
                ImagePathMenu = r.ImagePathMenu,
                ImageArray = r.ImageArray,


            });

            this.Restaurants = new ObservableCollection<RestaurantsItemViewModel>(
                myListRestaurantsItemViewModel.OrderBy(r => r.Name));

        }
        private async void LoadRestaurants()
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

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            };

            this.Myrestaurants = (List<Restaurant>)response.Result;

            var myListRestaurantsItemViewModel = Myrestaurants.Select(r => new RestaurantsItemViewModel
            {
                IdRestaurant = r.IdRestaurant,
                Name = r.Name,
                Type = r.Type,
                Remarks = r.Remarks,
                Phone = "Tel: " + r.Phone,
                Address = r.Address,
                ImagePathMenu = r.ImagePathMenu,
                ImageArray = r.ImageArray,


            });

            this.Restaurants = new ObservableCollection<RestaurantsItemViewModel>(
                myListRestaurantsItemViewModel.OrderBy(r => r.Name));
        }
        #endregion

        #region Commands

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }

        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.RecommendationUser))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorrRecomendacion,
                    Languages.Accept);
                return;
            }
            if (Restaurant == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorRestaurant,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Score))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorScore,
                    Languages.Accept);
                return;
            }

            var score = double.Parse(this.Score);
            if (score <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorScore,
                    Languages.Accept);
                return;
            }


            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

           // var mainViewModel = new MainViewModel();
            var recommendation = new Recommendation
            {
                RecommendationUser = this.RecommendationUser,
                Score = score,
                ImageArray = imageArray,
                IdRestaurant = this.Restaurant.IdRestaurant,
               // UserId = mainViewModel.UserFullName
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlRecommendationsController"].ToString();
            var response = await this.apiService.Post(url, prefix, controller, recommendation,Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;

            }

            var newRecommendation = (Recommendation)response.Result;
            var recommendationModel = RecommendationViewModel.GetInstance();
            recommendationModel.MyRecommendations.Add(newRecommendation);
            recommendationModel.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion


    }
}
