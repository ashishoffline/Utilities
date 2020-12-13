using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Utilities.Extensions;

namespace Utilities.Tests
{
    [TestClass()]
    public class UtilityHelperTests
    {
        [TestMethod()]
        public void GetEmbeddedResourceTest()
        {
            try
            {
                string content = UtilityHelper.GetEmbeddedResource("Utilities.Test.TestFile.txt");
                Assert.IsNotNull(content);
            }
            catch (System.Exception)
            {
                Assert.Fail();
            }
            
        }
    }
}