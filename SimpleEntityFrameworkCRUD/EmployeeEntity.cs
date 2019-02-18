using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEntityFrameworkCRUD
{
    public class EmployeeEntity
    {
        private int _Id;
        private string _Name;

        #region Contructor
        public EmployeeEntity()
        {

        }

        public EmployeeEntity(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        #endregion

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}
