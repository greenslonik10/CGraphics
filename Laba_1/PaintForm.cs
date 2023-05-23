using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Laba_1
{
    public partial class PaintForm : Form
    {

        private Bitmap image;
        private Color borderColor;
        private Color fillColor;
        private bool[,] visited;

        public PaintForm()
        {
            InitializeComponent();

            // Создаем и инициализируем изображение
            image = new Bitmap(400, 400);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                g.DrawRectangle(Pens.Black, 100, 100, 200, 200);
            }

            // Задаем цвет границы и цвет закраски
            borderColor = Color.Black;
            fillColor = Color.Red;

            // Инициализируем массив visited
            visited = new bool[image.Width, image.Height];
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Отображаем изображение на форме
            e.Graphics.DrawImage(image, 0, 0);
        }

        private void FloodFill(int x, int y)
        {
            // Получаем цвет текущей точки
            Color targetColor = image.GetPixel(x, y);

            // Если цвет текущей точки уже совпадает с цветом закраски, то выходим
            if (targetColor == fillColor)
                return;

            // Создаем стек для хранения координат пикселей
            Stack<(int, int)> stack = new Stack<(int, int)>();

            // Помещаем начальную точку в стек
            stack.Push((x, y));

            while (stack.Count > 0)
            {
                // Извлекаем точку из стека
                var (currentX, currentY) = stack.Pop();

                // Проверяем, находится ли точка в пределах изображения и не была ли она уже посещена
                if (currentX >= 0 && currentX < image.Width && currentY >= 0 && currentY < image.Height && !visited[currentX, currentY])
                {
                    // Проверяем цвет текущей точки
                    if (image.GetPixel(currentX, currentY) == targetColor)
                    {
                        // Закрашиваем текущую точку новым цветом
                        image.SetPixel(currentX, currentY, fillColor);
                        visited[currentX, currentY] = true;

                        // Добавляем соседние точки в стек для их дальнейшей обработки
                        stack.Push((currentX + 1, currentY));
                        stack.Push((currentX - 1, currentY));
                        stack.Push((currentX, currentY + 1));
                        stack.Push((currentX, currentY - 1));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Задаем координаты точки, с которой начнется закраска
            int startX = 150;
            int startY = 150;

            // Вызываем функцию для закраски области
            FloodFill(startX, startY);

            // Перерисовываем форму для отображения изменений
            Invalidate();
        }

    }
}
