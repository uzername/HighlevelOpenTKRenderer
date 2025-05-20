namespace HighLevelOpenTKRenderLib
{
    public class Scene  {
        public List<Light> SceneLights;
        public Light AmbientLight;
        public List<Object3D> SceneObjects;
        public Scene() { 
            SceneLights = new List<Light>();
            SceneObjects = new List<Object3D>();
        }
    }
}
