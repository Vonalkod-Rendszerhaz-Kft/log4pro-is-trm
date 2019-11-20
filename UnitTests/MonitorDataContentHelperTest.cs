using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Log4Pro.IS.TRM;
using System.Collections.Generic;
using System.Xml.Linq;

namespace UnitTests
{
    [TestClass]
    public class MonitorDataContentHelperTest
    {
        [TestMethod]
        public void GetXML()
        {
            var dataList = new Dictionary<string, string>()
            {
                { "D1", "1" },
                { "D2", "2" },
                { "D3", "3" },
            };
            var xml = MonitorDataContentHelper.CreateContent(dataList);
            int i = 0;
            foreach (var item in xml.Elements())
            {
                i++;
                Assert.AreEqual($"D{i}", item.Name.LocalName);
                Assert.AreEqual($"{i}", item.Value);
            }
            Assert.AreEqual(3, i);
        }
    }
}
