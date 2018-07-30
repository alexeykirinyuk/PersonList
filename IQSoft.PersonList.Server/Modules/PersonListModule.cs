using System;
using System.Threading.Tasks;
using System.Timers;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.Domain;
using LiteGuard;
using MediatR;
using Nancy;
using Nancy.ModelBinding;

namespace IQSoft.PersonList.Server.Modules
{
    public sealed class PersonListModule : NancyModule
    {
        private readonly IMediator _mediator;

        public PersonListModule(IMediator mediator)
        {
            Guard.AgainstNullArgument(nameof(mediator), mediator);

            _mediator = mediator;

            RegisterRequests();
        }

        private void RegisterRequests()
        {
            Get("/", _ => _mediator.Send(new GetPersonListQuery()));
            Get("/{id:int}", data => _mediator.Send(new GetPersonQuery {Id = data.id}));
            Post("/", data => WithValidation(() => _mediator.Send(this.Bind<AddPersonCommand>())));
            Put("/", data => WithValidation(() => _mediator.Send(this.Bind<UpdatePersonCommand>())));
            Delete("/", data => WithValidation(() => _mediator.Send(this.Bind<DeletePersonCommand>())));
        }

        private  async Task<object> WithValidation(Func<Task<Unit>> action)
        {
            try
            {
                await action();
                return new Response {StatusCode = HttpStatusCode.OK};
            }
            catch (ValidationException e)
            {
                return Response.AsJson(e.AsResponse(), HttpStatusCode.BadRequest);
            }
        }
    }
}