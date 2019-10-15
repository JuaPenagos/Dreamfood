
namespace DreamFood.ViewsModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using DreamFood.Helpers;
    using DreamFood.Common.Models;
    using DreamFood.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;



    public class RestaurantsViewModel : BaseViewModel
    {
        #region Attributes
        private ApiServices apiService;

        private ObservableCollection<RestaurantsItemViewModel> restaurants;

        private bool isRefreshing;

        private string filter;
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

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<RestaurantsItemViewModel> Restaurants
        {
            get { return this.restaurants; }
            set { this.SetValue(ref this.restaurants, value); }
        }

        #endregion

        #region Constructors
        public RestaurantsViewModel()
        {
            instance = this;
            this.apiService = new ApiServices();

            this.LoadRestaurants();
            
        }
        #endregion

        #region Singleton
        private static RestaurantsViewModel instance;

        public static RestaurantsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new RestaurantsViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
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
            var response = await this.apiService.GetList<Restaurant>(url, prefix, controller,Settings.TokenType,Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            };

            this.MyRestaurants = (List<Restaurant>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadRestaurants);
            }
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListRestaurantsItemViewModel = MyRestaurants.Select(r => new RestaurantsItemViewModel
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
            else
            {
                var myListRestaurantsItemViewModel = MyRestaurants.Select(r => new RestaurantsItemViewModel
                {
                    IdRestaurant = r.IdRestaurant,
                    Name = r.Name,
                    Type = r.Type,
                    Remarks = r.Remarks,
                    Phone = r.Phone,
                    Address = r.Address,
                    ImagePathMenu = r.ImagePathMenu,
                    ImageArray=r.ImageArray,
                    

                }).Where(r => r.Name.ToLower().Contains(this.Filter.ToLower())).ToList(); ;

                this.Restaurants = new ObservableCollection<RestaurantsItemViewModel>(
                    myListRestaurantsItemViewModel.OrderBy(r => r.Name));

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
