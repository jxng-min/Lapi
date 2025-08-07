namespace NPCService
{
    public interface INPCService
    {
        void Load();
        string GetName(NPCCode code);
    }
}