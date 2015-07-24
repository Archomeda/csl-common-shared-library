using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Utils;
using NUnit.Framework;

namespace CommonShared.UnitTests.Utils
{
    [TestFixture]
    public class ReflectionUtilsTest
    {
        private string somePrivateField = "";
        private int numberOfTimesPrivateMethodInvokations = 0;
        private static int numberOfTimesPrivateStaticMethodInvokations = 0;

        private void SomePrivateMethod()
        {
            this.numberOfTimesPrivateMethodInvokations++;
        }

        private int SomeOtherPrivateMethod()
        {
            return ++this.numberOfTimesPrivateMethodInvokations;
        }

        private static void SomePrivateStaticMethod()
        {
            numberOfTimesPrivateStaticMethodInvokations++;
        }

        private static int SomeOtherPrivateStaticMethod()
        {
            return ++numberOfTimesPrivateStaticMethodInvokations;
        }

        [SetUp]
        public void Init()
        {
            numberOfTimesPrivateStaticMethodInvokations = 0;
        }

        [Test]
        public void GetPrivateField()
        {
            this.somePrivateField = "Private!";
            string fieldValue = ReflectionUtils.GetPrivateField<string>(this, "somePrivateField");
            Assert.AreEqual(this.somePrivateField, fieldValue);
        }

        [Test]
        public void SetPrivateField()
        {
            this.somePrivateField = "Private!";
            string newFieldValue = "Private too!";
            ReflectionUtils.SetPrivateField(this, "somePrivateField", newFieldValue);
            Assert.AreEqual(newFieldValue, this.somePrivateField);
        }

        [Test]
        public void InvokePrivateMethod()
        {
            ReflectionUtils.InvokePrivateMethod(this, "SomePrivateMethod");
            Assert.AreEqual(1, this.numberOfTimesPrivateMethodInvokations);
        }

        [Test]
        public void InvokePrivateMethodWithReturn()
        {
            int number = ReflectionUtils.InvokePrivateMethod<int>(this, "SomeOtherPrivateMethod");
            Assert.AreEqual(number, this.numberOfTimesPrivateMethodInvokations);
        }

        [Test]
        public void InvokePrivateStaticMethod()
        {
            ReflectionUtils.InvokePrivateStaticMethod(this.GetType(), "SomePrivateStaticMethod");
            Assert.AreEqual(1, numberOfTimesPrivateStaticMethodInvokations);
        }

        [Test]
        public void InvokePrivateStaticMethodWithReturn()
        {
            int number = ReflectionUtils.InvokePrivateStaticMethod<int>(this.GetType(), "SomeOtherPrivateStaticMethod");
            Assert.AreEqual(number, numberOfTimesPrivateStaticMethodInvokations);
        }
    }
}
