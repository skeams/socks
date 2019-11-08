using System;
using Xunit;
using System.Collections.Generic;

namespace Sock
{
    public class SockRenderTest
    {
        [Fact]
        public void testFormatLines()
        {
            Render render = new Render();

            string[] input1 =
            {
                "aaaaaaaaaaa",
                "bbbbbbbbbbbbbbb",
            };

            string[] blueprint1 =
            {
                "aaaaaaaaaa",
                "a",
                "bbbbbbbbbb",
                "bbbbb"
            };

            List<string> result1 = render.formatLines(input1, 10);
            Assert.Equal(blueprint1, result1);

            string[] input2 =
            {
                "aaaaa bbbbbb ccccc",
                "dddd eeeee fffff",
            };

            string[] blueprint2 =
            {
                "aaaaa",
                "bbbbbb",
                "ccccc",
                "dddd eeeee",
                "fffff",
            };

            List<string> result2 = render.formatLines(input2, 10);
            Assert.Equal(blueprint2, result2);
        }
    }
}
