using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Log4Pro.IS.TRM
{
    /// <summary>
    /// Segédosztály a Monitor rekordok xml contetntjének előállítására
    /// </summary>
    public static class MonitorDataContentHelper
    {
        /// <summary>
        /// Üres document elementet ad
        /// </summary>
        /// <returns></returns>
        public static XElement GetEmptyDocument() => new XElement(XName.Get("document"));

        /// <summary>
        /// A kapott dictionary-ből képezi az adatokat kódoló XML strukturát
        /// </summary>
        /// <param name="values">adatok (kulcs-érték párok)</param>
        /// <returns>XML adatstruktúra az adatokból</returns>
        public static XElement CreateContent(Dictionary<string, string> values)
        {
            var document = GetEmptyDocument();
            foreach (var item in values)
            {
                document.Add(new XElement(item.Key, item.Value));
            }
            return document;
        }

        /// <summary>
        /// Monitor data rekord hibához
        /// </summary>
        /// <param name="ex">A hibát reprezemntáló kivétel</param>
        /// <returns>a hibát reprezentáló XML contetnt</returns>
        public static XElement GetMonitorDataForErrors(Exception ex)
        {
            var monitorDataList = new Dictionary<string, string>()
            {
                { "Error", ex.Message },
            };
            return MonitorDataContentHelper.CreateContent(monitorDataList);
        }
    }
}
