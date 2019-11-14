using System;
using System.Collections.Generic;

namespace Sock
{
    public class PieChart
    {
        public int radius;
        public List<FinanceItem> items;

        public ConsoleColor[,] chart;

        public PieChart(int radius, List<FinanceItem> items)
        {
            this.radius = radius;
            this.items = items;

            chart = new ConsoleColor[radius * 2, radius * 2];
            for (int y = 0; y < radius * 2; y++)
            {
                for (int x = 0; x < radius * 2; x++)
                {
                    chart[y, x] = ConsoleColor.Black;
                }
            }

            generateChart();
        }

        /// -------------------------------------------------------------
        ///
        public void generateChart()
        {
            double totalAmount = getPositiveSum();
            int colorIndex = 0;

            double remainder = totalAmount;
            float currentAngle = 0;

            foreach (FinanceItem item in items)
            {
                if (item.amount < 0)
                {
                    float percent = Convert.ToSingle(-item.amount * 100 / totalAmount);

                    for (int y = 0; y < radius * 2; y++)
                    {
                        for (int x = 0; x < radius * 2; x++)
                        {
                            if (isPointInSector(radius, x - radius + 0.5f, y - radius + 0.5f, percent, currentAngle))
                            {
                                chart[y, x] = Render.colors[colorIndex % Render.colors.Count];
                            }
                        }
                    }

                    currentAngle = 360 * percent / 100 + currentAngle; 

                    colorIndex++;
                }
            }
        }

        /// -------------------------------------------------------------
        ///
        /// Code taken from:
        /// https://www.geeksforgeeks.org/check-whether-point-exists-circle-sector-not/
        ///
        bool isPointInSector(float radius, float x, float y, float percent, float startAngle) 
        { 
            // calculate endAngle 
            float endAngle = 360 * percent / 100 + startAngle; 
        
            // Calculate polar co-ordinates 
            float polarradius = (float) Math.Sqrt(x * x + y * y); 
            
            float angle = (float) Math.Atan2(x, y) * -1 * (float)(180 / Math.PI) + 180;

            return angle >= startAngle && angle <= endAngle && polarradius < radius;
        } 

        /// -------------------------------------------------------------
        ///
        public double getPositiveSum()
        {
            double sum = 0;
            items.ForEach(item =>
                sum += item.amount > 0 ? item.amount : 0
            );
            return sum;
        }
    }
}