namespace DreamInMars.Dto
{
    public class Response
    {
        public Response(object result) =>
            Data = result;
        public Response(string error)
        { 
            Error = error;
            HasErrors = true;
        }
        public object Data { get;  }
        public string Error { get;  }
        public bool HasErrors { get; }

    }
}
