using POS.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ImageResize'
    public static class ImageResize
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ImageResize'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ImageResize.resizeImage(ImageInfo, string)'
        public static void resizeImage(Data.ImageInfo imageInfo, string pathToSave)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ImageResize.resizeImage(ImageInfo, string)'
        {
            string imageData = imageInfo.Src.Split(',').LastOrDefault();
            byte[] bytes = Convert.FromBase64String(imageData);
            using (MemoryStream ms = new MemoryStream(bytes))
            {

                using (Image image = Image.Load(ms))
                {
                    // Resize the image in place and return it for chaining.
                    // 'x' signifies the current image processing context.
                    image.Mutate(x => x.Resize(imageInfo.Width, imageInfo.Height));

                    // The library automatically picks an encoder based on the file extension then
                    // encodes and write the data to disk.
                    // You can optionally set the encoder to choose.
                    image.Save(pathToSave);
                }
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ImageResize.customImageWithOutResize(string, string)'
        public static void customImageWithOutResize(string imageSoruce, string pathToSave)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ImageResize.customImageWithOutResize(string, string)'
        {
            string imageData = imageSoruce.Split(',').LastOrDefault();
            byte[] bytes = Convert.FromBase64String(imageData);
            using (MemoryStream ms = new MemoryStream(bytes))
            {

                using (Image image = Image.Load(ms))
                {
                    // Resize the image in place and return it for chaining.
                    // 'x' signifies the current image processing context.
                    image.Mutate(x => x.Resize(200, 200));
                    // The library automatically picks an encoder based on the file extension then
                    // encodes and write the data to disk.
                    // You can optionally set the encoder to choose.
                    image.Save(pathToSave);
                }
            }
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ImageResize.precriptionImageWithOutResize(string, string)'
        public static void precriptionImageWithOutResize(string imageSoruce, string pathToSave)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ImageResize.precriptionImageWithOutResize(string, string)'
        {
            string imageData = imageSoruce.Split(',').LastOrDefault();
            byte[] bytes = Convert.FromBase64String(imageData);
            using (MemoryStream ms = new MemoryStream(bytes))
            {

                using (Image image = Image.Load(ms))
                {
                    // Resize the image in place and return it for chaining.
                    // 'x' signifies the current image processing context.
                    //image.Mutate(x => x.Resize(200, 200));
                    // The library automatically picks an encoder based on the file extension then
                    // encodes and write the data to disk.
                    // You can optionally set the encoder to choose.
                    image.Save(pathToSave);
                }
            }
        }
    }
}
