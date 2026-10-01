namespace Pallene.Sdk.Cdda
{
    public sealed class CdSaoCreator
    {
        public string? Mcn { get; set; } = null;

        public CdSaoCreator SetMcn(string? mcn)
        {
            Mcn = mcn;
            return this;
        }


    }
}
