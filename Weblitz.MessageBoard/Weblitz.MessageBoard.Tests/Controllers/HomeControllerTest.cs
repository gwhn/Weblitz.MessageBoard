using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Weblitz.MessageBoard.Web;
using Weblitz.MessageBoard.Web.Controllers;

namespace Weblitz.MessageBoard.Tests.Controllers
{

    [TestFixture]
    public class HomeControllerTest
    {

        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            ViewDataDictionary viewData = result.ViewData;
            Assert.That(viewData["Message"], Is.EqualTo("Welcome to ASP.NET MVC!"));
        }

        [Test]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

    }

} // namespace Weblitz.MessageBoard.Tests.Controllers
