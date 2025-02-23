﻿using System.Collections.Generic;
using RestSharp;
using tiv.elasticClient.APIs._cat.Models;
using tiv.elasticClient.Exceptions;
using tiv.elasticClient.ExtensionFunctions;

namespace tiv.elasticClient.APIs._cat
{
    public class CatAPI
    {
        public List<CatResponse> Get(IRestClient client, string indexPattern, out string resourceCall)
        {
            indexPattern = indexPattern.TrimSlashes();

            var resource = $"_cat/indices/{indexPattern}?format=json&pretty=true";

            resourceCall = $"{client.BaseUrl}{resource}";

            var request = new RestRequest(resource, Method.GET) {RequestFormat = DataFormat.Json};
            request.AddHeader("Accept", "application/json");

            var response = client.Execute<List<CatResponse>>(request);

            if (response.IsSuccessful) return response.Data;
            throw new RestCallException(response.StatusCode, response.StatusDescription, response.ErrorMessage, response.ErrorException);
        }

        public bool IndexExists(IRestClient client, string index)
        {
            index = index.TrimSlashes();

            var resource = $"_cat/indices/{index}?format=json&pretty=true";

            var request = new RestRequest(resource, Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("Accept", "application/json");

            var response = client.Execute<List<CatResponse>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data != null && response.Data.Count > 0) return true;

                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            throw new RestCallException(response.StatusCode, response.StatusDescription, response.ErrorMessage, response.ErrorException);
        }
    }
}
