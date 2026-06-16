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
        private Dictionary<string, TextureRegion> _regions = [];
        public Texture2D Texture { get; set; }

        public TextureAtlas(Texture2D texture)
        {
            Texture = texture;
        }

        public static TextureAtlas FromFile(ContentManager content, string name)
        {
            var filePath = Path.Combine(content.RootDirectory, name);

            using (var stream = TitleContainer.OpenStream(filePath))
                using (var reader = XmlReader.Create(stream))
            {
                var document = XDocument.Load(reader);
                var root = document.Root;

                var texturePath = root.Element("Texture").Value;
                var atlas = new TextureAtlas(content.Load<Texture2D>(texturePath));

                var regions = root.Element("Regions")?.Elements("Region");

                if (regions == null)
                    return atlas;
                
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

        public void Clear() => _regions.Clear();


        public Sprite CreateSprite(string name) => new Sprite(GetRegion(name));
    }
}