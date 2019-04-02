using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoPinController.FileUtils
{
    public interface IAsyncFileUtil
    {
        Encoding FileEncoding { get; }

        Task AppendTextAsync(string filePath, string content);
        void AppendText(string filePath, string content);

        Task<string> ReadTextAsync(string filePath);
        Task<char> ReadFirstCharacterAsync(string filePath);
        bool DirectoryExists(string directoryPath);
    }
}
