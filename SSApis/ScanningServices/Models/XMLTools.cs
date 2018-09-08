using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class XMLTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            System.Xml.XmlTextWriter tw = null;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(o.GetType());
                tw = new System.Xml.XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static Object ObjectToXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            System.Xml.Serialization.XmlSerializer serializer = null;
            System.Xml.XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new System.Xml.Serialization.XmlSerializer(objectType);
                xmlReader = new System.Xml.XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception exp)
            {
                //Handle Exception Code
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }

    }
}
