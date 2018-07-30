using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.Domain;
using Newtonsoft.Json;
using RestSharp;

namespace IQSoft.PersonList.API.Client
{
    public sealed class PersonListClient : IPersonListClient
    {
        private readonly RestClient _client;

        private static readonly IDictionary<PersonListApiActions, string> _methods =
            new Dictionary<PersonListApiActions, string>()
            {
                {PersonListApiActions.GetAll, "/"},
                {PersonListApiActions.Get, "/{id}"},
                {PersonListApiActions.Add, "/"},
                {PersonListApiActions.Update, "/"},
                {PersonListApiActions.Delete, "/{id}"}
            };

        public PersonListClient()
        {
            // TODO: move client address to configurations.
            _client = new RestClient("http://localhost:5002");
        }

        public GetPersonListQueryResult Get(GetPersonListQuery query)
        {
            return Execute<GetPersonListQueryResult>(PersonListApiActions.GetAll);
        }

        public GetPersonQueryResult Get(GetPersonQuery query)
        {
            return Execute<GetPersonQueryResult>(PersonListApiActions.Get,
                keys: new Dictionary<string, string>() {{"id", query.Id.ToString()}});
        }

        public void Post(AddPersonCommand command)
        {
            Request(PersonListApiActions.Add, Method.POST, command);
        }

        public void Put(UpdatePersonCommand command)
        {
            Request(PersonListApiActions.Update, Method.PUT, command);
        }

        public void Delete(DeletePersonCommand command)
        {
            Request(PersonListApiActions.Delete, Method.DELETE, command);
        }

        private void Request(PersonListApiActions action, Method method, object body)
        {
            IRestRequest request = GetRequest(PersonListApiActions.Add);
            request.Method = method;
            request.AddJsonBody(body);

            var response = _client.Execute(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw JsonConvert.DeserializeObject<ErrorResponse>(response.Content).AsException();
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                // TODO : craete custom exception.
                throw new Exception("Error when request for add person.");
            }
        }

        private T Execute<T>(PersonListApiActions objectType, IDictionary<string, string> parameters = null,
            IDictionary<string, string> keys = null) where T : new()
        {
            var response = _client.Execute<T>(GetRequest(objectType,
                parameters ?? new Dictionary<string, string>(), keys ?? new Dictionary<string, string>()));

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }

        private RestRequest GetRequest(PersonListApiActions objectType,
            IDictionary<string, string> parameters = null,
            IDictionary<string, string> keys = null)
        {
            if (!_methods.ContainsKey(objectType))
                throw new NotImplementedException();

            return GetRequest(_methods[objectType], parameters, keys);
        }

        private static RestRequest GetRequest(
            string url,
            IDictionary<string, string> parameters = null,
            IDictionary<string, string> keys = null)
        {
            var request = new RestRequest(url, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                OnBeforeDeserialization = resp => resp.ContentType = "application/json",
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            if (keys != null)
            {
                foreach (KeyValuePair<String, String> key in keys)
                {
                    request.AddParameter(key.Key, key.Value, ParameterType.UrlSegment);
                }
            }

            if (parameters != null)
            {
                foreach (KeyValuePair<String, String> parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }
            
            return request;
        }
    }
}