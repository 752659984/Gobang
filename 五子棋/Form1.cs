using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 五子棋
{
    public partial class Form1 : Form
    {
        //棋盘
        Gobang g = null;
        //每个格子的像素大小
        const int pSize = 50;
        //棋盘相对于容器的起点x,y坐标
        const int pBegin = 25;
        //微软语音
        SpeechSynthesizer synth = null;
        //记录棋盘的文件名
        string fileName = "";

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            lblWhiteLoc.Text = "0,0";
            lblBlackLoc.Text = "0,0";
            lblBlackScore.Text = "0";
            lblWhiteScore.Text = "0";
            lblWhiteSpan.Text = "0";
            lblBlackSpan.Text = "0";
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Paint += Form1_Paint;

            InitControl();
        }

        /// <summary>
        /// 窗口重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (mainPanel != null && g != null)
            {
                DrawGobang(mainPanel, pBegin);
                DrawCurrentPiece(mainPanel, g);
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            InitControl();

            fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".txt";

            g = new Gobang(15);

            mainPanel.MouseClick -= Panel_MouseClick;
            mainPanel.MouseClick += Panel_MouseClick;

            mainPanel.Refresh();

            DrawGobang(mainPanel, pBegin);
        }

        /// <summary>
        /// 测试声音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            AipSdkHelp.Speech("aaaaaaaaaaaaa", 5, 7, 4);

            if (synth != null)
            {
                synth.Dispose();
            }
            synth = new SpeechSynthesizer();
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("aaaaaaaaaaa");
        }

        /// <summary>
        /// 自动下棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoPlay_Click(object sender, EventArgs e)
        {
            AutoPlayChess();
        }

        /// <summary>
        /// 悔棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndoPlay_Click(object sender, EventArgs e)
        {
            if (g != null && !g.IsGameOver)
            {
                g.UndoPlayChess();

                mainPanel.Refresh();
                DrawGobang(mainPanel, pBegin);
                DrawCurrentPiece(mainPanel, g);

                if (g.Index - 1 >= 0)
                {
                    var cur = g.PlayChessLog[g.Index - 1];
                    MoveRedPoint(cur.X, cur.Y);
                }
            }
        }


        

        /// <summary>
        /// 绘制当前存在的棋子
        /// </summary>
        private void DrawCurrentPiece(Control c, Gobang gob)
        {
            var cb = gob.GetCheckerboard();
            if (cb != null)
            {
                //var ps = cb.Select(p => p.Where(q => q.Color != PieceColor.None).First()).ToArray();
                //foreach (var p in ps)
                //{

                //}
                for (var i = 0; i < cb.Length; ++i)
                {
                    for (var j = 0; j < cb[i].Length; ++j)
                    {
                        if (cb[i][j].Color != PieceColor.None)
                        {
                            var b = cb[i][j].Color == PieceColor.Black ? Brushes.Black : Brushes.White;
                            DrawPiece(c, cb[i][j].X * pSize + pBegin, cb[i][j].Y * pSize + pBegin, b);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 自动落子
        /// </summary>
        private void AutoPlayChess()
        {
            if (g == null || g.IsGameOver)
            {
                return;
            }

            var p = g.GetAutoPlayChessPoint();
            if (p != null)
            {
                PlayChess(mainPanel, p.X, p.Y);
            }
        }

        /// <summary>
        /// 在指定的棋盘坐标下子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private bool PlayChess(object sender, int x, int y)
        {
            var b = g.Handle == PieceColor.Black ? Brushes.Black : Brushes.White;
            var success = g.PlayChess(x, y);

            if (success)
            {
                //显示当前棋子下的位置
                if (g.Handle == PieceColor.Black)
                {
                    lblBlackLoc.Text = x + "," + y;
                }
                else
                {
                    lblWhiteLoc.Text = x + "," + y;
                }

                //记录下子过程
                WriteLog(string.Format("{0}棋：({1}, {2})", b == Brushes.Black ? "黑" : "白", x, y));

                //画出棋子
                DrawPiece(sender as Control, x * pSize + pBegin, y * pSize + pBegin, b);

                //画红点
                MoveRedPoint(x, y);
                RecoveryPoint();

                //计算黑白棋的分数
                lblBlackScore.Text = g.GetBoardValue(g.GetCheckerboard(), PieceColor.Black).ToString();
                lblWhiteScore.Text = g.GetBoardValue(g.GetCheckerboard(), PieceColor.White).ToString();

                //计算黑白棋分别使用的时间
                if (b == Brushes.Black)
                {
                    //如果刚才是黑棋下子
                    lblBlackSpan.Text = ((int)(g.BlackPlayTime - g.BlackStartTime).TotalMilliseconds).ToString() + "ms";
                }
                else
                {
                    //如果刚才是白棋下子
                    lblWhiteSpan.Text = ((int)(g.WhitePlayTime - g.WhiteStartTime).TotalMilliseconds).ToString() + "ms";
                }

                //检查是否有获胜方
                var p = g.CheckPiece(x, y);
                if (p != PieceColor.None && p != PieceColor.Null)
                {
                    var content = string.Format("{0}棋赢了！", p == PieceColor.Black ? "黑" : "白");

                    if (synth != null)
                    {
                        synth.Dispose();
                    }
                    synth = new SpeechSynthesizer();
                    synth.SpeakAsyncCancelAll();
                    synth.SpeakAsync(content);

                    MessageBox.Show(content);

                    (sender as Control).MouseClick -= Panel_MouseClick;
                    g.IsGameOver = true;

                    return false;
                }

                return true;
            }
            else
            {
                MessageBox.Show("当前位置已经有棋子！");
            }

            return false;
        }

        /// <summary>
        /// 点击棋盘落子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            //实际坐标转换为棋盘坐标
            var x = (e.X - pBegin + pSize / 2 - 1) / pSize;
            var y = (e.Y - pBegin + pSize / 2 - 1) / pSize;

            if (PlayChess(sender, x, y))
            {
                AutoPlayChess();
            }
        }




        /// <summary>
        /// 在指定位置画红点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveRedPoint(int x, int y)
        {
            //当前点
            var px = x * pSize + pBegin;
            var py = y * pSize + pBegin;
            DrawPointColor(mainPanel, px, py, Brushes.Red);
        }

        /// <summary>
        /// 将前一个点的颜色补充完整
        /// </summary>
        private void RecoveryPoint()
        {
            if (g.Index - 2 >= 0)
            {
                var up = g.PlayChessLog[g.Index - 2];
                var b = up.Color == PieceColor.White ? Brushes.White : Brushes.Black;
                var px2 = up.X * pSize + pBegin;
                var py2 = up.Y * pSize + pBegin;

                DrawPointColor(mainPanel, px2, py2, b);
            }
        }





        /// <summary>
        /// 绘制棋盘
        /// </summary>
        /// <param name="c"></param>
        /// <param name="begin">在c容器中开始绘制的位置，单位像素</param>
        private void DrawGobang(Control c, int begin)
        {
            if (c == null)
                return;

            DrawBorder(c, g.Size, begin);
            var p = (g.Size - 1) / 2;
            var p1 = p / 2 * pSize + begin;
            var p2 = (g.Size - p / 2) * pSize + begin;
            var p3 = p * pSize + begin;
            DrawPoint(c, p3, p3);
            DrawPoint(c, p1, p1);
            DrawPoint(c, p1, p2);
            DrawPoint(c, p2, p1);
            DrawPoint(c, p2, p2);
        }

        /// <summary>
        /// 画框
        /// </summary>
        /// <param name="c"></param>
        /// <param name="size">大小</param>
        /// <param name="begin">在c容器中开始绘制的位置，单位像素</param>
        private void DrawBorder(Control c, int size, int begin)
        {
            Graphics gh = c.CreateGraphics();
            for (var i = 0; i < size; ++i)
            {
                var start = i * pSize + begin;
                var end = (size - 1) * pSize + begin;
                gh.DrawLine(Pens.Black, new Point(begin, start), new Point(end, start));
                gh.DrawLine(Pens.Black, new Point(start, begin), new Point(start, end));
                if (i == (size - 1) / 2)
                {
                    for (var k = -1; k < 2; ++k)
                    {
                        start = i * pSize + k + begin;
                        end = (size - 1) * pSize + k + begin;
                        gh.DrawLine(Pens.Black, new Point(begin, start), new Point(end, start));
                        gh.DrawLine(Pens.Black, new Point(start, begin), new Point(start, end));
                    }
                }
            }
            gh.Dispose();
        }

        /// <summary>
        /// 画点
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x">真实坐标x</param>
        /// <param name="y">真实坐标y</param>
        private void DrawPoint(Control c, int x, int y)
        {
            DrawPointColor(c, x, y, Brushes.Black);
        }

        /// <summary>
        /// 画点
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        private void DrawPointColor(Control c, int x, int y, Brush b)
        {
            Graphics gh = c.CreateGraphics();
            gh.FillRectangle(b, x - 5, y - 5, 10, 10);
            gh.Dispose();
        }

        /// <summary>
        /// 画圆
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x">真实坐标x</param>
        /// <param name="y">真实坐标y</param>
        /// <param name="b"></param>
        private void DrawPiece(Control c, int x, int y, Brush b)
        {
            Graphics gh = c.CreateGraphics();
            gh.FillEllipse(b, x - 20, y - 20, 40, 40);
            gh.Dispose();
        }



        private void WriteLog(string msg)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine(msg);
            }
        }

        private void btnAB_Click(object sender, EventArgs e)
        {
            g.Count = 0;
            g.Waiting = new List<Point>();
            g.Max = -99999999;
            var a = g.AlphaBeta(g.GetCheckerboard(), 4, -99999999, 99999999, PieceColor.White, PieceColor.Black);
        }
        
    }
}
