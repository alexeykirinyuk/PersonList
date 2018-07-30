using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IQSoft.PersonList.API.Client;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.Domain;
using Microsoft.AspNetCore.Mvc;
using IQSoft.PersonList.Web.Models;

namespace IQSoft.PersonList.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonListClient _personListClient;

        public HomeController()
        {
            _personListClient = new PersonListClient();
        }

        public IActionResult Index()
        {
            var result = _personListClient.Get(new GetPersonListQuery());

            return View(result.Persons);
        }

        public IActionResult Add()
        {
            return View("Update", FormModel.FromModel(new Person()));
        }

        public IActionResult Update(int id)
        {
            var result = _personListClient.Get(new GetPersonQuery {Id = id});

            return View(FormModel.FromModel(result.Person));
        }

        [HttpPost]
        public IActionResult Add(FormModel<Person> form)
        {
            try
            {
                _personListClient.Post(new AddPersonCommand {Person = form.Data});
            }
            catch (ValidationException e)
            {
                return View("Update", FormModel.FromModel(form.Data, e));
            }

            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Update(FormModel<Person> form)
        {
            try
            {
                _personListClient.Put(new UpdatePersonCommand {Person = form.Data});
            }
            catch (ValidationException e)
            {
                return View(FormModel.FromModel(form.Data, e));
            }

            return Redirect("/");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Delete(int id)
        {
            _personListClient.Delete(new DeletePersonCommand {Id = id});

            return Redirect("/");
        }
    }
}