namespace DreamFood.Helpers
{
    using Xamarin.Forms;
    using Resources;
    using Interfaces;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string Restaurants
        {
            get { return Resource.Restaurants; }
        }
        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }
        public static string Search
        {
            get { return Resource.Search; }
        }

        public static string ImageSource
        {
            get { return Resource.ImageSource; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string NewPicture
        {
            get { return Resource.NewPicture; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string AddRecommendations
        {
            get { return Resource.AddRecommendations; }
        }


        public static string AddRestaurant
        {
            get { return Resource.AddRestaurant; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string Save
        {
            get { return Resource.Save; }
        }

        public static string Login
        {
            get { return Resource.Login; }
        }

        public static string EMail
        {
            get { return Resource.EMail; }
        }

        public static string EmailPlaceHolder
        {
            get { return Resource.EmailPlaceHolder; }
        }

        public static string Password
        {
            get { return Resource.Password; }
        }

        public static string PasswordPlaceHolder
        {
            get { return Resource.PasswordPlaceHolder; }
        }

        public static string Register
        {
            get { return Resource.Register; }
        }

        public static string FirstName
        {
            get { return Resource.FirstName; }
        }

        public static string FirstNamePlaceholder
        {
            get { return Resource.FirstNamePlaceholder; }
        }

        public static string LastName
        {
            get { return Resource.LastName; }
        }

        public static string LastNamePlaceholder
        {
            get { return Resource.LastNamePlaceholder; }
        }

        public static string Phone
        {
            get { return Resource.Phone; }
        }

        public static string PhonePlaceHolder
        {
            get { return Resource.PhonePlaceHolder; }
        }

        public static string PasswordConfirm
        {
            get { return Resource.PasswordConfirm; }
        }

        public static string PasswordConfirmPlaceHolder
        {
            get { return Resource.PasswordConfirmPlaceHolder; }
        }

        public static string FirstNameError
        {
            get { return Resource.FirstNameError; }
        }

        public static string LastNameError
        {
            get { return Resource.LastNameError; }
        }

        public static string EMailError
        {
            get { return Resource.EMailError; }
        }

        public static string PhoneError
        {
            get { return Resource.PhoneError; }
        }

        public static string TypeRestaurantError
        {
            get { return Resource.TypeRestaurantError; }
        }

        public static string DistanceRestaurantError
        {
            get { return Resource.DistanceRestaurantError; }
        }

        public static string WordKeyError
        {
            get { return Resource.WordKeyError; }
        }

        public static string PasswordError
        {
            get { return Resource.PasswordError; }
        }

        public static string PasswordConfirmError
        {
            get { return Resource.PasswordConfirmError; }
        }

        public static string PasswordsNoMatch
        {
            get { return Resource.PasswordsNoMatch; }
        }

        public static string RegisterConfirmation
        {
            get { return Resource.RegisterConfirmation; }
        }

        public static string Confirm
        {
            get { return Resource.Confirm; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }

        public static string SomethingWrong
        {
            get { return Resource.SomethingWrong; }
        }
        public static string RestaurantName
        {
            get { return Resource.RestaurantName; }
        }
        public static string RestaurantType
        {
            get { return Resource.RestaurantType; }
        }
        public static string RestaurantAddress
        {
            get { return Resource.RestaurantAddress; }
        }
        public static string RestaurantRemarks
        {
            get { return Resource.RestaurantRemarks; }
        }
        public static string RestaurantPhone
        {
            get { return Resource.RestaurantPhone; }
        }
        public static string PlaceholderRestaurantName
        {
            get { return Resource.PlaceholderRestaurantName; }
        }
        public static string PlaceholderRestaurantPhone
        {
            get { return Resource.PlaceholderRestaurantPhone; }
        }
        public static string PlaceholderRestaurantAddress
        {
            get { return Resource.PlaceholderRestaurantAddress; }
        }
        public static string PlaceholderRestaurantType
        {
            get { return Resource.PlaceholderRestaurantType; }
        }
        public static string Map
        {
            get { return Resource.Map; }
        }

        public static string Exit
        {
            get { return Resource.Exit; }
        }

        public static string SelectRestaurantPlaceHolder
        {
            get { return Resource.SelectRestaurantPlaceHolder; }
        }
        public static string ValorationPlaceHolder
        {
            get { return Resource.ValorationPlaceHolder; }
        }
        public static string RecomendationPlaceHolder
        {
            get { return Resource.RecomendationPlaceHolder; }
        }
        public static string Restaurant
        {
            get { return Resource.Restaurant; }
        }
        public static string Score
        {
            get { return Resource.Score; }
        }
        public static string Recommendation
        {
            get { return Resource.Recommendation; }
        }
        public static string NameErrorScore
        {
            get { return Resource.NameErrorScore; }
        }
        public static string NameErrorRestaurant
        {
            get { return Resource.NameErrorRestaurant; }
        }
        public static string NameErrorrRecomendacion
        {
            get { return Resource.NameErrorrRecomendacion; }
        }
        public static string NameErrorType
        {
            get { return Resource.NameErrorType; }
        }
        public static string NameErrorPhone
        {
            get { return Resource.NameErrorPhone; }
        }
        public static string NameErrorAddress
        {
            get { return Resource.NameErrorAddress; }
        }
    }
}