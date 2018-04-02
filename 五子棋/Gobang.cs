using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 五子棋
{
    public class PointScore
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Score { get; set; }
    }

    public enum PieceColor
    {
        /// <summary>
        /// 超出边界
        /// </summary>
        Null = -1,
        /// <summary>
        /// 空白
        /// </summary>
        None = 0,
        /// <summary>
        /// 黑子
        /// </summary>
        Black = 1,
        /// <summary>
        /// 白子
        /// </summary>
        White = 2
    }

    public class Piece: ICloneable
    {
        public int X { get; set; }

        public int Y { get; set; }

        public PieceColor Color { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class Gobang
    {
        #region 字段

        /// <summary>
        /// 棋子评分扫描起始坐标
        /// </summary>
        private Point scanBegin;
        /// <summary>
        /// 棋子评分扫描结束坐标
        /// </summary>
        private Point scanEnd;
        /// <summary>
        /// 评分正则集合
        /// </summary>
        private Dictionary<string, int> dicReges;
        /// <summary>
        /// 棋子判断输赢方向矢量
        /// </summary>
        private int[][] direction;
        /// <summary>
        /// 棋子评分方向矢量
        /// </summary>
        private int[][] valDirection;

        #endregion 字段



        #region 事件和委托

        public delegate void ValueChange(object sender, EventArgs e);
        public ValueChange HandleChange { get; set; }

        #endregion 事件和委托



        #region 属性

        /// <summary>
        /// 当前即将下的棋子的顺序，从0开始
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 棋盘
        /// </summary>
        private Piece[][] Checkerboard { get; set; }

        /// <summary>
        /// 下棋记录
        /// </summary>
        public Piece[] PlayChessLog { get; set; }

        /// <summary>
        /// 棋盘大小
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 当前执手
        /// </summary>
        private PieceColor handle;
        /// <summary>
        /// 当前执手
        /// </summary>
        public PieceColor Handle
        {
            get
            {
                return handle;
            }
            private set
            {
                handle = value;
                HandleChange?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// 是否游戏结束
        /// </summary>
        public bool IsGameOver { get; set; }

        /// <summary>
        /// 黑棋开始下棋时间
        /// </summary>
        public DateTime BlackStartTime { get; set; }

        /// <summary>
        /// 白棋开始下棋时间
        /// </summary>
        public DateTime WhiteStartTime { get; set; }

        /// <summary>
        /// 黑棋真正下棋时间
        /// </summary>
        public DateTime BlackPlayTime { get; set; }

        /// <summary>
        /// 白棋真正下棋时间
        /// </summary>
        public DateTime WhitePlayTime { get; set; }

        #endregion 属性



        #region 初始化

        /// <summary>
        /// 默认构造
        /// </summary>
        /// <param name="size"></param>
        public Gobang(int size)
        {
            if (size < 5 || size % 2 == 0)
            {
                throw new Exception("棋盘大小错误！");
            }

            InitCheckerboard(size);
        }

        /// <summary>
        /// 获取指定位置棋子的副本
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Piece this[int x, int y]
        {
            get
            {
                return Checkerboard[x][y].Clone() as Piece;
            }
        }

        /// <summary>
        /// 获取棋盘的副本
        /// </summary>
        /// <returns></returns>
        public Piece[][] GetCheckerboard()
        {
            return Checkerboard.Clone() as Piece[][];
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitCheckerboard(int size)
        {
            Size = size;
            Handle = PieceColor.Black;
            Checkerboard = new Piece[size][];
            scanBegin = new Point(size / 2, size / 2);
            scanEnd = new Point(size / 2, size / 2);
            IsGameOver = false;
            BlackStartTime = DateTime.Now;
            PlayChessLog = new Piece[Size * Size];
            Index = 0;

            for (var i = 0; i < Checkerboard.Length; ++i)
            {
                Checkerboard[i] = new Piece[size];
                for (var j = 0; j < Checkerboard[i].Length; ++j)
                {
                    Checkerboard[i][j] = new Piece() { X = i, Y = j, Color = PieceColor.None };
                }
            }

            direction = new int[][]
            {
                new int[2] { 0, -1 },
                new int[2] { 0, 1 },
                new int[2] { 1, -1 },
                new int[2] { 1, 0 },
                new int[2] { 1, 1 },
                new int[2] { -1, -1 },
                new int[2] { -1, 0 },
                new int[2] { -1, 1 }
            };

            valDirection = new int[][]
            {
                new int[2] { 0, 1 },
                new int[2] { 1, 1 },
                new int[2] { 1, 0 },
                new int[2] { 1, -1 }
            };


            dicReges = new Dictionary<string, int>
            {
                { "SSSSS", 100000 },
                { "NSSSSN", 40000 },
                { "NSNSSSN", 10000 },
                { "NSSNSSN", 10000 },
                { "NSSSSM", 5000 },
                { "NSSSN", 5000 },
                { "NSNSSN", 5000 },
                { "NNSSSM", 1000 },
                { "NSNSSM", 1000 },
                { "NSSNSM", 1000 },
                { "SNNSS", 500 },
                { "SNSNS", 500 },
                { "MNSSSNM", 500 },
                { "NNSSN", 200 },
                { "NSNSN", 150 },
                { "NSNNSN", 150 },
                { "NNNSSM", 100 },
                { "NNSNSM", 100 },
                { "NSNNSM", 100 },
                { "SNNNS", 100 },
                { "MNSNSNM", 100 },
                { "MNSSNNM", 100 },
                { "SM", 5 },
                { "MS", 5 },
                { "N", 0 },
                { "S", 0 },
                { "M", -10 },
                { "MSSSSM", -10 },
                { "MSSSM", -10 },
                { "MSSM", -10 },
                { "MSM", -10 }
            };
        }

        #endregion 初始化




        #region 获取棋子颜色

        /// <summary>
        /// 获取指定坐标的棋子的颜色
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PieceColor GetColor(int x, int y)
        {
            return GetColor(Checkerboard, x, y);
        }

        /// <summary>
        /// 获取指定坐标的棋子的颜色
        /// </summary>
        /// <param name="board"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PieceColor GetColor(Piece[][] board, int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
            {
                //超出边界
                return PieceColor.Null;
            }

            return Checkerboard[x][y].Color;
        }

        #endregion 获取棋子颜色




        #region 下棋相关

        /// <summary>
        /// 落子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool PlayChess(int x, int y)
        {
            //超出边界或者当前有棋子就不能下子
            if (GetColor(x, y) != PieceColor.None)
                return false;

            PlayChess(x, y, Handle);

            return true;
        }

        /// <summary>
        /// 落子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        private void PlayChess(int x, int y, PieceColor color)
        {
            if (Handle != color)
            {
                throw new Exception("当前下的子颜色不正确！");
            }

            var piece = new Piece() { X = x, Y = y, Color = color };
            Checkerboard[x][y] = piece;
            PlayChessLog[Index] = piece;
            ++Index;

            if (color == PieceColor.Black)
            {
                //当前是黑棋下子
                BlackPlayTime = DateTime.Now;
                WhiteStartTime = DateTime.Now;
            }
            else
            {
                //当前是白棋下子
                WhitePlayTime = DateTime.Now;
                BlackStartTime = DateTime.Now;
            }

            //换手
            Handle = Handle == PieceColor.Black ? PieceColor.White : PieceColor.Black;

            ChangeScan(x, y);
        }

        /// <summary>
        /// 改变扫描位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ChangeScan(int x, int y)
        {
            var sBegin = new Point(x - 4, y - 4);
            var sEnd = new Point(x + 4, y + 4);

            if (sBegin.X < scanBegin.X)
            {
                scanBegin.X = sBegin.X <= 0 ? 0 : sBegin.X;
            }
            if (sBegin.Y < scanBegin.Y)
            {
                scanBegin.Y = sBegin.Y <= 0 ? 0 : sBegin.Y;
            }

            if (sEnd.X > scanEnd.X)
            {
                scanEnd.X = sEnd.X >= (Size - 1) ? (Size - 1) : sEnd.X;
            }
            if (sEnd.Y > scanEnd.Y)
            {
                scanEnd.Y = sEnd.Y >= (Size - 1) ? (Size - 1) : sEnd.Y;
            }
        }

        /// <summary>
        /// 悔棋
        /// </summary>
        /// <returns></returns>
        public bool UndoPlayChess()
        {
            if (Index - 1 < 0)
            {
                return false;
            }

            var last = PlayChessLog[Index - 1];
            Checkerboard[last.X][last.Y].Color = PieceColor.None;
            PlayChessLog[Index - 1] = null;
            --Index;

            //换手
            Handle = Handle == PieceColor.Black ? PieceColor.White : PieceColor.Black;

            return true;
        }

        #endregion 下棋相关




        #region 检查是否有获胜方

        /// <summary>
        /// 检查是否有获胜方
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PieceColor CheckPiece(int x, int y)
        {
            for (var i = 0; i < direction.Length; ++i)
            {
                var result = CheckPieceByDirection(x, y, direction[i][0], direction[i][1], 5);
                if (result != PieceColor.None)
                {
                    return result;
                }
            }

            return PieceColor.None;
        }

        /// <summary>
        /// 检查是否有获胜方
        /// </summary>
        /// <param name="x">当前棋子位置ｘ坐标</param>
        /// <param name="y">当前棋子位置ｘ坐标</param>
        /// <param name="i">判断方向ｘ坐标</param>
        /// <param name="j">判断方向y坐标</param>
        /// <param name="max">获胜最大数</param>
        /// <returns></returns>
        private PieceColor CheckPieceByDirection(int x, int y, int i, int j, int max)
        {
            var count = 1;
            var c1 = CountPiece(x, y, i, j, max);//第一个方向的相同棋子数
            var c2 = CountPiece(x, y, -i, -j, max);//反方向的相同棋子数

            if (c1 + c2 + count == max - 1)
            {
                //获取正方向的下一个子
                var color1 = GetColor(x + (c1 + 1) * i, y + (c1 + 1) * j);
                //获取反方向的下一个子
                var color2 = GetColor(x + (c2 + 1) * -i, y + (c2 + 1) * -j);
                if (color1 == color2 && color2 == PieceColor.None)
                {
                    return GetColor(x, y);
                }
            }

            if (c1 + c2 + count >= max)
            {
                return GetColor(x, y);
            }

            return PieceColor.None;
        }

        /// <summary>
        /// 计算当前方向有多少个棋子和当前的棋子颜色一样，不包含当前位置
        /// </summary>
        /// <param name="x">当前位置棋子的X坐标</param>
        /// <param name="y">当前位置棋子的Y坐标</param>
        /// <param name="i">判断方向x坐标</param>
        /// <param name="j">判断方向y坐标</param>
        /// <param name="max">判断方向移动最大数，0为不限制</param>
        /// <returns></returns>
        private int CountPiece(int x, int y, int i, int j, int max)
        {
            PieceColor currColor = GetColor(x, y);//当前下的子

            if (currColor == PieceColor.None || currColor == PieceColor.Null)
            {
                return 0;
            }
            else
            {
                int count = -1;
                PieceColor nextColor;
                do
                {
                    ++count;
                    if (count >= max)
                        break;

                    x += i;
                    y += j;

                    nextColor = GetColor(x, y);

                } while (currColor == nextColor);

                return count;
            }
        }

        #endregion 检查是否有获胜方




        #region 获取值


        /// <summary>
        /// 获取当前位置的值
        /// </summary>
        /// <param name="color">即将下的棋子的颜色</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int GetChessValue(Piece[][] board, PieceColor color, int x, int y)
        {
            int result = 0;
            if (GetColor(x, y) != PieceColor.None)
            {
                //这个位子上有棋子，不用判断值
                return 0;
            }

            for (var i = 0; i < valDirection.Length; ++i)
            {
                var k = valDirection[i][0];
                var l = valDirection[i][1];

                //棋型判断，最大前进5-1个格子，因为超过之后不会造成影响
                var pdStr = GetChessString(board, color, x, y, k, l);
                var ndStr = GetChessString(board, color, x, y, -k, -l);
                if (pdStr.Replace("N", "") == "" && ndStr.Replace("N", "") == "")
                {
                    continue;
                }

                if (ndStr != "")
                {
                    ndStr = string.Concat(ndStr.Reverse());
                }
                var sumStr = ndStr + "S" + pdStr;
                //211?1102——MSS?SSNM
                result += GetValue(sumStr);
            }

            //相同棋型时，约靠近中间分数约高
            //result += (7 - Math.Abs(x - 7)) + (7 - Math.Abs(y - 7));
            //result += x + y;

            return result;
        }

        /// <summary>
        /// 根据棋型获取值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int GetValue(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            int result = 0;

            foreach (var r in dicReges)
            {
                Regex reg = new Regex(r.Key);
                if (reg.IsMatch(str))
                {
                    result = r.Value;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取棋局字符串，不包括当前位置，S表示自己，M表示对手，N表示空
        /// </summary>
        /// <param name="board"></param>
        /// <param name="color">表示自己的颜色</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private string GetChessString(Piece[][] board, PieceColor color, int x, int y, int i, int j)
        {
            var str = new string[4];
            for (var k = 1; k < 5; ++k)
            {
                var curColor = GetColor(board, x + k * i, y + k * j);
                if (curColor == PieceColor.Null)
                {
                    break;
                }

                str[k - 1] = curColor == color ? "S" : curColor == PieceColor.None ? "N" : "M";

                if (str[k - 1] == "M")
                {
                    break;
                }
            }

            return string.Concat(str);
        }


        /// <summary>
        /// 根据棋型获总值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int GetValueAll(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Replace("N", "") == "")
            {
                return 0;
            }
            int result = 0;

            foreach (var r in dicReges)
            {
                Regex reg = new Regex(r.Key);
                var ms = reg.Matches(str);
                //if (reg.IsMatch(str))
                //{
                //    result += r.Value;
                //}
                result += r.Value * ms.Count;
            }

            return result;
        }

        /// <summary>
        /// 获取棋局分数
        /// </summary>
        /// <param name="board"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public int GetBoardValue(Piece[][] board, PieceColor color)
        {
            int result = 0;

            //横向和纵向
            for (var x = 0; x < 15; ++x)
            {
                var hs = new string[15];
                var vs = new string[15];
                var n = 0;
                for (var y = 0; y < 15; ++y)
                {
                    //横向
                    hs[n] = board[x][y].Color == color ? "S" : board[x][y].Color == PieceColor.None ? "N" : "M";

                    //纵向
                    vs[n] = board[y][x].Color == color ? "S" : board[y][x].Color == PieceColor.None ? "N" : "M";

                    ++n;
                }

                result += GetValueAll(string.Concat(hs)) + GetValueAll(string.Concat(vs));
            }

            //斜方向
            for (var i = 0; i <= 10; ++i)
            {
                var leftDown = new string[15 - i];
                var rightUp = new string[15 - i];
                var rightDown = new string[15 - i];
                var leftUp = new string[15 - i];
                var n = 0;
                for (var j = 0; j + i <= 14; ++j)
                {
                    //左下  \
                    leftDown[n] = board[j][j + i].Color == color ? "S" : board[j][j + i].Color == PieceColor.None ? "N" : "M";

                    //右上  \
                    rightUp[n] = board[j + i][j].Color == color ? "S" : board[j + i][j].Color == PieceColor.None ? "N" : "M";

                    //右下  /
                    rightDown[n] = board[14 - j][j + i].Color == color ? "S" : board[14 - j][j + i].Color == PieceColor.None ? "N" : "M";

                    //左上  /
                    leftUp[n] = board[14 - (j + i)][j].Color == color ? "S" : board[14 - (j + i)][j].Color == PieceColor.None ? "N" : "M";

                    ++n;
                }

                result += GetValueAll(string.Concat(leftDown)) + GetValueAll(string.Concat(rightDown));
                result += GetValueAll(string.Concat(leftUp)) + GetValueAll(string.Concat(rightUp));
            }

            return result;
        }



        public int GetBoardValueNew(Piece[][] board, PieceColor color)
        {
            int result = 0;
            var match = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            for (var k = 0; k < 15; ++k)
            {
                //1,0 增量
                result += GetDirectionValue(board, color, match, 0, k, 1, 0, 14);

                //0,1 增量
                result += GetDirectionValue(board, color, match, k, 0, 0, 1, 14);

                if (k <= 10)
                {
                    //1,1 增量 k==0时会重复一次
                    result += GetDirectionValue(board, color, match, 0, k, 1, 1, 14 - k);
                    result += GetDirectionValue(board, color, match, k, 0, 1, 1, 14 - k);

                    //1,-1 增量 k==0时会重复一次
                    result += GetDirectionValue(board, color, match, 0, 14 - k, 1, -1, 14 - k);
                    result += GetDirectionValue(board, color, match, k, 14, 1, -1, 14 - k);
                }
            }

            return result;
        }

        private int GetDirectionValue(Piece[][] board, PieceColor color, PieceColor m, int x, int y, int i, int j, int max)
        {
            var self = 0;
            var count = 0;
            var score = 0;
            for (var k = 0; k < max; ++k)
            {
                if (board[x][y].Color == m)
                {


                    continue;
                }
                if (board[x][y].Color == color)
                {
                    ++self;
                    score += 100;
                }
                ++count;

                //next
                x = x + i;
                y = y + j;
            }

            return score;
        }



        private int GetChessValueByDir(Piece[][] board, PieceColor color, PieceColor match, int x, int y, int i, int j)
        {
            int result = 0;
            int max = 0;
            int cur = 0;
            int count = 0;
            int other = 0;

            int bx = x, by = y, k = 0;
            for (; k < 5; ++k)
            {
                bx = x - k * i;
                by = y - k * j;

                if (bx <= 0 || bx >= 14 || by <= 0 || by >= 14)
                    break;
            }

            int nx = bx, ny = by;
            int x1 = bx, y1 = by;
            for (var h = 0; h <= k + 4; ++h)
            {
                nx = bx + h * i;
                ny = by + h * j;

                if (nx < 0 || nx > 14 || ny < 0 || ny > 14)
                    break;

                if (board[nx][ny].Color == color)
                {
                    ++cur;
                }
                else
                {
                    if (board[nx][ny].Color == match)
                    {
                        ++other;
                    }
                    cur = 0;
                }

                if (cur > max)
                {
                    max = cur;
                    x1 = nx;
                    y1 = ny;
                }

                ++count;
            }

            if (max == 5)
            {
                //SSSSS
                return 100000;
            }

            if (max == 4)
            {
                //NSSSSN
                if (board[x1 - i * max][y1 - j * max].Color == PieceColor.None 
                    && GetColor(board, x1 + i, y1 + j) == PieceColor.None)
                {
                    return 40000;
                }
                //MSSSSM
                if (board[x1 - i * max][y1 - j * max].Color == match
                    && GetColor(board, x1 + i, y1 + j) == match)
                {
                    return -100;
                }

                //NSSSSM   MSSSSN
                return 5000;
            }

            if (max == 3)
            {

            }
            

            return result;
        }

        #endregion 获取值





        #region 自动下棋

        /// <summary>
        /// 获取分数1
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private PointScore Plugging(PieceColor self)
        {
            int max = -10000;
            int px = -1, py = -1;

            for (var x = scanBegin.X; x <= scanEnd.X; ++x)
            {
                for (var y = scanBegin.Y; y <= scanEnd.Y; ++y)
                {
                    var score = GetChessValue(Checkerboard, self, x, y);
                    if (score > max)
                    {
                        max = score;
                        px = x;
                        py = y;
                    }
                }
            }

            return new PointScore() { X = px, Y = py, Score = max };
        }

        /// <summary>
        /// 自动下棋1
        /// </summary>
        public PointScore GetAutoPlayChessPoint()
        {
            if (IsGameOver)
            {
                return null;
            }

            if (Index == 0)
            {
                return new PointScore() { X = 7, Y = 7 };
            }

            var slef = Handle;
            var match = slef == PieceColor.White ? PieceColor.Black : PieceColor.White;

            var p = Plugging(match);//PieceColor.Black
            var p2 = Plugging(slef);//PieceColor.White
            if (p2.Score > p.Score)
            {
                //PlayChess(mainPanel, p2.X, p2.Y);
                return p2;
            }
            else
            {
                //PlayChess(mainPanel, p.X, p.Y);
                return p;
            }
        }




        public int Count { get; set; }
        public List<Point> Waiting { get; set; }
        public int Max { get; set; }

        public int AlphaBeta(Piece[][] board, int deept, int alpha, int beta, PieceColor self, PieceColor match)
        {
            if (deept == 0)
            {
                ++Count;
                //return GetBoardValueNew(board, self);
                //return GetBoardValueNew(board, self) - GetBoardValueNew(board, match);
                return (new Random()).Next(-100, 10000);
            }

            if (deept % 2 == 0)
            {
                //MAX
                for (var x = scanBegin.X; x < scanEnd.X; ++x)
                {
                    for (var y = scanBegin.Y; y < scanEnd.Y; ++y)
                    {
                        if (board[x][y].Color != PieceColor.None)
                            continue;

                        //尝试下子
                        board[x][y].Color = self;

                        var val = AlphaBeta(board, deept - 1, alpha, beta, self, match);
                        //取消下子
                        board[x][y].Color = PieceColor.None;

                        if (deept == 4 && val >= Max)
                        {
                            if (val > Max)
                            {
                                Max = val;
                                Waiting.Clear();
                            }
                            Waiting.Add(new Point(x, y));
                        }

                        if (val > alpha)
                        {
                            alpha = val;
                        }

                        if (alpha >= beta)
                        {
                            return alpha;
                        }

                    }
                }

                return alpha;
            }
            else
            {
                //MIN
                for (var x = scanBegin.X; x < scanEnd.X; ++x)
                {
                    for (var y = scanBegin.Y; y < scanEnd.Y; ++y)
                    {
                        if (board[x][y].Color != PieceColor.None)
                            continue;

                        //尝试下子
                        board[x][y].Color = match;

                        var val = AlphaBeta(board, deept - 1, alpha, beta, self, match);
                        //取消下子
                        board[x][y].Color = PieceColor.None;

                        if (val < beta)
                        {
                            beta = val;
                        }

                        if (alpha >= beta)
                        {
                            return beta;
                        }

                    }
                }

                return beta;

            }


        }

        #endregion 自动下棋

    }
}
