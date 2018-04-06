using Microsoft.AspNetCore.Mvc;
using SecureShare.Webapp.Controllers;
using Xunit;

namespace SecureShare.Test
{
    public class HomeControllerTest : ControllerBase
    {
        private readonly HomeController _controllerUnderTest;


        public HomeControllerTest()
        {
            _controllerUnderTest = new HomeController();
        }


        [Fact]
        public void Returns_Index_View()
        {
            // Act
            var result = _controllerUnderTest.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(result, viewResult);
        }

        [Fact]
        public void Returns_Pricing_View()
        {
            // Act
            var result = _controllerUnderTest.Pricing();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(result, viewResult);
        }

        [Fact]
        public void Returns_About_View()
        {
            // Act
            var result = _controllerUnderTest.About();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(result, viewResult);
        }
    }
}