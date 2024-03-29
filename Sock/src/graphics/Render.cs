using System;
using System.Collections.Generic;

namespace Sock
{
    public static class Render
    {
        static readonly ConsoleColor frameColor = ConsoleColor.Green;
        static readonly ConsoleColor bannerColor = ConsoleColor.DarkCyan;
        static readonly ConsoleColor textColor = ConsoleColor.White;
        static readonly ConsoleColor pageTitleColor = ConsoleColor.Magenta;
        static readonly ConsoleColor statusColor = ConsoleColor.DarkGreen;
        static readonly ConsoleColor errorColor = ConsoleColor.Red;
        static readonly ConsoleColor highlightColor = ConsoleColor.DarkYellow;

        public static readonly List<ConsoleColor> colors = new List<ConsoleColor>{
            ConsoleColor.DarkYellow,
            ConsoleColor.DarkBlue,
            ConsoleColor.DarkGray,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta,
            // ConsoleColor.Blue,
            // ConsoleColor.DarkRed,
            ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.Cyan,
            ConsoleColor.DarkRed,
        };

        static readonly string frameVertical = "\u2551";
        static readonly string frameHorizontal = "\u2550";

        static readonly string frameTopLeft = "\u2554";
        static readonly string frameTopRight = "\u2557";
        static readonly string frameBottomLeft = "\u255A";
        static readonly string frameBottomRight = "\u255D";

        static readonly string frameJoinRight = "\u2560";
        static readonly string frameJoinLeft = "\u2563";
        static readonly string frameJoinBottom = "\u2566";
        static readonly string frameJoinTop = "\u2569";

        static readonly int xPadding = 3;
        static readonly int yPadding = 1;

        public static string currentStatus = "";
        public static bool statusIsWarning = false;

        static readonly string[] banner =
        {
            " ____             _",
            "/ ___|  ___   ___| | _____",
            "\'___ \' / _ \' / __| |/ / __|",
            " ___) | (_) | (__|   <\'__ \'",
            "|____/ \'___/ \'___|_|\'_\'___/",
        };

        static string previousStatus = "";

        /// -------------------------------------------------------------
        ///
        /// Clears the Screen
        ///
        public static void clearScreen()
        {
            Console.Clear();
        }

        /// -------------------------------------------------------------
        ///
        /// Helper method to position cursor at correct position for input.
        ///
        public static void positionInputCursor()
        {
            Console.SetCursorPosition(xPadding + 3, Console.WindowHeight - yPadding - 3);
        }

        /// -------------------------------------------------------------
        ///
        /// Renders contentLines in columns.
        ///
        public static void renderColumnContent(List<string> contentLines)
        {
            int columnWidth = 40; // TODO: Move to input props.
            int columnSpacing = 2;

            int xContentPadding = 2;
            int yContentPadding = 1;

            List<string> formattedContentLines = formatLines(contentLines, columnWidth);

            Console.ForegroundColor = textColor;

            int xMax = Console.WindowWidth - xPadding;
            int yMax = Console.WindowHeight - yPadding;

            int contentMinX = xPadding + 1 + xContentPadding;
            int contentMinY = yPadding + 8;
            int contentMaxX = xMax - 10;
            int contentMaxY = yMax - 6 - yContentPadding;

            int rowIndex = 0;
            int columnIndex = 0;
            
            foreach (string line in formattedContentLines)
            {
                Console.SetCursorPosition(contentMinX + (columnIndex * (columnWidth + columnSpacing)), contentMinY + rowIndex + yContentPadding);
                if (line.Length > 0 && line[0].Equals('^')) // Highlight this line
                {
                    Console.ForegroundColor = highlightColor;
                    Console.Write(line.Substring(1));
                    Console.ForegroundColor = textColor;
                }
                else
                {
                    Console.Write(line);
                }

                rowIndex++;
                if (rowIndex > (contentMaxY - contentMinY - 1))
                {
                    rowIndex = 0;
                    columnIndex++;

                    if (contentMinX + ((columnIndex + 1) * columnWidth) > contentMaxX)
                    {
                        break;
                    }
                }
            }
        }

        /// -------------------------------------------------------------
        ///
        /// Splits lines when they are longer than the columnWidth prop.
        /// Line is split on the first space occurring before the limit.
        ///
        public static List<string> formatLines(List<string> lines, int columnWidth)
        {
            List<string> formattedContentLines = new List<string>();

            foreach (string line in lines)
            {
                string lineBuffer = line;
                while (lineBuffer.Length > columnWidth)
                {
                    int splitIndex = columnWidth;

                    for (int i = splitIndex; i > 0; i--)
                    {
                        if (lineBuffer[i].Equals(' '))
                        {
                            splitIndex = i;
                            break;
                        }
                    }

                    formattedContentLines.Add(lineBuffer.Substring(0, splitIndex));
                    lineBuffer = lineBuffer.Substring(splitIndex);

                    if (lineBuffer[0].Equals(' '))
                    {
                        lineBuffer = lineBuffer.Substring(1);
                    }
                }

                formattedContentLines.Add(lineBuffer);
            }

            return formattedContentLines;
        }

        /// -------------------------------------------------------------
        ///
        /// Renders Page info
        ///
        public static void renderPageInfo(List<string> infoLines, string budgetTitle)
        {
            List<string> formattedPageInfo = formatLines(infoLines, Console.WindowWidth - (xPadding * 2) - 39);

            for (int y = 0; y < formattedPageInfo.Count; y++)
            {
                Console.SetCursorPosition(xPadding + 37, yPadding + 2 + y);

                if (y == 0)
                {
                    Console.ForegroundColor = pageTitleColor;
                    Console.Write(formattedPageInfo[y] + " (" + budgetTitle + ")");
                }
                else if (y == 1)
                {
                    Console.ForegroundColor = textColor;
                    Console.Write(formattedPageInfo[y]);
                }
            }
        }

        /// -------------------------------------------------------------
        ///
        /// Renders Chart
        ///
        public static void renderChart(ConsoleColor[,] chart, List<FinanceItem> items)
        {
            int sidePadding = 40;
            int startX = Console.WindowWidth - xPadding - (chart.GetLength(0) * 2) - sidePadding;
            int startY = yPadding + 10;

            for (int y = 0; y < chart.GetLength(0); y++)
            {
                for (int x = 0; x < chart.GetLength(1) * 2; x += 2)
                {
                    if (chart[y, x / 2] != ConsoleColor.Black)
                    {
                        Console.SetCursorPosition(startX + x, startY + y);
                        Console.ForegroundColor = chart[y, x / 2];
                        Console.Write("\u2588\u2588");
                    }
                }
            }

            int colorIndex = 0;
            foreach (FinanceItem item in items)
            {
                if (item.amount < 0)
                {
                    Console.SetCursorPosition(Console.WindowWidth - xPadding - (sidePadding - 5), startY + colorIndex * 2);
                    Console.ForegroundColor = Render.colors[colorIndex % Render.colors.Count];
                    Console.Write("\u2588");
                    Console.ForegroundColor = textColor;
                    Console.Write(" : " + item.title);
                    colorIndex++;
                }
            }
        }

        /// -------------------------------------------------------------
        ///
        /// Renders status
        ///
        public static void renderStatus()
        {
            Console.SetCursorPosition(xPadding + 53, Console.WindowHeight - yPadding - 3);
            Console.Write(new string(' ', previousStatus.Length));
            Console.SetCursorPosition(xPadding + 53, Console.WindowHeight - yPadding - 3);
            Console.ForegroundColor = statusIsWarning ? errorColor : statusColor;
            Console.Write(currentStatus);
            previousStatus = currentStatus;
            Console.ForegroundColor = textColor;
        }

        /// -------------------------------------------------------------
        ///
        /// Renders status text
        ///
        public static void setStatus(string statusText, bool isError)
        {
            currentStatus = statusText;
            statusIsWarning = isError;
            renderStatus();
        }

        /// -------------------------------------------------------------
        ///
        /// Renders Frame and banner
        ///
        public static void renderFrame()
        {
            int xMax = Console.WindowWidth - xPadding;
            int yMax = Console.WindowHeight - yPadding;

            Console.ForegroundColor = frameColor;

            // Top line
            Console.SetCursorPosition(xPadding, yPadding);
            Console.Write(frameTopLeft);
            for (int x = xPadding + 1; x < xMax - 1; x++)
            {
                Console.Write(frameHorizontal);
            }
            Console.Write(frameTopRight);

            // Left and right sides
            for (int y = yPadding + 1; y < yMax; y++)
            {
                Console.SetCursorPosition(xPadding, y);
                Console.Write(frameVertical);
                Console.SetCursorPosition(xMax - 1, y);
                Console.Write(frameVertical);
            }

            // Bottom line
            Console.SetCursorPosition(xPadding, yMax - 1);
            Console.Write(frameBottomLeft);
            for (int x = xPadding + 1; x < xMax - 1; x++)
            {
                Console.Write(frameHorizontal);
            }
            Console.Write(frameBottomRight);

            // Header divider line
            Console.SetCursorPosition(xPadding, yPadding + banner.GetLength(0) + 2);
            Console.Write(frameJoinRight);
            for (int x = xPadding + 1; x < xMax - 1; x++)
            {
                Console.Write(frameHorizontal);
            }
            Console.Write(frameJoinLeft);

            // Input divider line
            Console.SetCursorPosition(xPadding, yMax - 5);
            Console.Write(frameJoinRight);
            for (int x = xPadding + 1; x < xMax - 1; x++)
            {
                Console.Write(frameHorizontal);
            }
            Console.Write(frameJoinLeft);

            // Status divider line
            Console.SetCursorPosition(xPadding + 50, yMax - 5);
            Console.Write(frameJoinBottom);
            for (int y = yMax - 4; y < yMax - 1; y++)
            {
                Console.SetCursorPosition(xPadding + 50, y);
                Console.Write(frameVertical);
            }
            Console.SetCursorPosition(xPadding + 50, yMax - 1);
            Console.Write(frameJoinTop);

            // Banner divider line
            Console.SetCursorPosition(xPadding + 35, yPadding);
            Console.Write(frameJoinBottom);
            for (int y = yPadding + 1; y < yPadding + 7; y++)
            {
                Console.SetCursorPosition(xPadding + 35, y);
                Console.Write(frameVertical);
            }
            Console.SetCursorPosition(xPadding + 35, yPadding + 7);
            Console.Write(frameJoinTop);

            // Banner
            Console.ForegroundColor = bannerColor;
            for (int l = 0; l < banner.GetLength(0); l++)
            {
                Console.SetCursorPosition(xPadding + 3, yPadding + 1 + l);
                Console.Write(banner[l]);
            }
        }
    }
}
