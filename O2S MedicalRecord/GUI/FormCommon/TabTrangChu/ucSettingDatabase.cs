﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Npgsql;
using DevExpress.XtraSplashScreen;

namespace O2S_MedicalRecord.GUI.FormCommon.TabTrangChu
{
    public partial class ucSettingDatabase : UserControl
    {
        #region Khai bao
        private DAL.ConnectDatabase condb = new DAL.ConnectDatabase();

        #endregion

        public ucSettingDatabase()
        {
            InitializeComponent();
        }

        #region Load
        private void ucSettingDatabase_Load(object sender, EventArgs e)
        {
            try
            {
                LoadKetNoiDatabase();
                KiemTraTonTaiVaInsertLinkVersion();
                LoadCauHinhUpdateVersion();
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void KiemTraTonTaiVaInsertLinkVersion()
        {
            try
            {
                string kiemtraApp = "SELECT * FROM mrd_version WHERE app_type=0;";
                DataTable dataApp = condb.GetDataTable_HSBA(kiemtraApp);
                if (dataApp == null || dataApp.Rows.Count != 1)
                {
                    string insertApp = "INSERT INTO mrd_version(appversion,app_type) values('1.0.0.0','0') ;";
                    condb.ExecuteNonQuery_HSBA(insertApp);
                }
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void LoadCauHinhUpdateVersion()
        {
            try
            {
                string kiemtraApp = "SELECT app_link FROM mrd_version WHERE app_type=0;";
                DataTable dataApp = condb.GetDataTable_HSBA(kiemtraApp);
                if (dataApp != null || dataApp.Rows.Count > 0)
                {
                    txtUpdateVersionLink.Text = dataApp.Rows[0]["app_link"].ToString();
                }
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void LoadKetNoiDatabase()
        {
            try
            {
                // Giải mã giá trị lưu trong config
                this.txtDBHost.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["ServerHost"].ToString().Trim(), true);
                this.txtDBPort.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["ServerPort"].ToString().Trim(), true);
                this.txtDBUser.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Username"].ToString().Trim(), true);
                this.txtDBPass.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Password"].ToString().Trim(), true);
                this.txtDBName.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Database"].ToString().Trim(), true);
                this.txtDBHost_HSBA.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["ServerHost_HSBA"].ToString().Trim(), true);
                this.txtDBPort_HSBA.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["ServerPort_HSBA"].ToString().Trim(), true);
                this.txtDBUser_HSBA.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Username_HSBA"].ToString().Trim(), true);
                this.txtDBPass_HSBA.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Password_HSBA"].ToString().Trim(), true);
                this.txtDBName_HSBA.Text = O2S_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Database_HSBA"].ToString().Trim(), true);
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        #endregion
        private void btnDBKiemTra_Click(object sender, EventArgs e)
        {
            try
            {
                //May chu HIS
                bool boolfound = false;
                string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                    txtDBHost.Text, txtDBPort.Text, txtDBUser.Text, txtDBPass.Text, txtDBName.Text);
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                string sql = "SELECT * FROM tbuser";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    boolfound = true;
                    MessageBox.Show("Kết nối đến cơ sở dữ liệu HIS thành công!", "Thông báo");
                }
                if (boolfound == false)
                {
                    MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu HIS!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dr.Close();
                conn.Close();
                //May chu HSBA
                bool boolfound_HSBA = false;
                string connstring_HSBA = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                    txtDBHost_HSBA.Text, txtDBPort_HSBA.Text, txtDBUser_HSBA.Text, txtDBPass_HSBA.Text, txtDBName_HSBA.Text);
                NpgsqlConnection conn_HSBA = new NpgsqlConnection(connstring_HSBA);
                conn_HSBA.Open();
                string sql_HSBA = "SELECT * FROM mrd_license";
                NpgsqlCommand command_HSBA = new NpgsqlCommand(sql_HSBA, conn_HSBA);
                NpgsqlDataReader dr_HSBA = command_HSBA.ExecuteReader();
                if (dr_HSBA.Read())
                {
                    boolfound_HSBA = true;
                    MessageBox.Show("Kết nối đến cơ sở dữ liệu Hồ sơ bệnh án thành công!", "Thông báo");
                }
                if (boolfound_HSBA == false)
                {
                    MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu Hồ sơ bệnh án!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dr_HSBA.Close();
                conn_HSBA.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                O2S_MedicalRecord.Base.Logging.Error(ex);
            }
        }

        #region Luu
        private void btnDBLuu_Click(object sender, EventArgs e)
        {
            try
            {
                LuuLaiCauHinhFileConfig();
                LuuLaiDuongDanUpdateVersion();
                MessageBox.Show("Lưu dữ liệu thành công", "Thông báo");
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Error(ex);
            }
        }
        private void LuuLaiCauHinhFileConfig()
        {
            try
            {
                Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                _config.AppSettings.Settings["ServerHost"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBHost.Text.Trim(), true);
                _config.AppSettings.Settings["ServerPort"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBPort.Text.Trim(), true);
                _config.AppSettings.Settings["Username"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBUser.Text.Trim(), true);
                _config.AppSettings.Settings["Password"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBPass.Text.Trim(), true);
                _config.AppSettings.Settings["Database"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBName.Text.Trim(), true);
                _config.AppSettings.Settings["ServerHost_HSBA"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBHost_HSBA.Text.Trim(), true);
                _config.AppSettings.Settings["ServerPort_HSBA"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBPort_HSBA.Text.Trim(), true);
                _config.AppSettings.Settings["Username_HSBA"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBUser_HSBA.Text.Trim(), true);
                _config.AppSettings.Settings["Password_HSBA"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBPass_HSBA.Text.Trim(), true);
                _config.AppSettings.Settings["Database_HSBA"].Value = O2S_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDBName_HSBA.Text.Trim(), true);
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Error(ex);
            }
        }
        private void LuuLaiDuongDanUpdateVersion()
        {
            try
            {
                string sqlcommit = "update mrd_version set app_link= '" + txtUpdateVersionLink.Text.Trim() + "';";
                condb.ExecuteNonQuery_HSBA(sqlcommit);
            }
            catch (Exception ex)
            {
                O2S_MedicalRecord.Base.Logging.Error(ex);
            }
        }

        #endregion
        private void btnDBUpdate_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(O2S_MedicalRecord.Utilities.ThongBao.WaitForm1));
            try
            {
                if (O2S_MedicalRecord.DAL.KetNoiSCDLProcess.CapNhatCoSoDuLieu())
                {
                    MessageBox.Show("Cập nhật cơ sở dữ liệu thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Cập nhật cơ sở dữ liệu thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                O2S_MedicalRecord.Base.Logging.Error("Lỗi cập nhật cơ sở dữ liệu!" + ex.ToString());
            }
            SplashScreenManager.CloseForm();
        }


    }
}
