using KancelarWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Controllers
{
    //public class ResiliencyHelper
    //{
    //    private IDochazkaProvider _logger;

    //    public ResiliencyHelper(IDochazkaProvider logger)
    //    {
    //        _logger = logger;
    //    }

    //    public async Task<IActionResult> ExecuteResilient(Func<Task<IActionResult>> action, IActionResult fallbackResult)
    //    {
    //        var retryPolicy = Policy
    //            .Handle<Exception>((ex) =>
    //            {
    //                //_logger.LogWarning($"Error occured during request-execution. Polly will retry. Exception: {ex.Message}");
    //                return true;
    //            })
    //            .RetryAsync(5);

    //        var fallbackPolicy = Policy<IActionResult>
    //            .Handle<Exception>()
    //            .FallbackAsync(
    //                fallbackResult,
    //                (e, c) => Task.Run(() => null));
    //                //(e, c) => Task.Run(() => _logger.LogError($"Error occured during request-execution. Polly will fallback. Exception: {e.Exception.ToString()}")));

    //        return await fallbackPolicy
    //            .WrapAsync(retryPolicy)
    //            .ExecuteAsync(action)
    //            .ConfigureAwait(false);
    //    }

       
    //}
}
