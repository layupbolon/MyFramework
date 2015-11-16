using System;
using System.Collections.Generic;
using Framework.Core.Wechat;
using Framework.Core.Wechat.model;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            WechatPush wechatPush = new WechatPush();

            string fileUrl = @"F:\Eric\WorkSpace\Code\CSharp\MyFramework\Framework.Core\TestApp\bin\Debug\Images\123.jpg";
            //上传图片
            string picMediaId = wechatPush.UploadPicture(fileUrl);

            //构建图文信息
            PicTextList picTextList = new PicTextList();
            picTextList.articles = new List<PicTextInfo>()
            {
                new PicTextInfo()
                {
                    thumb_media_id = picMediaId,
                    title = "这是我Eric添加的标题",
                    content = "若看到此内容那就说明我发送成功了！",
                    show_cover_pic = "1"
                }
            };

            string picTextMediaId = wechatPush.UploadNews(picTextList);

            //发送图文信息
            PicTextMessage picTextMessage = new PicTextMessage();
            picTextMessage.filter = new Filter() { is_to_all = true };
            picTextMessage.mpnews = new Mpnews() { media_id = picTextMediaId };
            picTextMessage.msgtype = "mpnews";

            if (wechatPush.SendPicTextMessage(picTextMessage))
                Console.WriteLine("Success");
            else
            {
                Console.WriteLine("Fail");
            }
            Console.ReadKey();
        }
    }
}
