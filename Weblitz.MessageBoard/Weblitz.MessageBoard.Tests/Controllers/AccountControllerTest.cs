using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using NUnit.Framework;
using Weblitz.MessageBoard.Web.Controllers;
using Weblitz.MessageBoard.Web.Models;

namespace Weblitz.MessageBoard.Tests.Controllers
{

    [TestFixture]
    public class AccountControllerTest
    {

        [Test]
        public void ChangePassword_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePassword();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData["PasswordLength"], Is.EqualTo(10));
        }

        [Test]
        public void ChangePassword_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel()
            {
                OldPassword = "goodOldPassword",
                NewPassword = "goodNewPassword",
                ConfirmPassword = "goodNewPassword"
            };

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
            RedirectToRouteResult redirectResult = result as RedirectToRouteResult;
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("ChangePasswordSuccess"));
        }

        [Test]
        public void ChangePassword_Post_ReturnsViewIfChangePasswordFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel()
            {
                OldPassword = "goodOldPassword",
                NewPassword = "badNewPassword",
                ConfirmPassword = "badNewPassword"
            };

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.EqualTo(model));
            Assert.That(
                controller.ModelState[""].Errors[0].ErrorMessage,
                Is.EqualTo("The current password is incorrect or the new password is invalid.")
            );
            Assert.That(viewResult.ViewData["PasswordLength"], Is.EqualTo(10));
        }

        [Test]
        public void ChangePassword_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel()
            {
                OldPassword = "goodOldPassword",
                NewPassword = "goodNewPassword",
                ConfirmPassword = "goodNewPassword"
            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.EqualTo(model));
            Assert.That(viewResult.ViewData["PasswordLength"], Is.EqualTo(10));
        }

        [Test]
        public void ChangePasswordSuccess_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePasswordSuccess();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void LogOff_LogsOutAndRedirects()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOff();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
            RedirectToRouteResult redirectResult = result as RedirectToRouteResult;
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(
                ((MockFormsAuthenticationService)controller.FormsService).SignOut_WasCalled,
                Is.True
            );
        }

        [Test]
        public void LogOn_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOn();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithoutReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
            RedirectToRouteResult redirectResult = result as RedirectToRouteResult;
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(
                ((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled,
                Is.True
            );
        }

        [Test]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, "/someUrl");

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectResult>());
            RedirectResult redirectResult = result as RedirectResult;
            Assert.That(redirectResult.Url, Is.EqualTo("/someUrl"));
            Assert.That(
                ((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled,
                Is.True
            );
        }

        [Test]
        public void LogOn_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.EqualTo(model));
        }

        [Test]
        public void LogOn_Post_ReturnsViewIfValidateUserFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "badPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.EqualTo(model));
            Assert.That(
                controller.ModelState[""].Errors[0].ErrorMessage,
                Is.EqualTo("The user name or password provided is incorrect.")
            );
        }

        [Test]
        public void Register_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.Register();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData["PasswordLength"], Is.EqualTo(10));
        }

        [Test]
        public void Register_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel()
            {
                UserName = "someUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
            RedirectToRouteResult redirectResult = result as RedirectToRouteResult;
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Register_Post_ReturnsViewIfRegistrationFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel()
            {
                UserName = "duplicateUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.EqualTo(model));
            Assert.That(
                controller.ModelState[""].Errors[0].ErrorMessage,
                Is.EqualTo("Username already exists. Please enter a different user name.")
            );
            Assert.That(viewResult.ViewData["PasswordLength"], Is.EqualTo(10));
        }

        [Test]
        public void Register_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel()
            {
                UserName = "someUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.EqualTo(model));
            Assert.That(viewResult.ViewData["PasswordLength"], Is.EqualTo(10));
        }

        private static AccountController GetAccountController()
        {
            AccountController controller = new AccountController()
            {
                FormsService = new MockFormsAuthenticationService(),
                MembershipService = new MockMembershipService()
            };
            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return controller;
        }

        private class MockFormsAuthenticationService : IFormsAuthenticationService
        {
            public bool SignIn_WasCalled;
            public bool SignOut_WasCalled;

            public void SignIn(string userName, bool createPersistentCookie)
            {
                // verify that the arguments are what we expected
                Assert.AreEqual("someUser", userName);
                Assert.IsFalse(createPersistentCookie);

                SignIn_WasCalled = true;
            }

            public void SignOut()
            {
                SignOut_WasCalled = true;
            }
        }

        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }

        private class MockMembershipService : IMembershipService
        {
            public int MinPasswordLength
            {
                get { return 10; }
            }

            public bool ValidateUser(string userName, string password)
            {
                return (userName == "someUser" && password == "goodPassword");
            }

            public MembershipCreateStatus CreateUser(string userName, string password, string email)
            {
                if (userName == "duplicateUser")
                {
                    return MembershipCreateStatus.DuplicateUserName;
                }

                // verify that values are what we expected
                Assert.AreEqual("goodPassword", password);
                Assert.AreEqual("goodEmail", email);

                return MembershipCreateStatus.Success;
            }

            public bool ChangePassword(string userName, string oldPassword, string newPassword)
            {
                return (userName == "someUser" && oldPassword == "goodOldPassword" && newPassword == "goodNewPassword");
            }
        }

    }

} // namespace Weblitz.MessageBoard.Tests.Controllers
