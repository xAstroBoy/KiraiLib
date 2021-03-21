namespace KiraiLibs
{
    partial class KiraiRPC
    {
        public struct RPCData
        {
            public int id;
            public string sender;
            public string target;
            public string[] parameters;

            public RPCData(string sender, int id, string target, string[] parameters)
            {
                this.sender = sender;
                this.id = id;
                this.target = target;
                this.parameters = parameters;
            }
        }

        public enum RPCEventIDs
        {
            OnInit = 0x000,
        }
    }
}
