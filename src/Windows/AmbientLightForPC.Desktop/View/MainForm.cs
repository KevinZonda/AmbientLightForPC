﻿using System;
using System.Drawing;
using System.Windows.Forms;
using AmbientLightForPC.Library;
using AmbientLightForPC.Library.Controller;
using AmbientLightForPC.Plugin;

namespace AmbientLightForPC.Desktop.View
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void tkbBrightness_Scroll(object sender, EventArgs e)
        {
            numBrightness.Value = tkbBrightness.Value;
        }

        private void numBrightness_ValueChanged(object sender, EventArgs e)
        {
            tkbBrightness.Value = (int) numBrightness.Value;

            if (ckbAutoApply.Checked)
                Apply();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbBrightnessController.Items.Add(Shared.DefaultBrightnessController);
            cmbBrightnessController.Items.Add(Shared.GammaBrightnessController);
            cmbBrightnessController.SelectedIndex = 0;
            ntfMain.Icon = SystemIcons.Application;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void cmbBrightnessController_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selected = cmbBrightnessController.SelectedItem;
            if (selected == null)
                return;
            BrightnessControllerBase bcb = (BrightnessControllerBase) selected;

            lblControllerInfo.Text = $"Name: {bcb.Name}\nDescription: {bcb.Description}";

            tkbBrightness.Value = (int) (numBrightness.Value = bcb.TryGetBrightness());
        }

        private void Apply()
        {
            object selected = cmbBrightnessController.SelectedItem;
            if (selected == null)
                return;
            BrightnessControllerBase bcb = (BrightnessControllerBase) selected;
            bcb.TrySetBrightness((byte) tkbBrightness.Value);
        }

        private void ckbAutoApply_CheckedChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = !ckbAutoApply.Checked;
            if (ckbAutoApply.Checked)
                Apply();
        }

        private void ckbOverNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbOverNormal.Checked)
            {
                if (MessageBox.Show(
                    "Over 100 value may cause display unproperly! Are you sure you want to do this?",
                    "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    ckbOverNormal.Checked = false;
                }
            }
            else
            {
                if (tkbBrightness.Value > 100)
                    tkbBrightness.Value = (int) (numBrightness.Value = 100);
            }

            tkbBrightness.Maximum = (int) (numBrightness.Maximum = ckbOverNormal.Checked ? 255 : 100);
        }

        private void ntfMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            ntfMain.Visible = false;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                return;
            ntfMain.Visible = true;
            ShowInTaskbar = false;
        }
    }
}