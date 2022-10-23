using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Daily_Transmetter_reading_entry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        if (!IsPostBack)
        {
            ddLocation.DataBind();
            BindTransmitter();
            ddChannelNo.DataBind();
            ShowHidePowerAmplifiers();
            //GridView1.DataBind();
            BindGrid();
        }
    }

    private void InsertData()
    {
        SqlCommand command = new SqlCommand(@"INSERT INTO DailyTransmitterLogEntry (StationId, TransmitterMachineId, ChannelNumber, Date, TransmitterOutputPowerVideo, TransmitterOutputPowerAudio, ReflectedPowerVideo, ReflectedPowerAudio, ExciterInOperation, PumpInOperation, 
            LiouidStaticPressure, PumpOutputPressure, LiquidTemperature, DehydratorLinePressure, PA1AGCLevel, PA1VSWR, PA1Temperature, PA1T1, PA1T2, PA1T3, PA1T4, PA1T5, PA1T6, PA2AGCLevel, PA2VSWR, PA2Temperature,
            PA2T1, PA2T2, PA2T3, PA2T4, PA2T5, PA2T6, PA3AGCLevel, PA3VSWR, PA3Temperature, PA3T1, PA3T2, PA3T3, PA3T4, PA3T5, PA3T6, PA4AGCLevel, PA4VSWR, PA4Temperature, PA4T1, PA4T2, PA4T3, PA4T4, PA4T5, PA4T6,
            PA5AGCLevel, PA5VSWR, PA5Temperature, PA5T1, PA5T2, PA5T3, PA5T4, PA5T5, PA5T6, PA6AGCLevel, PA6VSWR, PA6Temperature, PA6T1, PA6T2, PA6T3, PA6T4, PA6T5, PA6T6, PA7AGCLevel, PA7VSWR, PA7Temperature,
            PA7T1, PA7T2, PA7T3, PA7T4, PA7T5, PA7T6, PA8AGCLevel, PA8VSWR, PA8Temperature, PA8T1, PA8T2, PA8T3, PA8T4, PA8T5, PA8T6, Remarks, EntryBy)  VALUES (@StationId, @TransmitterMachineId, @ChannelNumber, @Date, @TransmitterOutputPowerVideo, @TransmitterOutputPowerAudio, @ReflectedPowerVideo, @ReflectedPowerAudio, @ExciterInOperation, @PumpInOperation, 
            @LiouidStaticPressure, @PumpOutputPressure, @LiquidTemperature, @DehydratorLinePressure, @PA1AGCLevel, @PA1VSWR, @PA1Temperature, @PA1T1, @PA1T2, @PA1T3, @PA1T4, @PA1T5, @PA1T6, @PA2AGCLevel, @PA2VSWR, @PA2Temperature,
            @PA2T1, @PA2T2, @PA2T3, @PA2T4, @PA2T5, @PA2T6, @PA3AGCLevel, @PA3VSWR, @PA3Temperature, @PA3T1, @PA3T2, @PA3T3, @PA3T4, @PA3T5, @PA3T6, @PA4AGCLevel, @PA4VSWR, @PA4Temperature, @PA4T1, @PA4T2, @PA4T3, @PA4T4, @PA4T5, @PA4T6, @PA5AGCLevel, @PA5VSWR, @PA5Temperature, @PA5T1, @PA5T2, @PA5T3, @PA5T4, @PA5T5, @PA5T6, @PA6AGCLevel, @PA6VSWR, @PA6Temperature, @PA6T1, @PA6T2, @PA6T3, @PA6T4, @PA6T5, @PA6T6, @PA7AGCLevel, @PA7VSWR, @PA7Temperature,
            @PA7T1, @PA7T2, @PA7T3, @PA7T4, @PA7T5, @PA7T6, @PA8AGCLevel, @PA8VSWR, @PA8Temperature, @PA8T1, @PA8T2, @PA8T3, @PA8T4, @PA8T5, @PA8T6, @Remarks, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@StationId", ddLocation.SelectedValue);
        command.Parameters.AddWithValue("@TransmitterMachineId", ddTransmitter.SelectedValue);
        command.Parameters.AddWithValue("@ChannelNumber", ddChannelNo.SelectedValue);
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@TransmitterOutputPowerVideo", Convert.ToDecimal(txtTransmitterOutputPowerVideo.Text));
        command.Parameters.AddWithValue("@TransmitterOutputPowerAudio", Convert.ToDecimal(txtTransmitterOutputPowerAudio.Text));
        command.Parameters.AddWithValue("@ReflectedPowerVideo", Convert.ToDecimal(txtReflectedPowerVideo.Text));
        command.Parameters.AddWithValue("@ReflectedPowerAudio", Convert.ToDecimal(txtReflectedPowerAudio.Text));
        command.Parameters.AddWithValue("@ExciterInOperation", ddExciterInOperation.SelectedValue);
        command.Parameters.AddWithValue("@PumpInOperation", ddPumpInOperation.SelectedValue);
        command.Parameters.AddWithValue("@LiouidStaticPressure", Convert.ToDecimal(txtLiquidStaticPressure.Text));
        command.Parameters.AddWithValue("@PumpOutputPressure", Convert.ToDecimal(txtPumpOutputPressure.Text));
        command.Parameters.AddWithValue("@LiquidTemperature", Convert.ToDecimal(txtLiquidTemperature.Text));
        command.Parameters.AddWithValue("@DehydratorLinePressure", Convert.ToDecimal(txtDehydratorLinePressure.Text));
        command.Parameters.AddWithValue("@PA1AGCLevel", txtPA1AGCLevel.Text);
        command.Parameters.AddWithValue("@PA1VSWR", txtPA1VSWR.Text);
        command.Parameters.AddWithValue("@PA1Temperature", txtPA1Temperature.Text);
        command.Parameters.AddWithValue("@PA1T1", PA1T1.Text);
        command.Parameters.AddWithValue("@PA1T2", PA1T2.Text);
        command.Parameters.AddWithValue("@PA1T3", PA1T3.Text);
        command.Parameters.AddWithValue("@PA1T4", PA1T4.Text);
        command.Parameters.AddWithValue("@PA1T5", PA1T5.Text);
        command.Parameters.AddWithValue("@PA1T6", PA1T6.Text);
        command.Parameters.AddWithValue("@PA2AGCLevel", txtPA2AGCLevel.Text);
        command.Parameters.AddWithValue("@PA2VSWR", txtPA2VSWR.Text);
        command.Parameters.AddWithValue("@PA2Temperature", txtPA2Temperature.Text);
        command.Parameters.AddWithValue("@PA2T1", PA2T1.Text);
        command.Parameters.AddWithValue("@PA2T2", PA2T2.Text);
        command.Parameters.AddWithValue("@PA2T3", PA2T3.Text);
        command.Parameters.AddWithValue("@PA2T4", PA2T4.Text);
        command.Parameters.AddWithValue("@PA2T5", PA2T5.Text);
        command.Parameters.AddWithValue("@PA2T6", PA2T6.Text);
        command.Parameters.AddWithValue("@PA3AGCLevel", txtPA3AGCLevel.Text);
        command.Parameters.AddWithValue("@PA3VSWR", txtPA3VSWR.Text);
        command.Parameters.AddWithValue("@PA3Temperature", txtPA3Temperature.Text);
        command.Parameters.AddWithValue("@PA3T1", PA3T1.Text);
        command.Parameters.AddWithValue("@PA3T2", PA3T2.Text);
        command.Parameters.AddWithValue("@PA3T3", PA3T3.Text);
        command.Parameters.AddWithValue("@PA3T4", PA3T4.Text);
        command.Parameters.AddWithValue("@PA3T5", PA3T5.Text);
        command.Parameters.AddWithValue("@PA3T6", PA3T6.Text);
        command.Parameters.AddWithValue("@PA4AGCLevel", txtPA4AGCLevel.Text);
        command.Parameters.AddWithValue("@PA4VSWR", txtPA4VSWR.Text);
        command.Parameters.AddWithValue("@PA4Temperature", txtPA4Temperature.Text);
        command.Parameters.AddWithValue("@PA4T1", PA4T1.Text);
        command.Parameters.AddWithValue("@PA4T2", PA4T2.Text);
        command.Parameters.AddWithValue("@PA4T3", PA4T3.Text);
        command.Parameters.AddWithValue("@PA4T4", PA4T4.Text);
        command.Parameters.AddWithValue("@PA4T5", PA4T5.Text);
        command.Parameters.AddWithValue("@PA4T6", PA4T6.Text);
        command.Parameters.AddWithValue("@PA5AGCLevel", txtPA5AGCLevel.Text);
        command.Parameters.AddWithValue("@PA5VSWR", txtPA5VSWR.Text);
        command.Parameters.AddWithValue("@PA5Temperature", txtPA5Temperature.Text);
        command.Parameters.AddWithValue("@PA5T1", PA5T1.Text);
        command.Parameters.AddWithValue("@PA5T2", PA5T2.Text);
        command.Parameters.AddWithValue("@PA5T3", PA5T3.Text);
        command.Parameters.AddWithValue("@PA5T4", PA5T4.Text);
        command.Parameters.AddWithValue("@PA5T5", PA5T5.Text);
        command.Parameters.AddWithValue("@PA5T6", PA5T6.Text);
        command.Parameters.AddWithValue("@PA6AGCLevel", txtPA6AGCLevel.Text);
        command.Parameters.AddWithValue("@PA6VSWR", txtPA6VSWR.Text);
        command.Parameters.AddWithValue("@PA6Temperature", txtPA6Temperature.Text);
        command.Parameters.AddWithValue("@PA6T1", PA6T1.Text);
        command.Parameters.AddWithValue("@PA6T2", PA6T2.Text);
        command.Parameters.AddWithValue("@PA6T3", PA6T3.Text);
        command.Parameters.AddWithValue("@PA6T4", PA6T4.Text);
        command.Parameters.AddWithValue("@PA6T5", PA6T5.Text);
        command.Parameters.AddWithValue("@PA6T6", PA6T6.Text);
        command.Parameters.AddWithValue("@PA7AGCLevel", txtPA7AGCLevel.Text);
        command.Parameters.AddWithValue("@PA7VSWR", txtPA7VSWR.Text);
        command.Parameters.AddWithValue("@PA7Temperature", txtPA7Temperature.Text);
        command.Parameters.AddWithValue("@PA7T1", PA7T1.Text);
        command.Parameters.AddWithValue("@PA7T2", PA7T2.Text);
        command.Parameters.AddWithValue("@PA7T3", PA7T3.Text);
        command.Parameters.AddWithValue("@PA7T4", PA7T4.Text);
        command.Parameters.AddWithValue("@PA7T5", PA7T5.Text);
        command.Parameters.AddWithValue("@PA7T6", PA7T6.Text);
        command.Parameters.AddWithValue("@PA8AGCLevel", txtPA8AGCLevel.Text);
        command.Parameters.AddWithValue("@PA8VSWR", txtPA8VSWR.Text);
        command.Parameters.AddWithValue("@PA8Temperature", txtPA8Temperature.Text);
        command.Parameters.AddWithValue("@PA8T1", PA8T1.Text);
        command.Parameters.AddWithValue("@PA8T2", PA8T2.Text);
        command.Parameters.AddWithValue("@PA8T3", PA8T3.Text);
        command.Parameters.AddWithValue("@PA8T4", PA8T4.Text);
        command.Parameters.AddWithValue("@PA8T5", PA8T5.Text);
        command.Parameters.AddWithValue("@PA8T6", PA8T6.Text);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }

    private void BindGrid()
    {
        DataTable dataTable = SQLQuery.ReturnDataTable(@"SELECT DailyTransmitterLogEntry.Id, Location.Name AS Station, Transmitters.TransmitterName AS Machine, ChannelNumber, Date, TransmitterOutputPowerVideo, TransmitterOutputPowerAudio, ReflectedPowerVideo, ReflectedPowerAudio, ExciterInOperation, PumpInOperation, 
                         LiouidStaticPressure, PumpOutputPressure, LiquidTemperature, DehydratorLinePressure, PA1AGCLevel, PA1VSWR, PA1Temperature, PA1T1, PA1T2, PA1T3, PA1T4, PA1T5, PA1T6, PA2AGCLevel, PA2VSWR, PA2Temperature, 
                         PA2T1, PA2T2, PA2T3, PA2T4, PA2T5, PA2T6, PA3AGCLevel, PA3VSWR, PA3Temperature, PA3T1, PA3T2, PA3T3, PA3T4, PA3T5, PA3T6, PA4AGCLevel, PA4VSWR, PA4Temperature, PA4T1, PA4T2, PA4T3, PA4T4, PA4T5, PA4T6, 
                         PA5AGCLevel, PA5VSWR, PA5Temperature, PA5T1, PA5T2, PA5T3, PA5T4, PA5T5, PA5T6, PA6AGCLevel, PA6VSWR, PA6Temperature, PA6T1, PA6T2, PA6T3, PA6T4, PA6T5, PA6T6, PA7AGCLevel, PA7VSWR, PA7Temperature, 
                         PA7T1, PA7T2, PA7T3, PA7T4, PA7T5, PA7T6, PA8AGCLevel, PA8VSWR, PA8Temperature, PA8T1, PA8T2, PA8T3, PA8T4, PA8T5, PA8T6, Remarks
FROM            DailyTransmitterLogEntry 
INNER JOIN Location ON Location.LocationID = DailyTransmitterLogEntry.StationId
INNER JOIN Transmitters ON Transmitters.Id = DailyTransmitterLogEntry.TransmitterMachineId");
        GridView1.DataSource = dataTable;
        GridView1.DataBind();
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {

        if (btnSave.Text == "Save")
        {
            InsertData();
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Daily Transmitter Log Entry Successful!";
        }
        else
        {
            SQLQuery.ExecNonQry("Update ECDormitorySetup Set RoadZone=N'" + ddLocation.SelectedValue + "',RoadCircle=N'" + ddTransmitter.SelectedValue + "',Division=N'" + ddChannelNo.SelectedValue + "',Bname=N'" + txtTransmitterOutputPowerVideo.Text + "',Address=N'" + txtTransmitterOutputPowerAudio.Text + "',Construction=N'" + ddExciterInOperation.Text + "',Floor=N'" + ddPumpInOperation.Text + "',Status=N'" + ddStatus.SelectedValue + "',Deployment=N'" + ddDtype.SelectedValue + "',Comments=N'" + txtComments.Text + "', EntryBy='" + User.Identity.Name + "' where Id='" + lblId.Text + "'");


            btnSave.Text = "Save";
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Bungalow updated successfully";
        }

        txtTransmitterOutputPowerAudio.Text = "";
        ddExciterInOperation.Text = "";
        ddPumpInOperation.Text = "";
        // txtStatus.Text = "";
        txtComments.Text = "";
        //GridView1.DataBind();
        BindGrid();
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label labelID = GridView1.Rows[index].FindControl("Label1") as Label;
        lblId.Text = labelID.Text;
        DataTable dt = SQLQuery.ReturnDataTable("Select RoadZone, RoadCircle, Division, Address, Construction, Floor, Status, Deployment, Comments, Bname FROM ECDormitorySetup where Id='" + labelID.Text + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            ddLocation.SelectedValue = dtx[0].ToString();
            ddTransmitter.SelectedValue = dtx[1].ToString();
            ddChannelNo.SelectedValue = dtx[2].ToString();
            txtTransmitterOutputPowerAudio.Text = dtx[3].ToString();
            ddExciterInOperation.Text = dtx[4].ToString();
            ddPumpInOperation.Text = dtx[5].ToString();
            ddStatus.SelectedValue = dtx[6].ToString();
            ddDtype.Text = dtx[7].ToString();
            txtComments.Text = dtx[8].ToString();
            txtTransmitterOutputPowerVideo.Text = dtx[9].ToString();

        }
        btnSave.Text = "Update";
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Edit mode activeted....";
    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        Label lbId = GridView1.Rows[index].FindControl("Label1") as Label;
        SQLQuery.ExecNonQry("DELETE DailyTransmitterLogEntry WHERE Id='" + lbId.Text + "'");
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Deleted Successfully.";
        BindGrid();
    }


    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        txtTransmitterOutputPowerAudio.Text = "";
        ddExciterInOperation.Text = "";
        ddPumpInOperation.Text = "";
        txtTransmitterOutputPowerVideo.Text = "";
        txtComments.Text = "";

        //GridView1.DataBind();
        BindGrid();
        btnSave.Text = "Save";
    }

    private void BindTransmitter()
    {
        SQLQuery.PopulateDropDown("SELECT id, TransmitterName FROM Transmitters WHERE StationID = '" + ddLocation.SelectedValue + "'", ddTransmitter, "id", "TransmitterName");
    }
    protected void ddLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTransmitter();
        ddChannelNo.DataBind();
        ShowHidePowerAmplifiers();
    }

    protected void ShowHidePowerAmplifiers()
    {
        bool[] array = { false, false, false, false, false, false, false, false };
        if (ddTransmitter.SelectedValue != "0")
        {
            string numberOfAmplifiers = SQLQuery.ReturnString(@"SELECT NumberOfAmplifiers FROM Transmitters WHERE id = '" + ddTransmitter.SelectedValue + "'");
            if (numberOfAmplifiers != "")
            {
                for (int i = 0; i < Convert.ToDecimal(numberOfAmplifiers); i++)
                {
                    array[i] = true;
                }
            }
        }
        PA1.Visible = array[0];
        if (PA1.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-1'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA1T1.Enabled = PAT[0];
            PA1T2.Enabled = PAT[1];
            PA1T3.Enabled = PAT[2];
            PA1T4.Enabled = PAT[3];
            PA1T5.Enabled = PAT[4];
            PA1T6.Enabled = PAT[5];
        }
        PA2.Visible = array[1];
        if (PA2.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-2'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA2T1.Enabled = PAT[0];
            PA2T2.Enabled = PAT[1];
            PA2T3.Enabled = PAT[2];
            PA2T4.Enabled = PAT[3];
            PA2T5.Enabled = PAT[4];
            PA2T6.Enabled = PAT[5];
        }
        PA3.Visible = array[2];
        if (PA3.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-3'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA3T1.Enabled = PAT[0];
            PA3T2.Enabled = PAT[1];
            PA3T3.Enabled = PAT[2];
            PA3T4.Enabled = PAT[3];
            PA3T5.Enabled = PAT[4];
            PA3T6.Enabled = PAT[5];
        }
        PA4.Visible = array[3];
        if (PA4.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-4'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA4T1.Enabled = PAT[0];
            PA4T2.Enabled = PAT[1];
            PA4T3.Enabled = PAT[2];
            PA4T4.Enabled = PAT[3];
            PA4T5.Enabled = PAT[4];
            PA4T6.Enabled = PAT[5];
        }
        PA5.Visible = array[4];
        if (PA5.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-5'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA5T1.Enabled = PAT[0];
            PA5T2.Enabled = PAT[1];
            PA5T3.Enabled = PAT[2];
            PA5T4.Enabled = PAT[3];
            PA5T5.Enabled = PAT[4];
            PA5T6.Enabled = PAT[5];
        }
        PA6.Visible = array[5];
        if (PA6.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-6'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA6T1.Enabled = PAT[0];
            PA6T2.Enabled = PAT[1];
            PA6T3.Enabled = PAT[2];
            PA6T4.Enabled = PAT[3];
            PA6T5.Enabled = PAT[4];
            PA6T6.Enabled = PAT[5];
        }
        PA7.Visible = array[6];
        if (PA7.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-7'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA7T1.Enabled = PAT[0];
            PA7T2.Enabled = PAT[1];
            PA7T3.Enabled = PAT[2];
            PA7T4.Enabled = PAT[3];
            PA7T5.Enabled = PAT[4];
            PA7T6.Enabled = PAT[5];
        }
        PA8.Visible = array[7];
        if (PA8.Visible)
        {
            bool[] PAT = { false, false, false, false, false, false };
            string currentInAmpearesField = SQLQuery.ReturnString(@"SELECT ISNULL(CurrentInAmpearsField,0) AS CurrentInAmpearsField FROM Amplifiers WHERE TransmitterId = '" + ddTransmitter.SelectedValue + "' AND Name='PA-8'");
            if (currentInAmpearesField != "")
            {
                for (int i = 0; i < Convert.ToDecimal(currentInAmpearesField); i++)
                {
                    PAT[i] = true;
                }
            }
            PA8T1.Enabled = PAT[0];
            PA8T2.Enabled = PAT[1];
            PA8T3.Enabled = PAT[2];
            PA8T4.Enabled = PAT[3];
            PA8T5.Enabled = PAT[4];
            PA8T6.Enabled = PAT[5];
        }
    }

    //protected void ddTransmitter_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddLocation.DataBind();
    //}
    protected void ddTransmitter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ShowHidePowerAmplifiers();
    }
}
