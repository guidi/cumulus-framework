using System;

namespace CumulusFramework.Domain.Entity
{
    public class DBEnvironment
    {
        public DBEnvironment(String Name, Int32 DBMS, String Hostname, String Port, String Instance, String UserName, String Pass, Int32 Type)
        {
            this.Name = Name;
            this.DBMS = DBMS;
            this.Hostname = Hostname;
            this.Port = Port;
            this.Instance = Instance;
            this.User = UserName;
            this.Pass = Pass;
            this.EnvironmentType = Type;
        }

        public DBEnvironment()
        {

        }

        public virtual Int32? Id { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32 DBMS { get; set; }
        public virtual String Hostname { get; set; }
        public virtual String Port { get; set; }
        public virtual String Instance { get; set; }
        public virtual String User { get; set; }
        public virtual String Pass { get; set; }
        public virtual Int32? DBMSVersion { get; set; }
        public virtual Int32 EnvironmentType { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            DBEnvironment _ambient = (DBEnvironment)obj;
            return (this.Name == _ambient.Name);
        }

        public override int GetHashCode()
        {
            return (this.Name).GetHashCode();
        }
    }
}
