namespace 五子棋
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBlackScore = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWhiteScore = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblBlackSpan = new System.Windows.Forms.Label();
            this.lblWhiteSpan = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panDetail = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUndoPlay = new System.Windows.Forms.Button();
            this.lblWhiteLoc = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBlackLoc = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnAB = new System.Windows.Forms.Button();
            this.panDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(36, 314);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 490);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "声音";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(37, 373);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "自动下子";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnAutoPlay_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "黑棋得分：";
            // 
            // lblBlackScore
            // 
            this.lblBlackScore.AutoSize = true;
            this.lblBlackScore.Location = new System.Drawing.Point(88, 24);
            this.lblBlackScore.Name = "lblBlackScore";
            this.lblBlackScore.Size = new System.Drawing.Size(11, 12);
            this.lblBlackScore.TabIndex = 5;
            this.lblBlackScore.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "白棋得分：";
            // 
            // lblWhiteScore
            // 
            this.lblWhiteScore.AutoSize = true;
            this.lblWhiteScore.Location = new System.Drawing.Point(88, 53);
            this.lblWhiteScore.Name = "lblWhiteScore";
            this.lblWhiteScore.Size = new System.Drawing.Size(11, 12);
            this.lblWhiteScore.TabIndex = 7;
            this.lblWhiteScore.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "黑棋耗时：";
            // 
            // lblBlackSpan
            // 
            this.lblBlackSpan.AutoSize = true;
            this.lblBlackSpan.Location = new System.Drawing.Point(88, 97);
            this.lblBlackSpan.Name = "lblBlackSpan";
            this.lblBlackSpan.Size = new System.Drawing.Size(11, 12);
            this.lblBlackSpan.TabIndex = 9;
            this.lblBlackSpan.Text = "0";
            // 
            // lblWhiteSpan
            // 
            this.lblWhiteSpan.AutoSize = true;
            this.lblWhiteSpan.Location = new System.Drawing.Point(89, 127);
            this.lblWhiteSpan.Name = "lblWhiteSpan";
            this.lblWhiteSpan.Size = new System.Drawing.Size(11, 12);
            this.lblWhiteSpan.TabIndex = 11;
            this.lblWhiteSpan.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "白棋耗时：";
            // 
            // panDetail
            // 
            this.panDetail.Controls.Add(this.btnAB);
            this.panDetail.Controls.Add(this.label1);
            this.panDetail.Controls.Add(this.btnUndoPlay);
            this.panDetail.Controls.Add(this.button1);
            this.panDetail.Controls.Add(this.lblWhiteLoc);
            this.panDetail.Controls.Add(this.label3);
            this.panDetail.Controls.Add(this.button2);
            this.panDetail.Controls.Add(this.label2);
            this.panDetail.Controls.Add(this.lblWhiteSpan);
            this.panDetail.Controls.Add(this.btnStart);
            this.panDetail.Controls.Add(this.lblBlackScore);
            this.panDetail.Controls.Add(this.lblBlackLoc);
            this.panDetail.Controls.Add(this.label9);
            this.panDetail.Controls.Add(this.label4);
            this.panDetail.Controls.Add(this.lblBlackSpan);
            this.panDetail.Controls.Add(this.lblWhiteScore);
            this.panDetail.Controls.Add(this.label6);
            this.panDetail.Location = new System.Drawing.Point(779, 12);
            this.panDetail.Name = "panDetail";
            this.panDetail.Size = new System.Drawing.Size(145, 751);
            this.panDetail.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "白棋位置：";
            // 
            // btnUndoPlay
            // 
            this.btnUndoPlay.Location = new System.Drawing.Point(37, 428);
            this.btnUndoPlay.Name = "btnUndoPlay";
            this.btnUndoPlay.Size = new System.Drawing.Size(75, 23);
            this.btnUndoPlay.TabIndex = 13;
            this.btnUndoPlay.Text = "悔棋";
            this.btnUndoPlay.UseVisualStyleBackColor = true;
            this.btnUndoPlay.Click += new System.EventHandler(this.btnUndoPlay_Click);
            // 
            // lblWhiteLoc
            // 
            this.lblWhiteLoc.AutoSize = true;
            this.lblWhiteLoc.Location = new System.Drawing.Point(89, 179);
            this.lblWhiteLoc.Name = "lblWhiteLoc";
            this.lblWhiteLoc.Size = new System.Drawing.Size(23, 12);
            this.lblWhiteLoc.TabIndex = 13;
            this.lblWhiteLoc.Text = "0,0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "黑棋位置：";
            // 
            // lblBlackLoc
            // 
            this.lblBlackLoc.AutoSize = true;
            this.lblBlackLoc.Location = new System.Drawing.Point(89, 208);
            this.lblBlackLoc.Name = "lblBlackLoc";
            this.lblBlackLoc.Size = new System.Drawing.Size(23, 12);
            this.lblBlackLoc.TabIndex = 1;
            this.lblBlackLoc.Text = "0,0";
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(751, 751);
            this.mainPanel.TabIndex = 14;
            // 
            // btnAB
            // 
            this.btnAB.Location = new System.Drawing.Point(37, 544);
            this.btnAB.Name = "btnAB";
            this.btnAB.Size = new System.Drawing.Size(75, 23);
            this.btnAB.TabIndex = 15;
            this.btnAB.Text = "button3";
            this.btnAB.UseVisualStyleBackColor = true;
            this.btnAB.Click += new System.EventHandler(this.btnAB_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 777);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.panDetail);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gobang";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panDetail.ResumeLayout(false);
            this.panDetail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBlackScore;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblWhiteScore;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblBlackSpan;
        private System.Windows.Forms.Label lblWhiteSpan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panDetail;
        private System.Windows.Forms.Label lblBlackLoc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWhiteLoc;
        private System.Windows.Forms.Button btnUndoPlay;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button btnAB;
    }
}

