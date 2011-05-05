using System.Collections.Generic;
using System.Web;
using Moq;
using MvcFlash.Core.Providers;
using NUnit.Framework;

namespace MvcFlash.Tests
{
    [TestFixture]
    public class HttpContextFlashMessageServiceTests
    {
        [Test]
        public void Is_Of_Type_IFlashMessagePusher()
        {
            var mock = new Mock<HttpContextBase>();
            var target = new HttpContextFlashMessageService(mock.Object);

            Assert.IsInstanceOf<IFlashMessagePusher>(target);
        }

        [Test]
        public void Is_Of_Type_IFlashMessagePopper()
        {
            var mock = new Mock<HttpContextBase>();
            var target = new HttpContextFlashMessageService(mock.Object);

            Assert.IsInstanceOf<IFlashMessagePopper>(target);
        }

        [Test]
        public void Can_Create_By_Passing_My_Own_Context()
        {
            var mock = new Mock<HttpContextBase>();
            var target = new HttpContextFlashMessageService(mock.Object);

            Assert.IsNotNull(target);
        }

        [Test]
        [ExpectedException(ExpectedMessage = "you are not currently running where HttpContext is accessible", MatchType = MessageMatch.Contains)]
        public void Can_Create_By_Passing_Nothing_And_Default_Context_Be_Used()
        {
            var target = new HttpContextFlashMessageService();

            Assert.IsNotNull(target);
        }

        [Test]
        public void Can_Push_A_Message()
        {
            var mock = new Mock<HttpContextBase>();

            mock.Setup(m => m.Items).Returns(new Dictionary<object, object>());

            var message = new {type = "success", message = "Hooray!"};
            var service = new HttpContextFlashMessageService(mock.Object);

            service.Push(message);
        }

        [Test]
        public void Can_Pop_Messages()
        {
            var mock = new Mock<HttpContextBase>();

            mock.Setup(m => m.Items).Returns(new Dictionary<object, object>());

            var message = new { type = "success", message = "Hooray!" };
            var service = new HttpContextFlashMessageService(mock.Object);

            service.Push(message);

            var popped = service.Pop();

            Assert.IsNotNull(popped);
            Assert.IsTrue(popped.Count == 1);
        }

        [Test]
        public void Can_Clear_Messages()
        {
            var mock = new Mock<HttpContextBase>();

            mock.Setup(m => m.Items).Returns(new Dictionary<object, object>());

            var message = new { type = "success", message = "Hooray!" };
            var service = new HttpContextFlashMessageService(mock.Object);

            service.Push(message);
            service.Clear();
            var popped = service.Pop();

            Assert.IsNotNull(popped);
            Assert.IsTrue(popped.Count == 0);
        }
    }
}
