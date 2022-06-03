using Castle.DynamicProxy;
using Sentry;
using SentryExample.Core.Helpers;

namespace SentryExample.Core.Tracing
{
    public class SentryInterceptor : IInterceptor
    {
        private readonly IHub _sentryHub;
        public SentryInterceptor(IHub sentryHub) => _sentryHub = sentryHub;

        #region IInterceptor Members         
        public void Intercept(IInvocation invocation)
        {
            var classAndMethodName = $"{invocation.TargetType.Name}-{invocation.Method.Name}-{RandomHelper.RandomLong()}";
            var childSpan = _sentryHub.GetSpan()?.StartChild(classAndMethodName);
            try
            {
                invocation.Proceed();
                childSpan?.Finish(SpanStatus.Ok);
            }
            catch (Exception ex)
            {
                childSpan?.Finish(SpanStatus.InternalError);
            }
        }
        #endregion
    }
}
