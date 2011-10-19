using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using System.Configuration;

public partial class Administrator_Settings_UserControls_UserDriversTab : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    private DataTable createDataSource()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        DataTable newDataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "#";
        string Col_2 = "ФИО";
        string Col_3 = "Номер водителя";
        string Col_4 = "Комментарий";

            newDataTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
            newDataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
            newDataTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
            newDataTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            List<int> cardsList = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
            foreach(int cardId in cardsList)
            {
                dr = newDataTable.NewRow();
                dr[Col_1] = cardId;
                dr[Col_2] = dataBlock.cardsTable.GetCardName(cardId);
                dr[Col_3] = dataBlock.cardsTable.GetCardNumber(cardId);
                dr[Col_4] = dataBlock.cardsTable.GetCardNote(cardId);
                newDataTable.Rows.Add(dr);
            }
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        return newDataTable;
    }

    public void LoadDriversDataGrid()
    {
        DriversDataGrid.DataSource = createDataSource();
        DriversDataGrid.DataBind();
    }

    public bool ShowEdit
    {
        get
        {
            return EditPanel.Visible;
        }
        set
        {
            if (value == true)
                Load_EditInfo();
            EditPanel.Visible = value;
            DriversDataGridPanel.Visible = !value;
        }
    }

    public bool ShowAddNew
    {
        get
        {
            return EditPanel.Visible;
        }
        set
        {
            if (value == true)
                Load_NewInfo();
            EditPanel.Visible = value;
            DriversDataGridPanel.Visible = !value;
        }
    }

    private void Load_EditInfo()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            //Надо, чтобы обнулялось с прошлого раза, если не все поля заполнены(если будет валидация. должно быть все окей и без этого. Может удалить.
            Edit_DriversBirthDayTextBox.Text = "";
            Edit_DriversNOTETextBox.Text = "";
            //--------------
            string selectedIndexString = Selected_DriversDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Выберите водителя для редактирования");
            int selectedIndex = Convert.ToInt32(selectedIndexString);
            radioButtonList1.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                radioButtonList1.Items.Add("Группа " + i.ToString());
            }
            int userInfoId;
            dataBlock.OpenConnection();
            int userId = dataBlock.cardsTable.GetCardUserId(selectedIndex);
            //SURNAME
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
            Edit_SurnameLabel.Text = "Surname:";
            Edit_SurnameTextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            //NAME
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            Edit_NameLabel.Text = "Name:";
            Edit_NameTextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            //Patronymic
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
            Edit_PatronymicLabel.Text = "Patronimic:";
            Edit_PatronymicTextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            //Drivers CErtificate
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DriversCertificate);
            Edit_DriversCertificateLabel.Text = "Drivers certificate:";
            Edit_DriversCertificateTextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            //CardNumber
            //userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardNumber);
            Edit_DriversCardLabel.Text = "Card number:";
            Edit_DriversCardTextBox.Text = dataBlock.cardsTable.GetCardNumber(selectedIndex);
            // Phone number
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
            Edit_DriversPhoneNumberLabel.Text = "Phone number:";
            Edit_DriversPhoneNumberTextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            //BirthDay
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Birthday);
            Edit_DriversBirthDayLabel.Text = "Birthday";
            string curDate = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            if(curDate!="")
                Edit_DriversBirthDayTextBox.Text = DateTime.Parse(curDate).ToShortDateString();
            //Comment
            Edit_DriversNOTELabel.Text = "Commentary";
            Edit_DriversNOTETextBox.Text = dataBlock.cardsTable.GetCardNote(selectedIndex);
            //Group Selector;
            Edit_DriversGroupSelectLabel.Text = "Group Selector";
            //PHOTO
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_UserPhoto);
            string filePath = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            if (filePath == "")
                filePath = "~/images/unknown_person.jpg";
            DriversPhotoImage.ImageUrl = filePath;

            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
           /* Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());*/
            throw ex;
        }
    }

    private void Load_NewInfo()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            radioButtonList1.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                radioButtonList1.Items.Add("Группа " + i.ToString());
            }
            dataBlock.usersTable.OpenConnection();
            //SURNAME
            Edit_SurnameLabel.Text = "Surname:";
            Edit_SurnameTextBox.Text = "";
            //NAME
            Edit_NameLabel.Text = "Name:";
            Edit_NameTextBox.Text = "";
            //Patronymic
            Edit_PatronymicLabel.Text = "Patronimic:";
            Edit_PatronymicTextBox.Text = "";
            //Drivers CErtificate
            Edit_DriversCertificateLabel.Text = "Drivers certificate:";
            Edit_DriversCertificateTextBox.Text = "";
            //CardNumber
            Edit_DriversCardLabel.Text = "Card number:";
            Edit_DriversCardTextBox.Text = "";
            // Phone number
            Edit_DriversPhoneNumberLabel.Text = "Phone number:";
            Edit_DriversPhoneNumberTextBox.Text = "";
            //BirthDay
            Edit_DriversBirthDayLabel.Text = "Birthday";
            Edit_DriversBirthDayTextBox.Text ="";
            //Comment
            Edit_DriversNOTELabel.Text = "Commentary";
            Edit_DriversNOTETextBox.Text = "";
            //Group Selector;
            Edit_DriversGroupSelectLabel.Text = "Group Selector";
            //PHOTO
            string filePath = "~/images/unknown_person.jpg";
            DriversPhotoImage.ImageUrl = filePath;
        
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }

    public override bool Visible
    {
        get
        {
            return base.Visible;
        }
        set
        {
            base.Visible = value;
            ShowEdit = false;
        }
    }

    protected void DriversDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
      /*  foreach (DataGridItem oldrow in DriversDataGrid.Items)
        {
            ((RadioButton)oldrow.FindControl("DriversDataGrid_RadioButton")).Checked = false;
        }*/

        //Set the new selected row
        RadioButton rb = (RadioButton)sender;
        DataGridItem row = (DataGridItem)rb.NamingContainer;
        ((RadioButton)row.FindControl("DriversDataGrid_RadioButton")).Checked = true;
        Selected_DriversDataGrid_Index.Value = row.Cells[1].Text;
    }

    public void SaveAllUpdatedInformation()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string selectedIndexString = Selected_DriversDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Не выбран водитель для редактирования");
            int selectedDriverID = Convert.ToInt32(selectedIndexString);

            dataBlock.usersTable.OpenConnection();
            dataBlock.usersTable.OpenTransaction();
            SaveInfo(selectedDriverID, dataBlock);
            dataBlock.usersTable.CommitTransaction();
            dataBlock.usersTable.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.usersTable.RollbackConnection();
            dataBlock.usersTable.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
       
        //Load_EditInfo();
        HideEditWindow();
    }

    public void SaveAllNewInformation()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string surName = Edit_SurnameTextBox.Text;
            string name = Edit_NameTextBox.Text;
            string driversCard = Edit_DriversCardTextBox.Text;
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int newCardId = dataBlock.cardsTable.CreateNewCard(name + " " + surName, driversCard, dataBlock.cardsTable.driversCardTypeId, orgId, "Created manually " + name + " " + surName + " " + driversCard, curUserId);
           
            SaveInfo(newCardId, dataBlock);
        }
        catch (Exception ex)
        {
            dataBlock.usersTable.RollbackConnection();
            dataBlock.usersTable.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        dataBlock.usersTable.CommitTransaction();
        dataBlock.usersTable.CloseConnection();
       // Load_EditInfo();
        HideEditWindow();
    }

    private void SaveInfo(int selectedDriverID, DataBlock dataBlock)
    {
        string surName = Edit_SurnameTextBox.Text;
        string name = Edit_NameTextBox.Text;
        string patronimic = Edit_PatronymicTextBox.Text;
        string driversCertificate = Edit_DriversCertificateTextBox.Text;
        string driversCard = Edit_DriversCardTextBox.Text;
        string driversPhoneNumber = Edit_DriversPhoneNumberTextBox.Text;
        DateTime driversBirthDay = DateTime.Parse(Edit_DriversBirthDayTextBox.Text);
        string Note = Edit_DriversNOTETextBox.Text;
        int userInfoId;
        int userId = dataBlock.cardsTable.GetCardUserId(selectedDriverID);
        int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
        //SURNAME
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, surName);
        //NAME
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, name);
        //NAME_SURNAME
        dataBlock.cardsTable.ChangeCardName(name + " " + surName, selectedDriverID);
        //PATRONIMIC
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, patronimic);
        //DRIVERS CERTIFICATE
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DriversCertificate);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, driversCertificate);
        //DRIVERS CARD
        dataBlock.cardsTable.ChangeCardNumber(driversCard, selectedDriverID, curUserId);
        //PHONE NUMBER
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, driversPhoneNumber);
        //BIRTHDAY
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Birthday);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, driversBirthDay.ToString());
        //NOTE
        dataBlock.cardsTable.SetCardNote(selectedDriverID, Note);
    }

    private void HideEditWindow()
    {
        Session["Settings_GeneralTabException"] = "HideThisWindow";
        RaiseBubbleEvent(null, new EventArgs());
    }

    protected void Upload_Click(object Sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            if(MyFileUpload.PostedFile==null)
                throw new Exception("Непредвиденная ошибка!");

            if (MyFileUpload.PostedFile.FileName == "")
                throw new Exception("Укажите файл для загрузки!");

            string GuidFIleName = Guid.NewGuid().ToString();
            string fileName = Server.MapPath(".\\") + "Photos\\Drivers\\" + GuidFIleName + "_" + MyFileUpload.PostedFile.FileName;
            string fileNameToDB = "~/Administrator/Photos/Drivers/" + GuidFIleName + "_" + MyFileUpload.PostedFile.FileName;

            string selectedIndexString = Selected_DriversDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Не выбран водитель для редактирования");
            int selectedDriverID = Convert.ToInt32(selectedIndexString);

            int userInfoId;

            if (MyFileUpload.PostedFile.ContentType == "image/jpeg" || MyFileUpload.PostedFile.ContentType == "image/png")
            {
                MyFileUpload.PostedFile.SaveAs(fileName);

                dataBlock.OpenConnection();
                dataBlock.OpenTransaction();
                int userId = dataBlock.cardsTable.GetCardUserId(selectedDriverID);
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_UserPhoto);
                dataBlock.usersTable.EditUserInfo(userId, userInfoId, fileNameToDB);
                dataBlock.CommitTransaction();
                dataBlock.CloseConnection();
                DriversPhotoImage.ImageUrl = fileNameToDB;
            }
            else
                throw new Exception("Неправильный формат файла");
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }
}
