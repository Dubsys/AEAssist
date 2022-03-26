﻿using System.ComponentModel;

namespace AEAssist
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainTab = new System.Windows.Forms.TabControl();
            this.GeneralTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.BardTab = new System.Windows.Forms.TabPage();
            this.MainTab.SuspendLayout();
            this.GeneralTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.GeneralTab);
            this.MainTab.Controls.Add(this.BardTab);
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.Location = new System.Drawing.Point(0, 0);
            this.MainTab.Multiline = true;
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(800, 450);
            this.MainTab.TabIndex = 1;
            // 
            // GeneralTab
            // 
            this.GeneralTab.BackColor = System.Drawing.Color.Silver;
            this.GeneralTab.Controls.Add(this.label1);
            this.GeneralTab.Location = new System.Drawing.Point(4, 22);
            this.GeneralTab.Name = "GeneralTab";
            this.GeneralTab.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTab.Size = new System.Drawing.Size(792, 424);
            this.GeneralTab.TabIndex = 0;
            this.GeneralTab.Text = "GeneralSetting";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(68, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 62);
            this.label1.TabIndex = 0;
            this.label1.Text = "欢迎使用AEAssist,这个界面是通用设置界面";
            // 
            // BardTab
            // 
            this.BardTab.Location = new System.Drawing.Point(4, 22);
            this.BardTab.Name = "BardTab";
            this.BardTab.Size = new System.Drawing.Size(792, 424);
            this.BardTab.TabIndex = 1;
            this.BardTab.Text = "BardSetting";
            this.BardTab.UseVisualStyleBackColor = true;
            // 
            // GUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainTab);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GUI";
            this.ShowIcon = false;
            this.Text = "AEAssist";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.MainTab.ResumeLayout(false);
            this.GeneralTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabPage GeneralTab;
        private System.Windows.Forms.TabPage BardTab;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.TabControl MainTab;

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;

        #endregion
    }
}