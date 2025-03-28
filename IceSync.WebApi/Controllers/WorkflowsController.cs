using IceSync.Common.Contracts;
using IceSync.Common.Contracts.UniLoader.Workflow;
using IceSync.WebApp.Contracts;
using Microsoft.AspNetCore.Mvc;
using UniversalLoaderClient.Contracts;

namespace IceSync.WebApi.Controllers
{
    [ApiController]
    [Route("workflows")]
    public class WorkflowsController(ILogger<WorkflowsController> logger, IUniLoaderClientBuilder clientBuilder, IAuthCacheService authCacheService)
        : BaseController
    {
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            BaseResult<List<Workflow>> uniLoaderResponse;
            try
            {
                var authToken = await authCacheService.GetBearerTokenAsync();
                var client = clientBuilder.Build(authToken);
                
                uniLoaderResponse = await client.Workflows.AllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occured.");
                return Failure();
            }

            if (!uniLoaderResponse.Success || uniLoaderResponse.Data is null)
            {
                return Failure($"{uniLoaderResponse?.Error?.Message ?? "Couldn't reach UniLoader API"}.");
            }

            return Success(uniLoaderResponse.Data);
        }
        
        [HttpPost("{workflowId:int}/run")]
        public async Task<IActionResult> Run(int workflowId)
        {
            BaseResult uniLoaderResponse;
            try
            {
                var authToken = await authCacheService.GetBearerTokenAsync();
                var client = clientBuilder.Build(authToken);
                
                uniLoaderResponse = await client.Workflows.RunAsync(workflowId, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occured.");
                return Failure();
            }

            return uniLoaderResponse.Success
                ? Success()
                : Failure($"{uniLoaderResponse?.Error?.Message ?? "Couldn't reach UniLoader API"}.");
        }
    }
}