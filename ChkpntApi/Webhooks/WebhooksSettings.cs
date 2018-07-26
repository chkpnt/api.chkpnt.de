using System.IO;

namespace ChkpntApi.Webhooks
{
    
    public class WebhooksSettings
    {

        private string handlersPath;
        
        public string HandlersPath
        { 
            get => Path.GetFullPath(handlersPath);
            set => handlersPath = value;
        }
    }
}