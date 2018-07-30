using MediatR;
using Nancy.TinyIoc;

namespace IQSoft.PersonList.Server.Extensions
{
    public static class TinyIoContainerExtensions
    {
        public static void RegisterQuery<TQuery, TQueryResult, TQueryHandler>(this TinyIoCContainer container)
            where TQuery : IRequest<TQueryResult>
            where TQueryHandler : class, IRequestHandler<TQuery, TQueryResult>
        {
            container.Register<IRequestHandler<TQuery, TQueryResult>, TQueryHandler>()
                .AsMultiInstance();
        }
    }
}