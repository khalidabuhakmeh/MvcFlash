using System.Collections.Generic;
using System.Web;
using Moq;
using MvcFlash.Core;
using MvcFlash.Core.Providers;
using NUnit.Framework;

namespace MvcFlash.Tests
{
    [TestFixture]
    public class FlashTests
    {
        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<HttpContextBase>();
            mock.Setup(context => context.Items).Returns(new Dictionary<object, object>());

            FlashConfiguration.WithContext(() => mock.Object);
            FlashConfiguration.WithService(() => new HttpContextFlashMessageService(mock.Object));
        }

        [Test]
        public void Can_Configure_Flash()
        {
            var message = new {message = "hello"};
            Flash.Push(message);
        }

        [Test]
        public void Can_Push_Message()
        {
            Can_Configure_Flash();
        }

        [Test]
        public void Can_Create_Success_Message()
        {
            var message = Flash.Success("Whoopee!");
            Assert.IsNotNull(message);
            Assert.IsTrue(message.Text == "Whoopee!");
            Assert.IsTrue(message.Type == "success");
        }

        [Test]
        public void Can_Create_Error_Message()
        {
            var message = Flash.Error("Oh No!");
            Assert.IsNotNull(message);
            Assert.IsTrue(message.Text == "Oh No!");
            Assert.IsTrue(message.Type == "error");
        }

        [Test]
        public void Can_Create_Notice_Message()
        {
            var message = Flash.Notice("Umm, Yeah...");
            Assert.IsNotNull(message);
            Assert.IsTrue(message.Text == "Umm, Yeah...");
            Assert.IsTrue(message.Type == "notice");
        }

        [Test]
        public void Can_Create_Warning_Message()
        {
            var message = Flash.Warning("Hold up partner!");
            Assert.IsNotNull(message);
            Assert.IsTrue(message.Text == "Hold up partner!");
            Assert.IsTrue(message.Type == "warning");
        }
    }
}
