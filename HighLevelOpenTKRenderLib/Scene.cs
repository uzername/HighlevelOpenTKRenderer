using OpenTK.Mathematics;

namespace HighLevelOpenTKRenderLib
{
    public class Scene  {
        /// <summary>
        /// one color of scene gradient background
        /// </summary>
        public Color4 color1 = new Color4 { R = 0.2f, G = 0.7f, B = 0.9f, A = 1.0f };
        /// <summary>
        /// second color of scene gradient background
        /// </summary>
        public Color4 color2 = new Color4 { R = 1.0f, G = 1.0f, B = 1.0f, A = 1.0f };

        
        public List<Light> SceneLights;
        public Light AmbientLight;
        public List<Object3D> SceneObjects;
        
        //public SimpleCamera camera;
        public FirstPersonCamera camera;
        public SimpleObject3D object3D;
        public Scene() {
            SceneLights = new List<Light>();
            SceneObjects = new List<Object3D>();
            
            
        }
        public void AddTestObject()
        {
            uint[] testcubeIndices = new uint[] { };
            float[] testcubeVertices = new float[] { };
            MeshConstructor3D.GetCubeMesh(1.0f, out testcubeVertices, out testcubeIndices);
            object3D = new SimpleObject3D(testcubeVertices, testcubeIndices);
            object3D.MoveTo(0, 0, -3);
        }
    }
}
