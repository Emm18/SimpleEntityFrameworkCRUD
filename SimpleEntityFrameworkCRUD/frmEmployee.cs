using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEntityFrameworkCRUD
{
    public partial class frmEmployee : Form
    {

        #region variables
        bool _isActive = false;

        #endregion
        public frmEmployee()
        {
            InitializeComponent();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            resetSettings();
        }

        #region Methods
        public void addMode()
        {
            _isActive = true;

            lblEmployeeID.Text = getTempID().ToString();

            txtName.Enabled = true;
            txtName.Focus();
            txtName.Text = "";

            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = true;

            statusBar.Text = "Status : Adding New Employee...";
        }

        public void editMode()
        {
            _isActive = true;

            txtName.Enabled = true;
            txtName.Focus();

            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = true;

            statusBar.Text = "Status : Editing Employee information...";
        }

        public void resetSettings()
        {
            getAllEmployee();

            _isActive = false;

            lblEmployeeID.Text = "0";
            txtName.Text = "";

            txtName.Enabled = false;    

            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;

            statusBar.Text = "Status : ...";
        }

        public bool hasInvalidInput(string userInput)
        {
            bool checker = false;
            if (userInput == "" || userInput == null)
            {
                checker = true;
                MessageBox.Show("Please enter a name");
                txtName.Focus();
            }

            return checker;
        }


        public void getAllEmployee()
        {
            List<EmployeeEntity> listEmp = new List<EmployeeEntity>();
            using (EmployeeDbContainer db = new EmployeeDbContainer())
            {
                listEmp = db.Employees.Select(x => new EmployeeEntity { Id = x.Id, Name = x.Name }).ToList();
            }
            dgvEmployee.DataSource = listEmp;           
        }

        public void saveEmployee(EmployeeEntity obj)
        {
            using(EmployeeDbContainer db = new EmployeeDbContainer())
            {
                Employee emp = new Employee
                {
                    Id = obj.Id,
                    Name = obj.Name
                };
                db.Employees.Add(emp);
                db.SaveChanges();
            }
        }

        public void deleteEmployee(int Id)
        {
            using(EmployeeDbContainer db = new EmployeeDbContainer())
            {
                var emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
                db.Employees.Remove(emp);
                db.SaveChanges();
            }
        }

        public void updateEmployee(int Id , string Name)
        {
            using (EmployeeDbContainer db = new EmployeeDbContainer())
            {
                var emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
                emp.Name = Name;
                db.SaveChanges();
            }
        }

        public int getTempID()
        {
            int Id = 0;
            using(EmployeeDbContainer db = new EmployeeDbContainer())
            {
               var empID = db.Employees.OrderByDescending(x => x.Id).FirstOrDefault();
                if(empID == null)
                {
                    Id = 0;
                }
                else { 
                    Id = empID.Id;
                }
            }
            return Id + 1;
        }
        #endregion


        #region Buttons
        private void btnAdd_Click(object sender, EventArgs e)
        {
            addMode();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //save process
            if(hasInvalidInput(txtName.Text) == false) { 
            EmployeeEntity emp = new EmployeeEntity(Convert.ToInt32(lblEmployeeID.Text), txtName.Text);
            saveEmployee(emp);
            getAllEmployee();
            resetSettings();
            statusBar.Text = "Status : Successfully Saved!";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editMode();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (hasInvalidInput(txtName.Text) == false)
            {
                updateEmployee(Convert.ToInt32(lblEmployeeID.Text), txtName.Text);
                resetSettings();
                statusBar.Text = "Status : Successfully Updated!";
            }
        }

        private void dgvEmployee_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isActive == false) { 
            lblEmployeeID.Text = Convert.ToString(dgvEmployee[0, dgvEmployee.CurrentRow.Index].Value);
            txtName.Text = Convert.ToString(dgvEmployee[1, dgvEmployee.CurrentRow.Index].Value);

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?","Warning!",MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes) { 
                deleteEmployee(Convert.ToInt32(lblEmployeeID.Text));
                resetSettings();
                statusBar.Text = "Status : Successfully Deleted!";
            }
        }
        #endregion
    }
}
