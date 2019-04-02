using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController
{
    public abstract class InputPin : Pin
    {
        private bool _currentValue;

        protected InputPin(int number, IAsyncFileUtil fileUtils) : base(number, fileUtils)
        {
        }

        public event Action<InputPin, bool> InputValueChanged;

        public override PinDirectionType PinDirection => PinDirectionType.Input;

        public abstract Task<bool> GetInputValueAsync();

        public bool CurrentValue
        {
            get => _currentValue;
            protected set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    RaiseInputValueChanged();
                }
            }
        }

        private void RaiseInputValueChanged()
        {
            InputValueChanged?.Invoke(this, CurrentValue);
        }

        public async Task UpdateCurrentValueAsync()
        {
            CurrentValue = await GetInputValueAsync();
        }
    }
}
