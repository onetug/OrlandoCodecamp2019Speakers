using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers
{
    public abstract class PinController<TInputPin, TOutputPin> : IDisposable where TInputPin : InputPin where TOutputPin : OutputPin
    {
        private readonly Func<int, TInputPin> _createInputPinFunction;
        private readonly Func<int, TOutputPin> _createOutputPinFunction;

        private readonly TaskScheduler _taskScheduler;
        private readonly Task<Task> _continuouslyCheckingInputPinsTask;

        protected PinController(Func<int, TInputPin> cerateInputPinFunction, Func<int, TOutputPin> createOutputPinFunction, ITaskSchedulerUtility taskSchedulerUtility)
        {
            _createInputPinFunction = cerateInputPinFunction;
            _createOutputPinFunction = createOutputPinFunction;

            ConfiguredInputPins = ImmutableDictionary.Create<int, TInputPin>();
            ConfiguredOutputPins = ImmutableDictionary.Create<int, TOutputPin>();

            _taskScheduler = taskSchedulerUtility.GetScheduler();
            _continuouslyCheckingInputPinsTask = new Task<Task>(ContinuouslyCheckingInputPins, TaskCreationOptions.LongRunning);
        }

        public IImmutableDictionary<int, TInputPin> ConfiguredInputPins { get; private set; }
        public IImmutableDictionary<int, TOutputPin> ConfiguredOutputPins { get; private set; }

        public bool IsContinuouslyCheckingInputPins
        {
            get;
            private set;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            StopContinuouslyCheckingInputPins();
            _continuouslyCheckingInputPinsTask.Dispose();

            DisposePins(ConfiguredInputPins.Values, ConfiguredOutputPins.Values);
            ConfiguredInputPins = ImmutableDictionary.Create<int, TInputPin>();
            ConfiguredOutputPins = ImmutableDictionary.Create<int, TOutputPin>();
        }

        public TInputPin GetOrCreateInputPin(int pinNumber)
        {
            if (!ConfiguredInputPins.TryGetValue(pinNumber, out TInputPin pin))
            {
                pin = _createInputPinFunction(pinNumber);
                ConfiguredInputPins = ConfiguredInputPins.Add(pinNumber, pin);
            }

            return pin;
        }

        public TOutputPin GetOrCreateOutputPin(int pinNumber)
        {
            if (!ConfiguredOutputPins.TryGetValue(pinNumber, out TOutputPin pin))
            {
                pin = _createOutputPinFunction(pinNumber);
                ConfiguredOutputPins = ConfiguredOutputPins.Add(pinNumber, pin);
            }

            return pin;
        }

        public void StartContinuouslyCheckingInputPins()
        {
            if (!IsContinuouslyCheckingInputPins)
            {
                IsContinuouslyCheckingInputPins = true;
                _continuouslyCheckingInputPinsTask.Start(_taskScheduler);
            }
        }

        public void StopContinuouslyCheckingInputPins()
        {
            IsContinuouslyCheckingInputPins = false;
        }

        protected abstract void DisposePins(IEnumerable<TInputPin> inputPins, IEnumerable<TOutputPin> outputPins);

        private async Task ContinuouslyCheckingInputPins()
        {
            while (IsContinuouslyCheckingInputPins)
            {
                foreach(var inputPin in ConfiguredInputPins.Values)
                {
                    await inputPin.UpdateCurrentValueAsync();
                }
            }
        }
    }
}