using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Queue.Service.Interfaces;

namespace Queue.Service.Services
{
    public class ApplicationService:IApplicationService
    {
        private readonly System.Net.Http.HttpClient _client;
        private readonly string _url;

        public ApplicationService(string url)
        {
            _client = new System.Net.Http.HttpClient();
            _url = url;
        }

        public async Task<bool> SendData<T>(T model, string endPointSuffix = null) where T : class
        {
            try
            {   
                HttpContent jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(ReturnEndPoint(_url, endPointSuffix), jsonModel);
                if (response == null)
                    throw new Exception("null return api car");

                if (!string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result) && response.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ICollection<T>> GetData<T>() where T : class
        {
            try
            {
                var response = await _client.GetAsync(_url);
                if (response == null)
                    throw new Exception("null return api car");
                var responseString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                    return JsonSerializer.Deserialize<ICollection<T>>(responseString);
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateData<T>(T model, string id, string endPointSuffix = null) where T : class
        {
            try
            {
                HttpContent jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(ReturnEndPoint(_url, endPointSuffix) + $"/{id}", jsonModel);
                if (response == null)
                    throw new Exception("null return api car");

                if (!string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteData(string id, string endPointSuffix = null)
        {
            try
            {
                var response = await _client.DeleteAsync(ReturnEndPoint(_url, endPointSuffix) + $"/{id}");
                if (response == null)
                    throw new Exception("null return api car");

                if (!string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string ReturnEndPoint(string baseUrl, string endPointSuffix = null) => string.IsNullOrWhiteSpace(endPointSuffix) ? baseUrl : baseUrl + endPointSuffix;
    }
}
