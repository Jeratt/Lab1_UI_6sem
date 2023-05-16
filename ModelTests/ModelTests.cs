using DataLibrary;
using FluentAssertions;

namespace ModelTests
{
    public class ModelTests
    {
        [Fact]
        public void RawDataFromFieldsTest()
        {
            RawData rawData = new RawData(1.0, 5.0, 5, true, RawData.FRawCubic);

            rawData.NodeCnt.Should().Be(5);
            rawData.Grid.Length.Should().Be(5);
            rawData.Field.Length.Should().Be(5);
            for (double i = 1.0; i < 6.0; i++) {
                rawData.Grid[(int)i - 1].Should().Be(i);
                rawData.Field[(int)i - 1].Should().Be(i*i*i+3.0);
            }
        }

        [Fact]
        public void RawDataFromFileTest() {
            RawData rawData = new RawData(1.0, 5.0, 5, true, RawData.FRawCubic);
            rawData.Save("SaveTest");

            RawData rawDataFile = new RawData("SaveTest");
            rawData.NodeCnt.Should().Be(5);
            rawData.Grid.Length.Should().Be(5);
            rawData.Field.Length.Should().Be(5);
            for (double i = 1.0; i < 6.0; i++)
            {
                rawData.Grid[(int)i - 1].Should().Be(i);
                rawData.Field[(int)i - 1].Should().Be(i * i * i + 3.0);
            }
        }

        [Fact]
        public void SplineDataTest()
        {
            RawData rawData = new RawData(1.0, 5.0, 5, true, RawData.FRawCubic);

            SplineData spline = new SplineData(rawData, new double[2] { 6.0, 30.0 }, 10);
            spline.Interpolate();
            spline.Integral.Should().Be(168.0);
        }

        [Fact]
        public void ExceptionTest() {
            SplineData spline = new SplineData(null, new double[2] { 6.0, 30.0 }, 10);
            Action act = new Action(() => spline.Interpolate());
            act.Should().Throw<NullReferenceException>();
        }
    }
}