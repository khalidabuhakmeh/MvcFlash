using System.Dynamic;
using MvcFlash.Core.Helpers;
using NUnit.Framework;

namespace MvcFlash.Tests
{
    [TestFixture]
    public class DynamicHelpersTests
    {
        [Test]
        public void Can_Convert_Anonymous_To_Expando()
        {
            var target = new {id = 1};
            var result = DynamicHelpers.IfAnonymousCovertToExpando(target);

            Assert.IsInstanceOf<ExpandoObject>(result);
        }

        [Test]
        public void Does_Not_Try_And_Convert_Non_Anonymous_Objects()
        {
            const string target = "hello";
            var result = DynamicHelpers.IfAnonymousCovertToExpando(target);

            Assert.IsNotInstanceOf<ExpandoObject>(target);
            Assert.AreSame(target, result);
        }

        [Test]
        public void Will_Return_Null_If_Null_Given()
        {
            var result = DynamicHelpers.IfAnonymousCovertToExpando(null);
            Assert.IsNull(result);
        }
    }
}
