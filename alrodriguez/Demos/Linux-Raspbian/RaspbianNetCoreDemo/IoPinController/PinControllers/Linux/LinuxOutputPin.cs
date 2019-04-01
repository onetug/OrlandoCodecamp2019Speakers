using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers.Linux
{
    public class LinuxOutputPin : OutputPin
    {
        private const string OutputLowValue = "0";
        private const string OutputHighValue = "1";
        
        private readonly string _outputModeFilePath;

        public LinuxOutputPin(int number, IAsyncFileUtil fileUtils) : base(number, fileUtils)
        {
            _outputModeFilePath = $"/sys/class/gpio/gpio{this.NumberText}/value";
        }

        protected override void OnInitialize()
        {
            //First check if the pin has already been exported
            if (!FileUtils.DirectoryExists($"/sys/class/gpio/gpio{this.NumberText}"))
            {
                FileUtils.AppendText(LinuxPinController.ExportFilePath, NumberText);
            }

            var directionFilePath = $"/sys/class/gpio/gpio{this.NumberText}/direction";
            FileUtils.AppendText(directionFilePath, LinuxPinController.OutputDirectionValue);
        }

        protected override async Task OnDisposeAsync()
        {
            await SetOutputModeAsync(OutputModeType.Low);
            await UnexportPinAsync();
        }

        protected override async Task OnSetOutputModeAsync(OutputModeType outputMode)
        {
            var outputValue = OutputMode == OutputModeType.Low ? OutputLowValue : OutputHighValue;
            await FileUtils.AppendTextAsync(_outputModeFilePath, outputValue);
        }

        private async Task UnexportPinAsync()
        {
            await FileUtils.AppendTextAsync(LinuxPinController.UnexportFilePath, NumberText);
        }
    }
}
