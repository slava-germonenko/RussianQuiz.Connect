using System.Security.Cryptography;
using System.Text;


namespace RussianQuiz.Connect.Core.Settings
{
    public interface IHashingSettings
    {
        public Encoding Encoding { get; }

        public HashAlgorithm HashAlgorithm { get; }
    }
}