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
        //this variable is going to help determine if we are adding or editing information
        //so if it is true it means that we are in adding/editing
        //and the datagridview actions will not be allowed
        bool _isActive = false;

        #endregion
        public frmEmployee()
        {
            InitializeComponent();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            //clearing all fields
            resetSettings();
        }

        #region Methods
        public void addMode()
        {
            //addmode activated
            _isActive = true;

            //will get an temporary ID just to show in the label
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
            //editmode activated
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
            //everytime we reset all the fields, we refresh the datagridview to get all the updated data
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
            //this method will determine if the user has input a valid information like entering a null name
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
            //this will get all the employee from the databasea
            List<EmployeeEntity> listEmp = new List<EmployeeEntity>();
            using (EmployeeDbContainer db = new EmployeeDbContainer())
            {
                listEmp = db.Employees.Select(x => new EmployeeEntity { Id = x.Id, Name = x.Name }).ToList();
            }
            //populating the datagridview
            dgvEmployee.DataSource = listEmp;           
        }

        public void saveEmployee(EmployeeEntity obj)
        {
            //will save all the employee
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
            //will delete all employee
            using(EmployeeDbContainer db = new EmployeeDbContainer())
            {
                var emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
                db.Employees.Remove(emp);
                db.SaveChanges();
            }
        }

        public void updateEmployee(int Id , string Name)
        {
            //will update all the employee
            using (EmployeeDbContainer db = new EmployeeDbContainer())
            {
                var emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
                emp.Name = Name;
                db.SaveChanges();
            }
        }

        public int getTempID()
        {
            //getting a temporary ID
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
            addMode(); //add mode settings
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetSettings(); //reset all fields
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(hasInvalidInput(txtName.Text) == false) //check if theres an invalid input
            { 
            EmployeeEntity emp = new EmployeeEntity(Convert.ToInt32(lblEmployeeID.Text), txtName.Text);
            saveEmployee(emp); //call the method then pass the emp object
            getAllEmployee(); //refresh the datagrid
            resetSettings(); //reset all
            statusBar.Text = "Status : Successfully Saved!";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editMode(); //edit mode settings
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (hasInvalidInput(txtName.Text) == false) // check if theres an invalid input
            {
                updateEmployee(Convert.ToInt32(lblEmployeeID.Text), txtName.Text); //pass the id from the label
                resetSettings(); //reset all settings
                statusBar.Text = "Status : Successfully Updated!";
            }
        }

        private void dgvEmployee_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isActive == false) //if this is true it means the user is currently adding or editing 
            {
            lblEmployeeID.Text = Convert.ToString(dgvEmployee[0, dgvEmployee.CurrentRow.Index].Value);
            txtName.Text = Convert.ToString(dgvEmployee[1, dgvEmployee.CurrentRow.Index].Value);

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?","Warning!",MessageBoxButtons.YesNo); // just to make sure the user want to delete the information
            if(result == DialogResult.Yes) { 
                deleteEmployee(Convert.ToInt32(lblEmployeeID.Text)); //delete
                resetSettings(); // reset all
                statusBar.Text = "Status : Successfully Deleted!";
            }
        }
        #endregion
    }
}
