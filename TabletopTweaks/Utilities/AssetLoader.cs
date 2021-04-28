using System.IO;
using UnityEngine;

namespace TabletopTweaks.Utilities {
    class AssetLoader {
        // Loosely based on https://forum.unity.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
        public static class Image2Sprite {
            public static string icons_folder = "";
            public static Sprite Create(string filePath) {
                var bytes = File.ReadAllBytes(icons_folder + filePath);
                var texture = new Texture2D(64, 64, TextureFormat.DXT5, false);
                _ = texture.LoadImage(bytes);
                return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0, 0));
            }
        }
    }
}
