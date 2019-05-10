using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace 俄罗斯方块
{
    class Russia
    {
        public Point firstPoi = new Point(140, 20);
        public static Color[,] PlaceColor;
        public static bool[,] Place;
        public static int conWidth = 0;//总行数
        public static int conHeight = 0;//总高度
        public static int maxY = 0;
        public static int conMax = 0;
        public static int conMin = 0;
        bool[] tem_Array = { false, false, false, false };
        Color ConColor = Color.Coral;
        Point[] ArryPoi = new Point[4];
        Point[] Arryfront = new Point[4];
        int Cake = 20;//方块大小
        int Convertor = 0;//变换样式
        Control Mycontrol = new Control();
        public Label Label_Linage = new Label();
        public Label Label_Fraction = new Label();
        public static int[] ArrayCent = new int[] { 2, 5, 9, 15 };
        public Timer timer = new Timer();

        public void CakeMode(int n)
        {
            ArryPoi[0] = firstPoi;
            switch (n)
            {
                case 1://组合“L”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X, firstPoi.Y - Cake);//设置第二块方块的位置
                        ArryPoi[2] = new Point(firstPoi.X, firstPoi.Y + Cake);//设置第三块方块的位置
                        ArryPoi[3] = new Point(firstPoi.X + Cake, firstPoi.Y + Cake);//设置第四块方块的位置
                        ConColor = Color.Fuchsia;//设置当前方块的颜色
                        Convertor = 2;//记录方块的变换样式
                        break;
                    }
                case 2://组合“Z”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X, firstPoi.Y - Cake);
                        ArryPoi[2] = new Point(firstPoi.X - Cake, firstPoi.Y - Cake);
                        ArryPoi[3] = new Point(firstPoi.X + Cake, firstPoi.Y);
                        ConColor = Color.Yellow;
                        Convertor = 6;
                        break;
                    }
                case 3://组合倒“L”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X, firstPoi.Y - Cake);
                        ArryPoi[2] = new Point(firstPoi.X, firstPoi.Y + Cake);
                        ArryPoi[3] = new Point(firstPoi.X - Cake, firstPoi.Y + Cake);
                        ConColor = Color.CornflowerBlue;
                        Convertor = 8;
                        break;
                    }
                case 4://组合倒“Z”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X, firstPoi.Y - Cake);
                        ArryPoi[2] = new Point(firstPoi.X + Cake, firstPoi.Y - Cake);
                        ArryPoi[3] = new Point(firstPoi.X - Cake, firstPoi.Y);
                        ConColor = Color.Blue;
                        Convertor = 12;
                        break;
                    }
                case 5://组合“T”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X, firstPoi.Y - Cake);
                        ArryPoi[2] = new Point(firstPoi.X + Cake, firstPoi.Y - Cake);
                        ArryPoi[3] = new Point(firstPoi.X - Cake, firstPoi.Y - Cake);
                        ConColor = Color.Silver;
                        Convertor = 14;
                        break;
                    }
                case 6://组合“一”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X + Cake, firstPoi.Y);
                        ArryPoi[2] = new Point(firstPoi.X - Cake, firstPoi.Y);
                        ArryPoi[3] = new Point(firstPoi.X - Cake * 2, firstPoi.Y);
                        ConColor = Color.Red;
                        Convertor = 18;
                        break;
                    }
                case 7://组合“田”方块
                    {
                        ArryPoi[1] = new Point(firstPoi.X - Cake, firstPoi.Y);
                        ArryPoi[2] = new Point(firstPoi.X - Cake, firstPoi.Y - Cake);
                        ArryPoi[3] = new Point(firstPoi.X, firstPoi.Y - Cake);
                        ConColor = Color.LightGreen;
                        Convertor = 19;
                        break;
                    }
            }
        }

        public void ConvertorClear()
        {
            if (Mycontrol != null)
            {
                Graphics g = Mycontrol.CreateGraphics();
                Rectangle rect = new Rectangle(0, 0, Mycontrol.Width, Mycontrol.Height);
                MyPaint(g, new SolidBrush(Color.Black), rect);
            }
        }


        public void ConvertorDelete()
        {
            Graphics g = Mycontrol.CreateGraphics();
            for(int i = 0; i < ArryPoi.Length; i++)
            {
                Rectangle rect = new Rectangle(ArryPoi[i].X, ArryPoi[i].Y, 20, 20);
                MyPaint(g, new SolidBrush(Color.Black), rect);
            }
        }


        public void MyConvertorMode()
        {
            ConvertorDelete();
            ConvertorMode(Convertor);
            Protract(Mycontrol);
        }

        public void MyPaint(Graphics g,SolidBrush SolidB,Rectangle rect)
        {
            g.FillRectangle(SolidB, rect);
        }

        /// <summary>
        /// 方块变换
        /// </summary>
        /// <param name="n"></param>
        public void ConvertorMode(int n)
        {
            Point[] tem_ArrayPoi = new Point[4];
            Point tem_Poi = firstPoi;
            int tem_n = n;
            for (int i = 0; i < tem_ArrayPoi.Length; i++)
                tem_ArrayPoi[i] = ArryPoi[i];
            switch (n)
            {
                case 1://设置“L”方块的起始样式
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X + Cake, tem_Poi.Y + Cake);
                        tem_n = 2;//记录变换样式的标志
                        break;
                    }
                case 2://“L”方块向旋转的样式
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X + Cake, tem_Poi.Y - Cake);
                        tem_n = 3;
                        break;
                    }
                case 3://“L”方块向旋转的样式
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_n = 4;
                        break;
                    }
                case 4://“L”方块向旋转的样式
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X - Cake, tem_Poi.Y + Cake);
                        tem_n = 1;//返回方块的起始样式
                        break;
                    }
                case 5://Z
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_n = 6;
                        break;
                    }
                case 6:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_n = 5;
                        break;
                    }
                case 7://倒L
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X - Cake, tem_Poi.Y + Cake);
                        tem_n = 8;
                        break;
                    }
                case 8:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X + Cake, tem_Poi.Y + Cake);
                        tem_n = 9;
                        break;
                    }
                case 9:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X + Cake, tem_Poi.Y - Cake);
                        tem_n = 10;
                        break;
                    }
                case 10:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X - Cake, tem_Poi.Y - Cake);
                        tem_n = 7;
                        break;
                    }
                case 11://倒Z
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_n = 12;
                        break;
                    }
                case 12:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_n = 11;
                        break;
                    }
                case 13://T
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X - Cake, tem_Poi.Y - Cake);
                        tem_n = 14;
                        break;
                    }
                case 14:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_n = 15;
                        break;
                    }
                case 15:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_n = 16;
                        break;
                    }
                case 16:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_n = 13;
                        break;
                    }
                case 17://一
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X + Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X - Cake * 2, tem_Poi.Y);
                        tem_n = 18;
                        break;
                    }
                case 18:
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X, tem_Poi.Y + Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y + Cake * 2);
                        tem_n = 17;
                        break;
                    }
                case 19://田
                    {
                        tem_ArrayPoi[1] = new Point(tem_Poi.X - Cake, tem_Poi.Y);
                        tem_ArrayPoi[2] = new Point(tem_Poi.X - Cake, tem_Poi.Y - Cake);
                        tem_ArrayPoi[3] = new Point(tem_Poi.X, tem_Poi.Y - Cake);
                        tem_n = 19;
                        break;
                    }
            }
            bool tem_bool = true;
            for(int i = 0; i < tem_ArrayPoi.Length; i++)
            {
                if (tem_ArrayPoi[i].X / 20 < 0)
                {
                    tem_bool = false;
                    break;
                }
                if (tem_ArrayPoi[i].X / 20 > conWidth)
                {
                    tem_bool = false;
                    break;
                }
                if (tem_ArrayPoi[i].Y / 20 >=conHeight)
                {
                    tem_bool = false;
                    break;
                }
                if (Place[tem_ArrayPoi[i].X/20, tem_ArrayPoi[i].Y/20])
                {
                    tem_bool = false;
                    break;
                }
            }
            if (tem_bool)
            {
                for(int i = 0; i < tem_ArrayPoi.Length; i++)
                {
                    ArryPoi[i] = tem_ArrayPoi[i];
                }
                Convertor = tem_n;
            }
        }
        /// <summary>
        /// 绘制方块
        /// </summary>
        /// <param name="control"></param>
        public void Protract(Control control)
        {
            Mycontrol = control;
            Graphics g = control.CreateGraphics();
            for(int i = 0; i < ArryPoi.Length; i++)
            {
                Rectangle rect = new Rectangle(ArryPoi[i].X + 1, ArryPoi[i].Y + 1, 19, 19);
                MyPaint(g, new SolidBrush(ConColor), rect);
            }
        }

        public void ConvertorMove(int n)
        {
            switch (n)
            {
                case 0://下
                    {
                        for (int i = 0; i < Arryfront.Length; i++)
                            Arryfront[i] = new Point(ArryPoi[i].X, ArryPoi[i].Y + Cake);
                        break;
                    }
                case 1://左
                    {
                        for (int i = 0; i < Arryfront.Length; i++)
                            Arryfront[i] = new Point(ArryPoi[i].X - Cake, ArryPoi[i].Y);
                        break;
                    }
                case 2://右
                    {
                        for (int i = 0; i < Arryfront.Length; i++)
                            Arryfront[i] = new Point(ArryPoi[i].X + Cake, ArryPoi[i].Y);
                        break;
                    }
            }
            bool tem_bool = MoveStop(n);
            if (tem_bool)
            {
                ConvertorDelete();
                for (int i = 0; i < Arryfront.Length; i++)
                    ArryPoi[i] = Arryfront[i];
                firstPoi = ArryPoi[0];
                Protract(Mycontrol);
            }
            else
            {
                if (!tem_bool && n == 0)
                {
                    conMax = 0;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void PlaceInitialization()
        {
            conWidth = Mycontrol.Width / 20;
            conHeight = Mycontrol.Height / 20;
            Place = new bool[conWidth, conHeight];
            for(int i = 0; i < conWidth; i++)
            {
                for(int j = 0; j < conHeight; j++)
                {
                    Place[i, j] = false;
                    PlaceColor[i, j] = Color.Black;
                }
            }
            maxY = conHeight * Cake;
        }

        /// <summary>
        /// 检查出界
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool MoveStop(int n)
        {
            bool tem_bool = true;
            int tem_width = 0;
            int tem_height = 0;
            switch (n)
            {
                case 0:
                    {
                        for(int i = 0; i < Arryfront.Length; i++)
                        {
                            tem_width = Arryfront[i].X / 20;
                            tem_height = Arryfront[i].Y / 20;
                            if (tem_height > conHeight || Place[tem_width, tem_height])
                                tem_bool = false;
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < Arryfront.Length; i++)
                        {
                            tem_width = Arryfront[i].X / 20;
                            tem_height = Arryfront[i].Y / 20;
                            if (tem_width<0 || Place[tem_width, tem_height])
                                tem_bool = false;
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < Arryfront.Length; i++)
                        {
                            tem_width = Arryfront[i].X / 20;
                            tem_height = Arryfront[i].Y / 20;
                            if (tem_width > conWidth || Place[tem_width, tem_height])
                                tem_bool = false;
                        }
                        break;
                    }
            }
            return tem_bool;
        }
    }
}
