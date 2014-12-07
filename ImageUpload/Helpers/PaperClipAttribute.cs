using System;
using System.Web.Mvc;

[AttributeUsage(AttributeTargets.Property)]
public class PaperClipAttribute : Attribute, IMetadataAware
{
    // source http://stackoverflow.com/questions/5013824/paperclip-for-asp-net-mvc
    public PaperClipAttribute(string uploadPath, params string[] sizes)
    {
        if (string.IsNullOrEmpty(uploadPath))
        {
            throw new ArgumentException("Please specify an upload path");
        }

        UploadPath = uploadPath;
        Sizes = sizes;
    }

    public string UploadPath { get; private set; }
    public string[] Sizes { get; private set; }

    public void OnMetadataCreated(ModelMetadata metadata)
    {
        metadata.AdditionalValues["PaperClip"] = this;
    }
}