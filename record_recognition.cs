
using System.Text.Json;

namespace recognition{
    public class record_recognition{
        string url;
        string login;
        string password;
        string image_path;
        public record_recognition(string url, string login, string password, string image_path){
            this.url = url;
            this.login = login;
            this.password = password;
            this.image_path = image_path;
        }
        public async Task<string> UploadImg()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                Dictionary<string, string> auth = new Dictionary<string, string>();
                auth.Add(this.login, this.password);
                string upload_json = JsonSerializer.Serialize(auth);
                byte[] byte_arr = System.Text.Encoding.UTF8.GetBytes(upload_json);
                string byte_str = System.Convert.ToBase64String(byte_arr);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", byte_str);
                client.DefaultRequestHeaders.Add("json", "true");

                var file = new System.IO.FileInfo(this.image_path);
                if (!file.Exists)
                    throw new ArgumentException($"Unable to access file at: {this.image_path}", nameof(this.image_path));

                using (var stream = file.OpenRead())
                {
                    var multipartContent = new System.Net.Http.MultipartFormDataContent();
                    multipartContent.Add(
                        new System.Net.Http.StreamContent(stream),
                        "file_name",
                        file.Name);

                    System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, this.url);
                    request.Content = multipartContent;
                    var response = await client.SendAsync(request);
                    if((int)response.StatusCode != 200){
                        throw new Exception("Server returned an error \"" + response.ReasonPhrase +
                                          "\" with status code " + (int)response.StatusCode);
                    }
                    using (HttpContent content = response.Content)
                    {
                        string json = "";
                        json = await content.ReadAsStringAsync() ?? throw new Exception("Server returned null");
                        return json;
                    }
                }
            }
        }
    }
}
