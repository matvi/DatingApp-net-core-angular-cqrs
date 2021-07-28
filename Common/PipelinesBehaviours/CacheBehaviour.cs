using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Common.PipelinesBehaviours
{
    public class CacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICachable
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheBehaviour<TRequest, TResponse>> _logger;

        public CacheBehaviour(IMemoryCache memoryCache, ILogger<CacheBehaviour<TRequest, TResponse>> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;
            
            _logger.LogInformation($"Request {requestName} is configured to cache ");
            TResponse response;
            if (_memoryCache.TryGetValue(request.CacheKey, out response))
            {
                _logger.LogInformation($"Request {requestName} was cached. Returning value from cache");
                return response;
            }

            response = await next();
            
            _logger.LogInformation($"Request {requestName} is being cacheable");
            _memoryCache.Set(request.CacheKey, response);

            return response;

        }
    }
}