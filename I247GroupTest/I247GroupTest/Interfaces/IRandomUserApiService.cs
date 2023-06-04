using System;
using I247GroupTest.Models;

namespace I247GroupTest.Interfaces
{
	public interface IRandomUserApiService
	{
        Task<List<RequestedRandomUserDataModel>> GetRandomUserDataFromApi();
    }
}

