﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using System.Net;
using System.Diagnostics;
using MSO2_MedicalRecord.Base;
using System.IO;

namespace MSO2_MedicalRecord.FormCommon
{
    public partial class frmLogin : Form
    {
        MSO2_MedicalRecord.Base.ConnectDatabase condb = new MSO2_MedicalRecord.Base.ConnectDatabase();
        NpgsqlConnection conn;
        public frmLogin()
        {
            InitializeComponent();
        }

        #region Load
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                if (KiemTraKetNoiDenCoSoDuLieu() == false)
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                KiemTraInsertMayTram();
                LoadDataFromDatabase();

                if (ConfigurationManager.AppSettings["LoginUser"].ToString() != "" && ConfigurationManager.AppSettings["LoginPassword"].ToString() != "")
                {
                    this.txtUsername.Text = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["LoginUser"].ToString(), true);
                    this.txtPassword.Text = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["LoginPassword"].ToString(), true);
                    this.checkEditNhoPass.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["checkEditNhoPass"]);
                }
                else
                {
                    this.txtUsername.Text = "";
                    this.txtPassword.Text = "";
                }

                txtUsername.Focus();

                SessionLogin.SessionMachineName = Environment.MachineName;
                // Địa chỉ Ip
                String strHostName = Dns.GetHostName();
                IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
                //int nIP = 0;
                string listIP = "";
                for (int i = 0; i < iphostentry.AddressList.Count(); i++)
                {
                    listIP += iphostentry.AddressList[i] + ";";
                }
                SessionLogin.SessionMyIP = listIP;
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                SessionLogin.SessionVersion = fvi.FileVersion;
              //  KiemTraVaCopyFileLaucherNew(); chua co Lanucher
            }
            catch (Exception ex)
            {
                MSO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        private bool KiemTraKetNoiDenCoSoDuLieu()
        {
            bool result = false;
            try
            {
                string serverhost = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["ServerHost"].ToString().Trim() ?? "", true);
                string serveruser = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Username"].ToString().Trim(), true);
                string serverpass = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Password"].ToString().Trim(), true);
                string serverdb = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Database"].ToString().Trim(), true);
                string serverhost_HSBA = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["ServerHost_HSBA"].ToString().Trim() ?? "", true);
                string serveruser_HSBA = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Username_HSBA"].ToString().Trim(), true);
                string serverpass_HSBA = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Password_HSBA"].ToString().Trim(), true);
                string serverdb_HSBA = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(ConfigurationManager.AppSettings["Database_HSBA"].ToString().Trim(), true);

                if (conn == null)
                    conn = new NpgsqlConnection("Server=" + serverhost + ";Port=5432;User Id=" + serveruser + "; " + "Password=" + serverpass + ";Database=" + serverdb + ";CommandTimeout=1800000;");
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                conn.Close();
                result = true;
                //todo them ket noi den CSDL HSBA
            }
            catch (Exception ex)
            {
                Base.Logging.Error(ex);
            }
            return result;
        }
        private void KiemTraInsertMayTram()
        {
            try
            {
                SessionLogin.MaDatabase = MSO2_MedicalRecord.FormCommon.DangKyBanQuyen.KiemTraLicense.LayThongTinMaDatabase();
                //string tenmay = MSO2_MedicalRecord.FormCommon.DangKyBanQuyen.HardwareInfo.GetComputerName();
                string license_trang = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Encrypt("", true);

                string kiemtra_client = "SELECT * FROM mrd_license WHERE datakey='" + SessionLogin.MaDatabase + "' ;";
                DataView dv = new DataView(condb.getDataTable(kiemtra_client));
                if (dv != null && dv.Count > 0)
                {
                    //Kiem tra license
                    //MSO2_MedicalRecord.FormCommon.DangKyBanQuyen.kiemTraLicenseHopLe.KiemTraLicenseHopLe();
                }
                else
                {
                    string insert_client = "INSERT INTO mrd_license(datakey, licensekey) VALUES ('" + SessionLogin.MaDatabase + "','" + license_trang + "' );";
                    condb.ExecuteNonQuery(insert_client);
                }
            }
            catch (Exception ex)
            {
                Base.Logging.Error(ex);
            }
        }
        private void LoadDataFromDatabase()
        {
            try
            {
                //Set default
                //MSO2_MedicalRecord.GlobalStore.ThoiGianCapNhatTbl_tools_bndangdt_tmp = 0;
                //MSO2_MedicalRecord.GlobalStore.KhoangThoiGianLayDuLieu = DateTime.Now.Year - 1 + "-01-01 00:00:00";

                //Load thong tin Luu vao GlobalStore
                //string sqlDSOption = "SELECT toolsoptionid, toolsoptioncode, toolsoptionname, toolsoptionvalue, toolsoptionnote FROM mrd_option WHERE toolsoptionlook<>'1' ;";
                //DataView dataOption = new DataView(condb.getDataTable(sqlDSOption));
                //if (dataOption != null && dataOption.Count > 0)
                //{
                //    for (int i = 0; i < dataOption.Count; i++)
                //    {
                //        if (dataOption[i]["toolsoptioncode"].ToString().ToUpper() == "ThoiGianCapNhatTbl_tools_bndangdt_tmp".ToUpper())
                //        {
                //            MSO2_MedicalRecord.GlobalStore.ThoiGianCapNhatTbl_tools_bndangdt_tmp = Utilities.Util_TypeConvertParse.ToInt64(dataOption[i]["toolsoptionvalue"].ToString());
                //        }

                //        if (dataOption[i]["toolsoptioncode"].ToString().ToUpper() == "KhoangThoiGianLayDuLieu".ToUpper())
                //        {
                //            MSO2_MedicalRecord.GlobalStore.KhoangThoiGianLayDuLieu = dataOption[i]["toolsoptionvalue"].ToString();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MSO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        private void KiemTraVaCopyFileLaucherNew()
        {
            try
            {
                string versionDatabase = "";
                DataView dataVer = new DataView(condb.getDataTable("SELECT appversion from mrd_version where app_type=1 LIMIT 1;"));
                if (dataVer != null && dataVer.Count > 0)
                {
                    versionDatabase = dataVer[0]["appversion"].ToString();
                }
                //lấy thông tin version của phần mềm MSO2 MedicalRecord Launcher.exe
                FileVersionInfo.GetVersionInfo(Path.Combine(Environment.CurrentDirectory, "MSO2 MedicalRecord Launcher.exe"));
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.CurrentDirectory + "\\MSO2 MedicalRecord Launcher.exe");
                if (myFileVersionInfo.FileVersion.ToString() != versionDatabase)
                {
                    DataView dataurlfile = new DataView(condb.getDataTable("select app_link from mrd_version where app_type=1 limit 1;"));
                    if (dataurlfile != null && dataurlfile.Count > 0)
                    {
                        string tempDirectory = dataurlfile[0]["app_link"].ToString();
                        CopyFolder(tempDirectory, Environment.CurrentDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                Base.Logging.Error(ex);
            }
        }
        private static void CopyFolder(string SourceFolder, string DestFolder)
        {
            try
            {
                Directory.CreateDirectory(DestFolder); //Tao folder moi
                string[] files = Directory.GetFiles(SourceFolder);
                //Neu co file thi phai copy file
                foreach (string file in files)
                {
                    try
                    {
                        string name = Path.GetFileName(file);
                        string dest = Path.Combine(DestFolder, name);
                        File.Copy(file, dest, true);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                //string[] folders = Directory.GetDirectories(SourceFolder);
                //foreach (string folder in folders)
                //{
                //    string name = Path.GetFileName(folder);
                //    string dest = Path.Combine(DestFolder, name);
                //    CopyFolder(folder, dest);
                //}
            }
            catch (Exception ex)
            {
                Base.Logging.Error(ex);
            }
        }




        #endregion
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Mã hóa thông tin để so sánh trong DB
                string en_txtUsername = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtUsername.Text.Trim().ToLower(), true);
                string en_txtPassword = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtPassword.Text.Trim(), true);

                if (txtUsername.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.Focus();
                    return;
                }
                // tạo 1 tài khoản ở trên PM, không chứa trong DB để làm tài khoản admin
                else if (txtUsername.Text.ToLower() == Base.KeyTrongPhanMem.AdminUser_key && txtPassword.Text == Base.KeyTrongPhanMem.AdminPass_key)
                {
                    SessionLogin.SessionUsercode = txtUsername.Text.Trim().ToLower();
                    SessionLogin.SessionUsername = "Administrator";
                    //Load data
                    SessionLogin.SessionLstPhanQuyenNguoiDung = MSO2_MedicalRecord.Base.CheckPermission.GetListPhanQuyenNguoiDung();
                    SessionLogin.SessionlstPhanQuyen_KhoaPhong = MSO2_MedicalRecord.Base.CheckPermission.GetPhanQuyen_KhoaPhong();
                    //SessionLogin.SessionLstPhanQuyen_KhoThuoc = MSO2_MedicalRecord.Base.CheckPermission.GetPhanQuyen_KhoThuoc();
                    //SessionLogin.SessionLstPhanQuyen_PhongLuu = MSO2_MedicalRecord.Base.CheckPermission.GetPhanQuyen_PhongLuu();
                    frmMain frmm = new frmMain();
                    frmm.Show();
                    this.Visible = false;
                    MSO2_MedicalRecord.Base.Logging.Info("Application open successfull. Time=" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
                }
                else
                {
                    try
                    {
                        string command = "SELECT usercode, username, userpassword FROM mrd_tbluser WHERE usercode='" + en_txtUsername + "' and userpassword='" + en_txtPassword + "';";
                        DataView dv = new DataView(condb.getDataTable(command));
                        if (dv != null && dv.Count > 0)
                        {
                            MSO2_MedicalRecord.FormCommon.DangKyBanQuyen.KiemTraLicense.KiemTraLicenseHopLe();
                            SessionLogin.SessionUsercode = txtUsername.Text.Trim().ToLower();
                            SessionLogin.SessionUsername = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(dv[0]["username"].ToString(), true);
                            //Load data
                            SessionLogin.SessionLstPhanQuyenNguoiDung = MSO2_MedicalRecord.Base.CheckPermission.GetListPhanQuyenNguoiDung();
                            SessionLogin.SessionlstPhanQuyen_KhoaPhong = MSO2_MedicalRecord.Base.CheckPermission.GetPhanQuyen_KhoaPhong();
                            //SessionLogin.SessionLstPhanQuyen_KhoThuoc = MSO2_MedicalRecord.Base.CheckPermission.GetPhanQuyen_KhoThuoc();
                            //SessionLogin.SessionLstPhanQuyen_PhongLuu = MSO2_MedicalRecord.Base.CheckPermission.GetPhanQuyen_PhongLuu();
                            frmMain frmm = new frmMain();
                            frmm.Show();
                            this.Visible = false;
                            MSO2_MedicalRecord.Base.Logging.Info("Application open successfull. Time=" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MSO2_MedicalRecord.Base.Logging.Error(ex);
                        txtUsername.Focus();
                    }
                }

                // Khi được check vào nút ghi nhớ thì sẽ lưu tên đăng nhập và mật khẩu vào file config
                if (checkEditNhoPass.Checked)
                {
                    Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    _config.AppSettings.Settings["checkEditNhoPass"].Value = Convert.ToString(checkEditNhoPass.Checked);
                    _config.AppSettings.Settings["LoginUser"].Value = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtUsername.Text.Trim(), true);
                    _config.AppSettings.Settings["LoginPassword"].Value = MSO2_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtPassword.Text.Trim(), true);
                    _config.Save(ConfigurationSaveMode.Modified);

                    ConfigurationManager.RefreshSection("appSettings");
                }
                // không thì sẽ xóa giá trị đã lưu trong file congfig đi
                else
                {
                    Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    _config.AppSettings.Settings["checkEditNhoPass"].Value = "false";
                    _config.AppSettings.Settings["LoginUser"].Value = "";
                    _config.AppSettings.Settings["LoginPassword"].Value = "";
                    _config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception ex)
            {
                MSO2_MedicalRecord.Base.Logging.Warn("Dang nhap " + ex.ToString());
            }
        }

        // Khi nhập username và nhấn enter thì forcus vào ô nhập pass
        private void txtUsername_Properties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        // Sau khi nhập password và ấn enter thì đăng nhập luôn
        private void txtPassword_Properties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        // nếu viết vào ô username = "config" thì mở ra bảng để cấu hình DB
        private void txtUsername_EditValueChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text.ToUpper() == "CONFIG")
            {
                frmConnectDB frmcon = new frmConnectDB();
                frmcon.Dock = System.Windows.Forms.DockStyle.Bottom;
                frmcon.ShowDialog();
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Application.Exit();
                this.Dispose();
            }
            catch (Exception ex)
            {
                MSO2_MedicalRecord.Base.Logging.Error(ex);
            }
        }

        private void linkTroGiup_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Liên hệ với tác giả để được trợ giúp! \nAuthor: Hồng Minh Nhất \nE-mail: hongminhnhat15@gmail.com \nPhone: 0868-915-456", "Thông tin về tác giả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
