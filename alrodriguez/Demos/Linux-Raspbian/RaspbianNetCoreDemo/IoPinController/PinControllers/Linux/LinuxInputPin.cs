using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers.Linux
{
    public class LinuxInputPin : InputPin
    {
        private readonly string _inputValueFilePath;

        public LinuxInputPin(int number, IAsyncFileUtil fileUtils) : base(number, fileUtils)
        {
            var inputFileDirectory = $"/sys/class/gpio/gpio{NumberText}";
            var fileName = "value";
            _inputValueFilePath = Path.Combine(inputFileDirectory, fileName);
        }
        
        protected override void OnInitialize()
        {
            //First check if the pin has already been exported
            if (!FileUtils.DirectoryExists($"/sys/class/gpio/gpio{this.NumberText}"))
            {
                FileUtils.AppendText(LinuxPinController.ExportFilePath, NumberText);
            }

            var directionFilePath = $"/sys/class/gpio/gpio{this.NumberText}/direction";
            FileUtils.AppendText(directionFilePath, LinuxPinController.InputDirectionValue);
        }

        protected override async Task OnDisposeAsync()
        {
            await UnexportPinAsync();
        }

        public override async Task<bool> GetInputValueAsync()
        {
            //There should only be one character in the file, so we can save some time here
            var inputValue = await FileUtils.ReadFirstCharacterAsync(_inputValueFilePath);

            //0 means no signal, something else means there's a signal of some kind
            switch (inputValue)
            {
                case '0':
                    return false;
                case '1':
                    return true;
                default:
                    throw new InvalidDataException($"Invalid input pin value read with character: {inputValue}");
            }
        }

        private async Task UnexportPinAsync()
        {
            await FileUtils.AppendTextAsync(LinuxPinController.UnexportFilePath, NumberText);
        }
    }
}
