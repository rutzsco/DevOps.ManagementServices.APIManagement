using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using DevOps.ManagementServices.APIManagement.Model;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DevOps.ManagementServices.APIManagement
{
    public class CaptureEndpoint
    {
        private static HttpClient _httpClient;

        [FunctionName("CaptureEndpoint")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ExecutionContext context, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            var config = context.BuildConfiguraion();
            InitializeHttpClient(config);

            var resultString = await _httpClient.GetStringAsync($"/contentTypes?api-version=2018-06-01-preview");
            var contentTypes = JsonConvert.DeserializeObject<ContentTypeResponse>(resultString);
            var contentTypeIds = contentTypes.value.Select(x => x.id.Replace("/contentTypes/", ""));

            var contentItems = new List<string>();

            JArray jsonArray = new JArray();

            foreach (var contentType in contentTypeIds)
            {
                var contentItemsPayload = await _httpClient.GetStringAsync($"contentTypes/{contentType}/contentItems?api-version=2018-06-01-preview");
                jsonArray.Add(JObject.Parse(contentItemsPayload));
            }

            return new OkObjectResult(jsonArray);
        }

        private void InitializeHttpClient(IConfigurationRoot config)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(config["SourceServiceUrl"]);
                _httpClient.DefaultRequestHeaders.Add("Authorization", config["SourceServiceToken"]);
            }
        }
    }
}
