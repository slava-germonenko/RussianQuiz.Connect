using System;
using System.Text;

using RussianQuiz.Connect.Core.Services.Abstractions;
using RussianQuiz.Connect.Core.Settings;


namespace RussianQuiz.Connect.Core.Services
{
    public class HashingTool : IHashingTool
    {
        private readonly IHashingSettings _hashingSettings;

        private Encoding HashBytesEncoding => _hashingSettings.Encoding;


        public HashingTool(IHashingSettings hashingSettings)
        {
            _hashingSettings = hashingSettings;
        }


        public string GenerateHash(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using var hashAlgorithm = _hashingSettings.HashAlgorithm;

            var bytes = hashAlgorithm.ComputeHash(
                HashBytesEncoding.GetBytes(value)
            );

            return HashBytesEncoding.GetString(bytes);
        }

        public bool ValidateHash(string value, string hash)
        {
            return GenerateHash(value).Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}