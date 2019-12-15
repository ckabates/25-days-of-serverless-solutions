using Day7.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Day7.Services
{
    public interface IImageApiService
    {
        Task<ImageSearchResponse> GetImage(string keywords);
        Task<IEnumerable<ImageSearchResponse>> GetImages(string keywords, int count);
    }
}
