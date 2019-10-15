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
    using Xamarin.Forms.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AddRestaurantViewModel : BaseViewModel
    {

        #region Attributes

        private MediaFile file;

        private ImageSource imageSource;

        private ApiServices apiService;

        public bool isRunning;

        public bool isEnabled;

        #endregion

        #region Properties

        public string Name { get; set; }

        public string Type { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Remarks { get; set; }

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

        #endregion

        #region Constructors

        public AddRestaurantViewModel()
        {
            this.apiService = new ApiServices();
            this.IsEnabled = true;
            this.ImageSource = "picture";
        }
        #endregion

        #region Methods
 

        
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
            
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorRestaurant,
                    Languages.Accept);
                return;
            }


            if (string.IsNullOrEmpty(this.Type))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorType,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorPhone,
                    Languages.Accept
                    );
                return;
            }

            var phone = decimal.Parse(this.Phone);
            if (phone < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorPhone,
                    Languages.Accept
                    );
                return;
            }

        if (string.IsNullOrEmpty(this.Address))
        {
            await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameErrorAddress,
                    Languages.Accept
                    );
                return;
        }
        else
        {
                string error;
                try
                {
                    IEnumerable<Xamarin.Forms.Maps.Position> loc;

                    var geoCoder = new Geocoder();
                    loc = await geoCoder.GetPositionsForAddressAsync(this.Address);
                    var firstLoc = loc.FirstOrDefault();
                    this.Latitude = firstLoc.Latitude.ToString();
                    this.Longitude = firstLoc.Longitude.ToString();
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
        }

            var latitude = double.Parse(this.Latitude);
            var longitude = double.Parse(this.Longitude);

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


            var restaurant = new Restaurant
            {

                Name = this.Name,
                Type = this.Type,
                Phone = this.Phone,
                Address = this.Address,
                Latitude = latitude,
                Longitude = longitude,
                ImageArray = imageArray,
                Remarks = this.Remarks

            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlRestaurantsController"].ToString();
            var response = await this.apiService.Post(url, prefix, controller, restaurant, Settings.TokenType, Settings.AccessToken);

            if  (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;

            }

            var newRestaurant = (Restaurant)response.Result;
            var restaurantsModel = RestaurantsViewModel.GetInstance();
            restaurantsModel.MyRestaurants.Add(newRestaurant);
            restaurantsModel.RefreshList(); 
            
            this.IsRunning = false;
            this.IsEnabled = true;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
