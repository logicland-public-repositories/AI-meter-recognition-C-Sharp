using recognition;
using System.Text.Json;

namespace example{
    class Examp
    {
        static async Task Main(string[] args)
        {
            if(args.Length < 4){
                Console.WriteLine("incorrect number of arguments");
                Environment.Exit(0);
            }
            string url = args[1];
            string login = args[2];
            string password = args[3];
            string image_path = args[4];
            record_recognition record = new record_recognition(url, login, password, image_path);
            Dictionary<string,float> result = new Dictionary<string, float>();
            try{
                string output =  await record.UploadImg();
                result = JsonSerializer.Deserialize<Dictionary<string,float>>(output) ??
                                                    throw new Exception("Unexpected properties of null");
            }catch(Exception e){
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
            foreach (KeyValuePair<string, float> el in result)
            {
                Console.WriteLine("{0}: {1}",
                el.Key, el.Value);
            }
        }
    }
}

