using ChessLibrary;
using SolverLibrary;
using System.Windows.Forms;

namespace EightQueens
{
    public partial class frmMain : Form
    {
        ChessBoard _myBoard = new ChessBoard();
        int _iSuccess = 0, _iTotal = 0, _iExtraSuccess = 0;
        Boolean _bExtraCheck = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            int cellDimension = 60;
            int boardDimension = 8;
            int boardWidthHeight = cellDimension * boardDimension + 5;
            // configure visual board
            dgvChessBoard.ColumnCount = boardDimension;
            dgvChessBoard.AllowUserToAddRows = false; // Disable the last "new row"

            // Add 8 columns with headers
            for (int i = 0; i < boardDimension; i++)
            {
                dgvChessBoard.Columns[i].Name = $"Column {i + 1}";
                dgvChessBoard.Columns[i].Width = cellDimension;
            }

            // Add 8 rows
            for (int i = 0; i < boardDimension; i++)
            {
                dgvChessBoard.Rows.Add();
                this.dgvChessBoard.Rows[i].Height = cellDimension;
            }
            RefreshBoard();
            this.dgvChessBoard.DefaultCellStyle.SelectionBackColor = this.dgvChessBoard.DefaultCellStyle.BackColor;
            this.dgvChessBoard.DefaultCellStyle.SelectionForeColor = this.dgvChessBoard.DefaultCellStyle.ForeColor;
            dgvChessBoard.RowHeadersVisible = false;
            dgvChessBoard.ColumnHeadersVisible = false;
            dgvChessBoard.Width = boardWidthHeight;
            dgvChessBoard.Height = boardWidthHeight;
            chkHideNos.Top = dgvChessBoard.Top + boardWidthHeight + 20;
            cboStrategies.Items.Clear();
            cboStrategies.Items.Add("Hill Climb");
            cboStrategies.Items.Add("K Hill Climb");
            cboStrategies.Items.Add("Simulated Annealing");
            cboStrategies.Items.Add("Genetic Algorithm");
            cboStrategies.Items.Add("Random Restart Hill Climb");
            cboStrategies.Items.Add("Min Conflicts");
            cboStrategies.Items.Add("Tabu Search");
            cboStrategies.Items.Add("Stochastic Hill Climb");
            cboStrategies.Items.Add("K Simulated Annealing");
            cboStrategies.SelectedIndex = 0;
        }
        public void RefreshBoard()
        {
            this._myBoard.RandomizeQueens();
            this._myBoard.objStuffToStash = null;
            RedrawBoard();
            _iTotal++;
            _bExtraCheck = false;
        }
        public void RedrawBoard()
        {
            ClearBoard();
            ShowQueens();
            ShowConflicts();
        }
        public void ClearBoard()
        {
            if (dgvChessBoard.RowCount <= 0)
                return;

            for (Byte iRow = 0; iRow < 8; iRow++)
            {
                for (Byte iCol = 0; iCol < 8; iCol++)
                {
                    this.dgvChessBoard.Rows[iRow].Cells[iCol].Value = "";
                }
            }
        }
        public void ShowQueens()
        {
            if (dgvChessBoard.RowCount <= 0)
                return;

            foreach (Queen qn in _myBoard.Queens)
            {
                this.dgvChessBoard.Rows[qn.BoardPosition.Row].Cells[qn.BoardPosition.Column].Value = "Q";
            }
        }
        // calculate what sort of conflict if queen moved to this spot
        // and populate the board
        private void ShowConflicts()
        {
            for (Byte bytCol = 0; bytCol < 8; bytCol++)
            {
                for (Byte bytRow = 0; bytRow < 8; bytRow++)
                {
                    // add numbers to non-queen columns
                    if (this.dgvChessBoard[bytCol, bytRow].Value.ToString() != "Q")
                    {
                        if (!this.chkHideNos.Checked)
                            this.dgvChessBoard[bytCol, bytRow].Value = _myBoard.Tiles[bytCol * 8 + bytRow].Conflicts;
                        else
                            this.dgvChessBoard[bytCol, bytRow].Value = "";
                    }
                }
            }
        }

        private void btnCalcConflicts_Click(object sender, EventArgs e)
        {
            RedrawBoard();
            lblResults.Text = "Total Conflicts: " + this._myBoard.Queens[0].BoardPosition.Conflicts.ToString()
                + " Tries: " + _iTotal.ToString()
                + " Successes: " + _iSuccess.ToString()
                + " Extra Success: " + _iExtraSuccess.ToString();
        }

        private void dgvChessBoard_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sRowCol = dgvChessBoard.SelectedCells[0].RowIndex.ToString() +", " + dgvChessBoard.SelectedCells[0].ColumnIndex.ToString();
            lblResults.Text = sRowCol;
            this.btnMoveQueen.Enabled = true;
        }

        private void btnMoveQueen_Click(object sender, EventArgs e)
        {
            Byte bytCol = (Byte)this.dgvChessBoard.SelectedCells[0].ColumnIndex;
            Byte bytRow = (Byte)this.dgvChessBoard.SelectedCells[0].RowIndex;
            Tile tilLoc = new Tile(bytRow, bytCol);
            Queen qn = _myBoard.Queens[bytCol];
            this.dgvChessBoard[qn.BoardPosition.Column, qn.BoardPosition.Row].Value = "";
            this.dgvChessBoard[tilLoc.Column, tilLoc.Row].Value = "Q";
            //this.grdBoard[tilLoc.Column, tilLoc.Row].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
            _myBoard.Queens[bytCol].BoardPosition = tilLoc;
            //this.btnMoveQueen.Enabled = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyStrategy();
            RedrawBoard();
        }
        private void ApplyStrategy()
        {
            // if no strategy selected, then do nothing
            if (cboStrategies.Text == "")
            {
                MessageBox.Show("Please select a strategy");
                return;
            }

            SolutionStrategy ss = null;    // holder object
            // no need to check if we are done
            if (_myBoard.Status == "F")
            {
                this.btnApply.Enabled = false;
                return;
            }

            this.lblStatus.Text = "Applying Strategy...";
            Byte bytOldConflicts = this._myBoard.Queens[0].BoardPosition.Conflicts;
            // pick strategy
            switch (cboStrategies.Text)
            {
                case "Hill Climb":
                    ss = new HillClimbingStrategy(this._myBoard);
                    break;
                case "K Hill Climb":
                    ss = new KHillClimbStrategy(this._myBoard, (Byte)this.udTemp.Value);
                    break;
                case "Simulated Annealing":
                    ss = new SimulatedAnnealingStrategy(this._myBoard);
                    break;
                case "Genetic Algorithm":
                    ss = new GeneticStrategy(this._myBoard);
                    break;
                case "Random Restart Hill Climb":
                    ss = new RandomRestartStrategy(this._myBoard);
                    break;
                case "Min Conflicts":
                    ss = new MinConstraintStrategy(this._myBoard);
                    break;
                case "Tabu Search":
                    ss = new TabuStrategy(this._myBoard);
                    break;
                case "Stochastic Hill Climb":
                    ss = new StochasticHillClimb(this._myBoard);
                    break;
                case "K Simulated Annealing":
                    ss = new KSimulatedAnnealingStrategy(this._myBoard);
                    break;
            }

            // run strategy
            ss.ApplyStrategy();
            if (cboStrategies.Text == "Genetic Algorithm")
            {
                // for genetic algorithm we have to save all the original board states
                if (ss.NewConflicts < _myBoard.Queens[0].BoardPosition.Conflicts)
                    _myBoard = ss.TheBoard;
            }
            this.lblStatus.Text = ss.Status;
            if (_myBoard.Status == "S")   // switching methods
                _bExtraCheck = true;
            if (_myBoard.Status == "G")
            {
                _iSuccess++;
                if (_bExtraCheck)
                    _iExtraSuccess++;
            }
            if (_myBoard.Status == "F" || _myBoard.Status == "G")
            {
                this.tmrWait.Enabled = false;
                this.btnApply.Enabled = false;
            }
            // if looping and not done, then continue
            if (this.chkLoop.Checked)
            {
                if (_myBoard.Status != "F" && _myBoard.Status != "G")
                    this.tmrWait.Enabled = true;
            }
            if (this.chkRefreshRepeat.Checked)
            {
                if (_myBoard.Status == "F" || _myBoard.Status == "G")
                {
                    if (_iTotal <= this.udMaxTests.Value)
                    {
                        RefreshBoard();
                        this.tmrWait.Enabled = true;
                    }
                    else
                        Microsoft.VisualBasic.Interaction.Beep();
                }
            }
            lblResults.Text = "Total Conflicts: " + this._myBoard.Queens[0].BoardPosition.Conflicts.ToString()
                + " Tries: " + _iTotal.ToString()
                + " Successes: " + _iSuccess.ToString()
                + " Extra Success: " + _iExtraSuccess.ToString();
            Application.DoEvents();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshBoard();
            this.btnApply.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (Byte iRow = 0; iRow < 8; iRow++)
            {
                for (Byte iCol = 0; iCol < 8; iCol++)
                {
                    if (this.dgvChessBoard[iCol, iRow].Value.ToString() != "Q")
                        this.dgvChessBoard.Rows[iRow].Cells[iCol].Value = "";
                }
            }
        }

        private void tmrWait_Tick(object sender, EventArgs e)
        {
            this.tmrWait.Enabled = false;
            //ShowSolution();
            System.Threading.Thread.Sleep((int)this.udWaitTime.Value);
            ApplyStrategy();
            RedrawBoard();
            System.Threading.Thread.Sleep((int)this.udWaitTime.Value);
        }

        private void btnPossibleSolution_Click(object sender, EventArgs e)
        {
            ShowSolution();
        }
        private void ShowSolution()
        {
            MessageBox.Show("This function in not available yet");
            //Tile loc = this._myBoard.NextFreeTile();
            //if(loc != null)
            //    this.grdBoard.Rows[loc.Row].Cells[loc.Column].Selected = true;
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkLoop.Checked == false)
                this.btnApply.Enabled = true;
        }

        private void cboStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSwitches();
        }
        private void SetSwitches()
        {
            this.lblTemperature.Text = "Not Applicable";
            this.udTemp.Enabled = false;
            if (cboStrategies.Text == "K Hill Climbing")
            {
                this.lblTemperature.Text = "Slope of Stochastic";
                udTemp.Maximum = 10;
                udTemp.Minimum = 1;
                udTemp.Value = 3;
                this.udTemp.Enabled = true;
            }
            if (cboStrategies.Text == "Simulated Annealing"
                || cboStrategies.Text == "K Simulated Annealing")
            {
                this.lblTemperature.Text = "Temperature";
                udTemp.Maximum = 10000;
                udTemp.Minimum = 1;
                udTemp.Value = 1000;
                this.udTemp.Enabled = true;
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
