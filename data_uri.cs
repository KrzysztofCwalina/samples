#:package OpenAI@2.3.*-*
#:package Microsoft.Extensions.Logging@7.*-*
#:package Microsoft.Extensions.Logging.Console@7.*-*
#:property PublishAot=false

using System.Buffers.Text;

BinaryData data = BinaryData.FromStream(File.OpenRead("d:\\100TJersey2023.jpeg"));
string base64String = Convert.ToBase64String(data.ToArray());
Uri uri = new($"data:text/plain;base64,{base64String}");
Console.WriteLine(uri.AbsoluteUri);
Console.WriteLine(uri.AbsoluteUri.Length);