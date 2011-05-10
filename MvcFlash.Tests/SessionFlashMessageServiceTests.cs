using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using MvcFlash.Core.Providers;
using NUnit.Framework;

namespace MvcFlash.Tests
{
    public class SessionFlashMessageServiceTests
    {
        [Test]
        public void Is_Of_Type_IFlashMessagePusher()
        {
            var mock = new Mock<HttpContextBase>();
            var target = new SessionFlashMessageService(mock.Object);

            Assert.IsInstanceOf<IFlashMessagePusher>(target);
        }

        [Test]
        public void Is_Of_Type_IFlashMessagePopper()
        {
            var mock = new Mock<HttpContextBase>();
            var target = new SessionFlashMessageService(mock.Object);

            Assert.IsInstanceOf<IFlashMessagePopper>(target);
        }

        [Test]
        public void Can_Create_By_Passing_My_Own_Context()
        {
            var mock = new Mock<HttpContextBase>();
            var target = new SessionFlashMessageService(mock.Object);

            Assert.IsNotNull(target);
        }

        [Test]
        [ExpectedException(ExpectedMessage = "you are not currently running where HttpContext is accessible", MatchType = MessageMatch.Contains)]
        public void Can_Create_By_Passing_Nothing_And_Default_Context_Be_Used()
        {
            var target = new SessionFlashMessageService();

            Assert.IsNotNull(target);
        }

        [Test]
        public void Can_Push_A_Message()
        {
            var mock = new Mock<HttpContextBase>();
            var session = new HttpSessionMock();

            mock.Setup(m => m.Session).Returns(session);

            var message = new { type = "success", message = "Hooray!" };
            var service = new SessionFlashMessageService(mock.Object);

            service.Push(message);
        }

        [Test]
        public void Can_Pop_Messages()
        {
            var mock = new Mock<HttpContextBase>();
            var session = new HttpSessionMock();

            mock.Setup(m => m.Session).Returns(session);

            var message = new { type = "success", message = "Hooray!" };
            var service = new SessionFlashMessageService(mock.Object);

            service.Push(message);

            var popped = service.Pop();

            Assert.IsNotNull(popped);
            Assert.IsTrue(popped.Count == 1);
        }

        [Test]
        public void Can_Clear_Messages()
        {
            var mock = new Mock<HttpContextBase>();
            var session = new HttpSessionMock();
            mock.Setup(m => m.Session).Returns(session);

            var message = new { type = "success", message = "Hooray!" };
            var service = new SessionFlashMessageService(mock.Object);

            service.Push(message);
            service.Clear();
            var popped = service.Pop();

            Assert.IsNotNull(popped);
            Assert.IsTrue(popped.Count == 0);
        }

        [Test]
        public void Can_Select_One_From_Other_Messages()
        {
            var mock = new Mock<HttpContextBase>();
            var session = new HttpSessionMock();

            mock.Setup(m => m.Session).Returns(session);

            var message1 = new { type = "success", message = "Hooray!" };
            var message2 = new { type = "success", message = "Hooray!" };
            var message3 = new { type = "different", message = "Different!" };

            var service = new SessionFlashMessageService(mock.Object);

            service.Push(message1);
            service.Push(message2);
            service.Push(message3);

            var popped = service.Select(x => x.type == "different");

            Assert.IsNotNull(popped);
            Assert.IsTrue(popped.Count == 1);
            Assert.IsTrue(popped.First().type == "different");

            var others = service.Pop();

            Assert.IsNotNull(others);
            Assert.IsTrue(others.Count == 2);
            Assert.IsTrue(others.First().type == "success");
        }
    }

    /// <summary>
    /// HTTP session mockup.
    /// </summary>
    internal sealed class HttpSessionMock : HttpSessionStateBase
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        public override object this[string name]
        {
            get { return (objects.ContainsKey(name)) ? objects[name] : null; }
            set { objects[name] = value; }
        }
    }
}
