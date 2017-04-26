﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTab;

namespace MeO2_MedicalRecord.FormCommon
{
    public partial class ucDashboard : UserControl
    {
        #region Declaration
        MeO2_MedicalRecord.Base.ConnectDatabase condb = new MeO2_MedicalRecord.Base.ConnectDatabase();
        public string CurrentTabPage { get; set; }
        public int SelectedTabPageIndex { get; set; }
        internal frmMain frmMain;

        // khai báo 1 hàm delegate
        public delegate void GetString(string thoigian);
        // khai báo 1 kiểu hàm delegate
        public GetString MyGetData;

        #endregion

        public ucDashboard()
        {
            InitializeComponent();
        }

        private void ucDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                EnabledAndDisableControl();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        private void EnabledAndDisableControl()
        {
            try
            {
                navBarBCQLTongTheKhoa.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_01");
                navBarBCBenhNhanNoiTru.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_02");
                navBarBCBenhNhanNgoaiTru.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_03");
                navBarBCTongHopToanVien.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_04");
                navBarBCDTCLS.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_05");
                navBarBCXNTTuTruc.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_06");
                navBarBCBNSDThuocTaiKhoa.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_07");

                navBarDBDTTungKhoa.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_08");
                navBarDBBenhNhanNoiTru.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_09");
                navBarBCTHDTKhoa.Visible = MeO2_MedicalRecord.Base.CheckPermission.ChkPerModule("DASHBOARD_10");
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        #region Tabcontrol function
        //Dong tab
        private void xtraTabControlDashboard_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                XtraTabControl xtab = (XtraTabControl)sender;
                int i = xtab.SelectedTabPageIndex;
                DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs arg = e as DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs;
                xtab.TabPages.Remove((arg.Page as XtraTabPage));
                xtab.SelectedTabPageIndex = i - 1;
                //(arg.Page as XtraTabPage).PageVisible = false;
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void xtraTabControlDashboard_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                frmMain = new frmMain();
                this.CurrentTabPage = e.Page.Name;
                XtraTabControl xtab = new XtraTabControl();
                xtab = (XtraTabControl)sender;
                if (xtab != null)
                {
                    this.SelectedTabPageIndex = xtab.SelectedTabPageIndex;
                    //delegate - thong tin chuc nang
                    if (MyGetData != null)
                    {// tại đây gọi nó
                        MyGetData(xtab.TabPages[xtab.SelectedTabPageIndex].Tooltip);
                    }
                }
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        #endregion
        private void navBarBCQLTongTheKhoa_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_01");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_01", "BC quản lý tổng thể khoa", "BC quản lý tổng thể khoa. Lấy theo tiêu chí thời gian duyệt viện phí; doanh thu chia theo khoa/phòng bệnh nhân ra viện", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarBCBenhNhanNoiTru_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_02");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_02", "BC bệnh nhân nội trú", "BC bệnh nhân nội trú. Lấy theo tiêu chí thời gian duyệt viện phí; doanh thu chia theo khoa/phòng bệnh nhân ra viện", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarBCBenhNhanNgoaiTru_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_03");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_03", "BC bệnh nhân ngoại trú", "BC bệnh nhân ngoại trú. Lấy theo tiêu chí thời gian bệnh nhân đến khám; doanh thu chia theo khoa/phòng chỉ định", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarBCTongHopToanVien_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_04");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_04", "BC tổng hợp toàn viện", "BC tổng hợp toàn viện. Lấy theo tiêu chí thời gian duyệt viện phí; doanh thu chia theo khoa/phòng bệnh nhân ra viện", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarBCDTCLS_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_05");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_05", "BC doanh thu cận lâm sàng", "BC doanh thu cận lâm sàng. Lấy theo tiêu chí thời gian duyệt viện phí; doanh thu chia theo khoa/phòng chỉ định", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarBCXNTTuTruc_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_06");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_06", "BC xuất nhập tồn tủ trực", "Dashboard BC xuất nhập tồn tủ trực", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarBCBNSDThuocTaiKhoa_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_07");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_07", "BCBN sử dụng thuốc/vật tư tại khoa", "BC bệnh nhân sử dụng thuốc/vật tư tại khoa", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarDBDTTungKhoa_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_08");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_08", "Biểu đồ doanh thu khoa", "Biểu đồ doanh thu khoa", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
        private void navBarDBBenhNhanNoiTru_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_09");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_09", "Biểu đồ doanh thu các khoa nội trú", "Biểu đồ doanh thu các khoa nội trú", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        private void navBarBCTHDTKhoa_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                UserControl ucControlActive = new UserControl();
                ucControlActive = TabControlProcess.SelectUCControlActive("DASHBOARD_10");
                MeO2_MedicalRecord.FormCommon.TabControlProcess.TabCreating(xtraTabControlChucNang, "DASHBOARD_10", "Báo cáo tổng hợp doanh thu khoa - toàn viện", "Báo cáo tổng hợp doanh thu khoa - toàn viện. Doanh thu chia theo khoa/phòng chỉ định", ucControlActive);
                ucControlActive.Show();
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }




    }
}