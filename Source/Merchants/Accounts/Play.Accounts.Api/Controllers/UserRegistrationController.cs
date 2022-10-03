using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Api.Messages;
using Play.Accounts.Api.Models;
using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Domain.Repositories;
using Play.Merchants.Onboarding.Domain.Aggregates;
using Play.Merchants.Onboarding.Domain.Services;

namespace Play.Accounts.Api.Controllers
{
    public class UserRegistrationController : Controller
    {
        #region Instance Values

        private readonly IEnsureUniqueEmails _UniqueEmailChecker;
        private readonly IRepository<UserRegistration, UserRegistrationId> _UserRegistrationRepository;

        #endregion

        #region Constructor

        public UserRegistrationController(IEnsureUniqueEmails uniqueEmailChecker)
        {
            _UniqueEmailChecker = uniqueEmailChecker;
        }

        #endregion

        #region Instance Members

        // GET: UserRegistrationController
        public Response<UserRegistrationDto> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            Response<UserRegistrationDto> response = new Response<UserRegistrationDto>();

            try
            {
                response.Object = UserRegistration.CreateNewUserRegistration(registerUserRequest, _UniqueEmailChecker).AsDto();

                return response;
            }
            catch (Exception e)
            {
                response.Errored = true;
                response.ErrorMessage = e.Message;

                return response;
            }
        }

        // GET: UserRegistrationController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRegistrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRegistrationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRegistrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion
    }
}