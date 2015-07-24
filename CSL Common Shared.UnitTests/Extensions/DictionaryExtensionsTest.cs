using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonShared.Extensions;

namespace CommonShared.UnitTests.Extensions
{
    [TestFixture]
    public class DictionaryExtensionsTest
    {
        private IEnumerable TryGetValueOrDefaultData
        {
            get
            {
                yield return new TestCaseData(new Dictionary<int, int>() { { 1, 2 }, { 3, 4 } }, 1, 10).Returns(2);
                yield return new TestCaseData(new Dictionary<int, int>() { { 1, 2 }, { 3, 4 } }, 3, 10).Returns(4);
                yield return new TestCaseData(new Dictionary<int, int>() { { 1, 2 }, { 3, 4 } }, 5, 10).Returns(10);
                yield return new TestCaseData(new Dictionary<int, int>(), 0, 10).Returns(10);
            }
        }

        [Test, TestCaseSource("TryGetValueOrDefaultData")]
        public int TryGetValueOrDefault(IDictionary<int, int> dictionary, int key, int defaultValue)
        {
            int value;
            dictionary.TryGetValueOrDefault(key, defaultValue, out value);
            return value;
        }
    }
}
