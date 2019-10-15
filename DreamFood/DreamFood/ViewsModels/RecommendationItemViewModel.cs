using DreamFood.Common.Models;
using DreamFood.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DreamFood.ViewsModels
{
    public class RecommendationItemViewModel:Recommendation
    {

        #region Atributes
        private ApiServices apiService;
        #endregion

        #region Constructors
        public RecommendationItemViewModel()
        {
            this.apiService = new ApiServices();
        }
        #endregion

        #region Commands



        #endregion
    }
}

