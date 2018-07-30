using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using Autofac;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.CQRS.Queries;
using IQSoft.PersonList.CQRS.Queries.GetPerson;
using IQSoft.PersonList.CQRS.Queries.GetPersonList;
using IQSoft.PersonList.Server.Dal;
using IQSoft.PersonList.Server.Extensions;
using LiteGuard;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nancy;
using Nancy.Bootstrappers.Autofac;
using Nancy.TinyIoc;

namespace IQSoft.PersonList.Server
{
    public sealed class CustomNancyBootstrapper : AutofacNancyBootstrapper
    {
        private readonly string _connectionString;

        public CustomNancyBootstrapper(string connectionString)
        {
            Guard.AgainstNullArgument(nameof(connectionString), connectionString);

            _connectionString = connectionString;
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            container.Update(c =>
            {
                RegisterContext(c);
                RegisterMediator(c);
                RegisterRepositories(c);
            });
        }

        private void RegisterContext(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var opt = new DbContextOptionsBuilder<PersonListContext>();
                    opt.UseSqlServer(_connectionString);

                    return new PersonListContext(opt.Options);
                })
                .AsSelf()
                .InstancePerLifetimeScope();
        }

        private static void RegisterRepositories(ContainerBuilder c)
        {
            c.RegisterType<PersonRepository>()
                .As<IPersonRepository>()
                .InstancePerRequest();
        }

        private static void RegisterMediator(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Ping).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();

                builder
                    .RegisterAssemblyTypes(typeof(GetPersonListQueryHandler).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}