using Day7.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Day7.Services
{
    public interface IImageApiService
    {
        Task<ImageSearchResponse> GetRandomImage(string keywords);
    }
}
