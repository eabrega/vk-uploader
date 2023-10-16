using System.Net.Http.Headers;
using System.Text;
using VkNet;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace VkUploader.Helpers
{
    internal static class FileUploaderHelpers
    {
        internal static async Task<IReadOnlyCollection<MediaAttachment>> UploadPhotoAsync(
            this VkApi api,
            long groupId,
            string filePath)
        {
            var info = await api.Photo.GetMessagesUploadServerAsync(groupId);
            var result = await TransferFile(filePath, info.UploadUrl);

            return api.Photo.SaveMessagesPhoto(result);
        }

        internal static async Task<IReadOnlyCollection<Attachment>> UploadDocsAsync(
            this VkApi api,
            long peerId,
            string filePath)
        {
            var info = await api.Docs.GetMessagesUploadServerAsync(peerId, DocMessageType.Doc);
            var result = await TransferFile(filePath, info.UploadUrl);
            return await api.Docs.SaveAsync(result, "pdf");
        }

        private static async Task<string> TransferFile(string filePath, string uri)
        {
            var client = new HttpClient();

            var file = File.ReadAllBytes(filePath);
            var requestContent = new MultipartFormDataContent();
            var сontent = new ByteArrayContent(file);

            сontent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            requestContent.Add(сontent, "file", filePath);

            var uploadResult = await client.PostAsync(uri, requestContent);
            var response = await uploadResult.Content.ReadAsByteArrayAsync();
            return Encoding.ASCII.GetString(response);
        }
    }
}
