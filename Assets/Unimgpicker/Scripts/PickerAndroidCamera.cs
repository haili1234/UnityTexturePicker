#if UNITY_ANDROID
using UnityEngine;

namespace Kakera
{
    internal class PickerAndroidCamera : IPicker
    {
        private static readonly string PickerClass = "com.kakeragames.unimgpicker.Camera";

        public void Show(string title, string outputFileName, int maxSize)
        {
            using (var picker = new AndroidJavaClass(PickerClass))
            {
                picker.CallStatic("show", title, outputFileName, maxSize);
            }
        }
    }
}
#endif