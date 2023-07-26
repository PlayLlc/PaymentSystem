using AndroidX.Camera.Core;
using ZXing;
using ZXing.Common;
using Android.Graphics;

namespace PayWithPlay.Droid.Utils.Scanner
{
    public class ZXingBarcodeAnalyzer : Java.Lang.Object, ImageAnalysis.IAnalyzer
    {
        private readonly Action<string>? _onScannedAction;
        private readonly MultiFormatReader _multiFormatReader = new();
        private volatile bool isScanning;

        public ZXingBarcodeAnalyzer(Action<string> onScannedAction)
        {
            _onScannedAction = onScannedAction;
            _multiFormatReader.Hints = new Dictionary<DecodeHintType, object>
            {
                { DecodeHintType.POSSIBLE_FORMATS, new List<BarcodeFormat> 
                                                   {
                                                        BarcodeFormat.All_1D,
                                                        BarcodeFormat.UPC_A,
                                                        BarcodeFormat.UPC_E,
                                                        BarcodeFormat.EAN_13,
                                                        BarcodeFormat.EAN_8,
                                                        BarcodeFormat.CODABAR,
                                                        BarcodeFormat.CODE_39,
                                                        BarcodeFormat.CODE_93,
                                                        BarcodeFormat.CODE_128,
                                                        BarcodeFormat.ITF,
                                                        BarcodeFormat.RSS_14,
                                                        BarcodeFormat.RSS_EXPANDED,
                                                   } }
            };
        }

        public void Analyze(IImageProxy image)
        {
            if (isScanning)
            {
                image.Close();
                return;
            }

            isScanning = true;

            if ((image.Format == (int)ImageFormatType.Yuv420888 || image.Format == (int)ImageFormatType.Yuv422888
                || image.Format == (int)ImageFormatType.Yuv444888) && image.GetPlanes().Length == 3)
            {
                var rotatedImage = new RotatedImage(GetLuminancePlaneData(image), image.Width, image.Height);
                RotateImageArray(rotatedImage, image.ImageInfo.RotationDegrees);

                var planarYUVLuminanceSource = new PlanarYUVLuminanceSource(
                    rotatedImage.ByteArray,
                    rotatedImage.Width,
                    rotatedImage.Height,
                    0, 0,
                    rotatedImage.Width,
                    rotatedImage.Height,
                    false
                );
                var hybridBinarizer = new HybridBinarizer(planarYUVLuminanceSource);
                var binaryBitmap = new BinaryBitmap(hybridBinarizer);

                string? result = null;
                try
                {
                    var rawResult = _multiFormatReader.decodeWithState(binaryBitmap);
                    Console.WriteLine("Barcode: " + rawResult?.Text);
                    result = rawResult?.Text;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    _multiFormatReader.reset();
                    image.Close();
                }

                _onScannedAction?.Invoke(result);

                isScanning = false;
            }
        }

        private static byte[] GetLuminancePlaneData(IImageProxy image)
        {
            var plane = image.GetPlanes()[0];
            var buf = plane.Buffer;
            var data = new byte[buf.Remaining()];
            buf.Get(data);
            buf.Rewind();
            var width = image.Width;
            var height = image.Height;
            var rowStride = plane.RowStride;
            var pixelStride = plane.PixelStride;

            // remove padding from the Y plane data
            var cleanData = new byte[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    cleanData[y * width + x] = data[y * rowStride + x * pixelStride];
                }
            }
            return cleanData;
        }

        private static void RotateImageArray(RotatedImage imageToRotate, int rotationDegrees)
        {
            if (rotationDegrees == 0) return; // no rotation
            if (rotationDegrees % 90 != 0) return; // only 90 degree times rotations

            var width = imageToRotate.Width;
            var height = imageToRotate.Height;

            var rotatedData = new byte[imageToRotate.ByteArray!.Length];
            for (var y = 0; y < height; y++) // we scan the array by rows
            {
                for (var x = 0; x < width; x++)
                {
                    switch (rotationDegrees)
                    {
                        case 90:
                            rotatedData[x * height + height - y - 1] =
                                imageToRotate.ByteArray[x + y * width]; // Fill from top-right toward left (CW)
                            break;
                        case 180:
                            rotatedData[width * (height - y - 1) + width - x - 1] =
                                imageToRotate.ByteArray[x + y * width]; // Fill from bottom-right toward up (CW)
                            break;
                        case 270:
                            rotatedData[y + x * height] =
                                imageToRotate.ByteArray[y * width + width - x - 1]; // The opposite (CCW) of 90 degrees
                            break;
                    }
                }
            }

            imageToRotate.ByteArray = rotatedData;

            if (rotationDegrees != 180)
            {
                imageToRotate.Height = width;
                imageToRotate.Width = height;
            }
        }

        private class RotatedImage 
        {
            public RotatedImage(byte[]? byteArray, int width, int height)
            {
                ByteArray = byteArray;
                Width = width;
                Height = height;
            }

            public byte[]? ByteArray { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
