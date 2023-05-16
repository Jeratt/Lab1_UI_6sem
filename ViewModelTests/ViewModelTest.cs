using ViewModel;
using Moq;
using FluentAssertions;

namespace ViewModelTests
{
    public class ViewModelTest
    {
        [Fact]
        public void FromControlsTest()
        {
            var reporter_exp = new Mock<IErrorReporter>();
            var dialog_exp = new Mock<IDialogWindows>();
            ViewData viewData = new ViewData(reporter_exp.Object, dialog_exp.Object);

            viewData.Left = 1.0;
            viewData.Right = 5.0;
            viewData.NodeCnt = 5;
            viewData.NodeCntSpline = 55;
            viewData.IsUniform = true;
            viewData.Func = DataLibrary.FRawEnum.FRawCubic;
            viewData.Ders = new double[2] { 6.0, 30.0 };

            viewData.ExecuteRawDataFromControlsCommand.Execute(null);

            reporter_exp.Verify(reporter => reporter.reportError(It.IsAny<string>()), Times.Never);
            viewData.Integral.Should().Be("168");
        }

        [Fact]
        public void SaveConstraintTest()
        {
            var reporter_exp = new Mock<IErrorReporter>();
            var dialog_exp = new Mock<IDialogWindows>();
            ViewData viewData = new ViewData(reporter_exp.Object, dialog_exp.Object);

            viewData.SaveRawDataCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public void SaveOkTest()
        {
            var reporter_exp = new Mock<IErrorReporter>();
            var dialog_exp = new Mock<IDialogWindows>();
            ViewData viewData = new ViewData(reporter_exp.Object, dialog_exp.Object);

            viewData.Left = 1.0;
            viewData.Right = 5.0;
            viewData.NodeCnt = 5;
            viewData.NodeCntSpline = 55;
            viewData.IsUniform = true;
            viewData.Func = DataLibrary.FRawEnum.FRawCubic;
            viewData.Ders = new double[2] { 6.0, 30.0 };

            viewData.SaveRawDataCommand.CanExecute(null).Should().BeTrue();
        }

        [Fact]
        public void FromControlsConstraintTest()
        {
            var reporter_exp = new Mock<IErrorReporter>();
            var dialog_exp = new Mock<IDialogWindows>();
            ViewData viewData = new ViewData(reporter_exp.Object, dialog_exp.Object);

            viewData.ExecuteRawDataFromControlsCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public void FromFileConstraintTest()
        {
            var reporter_exp = new Mock<IErrorReporter>();
            var dialog_exp = new Mock<IDialogWindows>();
            ViewData viewData = new ViewData(reporter_exp.Object, dialog_exp.Object);

            viewData.NodeCntSpline = 1;

            viewData.ExecuteRawDataFromFileCommand.CanExecute(null).Should().BeFalse();
        }
    }
}