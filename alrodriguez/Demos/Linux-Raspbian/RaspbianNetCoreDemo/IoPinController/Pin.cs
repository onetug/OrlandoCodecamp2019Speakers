using System;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController
{
    public abstract class Pin : IDisposable
    {
        private bool _isInitialized;

        protected Pin(int number, IAsyncFileUtil fileUtils)
        {
            Number = number;
            NumberText = number.ToString();
            FileUtils = fileUtils;
            Initialize();
        }

        public int Number { get; }
        public string NumberText { get; }
        public IAsyncFileUtil FileUtils { get; }
        public abstract PinDirectionType PinDirection { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Task.Run(OnDisposeAsync);
            }
        }

        protected void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                OnInitialize();
            }
        }

        protected abstract void OnInitialize();

        protected abstract Task OnDisposeAsync();
    }
}
