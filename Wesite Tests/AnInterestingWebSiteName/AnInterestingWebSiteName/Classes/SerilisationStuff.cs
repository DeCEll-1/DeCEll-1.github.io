using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AnInterestingWebSiteName.Classes
{
    public class SerilisationStuff : DbContext
    {


        public string XMLSerialize(BaseClass thingToSerialize)
        {
            try
            {

                var stringWriter = new StringWriter();
                var serializer = new XmlSerializer(typeof(BaseClass));

                serializer.Serialize(stringWriter, thingToSerialize);
                return stringWriter.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BaseClass XMLDeseriliazier(string xmlData)
        {
            try
            {
                var stringReader = new StringReader(xmlData);
                var serializer = new XmlSerializer(typeof(BaseClass));
                return (BaseClass)serializer.Deserialize(stringReader);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}