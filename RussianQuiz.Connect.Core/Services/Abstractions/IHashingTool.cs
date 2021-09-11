namespace RussianQuiz.Connect.Core.Services.Abstractions
{
    public interface IHashingTool
    {
        public string GenerateHash(string value);

        public bool ValidateHash(string value, string hash);
    }
}