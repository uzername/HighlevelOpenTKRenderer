using OpenTK.Mathematics;

namespace HighLevelOpenTKRenderLib
{
    public class Scene  {
        public Color4 color1 = new Color4 { R = 0.2f, G = 0.7f, B = 0.9f, A = 1.0f };
        public Color4 color2 = new Color4 { R = 1.0f, G = 1.0f, B = 1.0f, A = 1.0f };

        public List<Light> SceneLights;
        public Light AmbientLight;
        public List<Object3D> SceneObjects;
        public Scene() { 
            SceneLights = new List<Light>();
            SceneObjects = new List<Object3D>();
        }
    }
}
