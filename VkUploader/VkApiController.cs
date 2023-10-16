using Microsoft.Extensions.Configuration;
using VkNet;
using VkNet.Model;
using VkUploader.Helpers;

namespace VkUploader;

internal class VkApiController
{
    private readonly VkApi _api;
    public VkApiController(IConfiguration options)
    {
        var token = options.GetValue<string>("token");

        _api = new VkApi();
        _api.Authorize(new ApiAuthParams
        {
            AccessToken = token
        });
    }

    internal async Task SendMessage(long groupId, string message, string? filePath = null)
    {
        if (filePath is not null)
        {
            var attachment = await _api.UploadPhotoAsync(groupId, filePath);
            _api.Messages.Send(new MessagesSendParams() { Message = message, RandomId = 0, PeerId = 2000000001, Attachments = attachment });
        }
        else
        {
            _api.Messages.Send(new MessagesSendParams() { Message = message, RandomId = 0, PeerId = 2000000001 });
        }
    }
    internal async Task SendMessageWithDock(long peerId, string message, string? filePath = null)
    {
        if (filePath is not null)
        {
            var attachment = await _api.UploadDocsAsync(peerId, filePath);
            var rnd = new Random().Next(0, int.MaxValue);
            _api.Messages.Send(new MessagesSendParams() { Message = $"{message}-{rnd}", RandomId = rnd, PeerId = peerId, Attachments = attachment.Select(x => x.Instance) });
        }
        else
        {
            _api.Messages.Send(new MessagesSendParams() { Message = message, RandomId = 0, PeerId = peerId });
        }
    }

}

