using CumulusFramework.Domain.Entity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace CumulusFramework.Enviroment.Util
{
    public static class EnvironmentUtil
    {
        private static String KEY = @"SOFTWARE\GuidiSistemas";
        //private static String CHAVE_64 = @"WOW6432Node\SOFTWARE\GuidiSistemas";
        private static String VALUE = "Ambientes";

        public static List<DBEnvironment> ConvertXMLToEnvironments(String xml, List<DBEnvironment> environments = null)
        {
            List<DBEnvironment> envs = new List<DBEnvironment>();
            if (String.IsNullOrEmpty(xml))
            {
                return null;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            string xpath = "configuracaoambientes/configuracaoambiente";
            var nodes = xmlDoc.SelectNodes(xpath);

            for (int i = 0; i < nodes.Count; i++)
            {
                DBEnvironment amb = new DBEnvironment();
                amb.Name = nodes[i].ChildNodes[0].InnerXml;
                amb.DBMS = Convert.ToInt32(nodes[i].ChildNodes[1].InnerXml);
                amb.Hostname = nodes[i].ChildNodes[2].InnerXml;
                amb.Port = nodes[i].ChildNodes[3].InnerXml;
                amb.Instance = nodes[i].ChildNodes[4].InnerXml;
                amb.User = nodes[i].ChildNodes[5].InnerXml;
                amb.Pass = nodes[i].ChildNodes[6].InnerXml;
                amb.EnvironmentType = Convert.ToInt32(nodes[i].ChildNodes[7].InnerXml);

                if (environments == null)
                {
                    envs.Add(amb);
                }
                else
                {
                    environments.Add(amb);
                }
            }

            return envs;
        }

        public static Boolean RemoveAllEnvironments()
        {
            try
            {
                //Remove toda a chave do registro
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).DeleteSubKeyTree(KEY);
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree(KEY);
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public static List<CumulusFramework.Domain.Entity.DBEnvironment> GetAllEnvironments()
        {
            List<Domain.Entity.DBEnvironment> _ambs = new List<Domain.Entity.DBEnvironment>();

            try
            {
                //Pega a string salva no registro e deserializa para uma lista de ambientes
                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).CreateSubKey(KEY);
                RegistryKey key64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(KEY);

                using (key)
                {
                    var temp = key.GetValue(VALUE);
                    var temp64 = key.GetValue(VALUE);
                    if (temp != null)
                    {
                        _ambs = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entity.DBEnvironment>>(temp.ToString());
                    }
                    else
                    {
                        using (key64)
                        {
                            temp = key.GetValue(VALUE);
                            if (temp64 != null)
                            {
                                _ambs = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entity.DBEnvironment>>(temp.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _ambs;
        }

        public static void SaveEnvironmentListToRegistry(List<CumulusFramework.Domain.Entity.DBEnvironment> ambients)
        {
            try
            {
                var duplicated = ambients.GroupBy(a => a.Name).Where(w => w.Count() > 1).Select(s => s.First());

                if (duplicated.Count() > 0)
                {
                    var ambiente = duplicated.ToList()[0];                                                            
                }

                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).CreateSubKey(KEY);
                    RegistryKey key64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(KEY);

                    using (key)
                    {
                        String _valores = System.Text.Json.JsonSerializer.Serialize(ambients);
                        key.SetValue(VALUE, _valores, RegistryValueKind.String);
                    }

                    using (key64)
                    {
                        String _values = System.Text.Json.JsonSerializer.Serialize(ambients);
                        key64.SetValue(VALUE, _values, RegistryValueKind.String);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
