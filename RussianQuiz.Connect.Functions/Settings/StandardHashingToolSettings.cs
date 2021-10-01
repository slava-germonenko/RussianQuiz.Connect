using System.Security.Cryptography;
using System.Text;

using RussianQuiz.Connect.Core.Settings;


namespace RussianQuiz.Connect.Functions.Settings
{
    public class StandardHashingSettings : IHashingSettings
    {
        public Encoding Encoding => Encoding.UTF8;

        public HashAlgorithm HashAlgorithm => SHA256.Create();
    }
}