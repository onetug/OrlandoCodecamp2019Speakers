using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IoPinController.FileUtils
{
    public class AsyncFileUtil : IAsyncFileUtil
    {
        public Encoding FileEncoding { get => Encoding.UTF8; }

        public async Task<string> ReadTextAsync(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                var fileBytes = new byte[inputStream.Length];
                await inputStream.ReadAsync(fileBytes, 0, fileBytes.Length);

                var fileText = FileEncoding.GetString(fileBytes);
                return fileText;
            }
        }

        public async Task<char> ReadFirstCharacterAsync(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                //Encoding is 8 bits, so we'll only read 1 byte to have the 1 character. At least, I think that's how that works
                var fileBytes = new byte[1];
                await inputStream.ReadAsync(fileBytes, 0, fileBytes.Length);

                var fileText = FileEncoding.GetString(fileBytes);
                return fileText[0];
            }
        }

        public async Task AppendTextAsync(string filePath, string content)
        {
            using (var outputStream = File.OpenWrite(filePath))
            {
                var stringBytes = FileEncoding.GetBytes(content);
                await outputStream.WriteAsync(stringBytes, 0, stringBytes.Length);
            }
        }

        public void AppendText(string filePath, string content)
        {
            using (var outputStream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                var stringBytes = FileEncoding.GetBytes(content);
                outputStream.Write(stringBytes, 0, stringBytes.Length);
            }
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
    }
}
