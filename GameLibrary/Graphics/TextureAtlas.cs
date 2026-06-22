using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
    public class TextureAtlas
    {
        public Texture2D Texture { get; set; }

        private readonly Dictionary<string, TextureRegion> _regions = [];
        private readonly Dictionary<string, Animation> _animations = [];


        public TextureAtlas(Texture2D texture)
            => Texture = texture;

        public static TextureAtlas FromFile(ContentManager content, string fileName)
        {
            var filePath = Path.Combine(content.RootDirectory, fileName);

            using (var stream = TitleContainer.OpenStream(filePath))
                using (var reader = XmlReader.Create(stream))
            {
                var document = XDocument.Load(reader);
                var root = document.Root;

                // Texture regions
                var texturePath = root.Element("Texture").Value;
                var atlas = new TextureAtlas(content.Load<Texture2D>(texturePath));

                var regions = root.Element("Regions")?.Elements("Region");

                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        var regionName = region.Attribute("name")?.Value;

                        if (string.IsNullOrEmpty(regionName))
                            continue;
                        
                        var x = int.Parse(region.Attribute("x")?.Value ?? "0");
                        var y = int.Parse(region.Attribute("y")?.Value ?? "0");

                        var width = int.Parse(region.Attribute("width")?.Value ?? "0");
                        var height = int.Parse(region.Attribute("height")?.Value ?? "0");
                        
                        atlas.AddRegion(regionName, x, y, width, height);
                    }
                }

                // Animations
                var animationElements = root.Element("Animations").Elements("Animation");

                if (animationElements != null)
                {
                    foreach (var animationElement in animationElements)
                    {
                        var animationName = animationElement.Attribute("name")?.Value;
                        var fps = int.Parse(animationElement.Attribute("fps")?.Value ?? "0");

                        var frames = new List<TextureRegion>();
                        var frameElements = animationElement.Elements("Frame");

                        if (frameElements != null)
                        {
                            foreach (var frameElement in frameElements)
                            {
                                var regionName = frameElement.Attribute("region").Value;
                                var region = atlas.GetRegion(regionName);
                                frames.Add(region);
                            }
                        }

                        var animation = new Animation(fps, frames);
                        atlas.AddAnimation(animationName, animation);
                    }
                }

                return atlas;
            }
        }


        public TextureRegion AddRegion(string name, int x, int y, int width, int height)
        {
            var region = new TextureRegion(Texture, x, y, width, height);
            _regions.Add(name, region);

            return region;
        }
        public TextureRegion GetRegion(string name) => _regions[name];
        public bool RemoveRegion(string name) => _regions.Remove(name);

        public void ClearRegions() => _regions.Clear();

        public void AddAnimation(string name, Animation animation)
            => _animations.Add(name, animation);
        public Animation GetAnimation(string name) => _animations[name];
        public bool RemoveAnimation(string name) => _animations.Remove(name);

        public void ClearAnimations() => _animations.Clear();


        public Sprite CreateSprite(string name) => new(GetRegion(name));
        public AnimatedSprite CreateAnimatedSprite(string name) => new(GetAnimation(name));
    }
}