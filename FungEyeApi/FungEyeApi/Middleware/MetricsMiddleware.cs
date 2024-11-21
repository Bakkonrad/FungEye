using Prometheus;

namespace FungEyeApi.Middleware
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Counter _requestCounter;
        private readonly Histogram _responseTimeHistogram;

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
            _requestCounter = Metrics.CreateCounter(
                "fungeye_api_requests_total",
                "Total number of requests",
                new CounterConfiguration
                {
                    LabelNames = new[] { "method", "endpoint", "status_code" }
                }
            );
            
            _responseTimeHistogram = Metrics.CreateHistogram(
                "fungeye_api_response_time_seconds",
                "Response time in seconds",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "method", "endpoint" },
                    Buckets = new[] { .001, .005, .01, .025, .05, .075, .1, .25, .5, .75, 1, 2.5, 5, 7.5, 10 }
                }
            );
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            var method = context.Request.Method;
            var startTime = DateTime.UtcNow;

            try
            {
                await _next(context);
            }
            finally
            {
                var statusCode = context.Response.StatusCode.ToString();
                _requestCounter.Labels(method, path, statusCode).Inc();

                var duration = (DateTime.UtcNow - startTime).TotalSeconds;
                _responseTimeHistogram.Labels(method, path).Observe(duration);
            }
        }
    }
}