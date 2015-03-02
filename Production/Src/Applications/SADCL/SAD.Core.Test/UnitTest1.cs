using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SAD.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var angler = new TargetPositionCalculator();

            var angles = angler.Calculate(1, 2, 3);

            Assert.IsTrue(angles.Phi - 56);
            Assert.IsTrue(angles.Theta == 10);

        }

    }
}
