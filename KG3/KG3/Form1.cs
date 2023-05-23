using System;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;
using System.Drawing;

namespace KG3
{
    public partial class Form1 : Form
    {
        private double _Width = 5;
        private double _Height = 5;
        private double _Length = 5;
        private float _Transparency = 1.0f;
        private int _LightCount = 0;

        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(0, 0, 0, 1);

            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            const double W = 7;
            double H = W * AnT.Height / AnT.Width;
            Gl.glOrtho(-W, W, -H, H, -200, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        private void Render(double x, double y)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glColor4f(1, 1, 1, _Transparency);

            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -20);
            Gl.glRotated((x - AnT.Height) * 180 / AnT.Height, 0, 1, 0);
            Gl.glRotated((y - AnT.Width) * 180 / AnT.Width, 1, 0, 0);

            Gl.glPushMatrix();
            Gl.glScaled(_Width, _Height, _Length);
            Glut.glutSolidCube(1);
            Gl.glPopMatrix();

            Gl.glPopMatrix();

            Gl.glFlush();
            AnT.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Gl.glDisable(Gl.GL_LIGHT0); // Отключаем первый источник света
                for (int i = 0; i < _LightCount; i++)
                {
                    Gl.glDisable(Gl.GL_LIGHT0 + i); // Отключаем все источники света
                }

                _LightCount = 0; // Сбрасываем счетчик источников света

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Index + 1 != dataGridView1.Rows.Count)
                    {
                        if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null)
                        {
                            int lightIndex = Gl.GL_LIGHT0 + _LightCount;
                            Gl.glEnable(lightIndex);

                            float[] position =
                            {
                                (float)Convert.ToDecimal(row.Cells[0].Value),
                                (float)Convert.ToDecimal(row.Cells[1].Value),
                                (float)Convert.ToDecimal(row.Cells[2].Value),
                                1.0f
                            };
                            Gl.glLightfv(lightIndex, Gl.GL_POSITION, position);

                            float[] color =
                            {
                                ((Color)row.Cells[3].Value).R / 255.0f,
                                ((Color)row.Cells[3].Value).G / 255.0f,
                                ((Color)row.Cells[3].Value).B / 255.0f,
                                1.0f
                            };
                            Gl.glLightfv(lightIndex, Gl.GL_DIFFUSE, color);

                            _LightCount++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка входных данных!");
            }
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            Render(e.X, e.Y);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (DialogResult.OK == colorDialog1.ShowDialog())
                {
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = colorDialog1.Color;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _Width = Convert.ToDouble(textBox1.Text);
            }
            catch (Exception)
            {
                _Width = 5;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _Transparency = trackBar1.Value / 10.0f;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _Height = Convert.ToDouble(textBox4.Text);
            }
            catch (Exception)
            {
                _Height = 5;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _Length = Convert.ToDouble(textBox5.Text);
            }
            catch (Exception)
            {
                _Length = 5;
            }
        }
    }
}
