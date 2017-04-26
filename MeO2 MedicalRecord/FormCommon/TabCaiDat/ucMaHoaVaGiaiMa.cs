﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeO2_MedicalRecord.FormCommon.TabCaiDat
{
    public partial class ucMaHoaVaGiaiMa : UserControl
    {
        public ucMaHoaVaGiaiMa()
        {
            InitializeComponent();
        }

        private void btnMaHoa_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtDauRa.Text = MeO2_MedicalRecord.Base.EncryptAndDecrypt.Encrypt(txtDauVao.Text, true);
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        private void btnGiaiMa_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtDauRa.Text = MeO2_MedicalRecord.Base.EncryptAndDecrypt.Decrypt(txtDauVao.Text, true);
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(txtDauRa.Text);
            }
            catch (Exception ex)
            {
                MeO2_MedicalRecord.Base.Logging.Warn(ex);
            }
        }
    }
}