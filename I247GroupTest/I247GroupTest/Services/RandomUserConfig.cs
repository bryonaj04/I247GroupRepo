using System;
using I247GroupTest.Interfaces;

namespace I247GroupTest.Services
{
    public class RandomUserConfig : IRandomUserConfigWrapper
    {
        private readonly IConfiguration _configuration;

        public RandomUserConfig (IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int UserLimit => _configuration.GetValue<int>("UserLimit");

     
        public int GetValue()
        {
            return UserLimit;
        }
    }
}

