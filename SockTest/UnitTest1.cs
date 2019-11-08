using Xunit;
using System.Collections.Generic;

namespace Sock
{
    public class SockRenderTest
    {
        [Fact]
        public void testFormatLines()
        {

            List<string> input1 = new List<string>()
            {
                "aaaaaaaaaaa",
                "bbbbbbbbbbbbbbb",
            };

            List<string> blueprint1 = new List<string>()
            {
                "aaaaaaaaaa",
                "a",
                "bbbbbbbbbb",
                "bbbbb"
            };

            List<string> result1 = Render.formatLines(input1, 10);
            Assert.Equal(blueprint1, result1);

             List<string> input2 = new List<string>()
            {
                "aaaaa bbbbbb ccccc",
                "dddd eeeee fffff",
            };

             List<string> blueprint2 = new List<string>()
            {
                "aaaaa",
                "bbbbbb",
                "ccccc",
                "dddd eeeee",
                "fffff",
            };

            List<string> result2 = Render.formatLines(input2, 10);
            Assert.Equal(blueprint2, result2);
        }
    }
}
