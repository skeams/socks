using System;
using System.Collections.Generic;

namespace Sock
{
    public static class Render
    {
        static readonly ConsoleColor frameColor = ConsoleColor.Green;
        static readonly ConsoleColor bannerColor = ConsoleColor.DarkCyan;
        static readonly ConsoleColor textColor = ConsoleColor.White;

        static readonly string frameVertical = "\u2551";
        static readonly string frameHorizontal = "\u2550";

        static readonly string frameTopLeft = "\u2554";
        static readonly string frameTopRight = "\u2557";
        static readonly string frameBottomLeft = "\u255A";
        static readonly string frameBottomRight = "\u255D";

        static readonly string frameJoinRight = "\u2560";
        static readonly string frameJoinLeft = "\u2563";
        // private readonly string frameJoinBottom = "\u2566";
        // private readonly string frameJoinTop = "\u2569";

        static readonly int xPadding = 3;
        static readonly int yPadding = 1;

        static readonly string[] banner =
        {
            " ____             _",
            "/ ___|  ___   ___| | _____",
            "\'___ \' / _ \' / __| |/ / __|",
            " ___) | (_) | (__|   <\'__ \'",
            "|____/ \'___/ \'___|_|\'_\'___/",
        };

        /// -------------------------------------------------------------
        ///
        ///  Clears the Screen
        ///
        public static void clearScreen()
        {
            Console.Clear();
        }

        /// -------------------------------------------------------------
        ///
        ///  Helper method to position cursor at correct position for input.
        ///
        public static void positionInputCursor()
        {
            Console.SetCursorPosition(xPadding + 3, Console.WindowHeight - yPadding - 3);
        }

        /// -------------------------------------------------------------
        ///
        ///    Renders contentLines in columns.
        ///
        public static void renderColumnContent(List<string> contentLines)
        {
            int columnWidth = 40; // TODO: Move to input props.
            int columnSpacing = 2;

            List<string> formattedContentLines = formatLines(contentLines, columnWidth);

            Console.ForegroundColor = textColor;

            int xMax = Console.WindowWidth - xPadding;
            int yMax = Console.WindowHeight - yPadding;

            int contentMinX = xPadding + 1 + columnSpacing;
            int contentMinY = yPadding + 8;
            int contentMaxX = xMax - 10;
            int contentMaxY = yMax - 6;

            int rowIndex = 0;
            int columnIndex = 0;
            
            foreach (string line in formattedContentLines)
            {
                Console.SetCursorPosition(contentMinX + (columnIndex * (columnWidth + columnSpacing)), contentMinY + rowIndex);
                Console.Write(line);

                rowIndex++;
                if (rowIndex > (contentMaxY - contentMinY))
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
