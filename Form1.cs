using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace connect
{
    public partial class Form1 : Form
    {
        public class Global
        {
            public static bool isFilled = false;  //false='○'棋  true ='●'棋
            public static string[,] x = new string[6, 7];  //棋盤
            public static int[] isConnect = new int[69];  //是否連線
            public static int[,] Connect_loc = new int[69, 8];  //69條連線的XY座標
            public static char[] text = new char[] { '○', '●' };
            public static string[] str_ordinal = new string[] { "一", "二", "三", "四", "五", "六", "七" };
            public static Random rnd = new Random();
            public static int button_click_time = 0;  //棋步數:為了避免滿棋盤
        }
        public void set_Connect_loc()//建立所有連線的4點座標
        {
            for (int i = 0; i < 21; i++)//直  左到右 下到上
            {
                for (int j = 0; j < 4; j++)
                {
                    Global.Connect_loc[i, 2 * j] = 5 - (j + i / 7);
                    Global.Connect_loc[i, 2 * j + 1] = i % 7;
                }
            }
            for (int i = 21; i < 45; i++)//橫  左到右 下到上
            {
                for (int j = 0; j < 4; j++)
                {
                    Global.Connect_loc[i, 2 * j] = 5 - ((i - 21) / 4);
                    Global.Connect_loc[i, 2 * j + 1] = (i - 21) % 4 + j;
                }
            }
            for (int i = 45; i < 57; i++)//斜右上  左到右 下到上
            {
                for (int j = 0; j < 4; j++)
                {
                    Global.Connect_loc[i, 2 * j] = 5 - ((i - 45) / 4 + j);
                    Global.Connect_loc[i, 2 * j + 1] = (i - 45) % 4 + j;
                }
            }
            for (int i = 57; i < 69; i++)//斜左上  左到右 下到上
            {
                for (int j = 0; j < 4; j++)
                {
                    Global.Connect_loc[i, 2 * j] = 5 - ((i - 57) / 4 + j);
                    Global.Connect_loc[i, 2 * j + 1] = (i - 57) % 4 + (3 - j);
                }
            }
        }
        public void check_Connect()//檢查所有連線
        {
            for (int i = 0; i < 69; i++)
            {
                if (Global.isConnect[i] == 0)
                {
                    string _1 = Global.x[Global.Connect_loc[i, 0], Global.Connect_loc[i, 1]];
                    string _2 = Global.x[Global.Connect_loc[i, 2], Global.Connect_loc[i, 3]];
                    string _3 = Global.x[Global.Connect_loc[i, 4], Global.Connect_loc[i, 5]];
                    string _4 = Global.x[Global.Connect_loc[i, 6], Global.Connect_loc[i, 7]];
                    if ((_1 + _2 + _3 + _4).Contains("○") && (_1 + _2 + _3 + _4).Contains("●"))//某條連線包含2種棋=此連線不會達成->不需要再檢查
                    {
                        Global.isConnect[i] = -1;
                    }
                    else if ((_1 + _2 + _3 + _4) == "○○○○" || (_1 + _2 + _3 + _4) == "●●●●")//某條連線某棋贏了
                    {
                        Global.isConnect[i] = 1;
                        End_to_Win();
                    }
                }
            }
        }
        public void reset_isConnect()//把所有連線的檢查回到原始狀態
        {
            for (int i = 0; i < 69; i++)
            {
                Global.isConnect[i] = 0;
            }
        }
        public void End_to_Win()//XX贏了
        {
            MessageBox.Show(Global.text[Convert.ToInt32(!Global.isFilled)] + " is Winner");
            resetBoard();
        }
        public void resetBoard()//棋盤復原+一些些東西復原
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Global.x[i, j] = "□";
                }
            }
            for (int i = 0; i < 6; i++)
            {
                printBoard(i);
            }
            reset_isConnect();
            Global.isFilled = false;
            Global.button_click_time = 0;
            label8.Text = "   選擇 第...行";
            label7.Text = "輪到:○";

        }
        public void printBoard(int r) //print每一行的Text
        {
            string temp_str = Global.x[r, 0] + Global.x[r, 1] + Global.x[r, 2] + Global.x[r, 3] + Global.x[r, 4] + Global.x[r, 5] + Global.x[r, 6];
            switch (r)
            {
                case 0:
                    label1.Text = temp_str;
                    break;
                case 1:
                    label2.Text = temp_str;
                    break;
                case 2:
                    label3.Text = temp_str;
                    break;
                case 3:
                    label4.Text = temp_str;
                    break;
                case 4:
                    label5.Text = temp_str;
                    break;
                case 5:
                    label6.Text = temp_str;
                    break;
            }
        }
        public int _ok(int r, int c) //判斷該直行是否可以繼續放
        {
            if (Global.x[r, c - 1] == "□")
                return r;  //輸出高度
            else if (r == 0) //超出棋格
                return 100;
            else
                return _ok(r - 1, c);  //該格被占 遞迴尋找
        }
        public void insert(int c)
        {
            int temp_R = _ok(5, c);
            Global.x[temp_R, c - 1] = Convert.ToString(Global.text[Convert.ToInt32(Global.isFilled)]);  //某格的棋子顏色
            Global.isFilled = !Global.isFilled;  //換顏色棋子
            printBoard(temp_R);  //某橫行變動Print某行 省電
            if (Global.button_click_time >= 8)  //至少8步(各4顆)再檢查連線 省時間 省電
            {
                check_Connect();
            }
        }
        public void makeAmove()
        {
            int c;  //隨機放棋
            do
            {
                c = Global.rnd.Next(1, 8);
            } while (Global.x[0, c - 1] != "□");
            label8.Text = Global.text[Convert.ToInt32(Global.isFilled)] + "選擇 第" + Global.str_ordinal[c - 1] + "行";
            label7.Text = "輪到:" + Global.text[Convert.ToInt32((!Global.isFilled))];
            insert(c);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            //label8.Text = 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Global.button_click_time < 42)
            {
                if (Global.button_click_time == 0) //初始化
                {
                    resetBoard();
                    set_Connect_loc();
                }
                Global.button_click_time += 1;
                makeAmove();
                if (Global.button_click_time == 42) //42滿棋
                {
                    label8.Text = "         滿了";
                    label7.Text = "滿了";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //reset
        {
            resetBoard();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("輪 流 放 棋，\n棋 子 會 落 至 最 底，\n4 棋 連 線 者 贏", "Help", MessageBoxButtons.OK,MessageBoxIcon.Question);
        }
    }
}
