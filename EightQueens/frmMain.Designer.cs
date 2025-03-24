namespace EightQueens
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dgvChessBoard = new DataGridView();
            gbAutomation = new GroupBox();
            udMaxTests = new NumericUpDown();
            lblMaxTests = new Label();
            udWaitTime = new NumericUpDown();
            lblDelay = new Label();
            chkRefreshRepeat = new CheckBox();
            chkLoop = new CheckBox();
            udAlpha = new NumericUpDown();
            lblAlpha = new Label();
            btnMoveQueen = new Button();
            btnApply = new Button();
            btnRefresh = new Button();
            btnClear = new Button();
            btnPossibleSolution = new Button();
            btnCalcConflicts = new Button();
            chkHideNos = new CheckBox();
            udTemp = new NumericUpDown();
            lblTemperature = new Label();
            tmrWait = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            lblResults = new ToolStripStatusLabel();
            cboStrategies = new ComboBox();
            lblStrategy = new Label();
            menuStrip1 = new MenuStrip();
            mnuFile = new ToolStripMenuItem();
            mnuExit = new ToolStripMenuItem();
            gbSettings = new GroupBox();
            gbControlPanel = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dgvChessBoard).BeginInit();
            gbAutomation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)udMaxTests).BeginInit();
            ((System.ComponentModel.ISupportInitialize)udWaitTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)udAlpha).BeginInit();
            ((System.ComponentModel.ISupportInitialize)udTemp).BeginInit();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            gbSettings.SuspendLayout();
            gbControlPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dgvChessBoard
            // 
            dgvChessBoard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChessBoard.Location = new Point(8, 22);
            dgvChessBoard.Margin = new Padding(1);
            dgvChessBoard.Name = "dgvChessBoard";
            dgvChessBoard.RowHeadersWidth = 82;
            dgvChessBoard.Size = new Size(239, 209);
            dgvChessBoard.TabIndex = 0;
            dgvChessBoard.CellClick += dgvChessBoard_CellClick;
            // 
            // gbAutomation
            // 
            gbAutomation.Controls.Add(udMaxTests);
            gbAutomation.Controls.Add(lblMaxTests);
            gbAutomation.Controls.Add(udWaitTime);
            gbAutomation.Controls.Add(lblDelay);
            gbAutomation.Controls.Add(chkRefreshRepeat);
            gbAutomation.Controls.Add(chkLoop);
            gbAutomation.Location = new Point(544, 301);
            gbAutomation.Margin = new Padding(1);
            gbAutomation.Name = "gbAutomation";
            gbAutomation.Padding = new Padding(1);
            gbAutomation.Size = new Size(303, 106);
            gbAutomation.TabIndex = 1;
            gbAutomation.TabStop = false;
            gbAutomation.Text = "Automation";
            // 
            // udMaxTests
            // 
            udMaxTests.Location = new Point(75, 62);
            udMaxTests.Margin = new Padding(1);
            udMaxTests.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            udMaxTests.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            udMaxTests.Name = "udMaxTests";
            udMaxTests.Size = new Size(59, 23);
            udMaxTests.TabIndex = 7;
            udMaxTests.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // lblMaxTests
            // 
            lblMaxTests.AutoSize = true;
            lblMaxTests.Location = new Point(3, 63);
            lblMaxTests.Margin = new Padding(1, 0, 1, 0);
            lblMaxTests.Name = "lblMaxTests";
            lblMaxTests.Size = new Size(58, 15);
            lblMaxTests.TabIndex = 6;
            lblMaxTests.Text = "Max Tests";
            // 
            // udWaitTime
            // 
            udWaitTime.Location = new Point(48, 22);
            udWaitTime.Margin = new Padding(1);
            udWaitTime.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            udWaitTime.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            udWaitTime.Name = "udWaitTime";
            udWaitTime.Size = new Size(63, 23);
            udWaitTime.TabIndex = 3;
            udWaitTime.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // lblDelay
            // 
            lblDelay.AutoSize = true;
            lblDelay.Location = new Point(3, 23);
            lblDelay.Margin = new Padding(1, 0, 1, 0);
            lblDelay.Name = "lblDelay";
            lblDelay.Size = new Size(39, 15);
            lblDelay.TabIndex = 2;
            lblDelay.Text = "Delay:";
            // 
            // chkRefreshRepeat
            // 
            chkRefreshRepeat.AutoSize = true;
            chkRefreshRepeat.Location = new Point(158, 62);
            chkRefreshRepeat.Margin = new Padding(1);
            chkRefreshRepeat.Name = "chkRefreshRepeat";
            chkRefreshRepeat.Size = new Size(127, 19);
            chkRefreshRepeat.TabIndex = 1;
            chkRefreshRepeat.Text = "Refresh and Repeat";
            chkRefreshRepeat.UseVisualStyleBackColor = true;
            // 
            // chkLoop
            // 
            chkLoop.AutoSize = true;
            chkLoop.Location = new Point(158, 23);
            chkLoop.Margin = new Padding(1);
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new Size(133, 19);
            chkLoop.TabIndex = 0;
            chkLoop.Text = "Loop to Completion";
            chkLoop.UseVisualStyleBackColor = true;
            chkLoop.CheckedChanged += chkLoop_CheckedChanged;
            // 
            // udAlpha
            // 
            udAlpha.Location = new Point(211, 67);
            udAlpha.Margin = new Padding(1);
            udAlpha.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            udAlpha.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            udAlpha.Name = "udAlpha";
            udAlpha.Size = new Size(48, 23);
            udAlpha.TabIndex = 5;
            udAlpha.Value = new decimal(new int[] { 990, 0, 0, 0 });
            // 
            // lblAlpha
            // 
            lblAlpha.AutoSize = true;
            lblAlpha.Location = new Point(167, 68);
            lblAlpha.Margin = new Padding(1, 0, 1, 0);
            lblAlpha.Name = "lblAlpha";
            lblAlpha.Size = new Size(38, 15);
            lblAlpha.TabIndex = 4;
            lblAlpha.Text = "Alpha";
            // 
            // btnMoveQueen
            // 
            btnMoveQueen.Location = new Point(4, 133);
            btnMoveQueen.Margin = new Padding(1);
            btnMoveQueen.Name = "btnMoveQueen";
            btnMoveQueen.Size = new Size(80, 35);
            btnMoveQueen.TabIndex = 0;
            btnMoveQueen.Text = "Move Queen";
            btnMoveQueen.UseVisualStyleBackColor = true;
            btnMoveQueen.Click += btnMoveQueen_Click;
            // 
            // btnApply
            // 
            btnApply.Location = new Point(4, 18);
            btnApply.Margin = new Padding(1);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(80, 35);
            btnApply.TabIndex = 2;
            btnApply.Text = "Apply Strategy";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(4, 170);
            btnRefresh.Margin = new Padding(1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(80, 35);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(4, 94);
            btnClear.Margin = new Padding(1);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(80, 36);
            btnClear.TabIndex = 2;
            btnClear.Text = "Clear #s";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnPossibleSolution
            // 
            btnPossibleSolution.Location = new Point(4, 208);
            btnPossibleSolution.Margin = new Padding(1);
            btnPossibleSolution.Name = "btnPossibleSolution";
            btnPossibleSolution.Size = new Size(80, 36);
            btnPossibleSolution.TabIndex = 1;
            btnPossibleSolution.Text = "Possible Solution";
            btnPossibleSolution.UseVisualStyleBackColor = true;
            btnPossibleSolution.Click += btnPossibleSolution_Click;
            // 
            // btnCalcConflicts
            // 
            btnCalcConflicts.Location = new Point(4, 56);
            btnCalcConflicts.Margin = new Padding(1);
            btnCalcConflicts.Name = "btnCalcConflicts";
            btnCalcConflicts.Size = new Size(80, 36);
            btnCalcConflicts.TabIndex = 0;
            btnCalcConflicts.Text = "Calc Conflicts";
            btnCalcConflicts.UseVisualStyleBackColor = true;
            btnCalcConflicts.Click += btnCalcConflicts_Click;
            // 
            // chkHideNos
            // 
            chkHideNos.AutoSize = true;
            chkHideNos.Location = new Point(478, 522);
            chkHideNos.Margin = new Padding(1);
            chkHideNos.Name = "chkHideNos";
            chkHideNos.Size = new Size(66, 19);
            chkHideNos.TabIndex = 3;
            chkHideNos.Text = "Hide #s";
            chkHideNos.UseVisualStyleBackColor = true;
            // 
            // udTemp
            // 
            udTemp.Location = new Point(95, 67);
            udTemp.Margin = new Padding(1);
            udTemp.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            udTemp.Name = "udTemp";
            udTemp.Size = new Size(56, 23);
            udTemp.TabIndex = 10;
            udTemp.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // lblTemperature
            // 
            lblTemperature.AutoSize = true;
            lblTemperature.Location = new Point(8, 67);
            lblTemperature.Margin = new Padding(1, 0, 1, 0);
            lblTemperature.Name = "lblTemperature";
            lblTemperature.Size = new Size(74, 15);
            lblTemperature.TabIndex = 9;
            lblTemperature.Text = "Temperature";
            // 
            // tmrWait
            // 
            tmrWait.Tick += tmrWait_Tick;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(32, 32);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus, lblResults });
            statusStrip1.Location = new Point(0, 543);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 8, 0);
            statusStrip1.Size = new Size(850, 22);
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(797, 17);
            lblStatus.Spring = true;
            lblStatus.Text = "Status";
            // 
            // lblResults
            // 
            lblResults.Name = "lblResults";
            lblResults.Size = new Size(44, 17);
            lblResults.Text = "Results";
            // 
            // cboStrategies
            // 
            cboStrategies.FormattingEnabled = true;
            cboStrategies.Items.AddRange(new object[] { "Hill Climb", "Stochastic Hill Climb", "DD-Stochastic Hill Climb", "Random Restart Hill Climb", "Simulated Annealing", "Min Conflicts", "Genetic Mutation", "Tabu Search", "DD-Simulated Annealing" });
            cboStrategies.Location = new Point(8, 38);
            cboStrategies.Margin = new Padding(1);
            cboStrategies.Name = "cboStrategies";
            cboStrategies.Size = new Size(260, 23);
            cboStrategies.TabIndex = 8;
            cboStrategies.SelectedIndexChanged += cboStrategies_SelectedIndexChanged;
            // 
            // lblStrategy
            // 
            lblStrategy.AutoSize = true;
            lblStrategy.Location = new Point(8, 17);
            lblStrategy.Margin = new Padding(1, 0, 1, 0);
            lblStrategy.Name = "lblStrategy";
            lblStrategy.Size = new Size(50, 15);
            lblStrategy.TabIndex = 9;
            lblStrategy.Text = "Strategy";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { mnuFile });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(4, 1, 0, 1);
            menuStrip1.Size = new Size(850, 24);
            menuStrip1.TabIndex = 11;
            menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuExit });
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new Size(37, 22);
            mnuFile.Text = "&File";
            // 
            // mnuExit
            // 
            mnuExit.Name = "mnuExit";
            mnuExit.Size = new Size(92, 22);
            mnuExit.Text = "E&xit";
            mnuExit.Click += mnuExit_Click;
            // 
            // gbSettings
            // 
            gbSettings.Controls.Add(cboStrategies);
            gbSettings.Controls.Add(lblStrategy);
            gbSettings.Controls.Add(lblAlpha);
            gbSettings.Controls.Add(udAlpha);
            gbSettings.Controls.Add(udTemp);
            gbSettings.Controls.Add(lblTemperature);
            gbSettings.Location = new Point(547, 410);
            gbSettings.Margin = new Padding(2);
            gbSettings.Name = "gbSettings";
            gbSettings.Padding = new Padding(2);
            gbSettings.Size = new Size(300, 131);
            gbSettings.TabIndex = 12;
            gbSettings.TabStop = false;
            gbSettings.Text = "Settings";
            // 
            // gbControlPanel
            // 
            gbControlPanel.Controls.Add(btnRefresh);
            gbControlPanel.Controls.Add(btnApply);
            gbControlPanel.Controls.Add(btnPossibleSolution);
            gbControlPanel.Controls.Add(btnClear);
            gbControlPanel.Controls.Add(btnMoveQueen);
            gbControlPanel.Controls.Add(btnCalcConflicts);
            gbControlPanel.Location = new Point(558, 28);
            gbControlPanel.Margin = new Padding(2);
            gbControlPanel.Name = "gbControlPanel";
            gbControlPanel.Padding = new Padding(2);
            gbControlPanel.Size = new Size(177, 268);
            gbControlPanel.TabIndex = 13;
            gbControlPanel.TabStop = false;
            gbControlPanel.Text = "Control Panel";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(850, 565);
            Controls.Add(gbControlPanel);
            Controls.Add(gbSettings);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(gbAutomation);
            Controls.Add(chkHideNos);
            Controls.Add(dgvChessBoard);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(1);
            Name = "frmMain";
            Text = "8 Queens Demo";
            Load += frmMain_Load;
            ((System.ComponentModel.ISupportInitialize)dgvChessBoard).EndInit();
            gbAutomation.ResumeLayout(false);
            gbAutomation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)udMaxTests).EndInit();
            ((System.ComponentModel.ISupportInitialize)udWaitTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)udAlpha).EndInit();
            ((System.ComponentModel.ISupportInitialize)udTemp).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            gbSettings.ResumeLayout(false);
            gbSettings.PerformLayout();
            gbControlPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvChessBoard;
        private GroupBox gbAutomation;
        private Button btnApply;
        private Button btnRefresh;
        private Button btnMoveQueen;
        private Button btnClear;
        private Button btnPossibleSolution;
        private Button btnCalcConflicts;
        private CheckBox chkRefreshRepeat;
        private CheckBox chkLoop;
        private Label lblDelay;
        private NumericUpDown udWaitTime;
        private NumericUpDown udAlpha;
        private Label lblAlpha;
        private Label lblMaxTests;
        private CheckBox chkHideNos;
        private NumericUpDown udMaxTests;
        private NumericUpDown udTemp;
        private Label lblTemperature;
        private System.Windows.Forms.Timer tmrWait;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblResults;
        private ComboBox cboStrategies;
        private Label lblStrategy;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuExit;
        private GroupBox gbSettings;
        private GroupBox gbControlPanel;
    }
}
