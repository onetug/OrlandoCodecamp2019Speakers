using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers.Linux
{
    public class LinuxPinController : PinController<LinuxInputPin, LinuxOutputPin>
    {
        public const string UnexportFilePath = "/sys/class/gpio/unexport";
        public const string ExportFilePath = "/sys/class/gpio/export";

        public const string OutputDirectionValue = "out";
        public const string InputDirectionValue = "in";

        public LinuxPinController(IAsyncFileUtil fileUtils, ITaskSchedulerUtility taskSchedulerUtility)
            : base((pinNumber) => new LinuxInputPin(pinNumber, fileUtils),
                  (pinNumber) => new LinuxOutputPin(pinNumber, fileUtils),
                  taskSchedulerUtility)
        {
        }

        protected override void DisposePins(IEnumerable<LinuxInputPin> inputPins, IEnumerable<LinuxOutputPin> outputPins)
        {
            foreach (var pin in inputPins)
            {
                pin.Dispose();
            }

            foreach (var pin in outputPins)
            {
                pin.Dispose();
            }
        }
    }
}
